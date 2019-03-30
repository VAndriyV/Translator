using Domain.Entities;
using System;
using System.Collections.Generic;
using TranslatorLogic.ReversePolishNotation.Configuration;
using System.Linq;
using Domain.Translator;
using Domain.ViewModels;

namespace TranslatorLogic.ReversePolishNotation
{
    public class RPNBuilder : IRPNBuilder
    {
        private Dictionary<int, List<string>> _priorities;

        private List<OutputLexeme> _inputLexemes;

        private Stack<string> _stack;

        private List<string> _RPN;

        private List<string> _marks;

        private List<string> _additionalCells;

        private string _loopArgument;

        private bool _loopIndicator;

        private List<string> _displayableMarks;

        private RPNBuilderTable _resultTable;

        public RPNBuilder()
        {
            _priorities = RPNPriorities.GetPrioritiesConfiguration();
            _stack = new Stack<string>();
            _RPN = new List<string>();
            _displayableMarks = new List<string>();
            _additionalCells = new List<string>();
            _loopIndicator = false;
            _loopArgument = string.Empty;
            _marks = new List<string>();
            _resultTable = new RPNBuilderTable();
        }

        public void BuildRPN(List<OutputLexeme> inputLexemes)
        {
            _inputLexemes = inputLexemes;

            foreach (var lexeme in _inputLexemes)
            {
                var lexemeName = lexeme.LexemeName;               

                if (lexeme.Idncode != null || lexeme.ConstantCode != null)
                {
                    _RPN.Add(lexemeName);
                }

                if (lexemeName == "int" || lexemeName == "float")
                {
                    _stack.Push(lexemeName);
                }

                if (lexemeName == "cin" || lexemeName == "cout")
                {
                    _stack.Push(lexemeName);
                }

                if (lexemeName == "{")
                {
                    var stackHead = _stack.Pop();
                    if (stackHead == "int")
                    {
                        _RPN.Add("IVD");
                    }
                    else if (stackHead == "float")
                    {
                        _RPN.Add("FVD");
                    }
                }
                if (lexemeName == "¶")
                {
                    if (_stack.Count > 0)
                    {
                        var stackHead = _stack.Peek();
                        if (stackHead == "int")
                        {
                            _stack.Pop();
                            _RPN.Add("IVD");
                        }
                        else if (stackHead == "float")
                        {
                            _stack.Pop();
                            _RPN.Add("FVD");
                        }
                        else if (stackHead == "cin")
                        {
                            _stack.Pop();
                            _RPN.Add("READ");
                        }
                        else if (stackHead == "cout")
                        {
                            _stack.Pop();
                            _RPN.Add("OUT");
                        }

                        while (true)
                        {
                            if (_stack.Count > 0)
                            {
                                stackHead = _stack.Peek();
                                if (stackHead.Contains("if"))
                                {
                                    _stack.Pop();
                                    _RPN.Add(_displayableMarks.Last() + ":");
                                    _displayableMarks.Remove(_displayableMarks.Last());
                                    _displayableMarks.Remove(_displayableMarks.Last());
                                    continue;
                                }
                                else if (stackHead.Contains("for"))
                                {
                                    break;
                                }
                                else if (stackHead.Contains("=") && stackHead != "=")
                                {
                                    string[] headAndMark = stackHead.Split('=');
                                    _RPN.Add("=");
                                    _RPN.Add(headAndMark[1] + ":");
                                    _stack.Pop();
                                }
                                else
                                {
                                    _RPN.Add(_stack.Pop());
                                }

                            }
                            else
                            {
                                break;
                            }

                        }
                    }
                }

                if (lexemeName == "=")
                {
                    if (_loopIndicator)
                    {
                        _loopArgument = _RPN.Last();
                        _loopIndicator = false;
                    }
                   
                    {
                        int lexemePriority = GetPriority(lexemeName);

                        while (true)
                        {
                            if (_stack.Count > 0)
                            {
                                var stackHead = _stack.Peek();
                                if (GetPriority(stackHead) >= lexemePriority)
                                {
                                    _RPN.Add(_stack.Pop());
                                }
                                else
                                {
                                    _stack.Push(lexemeName);
                                    break;
                                }
                            }
                            else
                            {
                                _stack.Push(lexemeName);
                                break;
                                
                            }
                        }
                    }
                }

                if (IsRegularLexeme(lexemeName))
                {
                    int lexemePriority = GetPriority(lexemeName);

                    while (true)
                    {
                        if (_stack.Count > 0)
                        {
                            var stackHead = _stack.Peek();
                            if (GetPriority(stackHead) >= lexemePriority)
                            {
                                _RPN.Add(_stack.Pop());
                            }
                            else
                            {
                                _stack.Push(lexemeName);
                                break;
                            }
                        }
                        else
                        {
                            _stack.Push(lexemeName);
                            break;
                        }
                    }
                }

                if (lexemeName == "(")
                {
                    _stack.Push(lexemeName);
                }

                if (lexemeName == ")")
                {
                    while (true)
                    {
                        if (_stack.Count > 0)
                        {
                            var stackHead = _stack.Pop();

                            if (stackHead != "(")
                            {
                                _RPN.Add(stackHead);
                            }
                            else
                            {
                                break;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                if (lexemeName == "?")
                {
                    while (true)
                    {
                        if (_stack.Count > 0)
                        {
                            var stackHead = _stack.Peek();
                            if (stackHead == "=")
                            {
                                _marks.Add("m" + (_marks.Count + 1));
                                _displayableMarks.Add(_marks.Last());
                                _RPN.Add(_displayableMarks.Last() + "CF");                                
                                break;
                            }
                            else
                            {
                                _RPN.Add(_stack.Pop());
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                if (lexemeName == ":")
                {
                    while (true)
                    {
                        if (_stack.Count > 0)
                        {
                            var stackHead = _stack.Peek();
                            if (stackHead == "=")
                            {
                                _RPN.Add(stackHead);
                                _RPN.Add("m" + (_marks.Count + 1) + "NC");
                                _RPN.Add(_displayableMarks.Last() + ":");
                                _marks.Add("m" + (_marks.Count + 1));
                                _displayableMarks.Add(_marks.Last());
                                _stack.Push(_stack.Pop() + _displayableMarks.Last());
                                break;
                            }
                            else
                            {
                                _RPN.Add(_stack.Pop());
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                if (lexemeName == "if")
                {
                    _stack.Push(lexemeName);
                }

                if (lexemeName == "then")
                {                   
                    while (true)
                    {
                        if (_stack.Count > 0)
                        {
                            var stackHead = _stack.Peek();
                            if (stackHead == "if")
                            {
                                _marks.Add("m" + (_marks.Count + 1));
                                _displayableMarks.Add(_marks.Last());
                                _RPN.Add(_displayableMarks.Last() + "CF");
                                _stack.Push(_stack.Pop() + _displayableMarks.Last());
                                break;
                            }
                            else
                            {
                                _RPN.Add(_stack.Pop());
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                if (lexemeName == "else")
                {
                    while (true)
                    {
                        if (_stack.Count > 0)
                        {
                            var stackHead = _stack.Peek();
                            if (stackHead.Contains("if"))
                            {

                                _RPN.Add("m" + (_displayableMarks.Count + 1) + "NC");
                                _RPN.Add(_displayableMarks.Last() + ":");
                                _marks.Add("m" + (_marks.Count + 1));
                                _displayableMarks.Add(_marks.Last());
                                _stack.Push(_stack.Pop() + _displayableMarks.Last());
                                break;
                            }
                            else
                            {
                                _RPN.Add(_stack.Pop());
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                if (lexemeName == "for")
                {
                    _loopIndicator = true;
                    _marks.Add("m" + (_marks.Count + 1));
                    _displayableMarks.Add(_marks.Last());
                    _marks.Add("m" + (_marks.Count + 1));
                    _displayableMarks.Add(_marks.Last());
                    _stack.Push(lexemeName + _displayableMarks[_displayableMarks.Count-1]+_displayableMarks[_displayableMarks.Count-2]);
                }

                if (lexemeName == "to")
                {
                    while (true)
                    {
                        if (_stack.Count > 0)
                        {
                            var stackHead = _stack.Peek();
                            if (stackHead.Contains("for"))
                            {
                                _RPN.Add(_displayableMarks[_displayableMarks.Count - 2] + ":");
                                _additionalCells.Add("r" + (_additionalCells.Count + 1));
                                _RPN.Add(_additionalCells.Last());
                               
                                break;

                            }
                            else
                            {
                                _RPN.Add(_stack.Pop());
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                if (lexemeName == "do")
                {
                    while (true)
                    {
                        if (_stack.Count > 0)
                        {
                            var stackHead = _stack.Peek();
                            if (stackHead.Contains("for"))
                            {
                                _RPN.Add("=");
                                _RPN.Add(_loopArgument);
                                _RPN.Add(_additionalCells.Last());
                                _RPN.Add("-");
                                _RPN.Add("0");
                                _RPN.Add("<=");
                                _RPN.Add(_displayableMarks.Last());
                                _RPN.Add("CF");
                                break;

                            }
                            else
                            {
                                _RPN.Add(_stack.Pop());
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                if (lexemeName == "end")
                {
                    while (true)
                    {
                        if (_stack.Count > 0)
                        {
                            var stackHead = _stack.Peek();
                            if (stackHead.Contains("for"))
                            {
                                _RPN.Add(_displayableMarks[_displayableMarks.Count - 2]);
                                _RPN.Add("NC");
                                _RPN.Add(_displayableMarks.Last() + ":");
                                _stack.Pop();
                                _displayableMarks.Remove(_displayableMarks.Last());
                                _displayableMarks.Remove(_displayableMarks.Last());
                                break;

                            }
                            else
                            {
                                _RPN.Add(_stack.Pop());
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                if (lexemeName == "}")
                {
                    string stackHead = string.Empty;
                    while (_stack.TryPop(out stackHead))
                    {
                        _RPN.Add(stackHead);
                    }
                }

                _resultTable.LoopIndicator.Add(_loopIndicator?'1':'0');
                _resultTable.LoopArgument.Add(_loopArgument);
                _resultTable.InputLexemes.Add(lexemeName);
                _resultTable.RPN.Add(_RPN.GetRange(0,_RPN.Count));
                _resultTable.Stack.Add(_stack.Reverse().ToList());

            }
        }

        private bool IsRegularLexeme(string lexeme)
        {
            List<string> lexemes = new List<string>()
            {
                "+","-","^","==","<",">",">=","<=","!=", "*","/"
            };

            return lexemes.Contains(lexeme);
        }

        private int GetPriority(string lexeme)
        {
            string correctLexeme = lexeme;

            if (lexeme.Contains("if"))
            {
                correctLexeme = "if";
            }
            if (lexeme.Contains("for"))
            {
                correctLexeme = "for";
            }

            foreach(var pair in _priorities)
            {
                if (pair.Value.Contains(correctLexeme))
                {
                    return pair.Key;
                }
            }
            throw new ArgumentException();
        }

        public List<string> GetRPN()
        {
           return _RPN;
        }

        public RPNBuilderTable GetResultTable()
        {
            return _resultTable;
        }
    }
}
