using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Domain.Translator;
using Domain.ViewModels;

namespace TranslatorLogic.Hash
{
    public class HashManager : IHashManager
    {
        private HashTable _linearHashingTable;

        private HashTable _squareHashingTable;

        private int _linearHashingIndex;

        private int _squareHashingIndex;

        private int _tableSize;

        private List<double> _linearHashingAverageTries;

        private List<double> _squareHashingAverageTries;

        private List<double> _additionalLinearHashingAverageTries;

        private List<double> _additionalSquareHashingAverageTries;

        private List<string> _variables;

        private List<string> _additionalVariables;

        private HashingResultTable _resultTable;

        public HashManager()
        {
            _linearHashingIndex = 0;
            _squareHashingIndex = 0;
            _tableSize = 1;
            _linearHashingTable = new HashTable();
            _squareHashingTable = new HashTable();
            _linearHashingTable.Rows = new List<HashRecord>();
            _squareHashingTable.Rows = new List<HashRecord>();
            _linearHashingAverageTries = new List<double>();
            _squareHashingAverageTries = new List<double>();
            _additionalLinearHashingAverageTries = new List<double>();
            _additionalSquareHashingAverageTries = new List<double>();
            _variables = new List<string>();
            _additionalVariables = new List<string>();
            _resultTable = new HashingResultTable();
        }

        public async Task PerformHashing(int tableSize)
        {
            _linearHashingIndex = 0;
            _squareHashingIndex = 0;
            _tableSize = tableSize;
            _linearHashingTable.Rows = new List<HashRecord>(tableSize);
            _squareHashingTable.Rows = new List<HashRecord>(tableSize);
            _linearHashingAverageTries = new List<double>();
            _squareHashingAverageTries = new List<double>();
            GenerateVariables(_tableSize);
           
            await FillTables(10);           
            await FillTables(50);            
            await FillTables(90);

            _resultTable.FakeLinearStatistic = _additionalLinearHashingAverageTries;
            _resultTable.FakeSquareStatistic = _additionalSquareHashingAverageTries;
            _resultTable.LinearStatistic = _linearHashingAverageTries;
            _resultTable.SquareStatistic = _squareHashingAverageTries;
        }

        public async Task FillTables(int percent)
        {
            int elementsToFill = (int)Math.Ceiling((double)percent / 100 * _tableSize);
           
            _linearHashingTable.Rows.Clear();
            await Task.Run(() => FillLinearHashTable(elementsToFill));
            await Task.Run(() => FakeFillLinearHashTable(_additionalVariables.Count));
            _squareHashingTable.Rows.Clear();
            await Task.Run(() => FillSquareHashTable(elementsToFill));
            await Task.Run(() => FakeFillSquareHashTable(_additionalVariables.Count));
        }

        private void FillLinearHashTable(int elementsToFill)
        {
            double tries = 0;
            for (int i = 0; i < elementsToFill; i++)
            {
                _linearHashingIndex = 1;
                var count = 0;
                while (true)
                {
                    count++;
                    var idx = LinearHash(_variables[i]);
                    if (_linearHashingTable.Rows.Any(p => p.Index == idx))
                    {
                        _linearHashingIndex++;
                    }
                    else
                    {
                        var hashRecord = new HashRecord()
                        {
                            Index = idx,
                            VariableName = _variables[i],
                            Value = _variables[i].GetHashCode().ToString()
                        };
                        _linearHashingTable.Rows.Add(hashRecord);

                        break;
                    }
                }
                tries += count;
            }
            _linearHashingAverageTries.Add(tries / elementsToFill);
        }

        private void FillSquareHashTable(int elementsToFill)
        {
            double tries = 0;
            for (int i =0; i < elementsToFill; i++)
            {
                _squareHashingIndex = 1;
                var count = 0;
                while (true)
                {
                    count++;
                    var idx = SquareHash(_variables[i]);
                    if (_squareHashingTable.Rows.Any(p => p.Index == idx))
                    {
                        _squareHashingIndex++;
                    }
                    else
                    {
                        var hashRecord = new HashRecord()
                        {
                            Index = idx,
                            VariableName = _variables[i],
                            Value = _variables[i].GetHashCode().ToString()
                        };
                        _squareHashingTable.Rows.Add(hashRecord);
                        break;
                    }
                }
                tries += count;
            }
            _squareHashingAverageTries.Add(tries / elementsToFill);
        }

        private void FakeFillLinearHashTable(int elementsToFill)
        {
            double tries = 0;            
            for (int i = 0; i < _additionalVariables.Count; i++)
            {
                _linearHashingIndex = 1;
                var count = 0;
                while (true)
                {
                    count++;
                    var idx = LinearHash(_additionalVariables[i]);
                    if (_linearHashingTable.Rows.Any(p => p.Index == idx))
                    {
                        _linearHashingIndex++;
                    }
                    else
                    {                       
                        break;
                    }
                }
                tries += count;
            }
            _additionalLinearHashingAverageTries.Add(tries / _additionalVariables.Count);
        }

        private void FakeFillSquareHashTable(int elementsToFill)
        {
            double tries = 0;
            for (int i = 0; i < _additionalVariables.Count; i++)
            {
                _squareHashingIndex = 1;
                var count = 0;
                while (true)
                {
                    count++;
                    var idx = SquareHash(_additionalVariables[i]);
                    if (_squareHashingTable.Rows.Any(p => p.Index == idx))
                    {
                        _squareHashingIndex++;
                    }
                    else
                    {                    
                        break;
                    }
                }
                tries += count;
            }
            _additionalSquareHashingAverageTries.Add(tries / _additionalVariables.Count);
        }

        private int PrimaryFunction(string variable)
        {
            int sum = 0;
            foreach (char c in variable)
            {
                if (char.IsDigit(c))
                {
                    sum += 100 + c;
                }
                else
                {
                    sum += char.ToUpper(c) - 64;
                }
            }

            return sum % _tableSize;
        }

        private int LinearHash(string variable)
        {
            return (PrimaryFunction(variable) + _linearHashingIndex) % _tableSize;
        }

        private int SquareHash(string variable)
        {
            int a = -1;
            int b = 3;
            int c = 56;

            return (PrimaryFunction(variable) + a * _squareHashingIndex * _squareHashingIndex
                + b * _squareHashingIndex + c) % _tableSize;
        }

        private void GenerateVariables(int count)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";           
            var random = new Random();
            var stringChars = new char[random.Next(10)+1];
            var variables = new HashSet<string>(count);
            while(variables.Count<count+100)
            {
                for (int i = 0; i < stringChars.Length; i++)
                {
                    stringChars[i] = chars[random.Next(chars.Length)];
                }

                var finalString = new string(stringChars);
                variables.Add(finalString);                
            }

            var variablesList = variables.ToList();
            _variables = variablesList.GetRange(0, count);
            _additionalVariables = variablesList.GetRange(count, variablesList.Count - count);
        }

        public HashingResultTable GetResultTable()
        {
            return _resultTable;
        }
    }
}
