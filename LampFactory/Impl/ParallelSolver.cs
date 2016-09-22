using Contracts;
using Impl.Model;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Impl
{
    public class ParallelSolver : ISolver
    {
        private Table SolveTable(Table table)
        {
            table.SetupLightBesideWalls();

            var tableSnapshots = new ConcurrentStack<Table>();
            tableSnapshots.Push(table.Clone());

            ConcurrentBag<Table> results = new ConcurrentBag<Table>();

            while (tableSnapshots.Count > 0)
            {
                Table snapshot;
                if (!tableSnapshots.TryPop(out snapshot)) break;

                var list = snapshot.GetMinCellList();

                Parallel.ForEach(list, (cell, loopState) =>
                {
                    var clone = snapshot.Clone();
                    clone.SetupLamp(cell.Item1, cell.Item2);

                    if (!clone.Invalid)
                    {
                        if (clone.IsReady())
                        {
                            results.Add(clone);
                            loopState.Stop();
                        }
                        else
                        {
                            tableSnapshots.Push(clone);
                        }
                    }
                });

                if (results.Any())
                    return results.FirstOrDefault();
            }

            return null;
        }

        public string Solve(string table)
        {
            var puzzle = new Table(table);
            Table result = SolveTable(puzzle);
            return result?.ToString().Replace('x', ' ');
        }
    }
}
