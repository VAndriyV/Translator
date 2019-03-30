using Domain.Entities;
using Domain.Exceptions;
using Domain.Services.Interfaces;
using Domain.Translator;
using Domain.Util;
using Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TranslatorLogic.SyntaxAnalyzer
{
    public class AscendingAnalyzer : IAscendingAnalyzer
    {
        private Dictionary<string, List<List<string>>> _grammar;

        private List<string> _allTerms;      

        private List<OutputLexeme> _inputLexemes;

        private int[,] _relationMatrix;

        private bool ind;

        private Stack<string> _stack;

        private List<AscendingAnalyzerTable> _resultTable;

        public AscendingAnalyzer()
        {
            _grammar = new Dictionary<string, List<List<string>>>();
            _allTerms = new List<string>();
            _stack = new Stack<string>();
            _inputLexemes = new List<OutputLexeme>();
            _resultTable = new List<AscendingAnalyzerTable>();
            ind = false;

        }

        public async Task<List<AscendingAnalyzerTable>> DoSyntaxAnalyze(Dictionary<string, List<List<string>>> grammar
            , List<string> allTerms
            , int[,] relationMatrix
            , List<OutputLexeme> inputLexemes) 
        {
            _grammar = grammar;
            _allTerms = allTerms;           
            _inputLexemes = inputLexemes;
            _relationMatrix = relationMatrix;

            _inputLexemes.Add(new OutputLexeme {
                Id = _inputLexemes.Count+1,
                Row = 1000,
                LexemeName="#",
                LexemeId = -1,
                Idncode = null,
                ConstantCode = null
            });

            _stack.Push("#");

            while (_inputLexemes.Count > 0)
            {
                if (ind)
                {
                    _resultTable.Add(new AscendingAnalyzerTable(ConvertStackToList(), GetLexemeNames(), 3));
                    break;
                }

                if (_inputLexemes[0].LexemeName == "#")
                {
                    ind = true;
                }

                string stackHead = _stack.Peek();

                int relation = GetRelation(stackHead, _inputLexemes.First().LexemeName);

                _resultTable.Add(new AscendingAnalyzerTable(ConvertStackToList(), GetLexemeNames(), relation));

                if (relation == 1 || relation == 2)
                {
                    _stack.Push(_inputLexemes.First().LexemeName);
                    _inputLexemes.RemoveAt(0);
                }
                else if (relation == 3)
                {
                    List<string> temp = new List<string>();
                    temp.Add(_stack.Pop());

                    while (_stack.Count > 0)
                    {
                        if (GetRelation(_stack.Peek(), temp[0]) == 1)
                            break;
                        else
                        {
                            temp.Insert(0, _stack.Pop());
                        }
                    }

                    string unTerm = GetAppropriateUnterm(temp);

                    if (unTerm == "")
                    {                       

                        var lineNumber = _inputLexemes.First().Row == 1000 ? "" : ("; Line: " + _inputLexemes.First().Row);

                        throw new SyntaxAnalyzeException("Syntax errors! ", "Can not get higher rule. See " + temp.Last() + " unterm; "
                            + lineNumber);
                    }

                    _stack.Push(unTerm);
                }
                else
                {                 
                    var lineNumber = _inputLexemes.First().Row == 1000 ? "" : ("; Line: " + _inputLexemes.First().Row);

                    throw new SyntaxAnalyzeException("Syntax error! ", "Lexeme: "+_inputLexemes.First().LexemeName 
                        + lineNumber);
                }

            }
            if (_stack.Peek() != "<пр>")
            {
                throw new SyntaxAnalyzeException("Syntax error! ", "Can not get axiom <пр>");
            }

            return _resultTable;
        }        


        private List<string> ConvertStackToList()
        {
            return _stack.Reverse().ToList();
        }

        private List<string> GetLexemeNames()
        {
            return _inputLexemes.Select(l => l.LexemeName).ToList();
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

        public List<AscendingAnalyzerTable> GetAscendingAnalyzeTable()
        {
            return _resultTable;
        }
    }
}
