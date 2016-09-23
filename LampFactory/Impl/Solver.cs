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
    internal class Solver : ISolver
    {
        private Table Solve2(Table table)
        {
            if (table.IsReady())
                return table;
            var list = table.GetMinCellList();
            if (list == null || list.Count == 0)
                return null;
            if (list.Count == 1)
            {
                table.SetupLamp(list[0].Item1, list[0].Item2);
                if (table.Invalid)
                    return null;
                return Solve2(table);
            }

            foreach (var cell in list)
            {
                var clone = table.Clone();
                clone.SetupLamp(cell.Item1, cell.Item2);
                if (clone.Invalid)
                    continue;
                var result = Solve2(clone);
                if (result != null)
                    return result;
            }
            return null;
        }

        public string Solve(string table)
        {
            var puzzle = new Table(table);
            puzzle.SetupLightBesideWalls();
            Table result = Solve2(puzzle);
            return result?.ToString().Replace('x', ' ');
        }
    }
}
