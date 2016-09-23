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

            Table result = null;
            while (tableSnapshots.Count > 0)
            {
                Table snapshot;
                if (!tableSnapshots.TryPop(out snapshot)) break;
                if (snapshot == null) continue;

                var list = snapshot.GetMinCellList();

                bool invalid = false;
                while (list != null && list.Count == 1)
                {
                    snapshot.SetupLamp(list[0].Item1, list[0].Item2);

                    if (snapshot.Invalid)
                    {
                        invalid = true;
                        break;
                    }

                    if (snapshot.IsReady())
                        return snapshot;

                    list = snapshot.GetMinCellList();
                }

                if (invalid)
                    continue;

                if (snapshot.Invalid)
                    continue;
                if (snapshot.IsReady())
                    return snapshot;

                Parallel.ForEach(list, (cell, loopState) =>
                {
                    var clone = snapshot.Clone();
                    clone.SetupLamp(cell.Item1, cell.Item2);

                    if (!clone.Invalid)
                    {
                        if (clone.IsReady())
                        {
                            result = clone;
                            loopState.Stop();
                        }
                        else
                        {
                            tableSnapshots.Push(clone);
                        }
                    }
                });

                if (result != null)
                    return result;
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
