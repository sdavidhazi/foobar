using System.Collections.Generic;
using System.Linq;

namespace Impl.Extensions
{
    public static class TableMapping
    {
        public const byte Wall0 = 0;
        public const byte Wall1 = 1;
        public const byte Wall2 = 2;
        public const byte Wall3 = 3;
        public const byte Wall4 = 4;
        public const byte Wall = 5;
        public const byte Lamp = 6;

        public const byte Free = 7;
        public const byte Lit = 8;
        public const byte Forbidden = 9;

        private static readonly Dictionary<char, byte> Mapping = new Dictionary<char, byte>()
        {
            {'0',Wall0},
            {'1',Wall1 },
            {'2',Wall2 },
            {'3',Wall3 },
            {'4',Wall4 },
            {'#',Wall },
            {' ',Free },
            {'o',Lamp },
            {'x',Lit},
            {'%',Forbidden},
        };

        public static char Map(this byte value)
        {
            return Mapping.Single(kv => kv.Value.Equals(value)).Key;
        }

        public static bool IsWall(this byte value)
        {
            var intValue = (int)value;
            return intValue >= 0 && intValue <= 5;
        }

        public static byte Map(this char key)
        {
            return Mapping[key];
        }
    }
}
