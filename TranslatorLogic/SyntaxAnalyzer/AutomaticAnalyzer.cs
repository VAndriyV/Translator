using Domain.Entities;
using Domain.Exceptions;
using Domain.Services.Interfaces;
using Domain.Translator;
using Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TranslatorLogic.SyntaxAnalyzer
{
    public class AutomaticAnalyzer : IAutomaticAnalyzer
    {
        private readonly IAutomaticConfigurationService _automaticConfigurationService;

        private List<AutomaticRule> _automaticRules;

        private AutomaticAnalyzerSteps _stepsTable;

        private List<OutputLexeme> _lexemes;

        private int _currentLexeme;

        private int _currentAlpha;

        private Stack<int> _stack;

        private int _counter;

        private bool isOkay;

        public AutomaticAnalyzer(IAutomaticConfigurationService automaticConfigurationService)
        {
            _automaticConfigurationService = automaticConfigurationService;
            _counter = 0;            
            _currentAlpha = 0;
            isOkay = false;
            _currentLexeme = 0;
            _stepsTable = new AutomaticAnalyzerSteps();
            _stack = new Stack<int>();
        }

        public async Task<AutomaticAnalyzerSteps> DoAnalyze(List<OutputLexeme> outputLexemes)
        {
            _lexemes = outputLexemes;
            _automaticRules = await _automaticConfigurationService.GetAutomaticRules();

            while (_currentLexeme < _lexemes.Count)
            {
                NextState();
            }

            if ((_currentLexeme >= _lexemes.Count || _currentAlpha != 5) && !isOkay)
            {
                var error = "To many lexemes!;";
                throw new SyntaxAnalyzeException("Syntax errors!", error);
            }

            return _stepsTable;
        }

        private void NextState()
        {
            var possibleStates = _automaticRules.Where(s => s.Alpha == _currentAlpha).ToList();

            var neededState = possibleStates.Find(s => s.Lexeme == _lexemes[_currentLexeme].LexemeName);

            if (_lexemes[_currentLexeme].LexemeId == 100)
            {
                neededState = possibleStates.Find(s => s.Lexeme == "idn");
            }

            if (_lexemes[_currentLexeme].LexemeId == 101)
            {
                neededState = possibleStates.Find(s => s.Lexeme == "con");
            }

            if (neededState != null)
            {
                _stepsTable.Stack.Add(new Stack<int>(_stack.Reverse()));

                if (_stepsTable.States.Count > _currentLexeme)
                {
                    _stepsTable.States.Last().Add(neededState.Alpha);
                }
                else
                {
                    _stepsTable.States.Add(new List<int>() { neededState.Alpha });
                }

                _stepsTable.Lexemes.Add(neededState.Lexeme);

                if (neededState.Information == "[=]Exit")
                {
                    _currentLexeme++;

                    if (_stack.Count() > 0)
                    {
                        _currentAlpha = _stack.Pop();
                    }
                    else
                    {
                        isOkay = true;
                        _stepsTable.Numbers.Add(_currentLexeme);
                        return;
                    }
                }
                else if (neededState.Beta != null)
                {
                    _currentLexeme++;

                    _currentAlpha = (int)neededState.Beta;

                    if (neededState.Stack != null)
                    {
                        _stack.Push((int)neededState.Stack);
                    }
                }

                _stepsTable.Numbers.Add(_currentLexeme);
            }
            else
            {
                neededState = possibleStates.Find(s => s.Lexeme == "_NOMATCH");

                if (neededState != null)
                {
                    if (neededState.Information != null && neededState.Information.Contains("[!=]Exit"))
                    {
                        if (_stack.Count() > 0)
                        {
                            if (_counter == _currentLexeme)
                            {
                                _stepsTable.States.Last().Add(neededState.Alpha);
                            }

                            _currentAlpha = _stack.Pop();
                        }
                        else
                        {
                            _currentLexeme++;
                            return;
                        }
                    }
                    else if (neededState.Information != null && neededState.Information.Contains("[!=]Error"))
                    {
                        var errorMessage = neededState.Information.Substring("[!=]Error".Length);
                        var error = errorMessage + " Line: " + _lexemes[_currentLexeme].Row + " (State " + _currentAlpha + " );";
                        throw new SyntaxAnalyzeException("Syntax errors! ", error);
                    }
                    else if (neededState.Beta != null)
                    {
                        _stepsTable.States.Add(new List<int>() { neededState.Alpha });

                        _currentAlpha = (int)neededState.Beta;

                        if (neededState.Stack != null)
                        {
                            _stack.Push((int)neededState.Stack);
                        }
                    }
                }
            }
            _counter = _currentLexeme;
        }

        public AutomaticAnalyzerSteps GetAnalyzerSteps() => _stepsTable;
    }
}
