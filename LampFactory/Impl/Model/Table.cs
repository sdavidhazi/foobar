using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Impl.Extensions;

namespace Impl.Model
{
    public class Table
    {
        private readonly byte[,] _table;
        // TODO: Try to cache wall positions to speed up SetupLampBesideWall method
        // TODO: Use a counter to track number of free places --> IsReady is simplified

        public int Length;

        private Table(Table table)
        {
            Length = table.Length;
            Invalid = table.Invalid;
            _table = (byte[,])table._table.Clone();
        }

        public Table(string rawTable)
        {
            rawTable = rawTable.Replace("\r", "");
            var lines = rawTable.Split('\n');
            Length = lines.Length;
            _table = new byte[Length, Length];

            // TODO: Try to use multiple tasks to setup the initial model
            for (var row = 0; row < Length; row++)
            {
                for (var column = 0; column < Length; column++)
                {
                    _table[row, column] = lines[row][column].Map();
                }
            }
        }

        public Table(int length)
        {
            Length = length;
            _table = new byte[Length, Length];
            for (var row = 0; row < Length; row++)
            {
                for (var column = 0; column < Length; column++)
                {
                    _table[row, column] = TableMapping.Free;
                }
            }
        }

        public bool Invalid { get; set; }

        public byte this[int row, int column]
        {
            get { return _table[row, column]; }
            set { _table[row, column] = value; }
        }

        public Table Clone()
        {
            return new Table(this);
        }

        public void SetupLamp(int row, int column)
        {
            if (_table[row, column] != TableMapping.Free)
            {
                throw new InvalidOperationException("Setting up a lamp is not allowed since the cell is already used.");
            }

            _table[row, column] = TableMapping.Lamp;
            Light(row, column);
            SetupLightBesideWalls();
        }

        public void SetupLightBesideWalls()
        {
            var missingLamp = false;
            for (var row = 0; row < Length; row++)
            {
                for (var col = 0; col < Length; col++)
                {
                    if (_table[row, col] > TableMapping.Wall4)
                        continue;

                    var lampCount = 0;
                    var freeCount = 0;

                    var freeRow = -1;
                    var freeCol = -1;

                    if (row > 0)
                    {
                        if (_table[row - 1, col] == TableMapping.Lamp)
                            lampCount++;
                        if (_table[row - 1, col] == TableMapping.Free)
                        {
                            freeCount++;
                            freeRow = row - 1;
                            freeCol = col;
                        }
                    }
                    if (row < Length - 1)
                    {
                        if (_table[row + 1, col] == TableMapping.Lamp)
                            lampCount++;
                        if (_table[row + 1, col] == TableMapping.Free)
                        {
                            freeCount++;
                            freeRow = row + 1;
                            freeCol = col;
                        }
                    }

                    if (col > 0)
                    {
                        if (_table[row, col - 1] == TableMapping.Lamp)
                            lampCount++;
                        if (_table[row, col - 1] == TableMapping.Free)
                        {
                            freeCount++;
                            freeRow = row;
                            freeCol = col - 1;
                        }
                    }
                    if (col < Length - 1)
                    {
                        if (_table[row, col + 1] == TableMapping.Lamp)
                            lampCount++;
                        if (_table[row, col + 1] == TableMapping.Free)
                        {
                            freeCount++;
                            freeRow = row;
                            freeCol = col + 1;
                        }
                    }

                    if (lampCount + freeCount < _table[row, col] || lampCount > _table[row, col])
                    {
                        Invalid = true;
                        return;
                    }

                    if (lampCount + freeCount == _table[row, col] && freeCount > 0)
                    {
                        SetupLamp(freeRow, freeCol);
                        return;
                    }
                }
            }
        }

        public List<Tuple<int, int>> GetMinCellList()
        {
            List<Tuple<int, int>> result = null;
            for (var row = 0; row < Length; row++)
            {
                for (var col = 0; col < Length; col++)
                {
                    if (_table[row, col] != TableMapping.Free)
                        continue;

                    var list = new List<Tuple<int, int>>();
                    list.Add(Tuple.Create(row, col));

                    for (var i = row - 1; i >= 0; i--)
                    {
                        if (_table[i, col] == TableMapping.Free)
                        {
                            list.Add(Tuple.Create(i, col));
                            continue;
                        }
                        if (_table[i, col] != TableMapping.Lit)
                            break;
                    }
                    for (var i = row + 1; i < Length; i++)
                    {
                        if (_table[i, col] == TableMapping.Free)
                        {
                            list.Add(Tuple.Create(i, col));
                            continue;
                        }
                        if (_table[i, col] != TableMapping.Lit)
                            break;
                    }
                    for (var i = col - 1; i >= 0; i--)
                    {
                        if (_table[row, i] == TableMapping.Free)
                        {
                            list.Add(Tuple.Create(row, i));
                            continue;
                        }
                        if (_table[row, i] != TableMapping.Lit)
                            break;
                    }
                    for (var i = col + 1; i < Length; i++)
                    {
                        if (_table[row, i] == TableMapping.Free)
                        {
                            list.Add(Tuple.Create(row, i));
                            continue;
                        }
                        if (_table[row, i] != TableMapping.Lit)
                            break;
                    }
                    if (result == null || list.Count < result.Count)
                    {
                        result = list;

                        if (result.Count == 1)
                            return result;
                    }
                }
            }
            return result;
        }

        private void Light(int row, int column)
        {
            for (var i = row + 1; i < Length; i++)
            {
                if (_table[i, column] == TableMapping.Free || _table[i, column] == TableMapping.Lit)
                    _table[i, column] = TableMapping.Lit;
                else
                    break;
            }

            for (var i = row - 1; i >= 0; i--)
            {
                if (_table[i, column] == TableMapping.Free || _table[i, column] == TableMapping.Lit)
                    _table[i, column] = TableMapping.Lit;
                else
                    break;
            }

            for (var i = column + 1; i < Length; i++)
            {
                if (_table[row, i] == TableMapping.Free || _table[row, i] == TableMapping.Lit)
                    _table[row, i] = TableMapping.Lit;
                else
                    break;
            }

            for (var i = column - 1; i >= 0; i--)
            {
                if (_table[row, i] == TableMapping.Free || _table[row, i] == TableMapping.Lit)
                    _table[row, i] = TableMapping.Lit;
                else
                    break;
            }
        }

        public bool IsReady()
        {
            for (var i = 0; i < Length; i++)
            {
                for (var j = 0; j < Length; j++)
                {
                    if (_table[Length - i - 1, Length - j - 1] == TableMapping.Free)
                        return false;
                }
            }
            return true;
        }

        public override string ToString()
        {
            var sb = new StringBuilder((Length * Length) + (Length * 2));
            for (var row = 0; row < Length; row++)
            {
                for (var col = 0; col < Length; col++)
                {
                    sb.Append(_table[row, col].Map());
                }
                if (row < Length - 1)
                    sb.AppendLine();
            }
            return sb.ToString();
        }
        

        public int NeighbourCount(int row, int col, byte cellType)
        {
            var count = 0;
            if (row > 0 && _table[row - 1, col] == cellType) ++count;
            if (row < Length - 1 && _table[row + 1, col] == cellType) ++count;
            if (col > 0 && _table[row, col - 1] == cellType) ++count;
            if (col < Length - 1 && _table[row, col + 1] == cellType) ++count;

            return count;
        }
        
        private int NeighborLamps(int row, int column)
        {
            Func<int, int, int> evaluateNeighbor = (x, y) =>
            {
                var rowIndex = row + x;

                if (rowIndex < 0) return 0;
                if (rowIndex >= Length) return 0;

                var colIndex = column + y;

                if (colIndex < 0) return 0;
                if (colIndex >= Length) return 0;

                return _table[rowIndex, colIndex] == TableMapping.Lamp ? 1 : 0;
            };

            var taskHorizontalLeft = Task.Run(() => evaluateNeighbor(-1, 0));
            var taskHorizontalRight = Task.Run(() => evaluateNeighbor(1, 0));
            var taskVerticalUp = Task.Run(() => evaluateNeighbor(0, 1));
            var taskVerticalDown = Task.Run(() => evaluateNeighbor(0, -1));

            var sum = Task.WhenAll(taskHorizontalLeft, taskHorizontalRight, taskVerticalUp, taskVerticalDown)
                .Result
                .Sum(x => x);

            return sum;
        }

        private bool IsCovered(int row, int column)
        {
            var value = _table[row, column];

            if (value != TableMapping.Free && value != TableMapping.Lamp) return false;

            Func<int, int, bool> evaluateLamps = (x, y) =>
            {
                var rowIndex = row + x;
                var colIndex = column + y;

                while (rowIndex >= 0 && rowIndex < Length && colIndex >= 0 && colIndex < Length)
                {
                    var next = _table[rowIndex, colIndex];

                    if (next == TableMapping.Lamp)
                        return true;

                    if (next.IsWall())
                        break;

                    rowIndex = rowIndex + x;
                    colIndex = colIndex + y;
                }

                return false;
            };

            var taskHorizontalLeft = Task.Run(() => evaluateLamps(-1, 0));
            var taskHorizontalRight = Task.Run(() => evaluateLamps(1, 0));
            var taskVerticalUp = Task.Run(() => evaluateLamps(0, 1));
            var taskVerticalDown = Task.Run(() => evaluateLamps(0, -1));

            var result = Task.WhenAll(taskHorizontalLeft, taskHorizontalRight, taskVerticalUp, taskVerticalDown)
                .Result
                .Any(x => x);

            return result;
        }

        private bool IsCorrect(int row, int column)
        {
            var value = _table[row, column];

            switch (value)
            {
                case TableMapping.Lamp:
                    return !IsCovered(row, column);
                case TableMapping.Free:
                    return IsCovered(row, column);
                case TableMapping.Wall:
                    return true;
                case TableMapping.Wall0:
                case TableMapping.Wall1:
                case TableMapping.Wall2:
                case TableMapping.Wall3:
                case TableMapping.Wall4:
                    return NeighborLamps(row, column) == value;
                default:
                    return false;
            }
        }

        public bool IsCorrect()
        {
            for (var row = 0; row < Length; row++)
            {
                for (var column = 0; column < Length; column++)
                {
                    if (!IsCorrect(row, column))
                        return false;
                }
            }

            return true;
        }
    }
}