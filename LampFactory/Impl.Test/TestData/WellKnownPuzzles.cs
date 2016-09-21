using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Impl.Test.TestData
{
    public static class WellKnownPuzzles
    {
        private static Lazy<IEnumerable<Tuple<string, string>>> KnownPuzzles =
             new Lazy<IEnumerable<Tuple<string, string>>>(() => GetPuzzles());

        private static IEnumerable<Tuple<string, string>> GetPuzzles()
        {
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Impl.Test.Puzzles.zip"))
            using (var zip = new ZipArchive(stream))
            {
                var extractDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Guid.NewGuid().ToString());

                zip.ExtractToDirectory(extractDirectory);

                var puzzles = Directory
                    .GetFiles(Path.Combine(extractDirectory), "*.txt", SearchOption.AllDirectories)
                    .Select(x => new Tuple<string, string>(new DirectoryInfo(Path.GetDirectoryName(x)).Name, File.ReadAllText(x)));

                return puzzles;
            }
        }

        public static IEnumerable<string> Puzzles_14x14
        {
            get
            {
                return KnownPuzzles.Value.Where(x => x.Item1.Equals("14x14")).Select(x => x.Item2).ToList();
            }
        }

        public static IEnumerable<string> Puzzles_20x20
        {
            get
            {
                return KnownPuzzles.Value.Where(x => x.Item1.Equals("20x20")).Select(x => x.Item2);
            }
        }

        public static IEnumerable<string> Puzzles_25x25
        {
            get
            {
                return KnownPuzzles.Value.Where(x => x.Item1.Equals("25x25")).Select(x => x.Item2);
            }
        }

        public static IEnumerable<string> Puzzles_30x30
        {
            get
            {
                return KnownPuzzles.Value.Where(x => x.Item1.Equals("30x30")).Select(x => x.Item2);
            }
        }

        public static IEnumerable<string> Puzzles_40x40
        {
            get
            {
                return KnownPuzzles.Value.Where(x => x.Item1.Equals("40x40")).Select(x => x.Item2);
            }
        }

        public static IEnumerable<string> Puzzles_70x70
        {
            get
            {
                return KnownPuzzles.Value.Where(x => x.Item1.Equals("70x70")).Select(x => x.Item2);
            }
        }

    }
}
