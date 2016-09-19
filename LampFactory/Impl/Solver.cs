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
        private Table Solve(Table table, int minRow, int minCol)
        {
            var length = table.Length;
            for (var col = minCol; col < length; col++)
            { 
                for (var row = minRow; row < length; row++)
                {
                    if (table[row, col]!=TableMapping.Free)
                        continue;

                    table.SetupLamp(row, col);
                    if (table.Invalid)
                        continue;

                    if (table.IsReady())
                        return table;

                    var result = Solve(table, row == length - 1 ? 0 : row + 1, row == length - 1 ? col + 1: col);
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
            return Solve(puzzle, 0, 0)?.ToString();
        }
    }
}
