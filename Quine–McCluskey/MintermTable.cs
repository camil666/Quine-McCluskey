using System.Collections.Generic;
using System.Linq;

namespace Quine_McCluskey
{
    public class MintermTable
    {
        #region Fields

        private List<int> _numerals = new List<int>();
        private List<Minterm> _primeImplicants = new List<Minterm>();
        private List<List<bool>> _table;

        #endregion

        #region Constructors

        public MintermTable(IEnumerable<Minterm> primeImplicants)
        {
            _numerals.AddRange(primeImplicants.SelectMany(item => item.Numerals).Where(item => !item.IsFi).Select(item => item.Number).Distinct().OrderBy(numeral => numeral));
            _primeImplicants.AddRange(primeImplicants);

            _table = new List<List<bool>>(_primeImplicants.Count);

            for (int i = 0; i < _primeImplicants.Count; i++)
            {
                _table.Add(new List<bool>());

                for (int j = 0; j < _numerals.Count; ++j)
                    _table[i].Add(_primeImplicants[i].Numerals.Where(item => item.Number == _numerals[j]).Count() > 0 ? true : false);
            }
        }

        #endregion

        #region Methods

        private double GetS(int index)
        {
            double result = 0;
            for (int columnIndex = 0; columnIndex < _numerals.Count; ++columnIndex)
            {
                if (_table[index][columnIndex])
                    result += 1.0 / GetP(columnIndex);
            }

            return result;
        }

        private int GetSBis(int index)
        {
            return _primeImplicants[index].Numerals.Count();
        }

        private int GetP(int index)
        {
            int result = 0;
            for (int i = 0; i < _table.Count(); ++i)
            {
                result += _table[i][index] == true ? 1 : 0;
            }

            return result;
        }

        private void RemoveColumnWithValue(int value)
        {
            int index = _numerals.IndexOf(value);
            if (index < 0)
                return;

            for (int i = 0; i < _table.Count(); ++i)
                _table[i].RemoveAt(_numerals.IndexOf(value));

            _numerals.Remove(value);
        }

        private void RemoveRow(int index)
        {
            _table.RemoveAt(index);
            _primeImplicants.RemoveAt(index);
        }

        private IEnumerable<Minterm> RemoveEmptyColumns()
        {
            for (int columnIndex = 0; columnIndex < _numerals.Count; ++columnIndex)
            {
                if (GetP(columnIndex) == 1)
                {
                    List<bool> column = _table.Select(item => item[columnIndex]).ToList();

                    Minterm selectedMinterm = _primeImplicants[column.IndexOf(true)];

                    foreach (var numeral in selectedMinterm.Numerals)
                        RemoveColumnWithValue(numeral.Number);

                    RemoveRow(_primeImplicants.IndexOf(selectedMinterm));

                    //zaczynamy od nowa
                    columnIndex = -1;

                    yield return selectedMinterm;
                }
            }
        }

        private void DeleteWorstRow()
        {
            var columnIndexes = new List<int>();
            int pMinimum = int.MaxValue;
            for (int columnIndex = 0; columnIndex < _numerals.Count(); ++columnIndex)
            {
                int p = GetP(columnIndex);
                if (p < pMinimum)
                {
                    pMinimum = p;
                    columnIndexes.Clear();
                    columnIndexes.Add(columnIndex);
                }
                else if (p == pMinimum)
                {
                    columnIndexes.Add(columnIndex);
                }
            }

            double sMinimum = double.MaxValue;
            var rowIndexes = new List<int>();
            for (int rowIndex = 0; rowIndex < _primeImplicants.Count(); ++rowIndex)
            {
                for (int columnIndex = 0; columnIndex < _numerals.Count(); ++columnIndex)
                {
                    if (columnIndexes.Contains(columnIndex) && _table[rowIndex][columnIndex])
                    {
                        double s = GetS(rowIndex);
                        if (s < sMinimum)
                        {
                            sMinimum = s;
                            rowIndexes.Clear();
                            rowIndexes.Add(rowIndex);
                        }
                        else if (s == sMinimum)
                        {
                            rowIndexes.Add(rowIndex);
                        }

                        break;
                    }
                }
            }

            if (rowIndexes.Count > 1)
            {
                var rowBisIndexes = new List<int>();
                int sBisMinimum = int.MaxValue;
                for (int rowIndex = 0; rowIndex < _primeImplicants.Count(); ++rowIndex)
                {
                    if (rowIndexes.Contains(rowIndex))
                    {
                        int sBis = GetSBis(rowIndex);
                        if (sBis < sBisMinimum)
                        {
                            sBisMinimum = sBis;
                            rowBisIndexes.Clear();
                            rowBisIndexes.Add(rowIndex);
                        }
                        else if (sBis == sBisMinimum)
                        {
                            rowBisIndexes.Add(rowIndex);
                        }

                        break;
                    }
                }

                RemoveRow(rowBisIndexes.First());
                return;
            }

            RemoveRow(rowIndexes.First());
        }

        /// <summary>
        /// Reduces minterms.
        /// </summary>
        /// <returns>The result of the reduction.</returns>
        public IEnumerable<Minterm> Reduce()
        {
            var results = new List<Minterm>();

            do
            {
                results.AddRange(RemoveEmptyColumns());
                if (_numerals.Any())
                    DeleteWorstRow();
            } while (_numerals.Any());


            return results;
        }

        #endregion
    }
}
