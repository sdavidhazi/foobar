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
            for (var col = fromCol; col < length; col++)
            { 
                for (var row = fromRow; row < length; row++)
                {
                    if (table[row, col]!=TableMapping.Free)
                        continue;

                    var clone = table.Clone();

                    clone.SetupLamp(row, col);
                    if (clone.Invalid)
                        continue;

                    if (clone.IsReady())
                        return clone;

                    var result = Solve(clone, row == length - 1 ? 0 : row + 1, row == length - 1 ? col + 1: col);
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
