using System;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Impl.Extensions;
using System.Collections.Generic;

namespace Impl.Model
{
    public class Table
    {
        // TODO: Balazs: Use a list of prioritized indexes to iterate the table
        // TODO: Use a two dimensional array instead of array in array        
        // TODO: Try to cache wall positions to speed up SetupLampBesideWall method
        // TODO: Use a counter to track number of free places --> IsReady is simplified

        public int Length { get; }
        public bool Invalid { get; set; }
        private readonly byte[][] _table;

        private Table(Table table)
        {                        
            Length = table.Length;
            Invalid = table.Invalid;
            _table = new byte[Length][];
            for (int i=0; i<Length; i++)
                _table[i] = (byte[])table._table[i].Clone();
            //for (int i = 0; i < Length; i++)
            //{
            //    _table[i] = new byte[Length];
            //    for (int j = 0; j < Length; j++)
            //        _table[i][j] = table._table[i][j];

            //}
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

        public byte this[int row, int column]
        {
            get
            {
                return _table[row][column];
            }
            set
            {
                _table[row][column] = value;
            }
        }

        public bool HasNeighbour(int row, int col, byte cellType)
        {
            if (row > 0 && _table[row - 1][col] == cellType) return true;
            if (row < Length - 1 && _table[row + 1][col] == cellType) return true;
            if (col > 0 && _table[row][col - 1] == cellType) return true;
            if (col < Length - 1 && _table[row][col + 1] == cellType) return true;
            return false;

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
            SetupLightBesideWalls();

            //Console.WriteLine(this);
            //Console.WriteLine();
            //if (this.Invalid)
            //    Console.WriteLine("Invalid");
        }

        public void SetupLightBesideWalls()
        {
            for (int row = 0; row < Length; row++)
            {

                for (int col = 0; col < Length; col++)
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
                    if (row < Length - 1)
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
                        if (_table[row][col - 1] == TableMapping.Lamp)
                            lampCount++;
                        if (_table[row][col - 1] == TableMapping.Free)
                        {
                            freeCount++;
                            freeRow = row;
                            freeCol = col - 1;
                        }
                    }
                    if (col < Length - 1)
                    {
                        if (_table[row][col + 1] == TableMapping.Lamp)
                            lampCount++;
                        if (_table[row][col + 1] == TableMapping.Free)
                        {
                            freeCount++;
                            freeRow = row;
                            freeCol = col + 1;
                        }
                    }

                    if (lampCount + freeCount < _table[row][col] || lampCount > _table[row][col])
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
                    if (_table[Length-i-1][Length-j-1] == TableMapping.Free)
                        return false;
                }
            }
            return true;
        }


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

            var taskHorizontalLeft = Task.Run(()=> evaluateLamps(-1, 0));
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

            var taskHorizontalLeft = Task.Run(() => evaluateNeighbor(-1, 0));
            var taskHorizontalRight = Task.Run(() => evaluateNeighbor(1, 0));
            var taskVerticalUp = Task.Run(() => evaluateNeighbor(0, 1));
            var taskVerticalDown = Task.Run(() => evaluateNeighbor(0, -1));

            var sum = Task.WhenAll(taskHorizontalLeft, taskHorizontalRight, taskVerticalUp, taskVerticalDown)
                .Result
                .Sum(x => x);

            return sum;
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
                if (row < Length - 1)
                    sb.AppendLine();
            }
            return sb.ToString();
        }
    }
}