using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
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
            throw new NotImplementedException();   
        }

        public IEnumerable<string> TransformByRotation(string referencePuzzle)
        {
            throw new NotImplementedException();
        }
    }
}
