using System;
using System.Collections.Generic;
using System.Linq;
using Contracts;
using Impl.Extensions;
using Impl.Model;
using Impl.Test.Extensions;
using Impl.Test.Random;

namespace Impl.Test.TestData
{
    class TestDataFactory
    {
        private readonly ISolver _solver;
        private readonly IRandomProvider _random;

        public TestDataFactory(ISolver solver, IRandomProvider random)
        {
            _solver = solver;
            _random = random;
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
                            colIndex = table.Length - column - 1;
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

        public string GenerateRandomPuzzle(int size)
        {
            if (size <= 1)
                throw new ArgumentException("Puzzle size must be grather than 1", nameof(size));

            var numberOfWalls = _random.Next(1, (size * size) -1);
            var wallPositions = new List<Tuple<int,int>>();
            var table = new Table(size);

            // Setup puzzle without wall constraints
            for (int i = 0; i < numberOfWalls; i++)
            {
                int row = _random.Next(0, size - 1);
                int column = _random.Next(0, size - 1);

                while (table[row, column] == TableMapping.Wall)
                {
                    row = _random.Next(0, size - 1);
                    column = _random.Next(0, size - 1);
                }

                table[row, column] = TableMapping.Wall;
                wallPositions.Add(new Tuple<int, int>(row,column));
            }

            // Solve the basic puzzle
            var basePuzzle = table.ToString();
            var result = _solver.Solve(basePuzzle);
            var resultTable = new Table(result);

            // Add wall contraints
            var numberOfWallReplace = _random.Next(0, numberOfWalls);

            // Shuffle wasll positions
            wallPositions.Shuffle(_random);
            var wallPositionsToBeReplaced = wallPositions.Take(numberOfWallReplace);

            foreach (var position in wallPositionsToBeReplaced)
            {
                int row = position.Item1;
                int column = position.Item2;

                resultTable[row, column] = (byte)resultTable.NeighbourCount(row, column, TableMapping.Lamp);
            }

            // Pass back the puzzle
            return resultTable.ToString().Replace('o', ' ').Replace('x', ' ');
        }
    }
}
