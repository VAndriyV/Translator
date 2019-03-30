using Domain.Entities;
using Domain.Exceptions;
using Domain.Translator;
using System.Collections.Generic;

namespace TranslatorLogic.SyntaxAnalyzer
{
    public class RecursiveAnalyzer : IRecursiveAnalyzer
    {
        private List<OutputLexeme> _lexemes;

        private int _lexemesCount;

        private int _currentLexeme;

        private string _errors;

        public void DoAnalyze(List<OutputLexeme> lexemes)
        {
            _lexemes = lexemes;
            _lexemesCount = _lexemes.Count;
            _currentLexeme = 0;
            _errors = string.Empty;
            if (!Program())
                throw new SyntaxAnalyzeException("Syntax errors! ", _errors);
        }
        private bool Program()
        {
            if (DeclarationList())
            {

                if (InRange() && _lexemes[_currentLexeme].LexemeId == 27)//{
                {
                    _currentLexeme++;
                    if (InRange() && _lexemes[_currentLexeme].LexemeId == 31)// ¶
                    {
                        _currentLexeme++;
                        if (OperatorsList())
                        {
                            if (InRange() && _lexemes[_currentLexeme].LexemeId == 28)//}
                            {
                                _currentLexeme++;
                            }
                            else
                            {
                                _errors += "} expected. Line: " + (_lexemes[_currentLexeme - 1].Row + 1) + ";";
                                return false;
                            }
                        }
                        else
                        {
                            _errors += "Invalid operator list format!;";
                            return false;
                        }
                    }
                    else
                    {
                        _errors += "¶ expected  Line: " + _lexemes[_currentLexeme - 1].Row + ";";
                        return false;
                    }
                }
                else
                {
                    _errors += "{ expected " + "Line: " + _lexemes[_currentLexeme-1].Row + ";";
                    return false;
                }
                if (_currentLexeme == _lexemesCount)
                {
                    return true;
                }
                else
                {
                    _errors += "To many lexemes!;";
                    return false;
                }

            }
            else
            {
                _errors += "Invalid declaration list format!;";
                return false;
            }
        }
        private bool DeclarationList()
        {
            if (Declaration())
            {
                while (InRange() && _lexemes[_currentLexeme].LexemeId == 31)
                {
                    _currentLexeme++;
                    if (Declaration())
                    {
                    }
                    else
                    {
                        _errors += "Invalid declaration list format: declaration expected " + "Line: " + _lexemes[_currentLexeme].Row + ";";
                        return false;
                    }
                }
                return true;
            }
            else
            {
                _errors += "Invalid declaration format! " + "Line: " + _lexemes[_currentLexeme].Row + ";";
                return false;
            }
        }

        private bool Declaration()
        {
            if (Type())
            {
                if (VariablesList())
                {
                    return true;
                }
                else
                {
                    _errors += "Invalid variables list format! " + "Line: " + _lexemes[_currentLexeme].Row + ";";
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        private bool Type()
        {
            if (InRange())
            {
                if (_lexemes[_currentLexeme].LexemeId == 1 || _lexemes[_currentLexeme].LexemeId == 2)
                {
                    _currentLexeme++;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        private bool VariablesList()
        {
            if (InRange() && _lexemes[_currentLexeme].LexemeId == 100)
            {
                _currentLexeme++;
                while (_currentLexeme < _lexemesCount && _lexemes[_currentLexeme].LexemeId == 26)
                {
                    _currentLexeme++;
                    if (InRange() && _lexemes[_currentLexeme].LexemeId == 100)
                    {
                        _currentLexeme++;
                    }
                    else
                    {
                        _errors += "Invalid variables list format: IDN expected after , " + "Line: " + _lexemes[_currentLexeme].Row + ";";
                        return false;
                    }
                }
                return true;
            }
            else
            {
                _errors += "Invalid variables list format: IDN expected after type lexeme " + "Line: " + _lexemes[_currentLexeme].Row + ";";
                return false;
            }
        }

        private bool OperatorsList()
        {
            if (Operator())
            {
                if (InRange() && _lexemes[_currentLexeme].LexemeId == 31)// ¶
                {
                    _currentLexeme++;
                    while (InRange() && _lexemes[_currentLexeme].LexemeId != 28)
                    {
                        if (Operator())
                        {
                            if (InRange() && _lexemes[_currentLexeme].LexemeId == 31)
                            {
                                _currentLexeme++;
                            }
                            else
                            {
                                _errors += "Invalid operator list format: ¶ expected after operator " + "Line: " + _lexemes[_currentLexeme - 1].Row + ";";
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                    return true;
                }
                else
                {
                    _errors += "Invalid operator list format: ¶ expected after operator " + "Line: " + _lexemes[_currentLexeme-1].Row + ";";
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        private bool Operator()
        {
            if (InRange())
            {
                if (_lexemes[_currentLexeme].LexemeId == 10) //cin
                {
                    _currentLexeme++;
                    if (InRange() && _lexemes[_currentLexeme].LexemeId == 17)//<<
                    {
                        _currentLexeme++;
                        if (InRange() && _lexemes[_currentLexeme].LexemeId == 100)
                        {
                            _currentLexeme++;
                            while (InRange() && _lexemes[_currentLexeme].LexemeId == 17)
                            {
                                _currentLexeme++;
                                if (InRange() && _lexemes[_currentLexeme].LexemeId == 100)
                                {
                                    _currentLexeme++;
                                }
                                else
                                {
                                    _errors += "Invalid input operator format: IDN expected after << " + "Line: " + _lexemes[_currentLexeme].Row + ";";
                                    return false;
                                }
                            }
                            return true;
                        }
                        else
                        {
                            _errors += "Invalid input operator format: IDN expected after << " + "Line: " + _lexemes[_currentLexeme].Row + ";";
                            return false;
                        }
                    }
                    else
                    {
                        _errors += "Invalid input operator format: << expected after cin " + "Line: " + _lexemes[_currentLexeme].Row + ";";
                        return false;
                    }
                }
                else if (_lexemes[_currentLexeme].LexemeId == 11)//cout
                {
                    _currentLexeme++;
                    if (InRange() && _lexemes[_currentLexeme].LexemeId == 18)//>>
                    {
                        _currentLexeme++;
                        if (InRange() && _lexemes[_currentLexeme].LexemeId == 100)
                        {
                            _currentLexeme++;
                            while (InRange() && _lexemes[_currentLexeme].LexemeId == 18)
                            {
                                _currentLexeme++;
                                if (InRange() && _lexemes[_currentLexeme].LexemeId == 100)
                                {
                                    _currentLexeme++;
                                }
                                else
                                {
                                    _errors += "Invalid output operator format: IDN expected after >> " + "Line: " + _lexemes[_currentLexeme].Row + ";";
                                    return false;
                                }
                            }
                            return true;
                        }
                        else
                        {
                            _errors += "Invalid output operator format: IDN expected after >> " + "Line: " + _lexemes[_currentLexeme].Row + ";";
                            return false;
                        }
                    }
                    else
                    {
                        _errors += "Invalid output operator format: >> expected after cout " + "Line: " + _lexemes[_currentLexeme].Row + ";";
                        return false;
                    }
                }
                else if (_lexemes[_currentLexeme].LexemeId == 3)//for
                {
                    _currentLexeme++;
                    if (InRange() && _lexemes[_currentLexeme].LexemeId == 100)//idn
                    {
                        _currentLexeme++;
                        if (InRange() && _lexemes[_currentLexeme].LexemeId == 23)//=
                        {
                            _currentLexeme++;
                            if (Expression())
                            {
                                if (InRange() && _lexemes[_currentLexeme].LexemeId == 4)//to
                                {
                                    _currentLexeme++;
                                    if (Expression())
                                    {
                                        if (InRange() && _lexemes[_currentLexeme].LexemeId == 5)//do
                                        {
                                            _currentLexeme++;
                                            if (Operator())
                                            {
                                                if (InRange() && _lexemes[_currentLexeme].LexemeId == 31)//¶
                                                {
                                                    _currentLexeme++;
                                                    while (InRange() && _lexemes[_currentLexeme].LexemeId != 6 && _lexemes[_currentLexeme].LexemeId != 28)//end }
                                                    {
                                                        if (Operator())
                                                        {
                                                            if (InRange() && _lexemes[_currentLexeme].LexemeId == 31)
                                                            {
                                                                _currentLexeme++;
                                                            }
                                                            else
                                                            {
                                                                _errors += "Invalid operator list format in cycle operator: ¶ expected after operator " + "Line: " + _lexemes[_currentLexeme].Row + ";";
                                                                return false;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            _errors += "Invalid operator list format in cycle operator: unknown operator " + "Line: " + _lexemes[_currentLexeme].Row + ";";
                                                            return false;
                                                        }
                                                    }
                                                    if (InRange() && _lexemes[_currentLexeme].LexemeId == 6)
                                                    {
                                                        _currentLexeme++;
                                                        return true;
                                                    }
                                                    else
                                                    {
                                                        _errors += "Invalid cycle operator format: end expected in cycle operator" + "Line: " + _lexemes[_currentLexeme].Row + ";";
                                                        return false;
                                                    }
                                                }
                                                else
                                                {
                                                    _errors += "Invalid operator list format: ¶ expected after operator " + "Line: " + _lexemes[_currentLexeme].Row + ";";
                                                    return false;
                                                }
                                            }
                                            else
                                            {
                                                _errors += "Invalid operator list format: unknown operator " + "Line: " + _lexemes[_currentLexeme].Row + ";";
                                                return false;
                                            }

                                        }
                                        else
                                        {
                                            _errors += "Invalid cycle operator format: do expected after expression " + "Line: " + _lexemes[_currentLexeme].Row + ";";
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        _errors += "Invalid cycle operator format: expression expected after to " + "Line: " + _lexemes[_currentLexeme].Row + ";";
                                        return false;
                                    }
                                }
                                else
                                {
                                    _errors += "Invalid cycle operator format: to expected after expression " + "Line: " + _lexemes[_currentLexeme].Row + ";";
                                    return false;
                                }
                            }
                            else
                            {
                                _errors += "Invalid cycle operator format: expression expected after = " + "Line: " + _lexemes[_currentLexeme].Row + ";";
                                return false;
                            }
                        }
                        else
                        {
                            _errors += "Invalid cycle operator format: IDN expected after for " + "Line: " + _lexemes[_currentLexeme].Row + ";";
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (_lexemes[_currentLexeme].LexemeId == 7)//if
                {
                    _currentLexeme++;
                    if (Relation())
                    {
                        if (InRange() && _lexemes[_currentLexeme].LexemeId == 8)//then
                        {
                            _currentLexeme++;
                            if (Operator())
                            {
                                if (InRange() && _lexemes[_currentLexeme].LexemeId == 9)//else
                                {
                                    _currentLexeme++;

                                    if (Operator())
                                    {
                                        return true;
                                    }
                                    else
                                    {
                                        _errors += "Invalid conditional operator format: operator expected after else and ¶  " + "Line: " + _lexemes[_currentLexeme].Row + ";";
                                        return false;
                                    }
                                }
                                else
                                {
                                    _errors += "Invalid conditional operator format: else expected " + "Line: " + _lexemes[_currentLexeme].Row + ";";
                                    return false;
                                }

                            }
                            else
                            {
                                return false;
                            }


                        }
                        else
                        {
                            _errors += "Invalid conditional operator format: then expected after relation " + "Line: " + _lexemes[_currentLexeme].Row + ";";
                            return false;
                        }
                    }
                    else
                    {
                        _errors += "Invalid conditional operator format: invalid relation format " + "Line: " + _lexemes[_currentLexeme].Row + ";";
                        return false;
                    }
                }
                else if (_lexemes[_currentLexeme].LexemeId == 100)//idn
                {
                    _currentLexeme++;
                    if (InRange() && _lexemes[_currentLexeme].LexemeId == 23)// =
                    {
                        _currentLexeme++;
                        if (Expression())
                        {
                            if (RelationSign())
                            {
                                if (Expression())
                                {
                                    if (InRange() && _lexemes[_currentLexeme].LexemeId == 16)// ?
                                    {
                                        _currentLexeme++;
                                        if (Expression())
                                        {
                                            if (InRange() && _lexemes[_currentLexeme].LexemeId == 15)// :
                                            {
                                                _currentLexeme++;
                                                if (Expression())
                                                {
                                                    return true;
                                                }
                                                else
                                                {
                                                    return false;
                                                }
                                            }
                                            else
                                            {
                                                _errors += "Invalid ternary operator format: : expected " + "Line: " + _lexemes[_currentLexeme].Row + ";";
                                                return false;
                                            }
                                        }
                                        else
                                        {
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        _errors += "Invalid ternary operator format: ? expected " + "Line: " + _lexemes[_currentLexeme].Row + ";";
                                        return false;
                                    }
                                }
                                else
                                {
                                    return false;
                                }
                            }
                            return true;
                        }
                        else
                        {
                            _errors += "Invalid operator format after = " + "Line: " + _lexemes[_currentLexeme].Row + ";";
                            return false;
                        }

                    }
                    else
                    {
                        _errors += "= expected after IDN " + "Line: " + _lexemes[_currentLexeme].Row + ";";
                        return false;
                    }
                }
                else
                {
                    _errors += "Unknown operator format " + "Line: " + _lexemes[_currentLexeme].Row + ";";
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        private bool Relation()
        {
            if (Expression())
            {
                if (RelationSign())
                {
                    if (Expression())
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    _errors += "Invalid relation sign. " + "Line: " + _lexemes[_currentLexeme].Row + ";";
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        private bool RelationSign()
        {
            if (InRange())
            {
                if (_lexemes[_currentLexeme].LexemeId == 19 // <
                    || _lexemes[_currentLexeme].LexemeId == 20 // >
                    || _lexemes[_currentLexeme].LexemeId == 21 // <=
                    || _lexemes[_currentLexeme].LexemeId == 22 // >= 
                    || _lexemes[_currentLexeme].LexemeId == 24 // ==
                    || _lexemes[_currentLexeme].LexemeId == 25) // != 
                {
                    _currentLexeme++;
                    return true;
                }
                else
                {

                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        private bool Expression()
        {
            if (Term())
            {
                while (InRange() && (_lexemes[_currentLexeme].LexemeId == 33 || _lexemes[_currentLexeme].LexemeId == 34))
                {
                    _currentLexeme++;
                    if (!Term())
                    {
                        return false;
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool Term()
        {
            if (Factor())
            {

                while (InRange() && (_lexemes[_currentLexeme].LexemeId == 35 || _lexemes[_currentLexeme].LexemeId == 36))// * /
                {
                    _currentLexeme++;
                    if (!Factor())
                    {
                        return false;
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool Factor()
        {
            if (SimplestExprPart())
            {
                while (InRange() && _lexemes[_currentLexeme].LexemeId == 32)//^
                {
                    _currentLexeme++;
                    if (!SimplestExprPart())
                    {
                        return false;
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool SimplestExprPart()
        {
            if (InRange())
            {
                if (_lexemes[_currentLexeme].LexemeId == 100 || _lexemes[_currentLexeme].LexemeId == 101)
                {
                    _currentLexeme++;
                    return true;
                }
                else if (_lexemes[_currentLexeme].LexemeId == 29)//(
                {
                    _currentLexeme++;
                    if (Expression())
                    {
                        if (_lexemes[_currentLexeme].LexemeId == 30)//)
                        {
                            _currentLexeme++;
                            return true;
                        }
                        else
                        {
                            _errors += ") expected after " + _lexemes[_currentLexeme - 1].LexemeName + " Line: " + _lexemes[_currentLexeme].Row + ";";
                            return false;
                        }
                    }
                    else
                    {
                        _errors += "Invalid expression " + "Line: " + _lexemes[_currentLexeme].Row + ";";
                        return false;
                    }
                }
                else
                {
                    _errors += "IDN or Const or ( expected after " + _lexemes[_currentLexeme - 1].LexemeName + " Line: " + _lexemes[_currentLexeme].Row + ";";
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        private bool InRange()
        {
            return _currentLexeme < _lexemesCount;
        }
    }
}
