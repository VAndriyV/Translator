using Domain.Entities;
using Domain.Translator;
using Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TranslatorLogic.ReversePolishNotation
{
    public class RPNExpressionCalculator : IRPNExpressionCalculator
    {
        private List<string> _RPN;

        private Dictionary<string, int> _variables;

        private List<OutputIdn> _IDNs;

        private Stack<double> _stack;

        private List<RPNCalculatorTable> _resultTable;    
        
        private List<string> _operations;

        public RPNExpressionCalculator()
        {
            _RPN = new List<string>();
            _variables = new Dictionary<string, int>();
            _IDNs = new List<OutputIdn>();
            _stack = new Stack<double>();
            _operations = new List<string>() { "+", "*", "/", "^", "-" };
            _resultTable = new List<RPNCalculatorTable>();
        }

        public double Calculate(List<string> rpn, Dictionary<string, int> variables, List<OutputIdn> idns)
        {
            _RPN = new List<string>(rpn);
            _variables = new Dictionary<string, int>(variables);
            _IDNs = new List<OutputIdn>(idns);         

            while (_RPN.Count > 0)
            {
                _resultTable.Add(GetTableItem());
                var obj = _IDNs.Find(o => o.Name == _RPN[0]);
                if (obj != null)
                {
                    
                    _stack.Push(_variables[obj.Name]);
                    _RPN.RemoveAt(0);
                }
                else if (!_operations.Contains(_RPN[0]))
                {
                    double res;
                    if (double.TryParse(_RPN[0], out res))
                    {
                        _stack.Push((double)res);
                        _RPN.RemoveAt(0);
                    }
                    else { throw new Exception("невірна константа"); }
                }
                else
                {                   
                    if (_RPN[0] == "+")
                    {
                        double val1 = _stack.Pop();
                        double val2 = _stack.Pop();
                        _stack.Push(val1 + val2);
                        _RPN.RemoveAt(0);
                    }
                    else if (_RPN[0] == "-")
                    {
                        double val1 = _stack.Pop();
                        double val2 = _stack.Pop();
                        _stack.Push(val2 - val1);
                        _RPN.RemoveAt(0);
                    }
                    else if (_RPN[0] == "*")
                    {
                        double val1 = _stack.Pop();
                        double val2 = _stack.Pop();
                        _stack.Push(val1 * val2);
                        _RPN.RemoveAt(0);
                    }
                    else if (_RPN[0] == "/")
                    {
                        double val1 = _stack.Pop();
                        double val2 = _stack.Pop();
                        _stack.Push(val2 / val1);
                        _RPN.RemoveAt(0);
                    }
                    else if (_RPN[0] == "^")
                    {
                        double val1 = _stack.Pop();
                        double val2 = _stack.Pop();
                        _stack.Push(Math.Pow(val2,val1));
                        _RPN.RemoveAt(0);
                    }
                }
            }
            _resultTable.Add(GetTableItem());

            return _stack.Pop();
        }

        private RPNCalculatorTable GetTableItem()
        {
            List<string> stack = new List<string>();
            for (int i = _stack.Count - 1; i >= 0; i--)
            {
                stack.Add(_stack.ElementAt(i).ToString());
            }

            List<string> rpn = new List<string>();

            for (int i = 0; i < _RPN.Count; i++)
            {
                rpn.Add(_RPN[i]);
            }
            return new RPNCalculatorTable {Stack = stack,RPN= rpn };
        }

        public List<RPNCalculatorTable> GetResultTable()
        {
            return _resultTable;
        }
    }
}
