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
        private int firstNonWallIndex = -1;
        // TODO: Profile the code to see expensive parts
        private Table Solve(Table table, IList<Tuple<int, int>> cells, int fromIndex)
        {
            // TODO: Try to use stack instead of recursive calls 
            // TODO: Try to indetify wrong "directions" earlier in the logical tree
            var length = table.Length;
            for (int i = fromIndex; i < cells.Count; i++)
            {
                var row = cells[i].Item1;
                var col = cells[i].Item2;

                if (i < firstNonWallIndex && !table.HasMissingWallLamps)
                    continue;

                if (i >= firstNonWallIndex && table.HasMissingWallLamps)
                    return null;

                if (table[row, col] != TableMapping.Free)
                    continue;

                var clone = table.Clone();

                clone.SetupLamp(row, col);
                if (clone.Invalid)
                    continue;

                if (clone.IsReady())
                    return clone;

                var result = Solve(clone, cells, i + 1);
                if (result != null)
                    return result;
            }
            return null;
        }

        private IList<Tuple<int, int>> CreatePriorityQueue(Table table)
        {
            byte[,] priorities = new byte[table.Length, table.Length];
            var length = table.Length;

            List<KeyValuePair<byte, Tuple<int, int>>> list = new List<KeyValuePair<byte, Tuple<int, int>>>();

            for (int row = 0; row < length; row++)
            {
                for (int col = 0; col < length; col++)
                {
                    byte priority = 0;
                    if (table[row, col] != TableMapping.Free)
                        priority = 255;
                    else if (table.HasNeighbour(row, col, TableMapping.Wall0))
                        priority = 255;
                    else if (table.HasNeighbour(row, col, TableMapping.Wall4))
                        priority = 0;
                    else if (table.HasNeighbour(row, col, TableMapping.Wall3))
                        priority = 1;
                    else if (table.HasNeighbour(row, col, TableMapping.Wall2))
                        priority = 2;
                    else if (table.HasNeighbour(row, col, TableMapping.Wall1))
                        priority = 3;
                    else
                        priority = 10;

                    if (priority == 255)
                        continue;
                    list.Add(new KeyValuePair<byte, Tuple<int, int>>(priority, Tuple.Create(row, col)));
                }
            }

            var result = list.OrderBy(kv => kv.Key);
            firstNonWallIndex = result.Count(kv => kv.Key < 9);
            return result.Select(kv => kv.Value).ToList();
        }

        public string Solve(string table)
        {
            var puzzle = new Table(table);
            puzzle.SetupLightBesideWalls();
            Console.WriteLine(puzzle.ToString());
            var result = Solve(puzzle, CreatePriorityQueue(puzzle), 0);
            Console.WriteLine(result?.ToString());
            firstNonWallIndex = -1;
            return result?.ToString().Replace('x', ' ');
        }
    }
}
