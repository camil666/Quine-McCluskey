using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quine_McCluskey
{
    public class MintermTable
    {
        #region Fields

        List<int> _numerals = new List<int>();
        List<Minterm> _primeImplicants = new List<Minterm>();
        List<List<bool>> _table;

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

        private int GetColumnCoverage(int index)
        {
            int result = 0;
            for (int i = 0; i < _table.Count(); ++i)
            {
                result += _table[i][index] == true ? 1 : 0;
            }

            return result;
        }

        private void RemoveColumn(int index)
        {
            for (int i = 0; i < _table.Count(); ++i)
                _table[i].RemoveAt(index);

            _numerals.RemoveAt(index);
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
                if (GetColumnCoverage(columnIndex) == 1)
                {
                    Minterm selectedMinterm = _primeImplicants
                        .Where(item => item.Numerals.Where(numeral => numeral.Number == _numerals[columnIndex]).Count() > 0).FirstOrDefault();

                    foreach (var numeral in selectedMinterm.Numerals)
                        RemoveColumnWithValue(numeral.Number);

                    //zaczynamy od nowa
                    columnIndex = -1;

                    yield return selectedMinterm;
                }
            }
        }

        private int ReduceRows()
        {
            int rowsReduced = 0;

            for (int rowIndex = 0; rowIndex < _primeImplicants.Count; ++rowIndex) //remove empty rows
            {
                if (_table[rowIndex].All(item => !item))    //if row is empty
                {
                    RemoveRow(rowIndex);
                    rowIndex = rowIndex == 0 ? 0 : rowIndex - 1;
                    ++rowsReduced;
                }
            }

            for (int rowIndex = 0; rowIndex < _primeImplicants.Count; ++rowIndex) //remove same rows
            {
                for (int rowToCheckIndex = 0; rowToCheckIndex < _primeImplicants.Count; ++rowToCheckIndex)
                {
                    if (rowIndex == rowToCheckIndex)
                        continue;

                    bool rowCanBeDeleted = true;

                    for (int bitIndex = 0; bitIndex < _numerals.Count; ++bitIndex)
                    {
                        if (!_table[rowIndex][bitIndex] && _table[rowIndex][bitIndex] != _table[rowToCheckIndex][bitIndex])
                        {
                            rowCanBeDeleted = false;
                        }
                    }

                    if (rowCanBeDeleted)
                    {
                        RemoveRow(rowToCheckIndex);
                        rowIndex = rowIndex == 0 ? 0 : rowIndex - 1;
                        rowToCheckIndex = rowToCheckIndex == 0 ? 0 : rowToCheckIndex - 1;
                        ++rowsReduced;
                    }
                }
            }

            return rowsReduced;
        }

        private int ReduceColumns()
        {
            int columnsReduced = 0;

            for (int columnIndex = 0; columnIndex < _numerals.Count; ++columnIndex)
            {
                List<bool> column = _table.Select(item => item[columnIndex]).ToList();

                for (int columnToCheckIndex = 0; columnToCheckIndex < _numerals.Count; ++columnToCheckIndex)
                {
                    if (columnIndex == columnToCheckIndex)
                        continue;

                    bool columnCanBeDeleted = true;

                    List<bool> columnToCheck = _table.Select(item => item[columnToCheckIndex]).ToList();

                    for (int bitIndex = 0; bitIndex < _primeImplicants.Count; ++bitIndex)
                    {
                        if (!column[bitIndex] && column[bitIndex] != columnToCheck[bitIndex])
                        {
                            columnCanBeDeleted = false;
                        }
                    }

                    if (columnCanBeDeleted)
                    {
                        RemoveColumn(columnToCheckIndex);
                        ++columnsReduced;
                        columnIndex = columnIndex == 0 ? 0 : columnIndex - 1;
                        columnToCheckIndex = columnToCheckIndex == 0 ? 0 : columnToCheckIndex - 1;
                    }
                }
            }

            return columnsReduced;
        }

        public IEnumerable<Minterm> Reduce()
        {
            var results = new List<Minterm>();

            int rowsReduced = 0;
            int columnsReduced = 0;

            do
            {
                results.AddRange(RemoveEmptyColumns());
                rowsReduced = ReduceRows();
                columnsReduced = ReduceColumns();
            } while (rowsReduced != 0 || columnsReduced != 0);


            return results;
        }

        #endregion
    }
}
