using Domain.Translator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace TranslatorLogic.RelationManager
{
    /*Будується матриця відношень FIRST+
 помножити матрицю EQ на F. Таким чином, якщо є рівність R=S,  S-нетермінал
то встановлюється відношення < між R  і множиною FIRST+(S)
 Будується матриця відношень LAST+
 Якщо є відношення R=S і R — нетермінал, то встановлюємо відношення
LAST+ (R) > S. Якщо S — нетермінал, то встановлюємо відношення LAST+(R) >FIRST+(S). 
Встановлюємо відношення передування з граничним символом #, і
остаточно отримаємо матрицю відношень передування */

    public class RelationManager : IRelationManager
    {
        private Dictionary<string, List<List<string>>> _grammar = new Dictionary<string, List<List<string>>>();

        private Dictionary<string, List<string>> _lastPlus = new Dictionary<string, List<string>>();

        private Dictionary<string, List<string>> _firstPlus = new Dictionary<string, List<string>>();

        private HashSet<string> _allTerms = new HashSet<string>();

        private HashSet<string> _unTerm = new HashSet<string>();

        private string[,] _relationMatrix;

        private string _grammarText;

        private bool isOkay;

        public void SetRelations(string grammarText)
        {
            isOkay = true;

            if (!string.IsNullOrEmpty(grammarText))
            {
                _grammarText = grammarText;

                GetGrammarAndTerms();

                SetEqualyRelation();

                GetFirstPlus();

                GetLastPlus();

                SetLessRelation();

                SetMoreRelation();

                SetSharpRelation();
            }
            else
            {
                var error = "No grammar text";
                throw new ArgumentException(error);
            }
        }

        private void GetGrammarAndTerms()
        {
            string[] rows = _grammarText.Split('\n');

            for (int i = 0; i < rows.Count(); i++)
                if (rows[i] != "")
                    rows[i] = rows[i].Trim();

            foreach (var row in rows)
            {
                if (row != "")
                {
                    string leftPart = GetLeftPart(row);
                    string rightPart = GetRightPart(row);

                    if (leftPart== "<сп.оп>")
                    {

                    }

                    _allTerms.Add(leftPart);
                    _unTerm.Add(leftPart);

                    _grammar[leftPart] = new List<List<string>>();

                    List<string> termList;

                    string[] rParts = rightPart.Split('|');

                    foreach (var rPart in rParts)
                    {
                        string[] terms = rPart.Split(' ');
                        termList = terms.ToList();
                        _grammar[leftPart].Add(termList);

                        foreach (string term in terms)
                        {
                            if (term == "<сп.оп>")
                            {

                            }
                            _allTerms.Add(term);
                        }
                    }
                }
            }
        }

        private string GetLeftPart(string input)
        {
            int index = input.IndexOf("::=");
            return input.Substring(0, index);
        }

        private string GetRightPart(string input)
        {
            int index = input.IndexOf("::=");
            return input.Substring(index + 3);
        }

        private void GetFirstPlus()
        {
            foreach (string unTerm in _unTerm)
            {
                HashSet<string> tempFirstPlus = new HashSet<string>();
                List<List<string>> rightParts = _grammar[unTerm];
                HashSet<string> tempUnTerm = new HashSet<string>();

                foreach (var rPart in rightParts)
                {
                    string rightFirst = rPart.First();
                    tempFirstPlus.Add(rightFirst);

                    if (_unTerm.Contains(rightFirst))
                        tempUnTerm.Add(rightFirst);
                }

                /*знаходимо First+ для нетерміналів у правилі*/
                int iter = 0;

                while (iter < tempUnTerm.Count)
                {
                    string currentUnTerm = tempUnTerm.ElementAt(iter);
                    rightParts = _grammar[currentUnTerm];

                    foreach (var rPart in rightParts)
                    {
                        string rightFirst = rPart.First();
                        tempFirstPlus.Add(rightFirst);
                        if (_unTerm.Contains(rightFirst))
                            tempUnTerm.Add(rightFirst);
                    }

                    iter++;
                }
                _firstPlus[unTerm] = new List<string>();
                _firstPlus[unTerm].AddRange(tempFirstPlus);
            }
        }
        public void GetLastPlus()
        {
            foreach (string unTerm in _unTerm)
            {
                HashSet<string> tempLastPlus = new HashSet<string>();
                List<List<string>> rightParts = _grammar[unTerm];
                HashSet<string> tempUnTerm = new HashSet<string>();

                foreach (var rPart in rightParts)
                {
                    string rightLast = rPart.Last();
                    tempLastPlus.Add(rightLast);
                    if (_unTerm.Contains(rightLast))
                        tempUnTerm.Add(rightLast);
                }

                /*знаходимо Last+ для нетерміналів у правилі*/
                int iter = 0;

                while (iter < tempUnTerm.Count)
                {
                    string currentUnTerm = tempUnTerm.ElementAt(iter);
                    rightParts = _grammar[currentUnTerm];

                    foreach (var rPart in rightParts)
                    {
                        string rightLast = rPart.Last();
                        tempLastPlus.Add(rightLast);
                        if (_unTerm.Contains(rightLast))
                            tempUnTerm.Add(rightLast);
                    }

                    iter++;
                }
                _lastPlus[unTerm] = new List<string>();
                _lastPlus[unTerm].AddRange(tempLastPlus);
            }
        }

        private void SetEqualyRelation()
        {
            _relationMatrix = GetEmptyMatrix(_allTerms.Count);

            List<string> tempList = _allTerms.ToList();

            foreach (var unTerm in _grammar)
            {
                for (int i = 0; i < unTerm.Value.Count; i++)
                {
                    for (int j = 0; j < unTerm.Value[i].Count - 1; j++)
                    {
                        int indexI = tempList.IndexOf(unTerm.Value[i][j]);
                        int indexJ = tempList.IndexOf(unTerm.Value[i][j + 1]);
                        _relationMatrix[indexI, indexJ] = "=";
                    }
                }
            }
        }

        private void SetLessRelation()
        {
            List<string> allTermsList = _allTerms.ToList();

            foreach (var term in _allTerms)
            {
                int indexI = allTermsList.IndexOf(term);

                for (int j = 0; j < allTermsList.Count; j++)
                {
                    if (_relationMatrix[indexI, j] == "=" && _unTerm.Contains(allTermsList[j]))
                    {
                        string unTerm = allTermsList[j];

                        for (int k = 0; k < _firstPlus[unTerm].Count; k++)
                        {
                            int indexJ = allTermsList.IndexOf(_firstPlus[unTerm][k]);
                            if (_relationMatrix[indexI, indexJ] != "<")
                            {
                                _relationMatrix[indexI, indexJ] += "<";
                                if(_relationMatrix[indexI, indexJ].Length>1)
                                {
                                    isOkay = false;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void SetMoreRelation()
        {
            List<string> allTermsList = _allTerms.ToList();

            foreach (string unTerm in _unTerm)
            {
                int indexI = allTermsList.IndexOf(unTerm);

                for (int j = 0; j < allTermsList.Count; j++)
                {
                    if (_relationMatrix[indexI, j] == "=")
                    {
                        string S = allTermsList[j];
                        List<string> lastPlusR = _lastPlus[unTerm];

                        InsertMoreRelationInMatrix(lastPlusR, S);

                        if (_unTerm.Contains(S))
                        {
                            List<string> firstPlusS = _firstPlus[S];
                            InsertMoreRelationInMatrix(lastPlusR, firstPlusS);
                        }
                    }
                }
            }
        }

        private void InsertMoreRelationInMatrix(List<string> lastPlusR, List<string> firstPlusS)
        {
            List<string> allTermsList = _allTerms.ToList();

            foreach (string lastPlusRItem in lastPlusR)
            {
                int indexI = allTermsList.IndexOf(lastPlusRItem);

                foreach (string firstPlusSItem in firstPlusS)
                {
                    int indexJ = allTermsList.IndexOf(firstPlusSItem);
                    if (_relationMatrix[indexI, indexJ] != ">")
                    {
                        _relationMatrix[indexI, indexJ] += ">";
                        if (_relationMatrix[indexI, indexJ].Length > 1)
                        {
                            isOkay = false;
                        }
                    }
                }
            }
        }
        private void InsertMoreRelationInMatrix(List<string> lastPlusR, string S)
        {
            List<string> allTermsList = _allTerms.ToList();
            int indexJ = allTermsList.IndexOf(S);

            foreach (string lastPlusRItem in lastPlusR)
            {
                int indexI = allTermsList.IndexOf(lastPlusRItem);
                if (_relationMatrix[indexI, indexJ] != ">")
                {
                    _relationMatrix[indexI, indexJ] += ">";
                    if (_relationMatrix[indexI, indexJ].Length > 1)
                    {
                        isOkay = false;
                    }
                }
            }
        }

        public string[,] GetRelationMatrix() => _relationMatrix;

        public List<string> GetAllTerms() => _allTerms.ToList();

        private void SetSharpRelation()
        {
            _allTerms.Add("#");
            string[,] result = GetEmptyMatrix(_allTerms.Count);

            for (int i = 0; i < _allTerms.Count - 1; i++)
            {
                for (int j = 0; j < _allTerms.Count - 1; j++)
                    result[i, j] = _relationMatrix[i, j];

                result[i, _allTerms.Count - 1] = ">";
                result[_allTerms.Count - 1, i] = "<";
            }
            _relationMatrix = result;
        }

        public Dictionary<string, List<List<string>>> GetGrammar()
        {
            return _grammar;
        }

        private string[,] GetEmptyMatrix(int size)
        {
            string[,] result = new string[size, size];

            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    result[i, j] = "";
            return result;
        }

        public bool IsGrammarCorrect()
        {
            return isOkay;
        }

      
    }
}
