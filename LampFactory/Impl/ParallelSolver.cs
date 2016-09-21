using Contracts;
using Impl.Model;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

namespace Impl
{
    public class ParallelSolver : ISolver
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

            ConcurrentBag<Table> results = new ConcurrentBag<Table>();
            Parallel.ForEach(list, (cell, loopState) =>
               {
                   var clone = table.Clone();
                   clone.SetupLamp(cell.Item1, cell.Item2);

                   if (!clone.Invalid)
                   {
                       var result = Solve2(clone);
                       if (result != null)
                       {
                           results.Add(result);
                           loopState.Stop();
                       }
                   }
               });

            return results.FirstOrDefault();
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
