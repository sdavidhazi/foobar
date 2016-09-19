using System;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Impl.Extensions;

namespace Impl.Model
{
    public class Table
    {
        public int Length { get; }
        public bool Invalid { get; set; }
        private readonly byte[][] _table;

        private Table(Table table)
        {
            Length = table.Length;
            _table = (byte[][])table._table.Clone();
        }

        public Table(string rawTable)
        {
            rawTable = rawTable.Replace("\r", "");
            var lines = rawTable.Split('\n');
            Length = lines.Length;
            _table = new byte[Length][];

            // TODO: Try to use multiple tasks to setup the initial model
            for (var row = 0; row < Length; row++)
            {
                _table[row] = new byte[Length];
                for (var column = 0; column < Length; column++)
                {
                    _table[row][column] = lines[row][column].Map();
                }
            }
        }

        public Table Clone()
        {
            return new Table(this);
        }

        public bool IsCorrect()
        {
            // TODO: Try to use multiple tasks to evaluate correctness
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

        public void SetupLamp(int row, int column)
        {
            if (_table[row][column] != TableMapping.Free)
            {
                throw new InvalidOperationException("Setting up a lamp is not allowed since the cell is already used.");
            }

            _table[row][column] = TableMapping.Lamp;
            Light(row, column);
            //Console.WriteLine(this);

            SetupLightBesideWalls();
        }

        public void SetupLightBesideWalls()
        {
            for (int col = 0; col < Length; col++)
            {
                for (int row = 0; row < Length; row++)
                {
                    if (_table[row][col] > TableMapping.Wall4)
                        continue;

                    int lampCount = 0;
                    int freeCount = 0;

                    int freeRow = -1;
                    int freeCol = -1;

                    if (row > 0)
                    {
                        if (_table[row - 1][col] == TableMapping.Lamp)
                            lampCount++;
                        if (_table[row - 1][col] == TableMapping.Free)
                        {
                            freeCount++;
                            freeRow = row - 1;
                            freeCol = col;
                        }
                    }
                    if (row < Length-1)
                    {
                        if (_table[row + 1][col] == TableMapping.Lamp)
                            lampCount++;
                        if (_table[row + 1][col] == TableMapping.Free)
                        {
                            freeCount++;
                            freeRow = row + 1;
                            freeCol = col;
                        }
                    }

                    if (col > 0)
                    {
                        if (_table[row][col-1] == TableMapping.Lamp)
                            lampCount++;
                        if (_table[row][col-1] == TableMapping.Free)
                        {
                            freeCount++;
                            freeRow = row;
                            freeCol = col-1;
                        }
                    }
                    if (col < Length - 1)
                    {
                        if (_table[row][col+1] == TableMapping.Lamp)
                            lampCount++;
                        if (_table[row][col+1] == TableMapping.Free)
                        {
                            freeCount++;
                            freeRow = row;
                            freeCol = col+1;
                        }
                    }

                    if (lampCount + freeCount < _table[row][col])
                    {
                        Invalid = true;
                        return;
                    }

                    if (lampCount + freeCount == _table[row][col] && freeCount > 0)
                    {
                        SetupLamp(freeRow, freeCol);
                        return;
                    }
                }
            }
        }

        
        private void Light(int row, int column)
        {
            for (int i = row + 1; i < Length; i++)
            {
                if (_table[i][column] == TableMapping.Free || _table[i][column] == TableMapping.Lit)
                    _table[i][column] = TableMapping.Lit;
                else
                    break;
            }

            for (int i = row - 1; i >= 0; i--)
            {
                if (_table[i][column] == TableMapping.Free || _table[i][column] == TableMapping.Lit)
                    _table[i][column] = TableMapping.Lit;
                else
                    break;
            }

            for (int i = column + 1; i < Length; i++)
            {
                if (_table[row][i] == TableMapping.Free || _table[row][i] == TableMapping.Lit)
                    _table[row][i] = TableMapping.Lit;
                else
                    break;
            }

            for (int i = column - 1; i >= 0; i--)
            {
                if (_table[row][i] == TableMapping.Free || _table[row][i] == TableMapping.Lit)
                    _table[row][i] = TableMapping.Lit;
                else
                    break;
            }
        }

        public bool IsReady()
        {
            for (int i = 0; i < Length; i++)
            {
                for (int j = 0; j < Length; j++)
                {
                    if (_table[i][j] == TableMapping.Free)
                        return false;
                }
            }
            return true;
        }

        public byte this[int row, int col] => _table[row][col];

        private bool IsCovered(int row, int column)
        {
            var value = _table[row][column];

            if (value != TableMapping.Free && value != TableMapping.Lamp) return false;

            Func<int, int, bool> evaluateLamps = (x, y) =>
            {
                var rowIndex = row + x;
                var colIndex = column + y;

                while (rowIndex >= 0 && rowIndex < Length && colIndex >= 0 && colIndex < Length)
                {
                    var next = _table[rowIndex][colIndex];

                    if (next == TableMapping.Lamp)
                        return true;

                    if (next.IsWall())
                        break;

                    rowIndex = rowIndex + x;
                    colIndex = colIndex + y;
                }

                return false;
            };

            var taskHorizontalLeft = new Task<bool>(() => evaluateLamps(-1, 0));
            var taskHorizontalRight = new Task<bool>(() => evaluateLamps(1, 0));
            var taskVerticalUp = new Task<bool>(() => evaluateLamps(0, 1));
            var taskVerticalDown = new Task<bool>(() => evaluateLamps(0, -1));

            return Task.WhenAll(taskHorizontalLeft, taskHorizontalRight, taskVerticalUp, taskVerticalDown)
                .Result
                .Any(x => x);
        }

        private bool IsCorrect(int row, int column)
        {
            var value = _table[row][column];

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

                return _table[rowIndex][colIndex] == TableMapping.Lamp ? 1 : 0;
            };

            var taskHorizontalLeft = new Task<int>(() => evaluateNeighbor(-1, 0));
            var taskHorizontalRight = new Task<int>(() => evaluateNeighbor(1, 0));
            var taskVerticalUp = new Task<int>(() => evaluateNeighbor(0, 1));
            var taskVerticalDown = new Task<int>(() => evaluateNeighbor(0, -1));

            return Task.WhenAll(taskHorizontalLeft, taskHorizontalRight, taskVerticalUp, taskVerticalDown)
                .Result
                .Sum(x => x);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int row = 0; row < Length; row++)
            {
                for (int col = 0; col < Length; col++)
                {
                    sb.Append(_table[row][col].Map());
                }
                if (row < Length-1)
                    sb.AppendLine();
            }
            return sb.ToString();
        }
    }
}