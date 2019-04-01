using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Domain.Translator;
using Domain.Exceptions;
using Domain.ViewModels;

namespace TranslatorLogic.Executor
{
    public class Executor : IExecutor 
    {
        private List<OutputIdn> _IDNs;

        private Dictionary<string, int> _integerIDNs;

        private Dictionary<string, long> _floatIDNs;

        private Dictionary<string, long> _additionalCells;

        private Stack<string> _stack;

        private List<string> _RPN;

        private Dictionary<string, int> _marks;

        private string _result;

        private int _idx;

        private string[] _arithmeticOperations = { "+", "*", "/", "^", "-" };

        private string[] _logicalOperations= {"<", ">", "<=", ">=", "==", "!="};

        private HashSet<string> _variablesToInit;

        private ExecutorTable _resultTable;

        public Executor()
        {
            _IDNs = new List<OutputIdn>();
            _integerIDNs = new Dictionary<string, int>();
            _floatIDNs = new Dictionary<string, long>();
            _additionalCells = new Dictionary<string, long>();
            _stack = new Stack<string>();
            _RPN = new List<string>();
            _marks = new Dictionary<string, int>();
            _result = string.Empty;
            _variablesToInit = new HashSet<string>();
            _resultTable = new ExecutorTable();
            _idx = 0;            
        }

        public string Execute(Dictionary<string,int> marks, List<string> RPN, List<OutputIdn> IDNs
            ,Dictionary<string,long> additinalCells)
        {
            _idx = 0;
            _result = string.Empty;
            _RPN = RPN;
            _IDNs = IDNs;
            _marks = marks;
            _additionalCells = additinalCells;
            _integerIDNs = new Dictionary<string, int>();
            _floatIDNs = new Dictionary<string, long>();
            _variablesToInit = new HashSet<string>();
            _resultTable = new ExecutorTable();
            ProceedExecution();
            return _result;
        }

        public string ContinueExecution(Dictionary<string,long> values,int idx)
        {
            foreach(var kv in values)
            {
                if (_integerIDNs.ContainsKey(kv.Key))
                {
                    _integerIDNs[kv.Key] =(int)kv.Value;
                }
                else if (_floatIDNs.ContainsKey(kv.Key))
                {
                    _floatIDNs[kv.Key]=kv.Value;
                }
            }
            _idx = idx;
            ProceedExecution();
            return _result;
        }

        private void ProceedExecution()
        {
            _resultTable.Stack.Add(new List<string>(_stack.Reverse()));
            _resultTable.RPN.Add(_RPN.GetRange(_idx, (_RPN.Count - _idx)));
            while (_idx < _RPN.Count)
            {
                var idn = _IDNs.Find(a => a.Name == _RPN[_idx]);
                if (idn != null)
                {
                    _resultTable.Description.Add("Ідент. в стек");
                    _stack.Push(idn.Name);
                    _idx++;
                }
                else if (_additionalCells.ContainsKey(_RPN[_idx]))
                {
                    _resultTable.Description.Add("Додат. ком. в стек");
                    _stack.Push(_RPN[_idx]);
                    _idx++;
                }
                else if (long.TryParse(_RPN[_idx], out long constant))
                {
                    _resultTable.Description.Add("Конст. в стек");
                    _stack.Push(_RPN[_idx]);
                    _idx++;
                }
                else if (_RPN[_idx] == "IVD")
                {
                    _resultTable.Description.Add("Ог. типу int");
                    IntegerDeclaration();
                    _idx++;
                }
                else if (_RPN[_idx] == "FVD")
                {
                    _resultTable.Description.Add("Ог. типу float");
                    FloatDeclaration();
                    _idx++;
                }
                else if (_arithmeticOperations.Contains(_RPN[_idx]))
                {
                    _resultTable.Description.Add("Викон. арифм. опер." + _RPN[_idx]);
                    ProceedArithmeticOperation(_RPN[_idx]);
                    _idx++;
                }
                else if (_logicalOperations.Contains(_RPN[_idx]))
                {
                    _resultTable.Description.Add("Викон. опер. відн." + _RPN[_idx]);
                    ProceedRelation(_RPN[_idx]);
                    _idx++;
                }
                else if (_RPN[_idx] == "=")
                {
                    _resultTable.Description.Add("Присвоєння значення");
                    var valueOrIdn = _stack.Pop();
                    var variable = _stack.Pop();
                    var value = GetIdnValue(valueOrIdn);
                    SetIdnValue(variable, value);
                    _idx++;
                }
                else if (_RPN[_idx] == "OUT")
                {
                    _resultTable.Description.Add("Виведення");
                    Output();
                    _idx++;
                }
                else if (_RPN[_idx] == "READ")
                {
                    _resultTable.Description.Add("Зчитування");
                    Input();
                    _idx++;
                    var tempList = new HashSet<string>(_variablesToInit);
                    _variablesToInit.Clear();
                    throw new VariablesInitializeException(tempList);
                }
                else if (_RPN[_idx].Contains("CF"))
                {
                    _resultTable.Description.Add("Обробка ум.пер. за хибн.");
                    Conditional(_RPN[_idx]);
                }
                else if (_RPN[_idx].Contains("NC"))
                {
                    _resultTable.Description.Add("Обробка без ум. пер.");
                    NoConditional(_RPN[_idx]);
                }
                else if (_marks.ContainsKey(_RPN[_idx].Remove(_RPN[_idx].Length - 1)))
                {
                    _resultTable.Description.Add("Пропуск огол. мітки");
                    _idx++;
                }

                _resultTable.Stack.Add(new List<string>(_stack.Reverse()));
                _resultTable.RPN.Add(_RPN.GetRange(_idx, (_RPN.Count - _idx)));
            }
            _resultTable.Description.Add("Кінець");
        }

        private void Output()
        {
            while (_stack.TryPop(out string idn))
            {
                if (_integerIDNs.ContainsKey(idn))
                {
                    _result += _integerIDNs[idn] + " ;";
                }
                else if (_floatIDNs.ContainsKey(idn))
                {
                    _result += _floatIDNs[idn] + " ;";
                }
            }
        }

        private void Input()
        {
            while (_stack.TryPop(out string idn))
            {
                if (_integerIDNs.ContainsKey(idn) || _floatIDNs.ContainsKey(idn))
                {
                    _variablesToInit.Add(idn);
                }                
            }
        }

        private void IntegerDeclaration()
        {
            while (_stack.TryPop(out string idn))
            {
                _integerIDNs.Add(idn, 1234567);                
            }
          
        }

        private void FloatDeclaration()
        {
            while (_stack.TryPop(out string idn))
            {
                _floatIDNs.Add(idn, 12345678);
            }
          
        }

        private void Conditional(string command)
        {            
            var relationResult = bool.Parse(_stack.Pop());
            if (!relationResult)
            {
                _idx = GetMarkValue(command);
            }
            else
            {
                _idx=_idx+1;
            }
        }

        private void NoConditional(string command)
        {
            _idx = GetMarkValue(command);
        }

        private int GetMarkValue(string command)
        {
            var mark = string.Empty;
            if (command.Contains("CF"))
            {
                mark = command.Substring(0, command.IndexOf("CF"));
            }
            else if (command.Contains("NC"))
            {
                mark = command.Substring(0, command.IndexOf("NC"));
            }            
            var markValue = _marks[mark];
            return markValue;
        }

        private void ProceedArithmeticOperation(string operationSign)
        {
            long result;
            switch (operationSign)
            {
                case "+":
                    result = Add();
                    break;
                case "-":
                    result = Subtract();
                    break;
                case "*":
                    result = Multiply();
                    break;
                case "/":
                    result = Divide();
                    break;
                case "^":
                    result = Power();
                    break;
                default:
                    result = 12345679;
                    break;
            }
            _stack.Push(result.ToString());
        }

        private void ProceedRelation(string relationSign)
        {
            bool result;
            switch (relationSign)
            {
                case ">":
                    result = More();
                    break;
                case ">=":
                    result = MoreOrEqual();
                    break;
                case "<":
                    result = Less();
                    break;
                case "<=":
                    result = LessOrEqual();
                    break;
                case "==":
                    result = Equal();
                    break;
                case "!=":
                    result = NotEqual();
                    break;
                default:
                    result = false;
                    break;
            }
            _stack.Push(result.ToString());
        }

        private long Add()
        {
            ExtractValuesForBinaryOperation(out long val1, out long val2);
            return val1 + val2;
        }

        private long Multiply()
        {
            ExtractValuesForBinaryOperation(out long val1, out long val2);
            return val1 * val2;
        }

        private long Divide()
        {
            ExtractValuesForBinaryOperation(out long val1, out long val2);
            return val2 / val1;
        }

        private long Subtract()
        {
            ExtractValuesForBinaryOperation(out long val1, out long val2);
            return val2 - val1;
        }

        private long Power()
        {
            ExtractValuesForBinaryOperation(out long val1, out long val2);
            return (long)Math.Pow(val2, val1);
        }

        private bool More()
        {
            ExtractValuesForBinaryOperation(out long val1, out long val2);
            return val2 > val1;
        }

        private bool MoreOrEqual()
        {
            ExtractValuesForBinaryOperation(out long val1, out long val2);
            return val2 >= val1;
        }

        private bool Less()
        {
            ExtractValuesForBinaryOperation(out long val1, out long val2);
            return val2 < val1;
        }

        private bool LessOrEqual()
        {
            ExtractValuesForBinaryOperation(out long val1, out long val2);
            return val2 <= val1;
        }

        private bool Equal()
        {
            ExtractValuesForBinaryOperation(out long val1, out long val2);
            return val2 == val1;
        }

        private bool NotEqual()
        {
            ExtractValuesForBinaryOperation(out long val1, out long val2);
            return val2 != val1;
        }

        private void ExtractValuesForBinaryOperation(out long val1,out long val2 )
        {
            var idnName1 = _stack.Pop();
            var idnName2 = _stack.Pop();
            if (_integerIDNs.ContainsKey(idnName1))
            {
                val1 = _integerIDNs[idnName1];
            }
            else if (_floatIDNs.ContainsKey(idnName1))
            {
                val1 = _floatIDNs[idnName1];
            }
            else if (_additionalCells.ContainsKey(idnName1))
            {
                val1 = _additionalCells[idnName1];
            }
            else
            {
                val1 = long.Parse(idnName1);
            }

            if (_integerIDNs.ContainsKey(idnName2))
            {
                val2 = _integerIDNs[idnName2];
            }
            else if (_floatIDNs.ContainsKey(idnName2))
            {
                val2 = _floatIDNs[idnName2];
            }
            else if (_additionalCells.ContainsKey(idnName2))
            {
                val2 = _additionalCells[idnName2];
            }
            else
            {
                val2 = long.Parse(idnName2);
            }
        }

        private long GetIdnValue(string idn)
        {
            long result;
            if (_integerIDNs.ContainsKey(idn))
            {
                result = _integerIDNs[idn];
            }
            else if (_floatIDNs.ContainsKey(idn))
            {
                result = _floatIDNs[idn];
            }
            else if (_additionalCells.ContainsKey(idn))
            {
                result = _additionalCells[idn];
            }
            else
            {
                result = long.Parse(idn);
            }
            return result;
        }

        private void SetIdnValue(string idn, long value)
        {
            if(_integerIDNs.ContainsKey(idn))
            {
                _integerIDNs[idn] = (int)value;
            }
            else if (_floatIDNs.ContainsKey(idn))
            {
                 _floatIDNs[idn] = value;
            }
            else if (_additionalCells.ContainsKey(idn))
            {
                _additionalCells[idn] = value;
            }
        }

        public int GetLastIdx()
        {
            return _idx;
        }

        public ExecutorTable GetResultTable()
        {
            return _resultTable;
        }
    }
}
