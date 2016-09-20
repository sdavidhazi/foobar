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
        // TODO: Profile the code to see expensive parts
        private Table Solve(Table table, int fromRow)
        {
            // TODO: Try to use stack instead of recursive calls 
            // TODO: Try to indetify wrong "directions" earlier in the logical tree
            var length = table.Length;
            for (var row = fromRow; row < length; row++)
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

                    var result = Solve(clone,  col == length - 1 ? row+1 : row);
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
            var result = Solve(puzzle, 0);
            return result?.ToString().Replace('x',' ');
        }
    }
}
