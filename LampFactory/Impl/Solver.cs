using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts;
using Impl.Extensions;
using Impl.Model;

namespace Impl
{
    public class Solver : ISolver
    {
        private Table Solve(Table table, int fromRow, int fromCol)
        {
            var length = table.Length;
            for (var row = 0; row < length; row++)
            {
                for (var col = 0; col < length; col++)
                {

                    if (table[row, col] != TableMapping.Free)
                        continue;

                    var clone = table.Clone();

                    clone.SetupLamp(row, col);
                    if (clone.Invalid)
                        continue;

                    if (clone.IsReady())
                        return clone;

                    var result = Solve(clone, col == length - 1 ? 0 : col + 1, col == length - 1 ? row + 1 : row);
                    if (result != null)
                        return result;
                }
            }

            return null;
        }
        public string Solve(string table)
        {
            var puzzle = new Table(table);
            puzzle.SetupLightBesideWalls();
            Console.WriteLine(puzzle);
            var result = Solve(puzzle, 0, 0);
            Console.WriteLine(result);
            return result?.ToString();
        }
    }
}
