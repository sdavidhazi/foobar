using System.Collections.Generic;
using Contracts;
using Impl.Model;

namespace Impl
{
    public class StackSolver : ISolver
    {
        private Table SolveTable(Table table)
        {
            table.SetupLightBesideWalls();

            var tableSnapshots = new Stack<Table>();
            tableSnapshots.Push(table.Clone());

            while (tableSnapshots.Count > 0)
            {
                var snapshot = tableSnapshots.Pop();
                var list = snapshot.GetMinCellList();

                foreach (var cell in list)
                {
                    var clone = snapshot.Clone();
                    clone.SetupLamp(cell.Item1, cell.Item2);

                    if (clone.Invalid)
                        continue;

                    if (clone.IsReady())
                        return clone;

                    tableSnapshots.Push(clone);
                }
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
