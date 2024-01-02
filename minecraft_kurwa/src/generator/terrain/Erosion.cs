//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

using minecraft_kurwa.src.generator.terrain.noise;
using minecraft_kurwa.src.global;
using System;

namespace minecraft_kurwa.src.generator.terrain {
    internal static class Erosion {
        internal const int SCALE = 30;
        internal const int RARITY = 3;

        internal static void GenerateErosion() {
            bool[,] erosion = new bool[Settings.WORLD_SIZE, Settings.WORLD_SIZE];

            SimplexNoise s = new(Settings.SEED);

            for (ushort x = 0; x < Settings.WORLD_SIZE; x++) {
                for (ushort y = 0; y < Settings.WORLD_SIZE; y++) {
                    byte value1 = (byte)Math.Round(s.Calculate(x, y, SCALE, RARITY), 0);
                    byte value2 = (byte)Math.Round(s.Calculate(x + 5000, y + 5000, SCALE, RARITY), 0);

                    erosion[x, y] = value1 + value2 == RARITY * 2;
                }
            }

            for (ushort x = 0; x < Settings.WORLD_SIZE; x++) {
                for (ushort y = 0; y < Settings.WORLD_SIZE; y++) {
                    for (int x2 = -1; x2 < 2; x2++) {
                        if (x + x2 < 0 || x + x2 >= Settings.WORLD_SIZE) continue;

                        for (int y2 = -1; y2 < 2; y2++) {
                            if (y + y2 < 0 || y + y2 >= Settings.WORLD_SIZE) continue;

                            if (erosion[x, y] != erosion[x + x2, y + y2] && Global.RANDOM.Next(0, 2) == 0) erosion[x, y] = erosion[x + x2, y + y2];
                        }
                    }
                }
            }

            for (ushort x = 0; x < Settings.WORLD_SIZE; x++) {
                for (ushort y = 0; y < Settings.WORLD_SIZE; y++) {
                    byte erosionLevel = (byte)(Math.Round(s.Calculate(x + 10000, y + 10000, SCALE * 3, 1), 0) + 1);
                    for (; Global.HEIGHT_MAP[x, y] - erosionLevel < 0; erosionLevel--) ;

                    Global.HEIGHT_MAP[x, y] -= (ushort)(erosion[x, y] ? erosionLevel : 0);
                }
            }
        }
    }
}
