using System;
using System.Linq;
using System.Threading.Tasks;
using Impl.Extensions;

namespace Impl.Model
{
    public class Table
    {
        private readonly int _length;
        private readonly byte[][] _table;

        public Table(string rawTable)
        {
            var lines = rawTable.Split();
            _length = lines.Length;
            _table = new byte[_length][];

            // TODO: Try to use multiple tasks to setup the initial model
            for (var row = 0; row < _length; row++)
            {
                for (var column = 0; column < _length; column++)
                {
                    _table[row][column] = lines[row][column].Map();
                }
            }
        }

        public bool IsCorrect()
        {
            // TODO: Try to use multiple tasks to evaluate correctness
            for (var row = 0; row < _length; row++)
            {
                for (var column = 0; column < _length; column++)
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
        }

        private bool IsCovered(int row, int column)
        {
            var value = _table[row][column];

            if (value != TableMapping.Free && value != TableMapping.Lamp) return false;

            Func<int, int, bool> evaluateLamps = (x, y) =>
            {
                var rowIndex = row + x;
                var colIndex = column + y;

                while (rowIndex >= 0 && rowIndex < _length && colIndex >= 0 && colIndex < _length)
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
                if (rowIndex >= _length) return 0;

                var colIndex = column + y;

                if (colIndex < 0) return 0;
                if (colIndex >= _length) return 0;

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
    }
}