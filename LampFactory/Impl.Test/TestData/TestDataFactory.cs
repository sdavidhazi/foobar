using System.Collections.Generic;
using System.Linq;
using Contracts;
using Impl.Extensions;
using Impl.Model;

namespace Impl.Test.TestData
{
    class TestDataFactory
    {
        private readonly ISolver _solver;

        public TestDataFactory(ISolver solver)
        {
            _solver = solver;
        }

        public string TransformByReplace(string referencePuzzle, int numberOfWallsToReplace)
        {
            // Solve the original puzzle
            var solution = _solver.Solve(referencePuzzle);

            // Transform data to a table
            var table = new Table(solution);

            int replaced = 0;
            for (int row = 0; row < table.Length; row++)
            {
                for (int column = 0; column < table.Length; column++)
                {
                    if (table[row, column] == TableMapping.Wall)
                    {
                        table[row, column] = (byte)table.NeighbourCount(row, column, TableMapping.Lamp);
                        replaced++;
                        if (replaced == numberOfWallsToReplace)
                            break;
                    }
                }

                if (replaced == numberOfWallsToReplace)
                    break;
            }

            return table.ToString().Replace('o', ' ').Replace('x', ' ');
        }

        public IEnumerable<string> TransformByReplace(string referencePuzzle)
        {
            int maxReplaceCount = referencePuzzle.Count(x => x.Equals('#'));
            for (int i = 0; i <= maxReplaceCount; i++)
            {
                yield return TransformByReplace(referencePuzzle, i);
            }
        }

        public string TransformByRotation(string referencePuzzle, int angle)
        {
            var table = new Table(referencePuzzle);
            var tableTransformed = table.Clone();

            for (int row = 0; row < table.Length; row++)
            {
                for (int column = 0; column < table.Length; column++)
                {
                    int rowIndex = row;
                    int colIndex = column;
                    switch (angle)
                    {
                        case 90:
                            rowIndex = column;
                            colIndex = table.Length - row - 1;
                            break;
                        case 180:
                            rowIndex = table.Length - row - 1;
                            colIndex = column;
                            break;
                        case 270:
                            rowIndex = table.Length - column - 1;
                            colIndex = row;
                            break;
                    }
                    tableTransformed[row, column] = table[rowIndex, colIndex];
                }
            }

            return tableTransformed.ToString().Replace('o', ' ').Replace('x', ' ');
        }

        public IEnumerable<string> TransformByRotation(string referencePuzzle)
        {
            yield return TransformByRotation(referencePuzzle, 0);
            yield return TransformByRotation(referencePuzzle, 90);
            yield return TransformByRotation(referencePuzzle, 180);
            yield return TransformByRotation(referencePuzzle, 270);
        }
    }
}
