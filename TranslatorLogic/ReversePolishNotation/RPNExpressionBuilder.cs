using Domain.Entities;
using Domain.Exceptions;
using Domain.Translator;
using Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TranslatorLogic.ReversePolishNotation
{
    public class RPNExpressionBuilder : IRPNExpressionBuilder
    {        

        private List<RPNExpressionToken> _inputTokens;

        private Dictionary<string, List<List<string>>> _grammar;

        private List<string> _allTerms;

        private int[,] _relationMatrix;

        private List<string> _RPN;

        private Stack<RPNExpressionToken> _stack;

        private List<RPNExpressionTable> _resultTable;

        private List<OutputLexeme> _inputLexemes;

        public RPNExpressionBuilder()
        {
            _grammar = new Dictionary<string, List<List<string>>>();
            _allTerms = new List<string>();
            _stack = new Stack<RPNExpressionToken>();
            _resultTable = new List<RPNExpressionTable>();
            _RPN = new List<string>();
            _inputTokens = new List<RPNExpressionToken>();
        }

        private void ParseInputLexemes()
        {         
            foreach(var lexeme in _inputLexemes)
            {
                if (lexeme.Idncode != null)
                {
                    _inputTokens.Add(new RPNExpressionToken(lexeme.LexemeName, "IDN"));
                }
                else if(lexeme.ConstantCode != null)
                {
                    _inputTokens.Add(new RPNExpressionToken(lexeme.LexemeName, "CON"));
                }
                else
                {
                    _inputTokens.Add(new RPNExpressionToken(lexeme.LexemeName, " "));
                }
            }          
        }

        public void BuildRPN(Dictionary<string, List<List<string>>> grammar
            , List<string> allTerms
            , int[,] relationMatrix            
            , List<OutputLexeme> inputLexemes)
        {
            _grammar = new Dictionary<string, List<List<string>>>();
            _allTerms = new List<string>();
            _stack = new Stack<RPNExpressionToken>();
            _resultTable = new List<RPNExpressionTable>();
            _RPN = new List<string>();
            _inputTokens = new List<RPNExpressionToken>();
            _grammar = grammar;
            _allTerms = allTerms;           
            _relationMatrix = relationMatrix;           
            _inputLexemes = inputLexemes;

            ParseInputLexemes();

            _stack = new Stack<RPNExpressionToken>();
            _stack.Push(new RPNExpressionToken("#", "strStart"));
            _inputTokens.Add(new RPNExpressionToken("#", "strEnd"));          

            Dictionary<string, string> SemantProg = new Dictionary<string, string>();
            SemantProg.Add("<E>+<T1>", "+");
            SemantProg.Add("<E>-<T1>", "-");            
            SemantProg.Add("<T>*<F>", "*");
            SemantProg.Add("<T>/<F>", "/");
            SemantProg.Add("<M>^<F>", "^");
            SemantProg.Add("IDN", "Next");
            SemantProg.Add("CON", "Next");
            bool ind = false;
            _resultTable = new List<RPNExpressionTable>();

            while (_inputTokens.Count > 0)
            {
                if (ind)
                {
                    _resultTable.Add(GetRPNExpressionTable(_stack, _inputTokens, 3, _RPN));
                    break;
                }

                RPNExpressionToken stackHead = _stack.Peek();

                RPNExpressionToken currentToken = _inputTokens.First();

                string val1 = "";

                string val2 = "";

                if (stackHead.Type == "IDN" || stackHead.Type == "CON")
                    val1 = stackHead.Type;
                else
                    val1 = stackHead.Name;
                if (currentToken.Type == "IDN" || currentToken.Type == "CON")
                    val2 = currentToken.Type;
                else
                    val2 = currentToken.Name;

                int relation;
                try
                {
                    relation = GetRelation(val1, val2);
                }
                catch(Exception e)
                {
                    throw new RPNException("Error during build Reverse Polish notation", " Check input string");
                }

                _resultTable.Add(GetRPNExpressionTable(_stack, _inputTokens, relation, _RPN));

                if (relation == 1 || relation == 2)
                {
                    _stack.Push(_inputTokens.First());
                    _inputTokens.RemoveAt(0);
                }
                else if (relation == 3)
                {
                    List<string> temp = new List<string>();
                    string IDN = "";
                    if (_stack.Peek().Type != "IDN" && _stack.Peek().Type != "CON")
                        temp.Add(_stack.Pop().Name);
                    else
                    {
                        IDN = _stack.Peek().Name;
                        temp.Add(_stack.Pop().Type);
                    }
                    while (_stack.Count > 0)
                    {
                        string val = "";

                        if (_stack.Peek().Type != "CON" && _stack.Peek().Type != "IDN")
                            val = _stack.Peek().Name;
                        else
                            val = _stack.Peek().Type;
                        if (GetRelation(val, temp[0]) == 1)
                            break;
                        else
                        {
                            string forInsert = "";

                            if (_stack.Peek().Type != "IDN" && _stack.Peek().Type != "CON")
                                forInsert = _stack.Pop().Name;
                            else
                                forInsert = _stack.Pop().Type;
                            temp.Insert(0, forInsert);
                        }
                    }

                    string unTerm = GetAppropriateUnterm(temp);

                    if (unTerm == "")
                        throw new RPNException("Error during build Reverse Polish notation: ", temp.Last());
                    
                    _stack.Push(new RPNExpressionToken(unTerm, "unTerm"));

                    string key = "";
                    for (int i = 0; i < temp.Count; i++)
                        key += temp[i];
                    if (SemantProg.ContainsKey(key))
                        if (SemantProg[key] != "Next")
                            _RPN.Add(SemantProg[key]);
                        else _RPN.Add(IDN);
                }
                else if (relation == 0)
                {
                    throw new RPNException("Error during build Reverse Polish notation, tokens can not be together: "
                        ,_stack.First().Name+" "+_inputTokens.First().Name);
                }
                if (_inputTokens[0].Name == "#" && _stack.Peek().Name == "<E2>" && _stack.Count<=2)
                    ind = true;
            }           
        }

        private int GetRelation(string term1, string term2)//отримуємо відношення між двома термами
        {
            int indexI = _allTerms.IndexOf(term1);

            int indexJ = _allTerms.IndexOf(term2);

            if (indexI != -1 && indexJ != -1)
            {
                return _relationMatrix[indexI, indexJ];
            }
            else
            {
                throw new ArgumentException("Unknown term!");
            }
        }

        private string GetAppropriateUnterm(List<string> rightPart)
        {
            foreach (var rule in _grammar)
            {
                for (int i = 0; i < rule.Value.Count; i++)
                {
                    if (rule.Value[i].SequenceEqual(rightPart))
                    {
                        return rule.Key;
                    }
                }
            }

            return "";
        }

        private RPNExpressionTable GetRPNExpressionTable(Stack<RPNExpressionToken> stack,
            List<RPNExpressionToken> inputTokens, int relation, List<string> rpn)
        {
            List<string> stackResult = new List<string>();
            List<string> inputTokensResult = new List<string>();
            List<string> rpnForTable = new List<string>(rpn);

            for (int i = stack.Count - 1; i >= 0; i--)
                stackResult.Add(stack.ElementAt(i).Name);

            for (int i = 0; i < inputTokens.Count; i++)
                inputTokensResult.Add(inputTokens[i].Name);

           

            string Relation = "";

            if (relation == 1)
                Relation = "<";
            if (relation == 2)
                Relation = "=";
            if (relation == 3)
                Relation = ">";

            return new RPNExpressionTable
            {
                Stack = stackResult,
                InputTokens = inputTokensResult,
                Relation = Relation,
                RPN = rpnForTable
            };
        }

        public List<OutputIdn> GetOutputIdns()
        {
            List<OutputIdn> result = new List<OutputIdn>();                         

            foreach (var token in _inputLexemes)
            {
                if(token.LexemeId==100)
                {
                    result.Add(new OutputIdn { Id = token.Id, Name = token.LexemeName, Type = "int" });
                   
                }
            }

            return result;
        }

        public List<string> GetRPN()
        {
            return _RPN;
        }

        public List<RPNExpressionTable> GetResultTable()
        {
            return _resultTable;
        }
    }
}
