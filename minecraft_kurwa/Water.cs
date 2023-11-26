//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

using System;

namespace minecraft_kurwa {
    internal static class Water {
        private const int TOTAL_FREEZING_DISTANCE = 20; // any water block closer to a polar biome than this will freeze
        private const int MIN_FREEZING_DISTANCE = 45;   // minimum distance from a polar biome to freeze
        private const int ICE_HOLES = 0;                // 0 and less => no holes in ice; 100 and more => no ice

        internal static void Generate() {
            Random random = new(Settings.SEED);
            for (ushort x = 0; x < Settings.WORLD_SIZE; x++) {
                for (ushort y = 0; y < Settings.WORLD_SIZE; y++) {
                    if (Global.HEIGHT_MAP[x, y] < Settings.WATER_LEVEL) {
                        Global.VOXEL_MAP[x, y, Settings.WATER_LEVEL] = CanFreeze(x, y, (ushort)random.Next(TOTAL_FREEZING_DISTANCE, MIN_FREEZING_DISTANCE))
                            ? random.Next(0, 100) > ICE_HOLES - 1
                                ? (byte)VoxelType.ICE
                                : (byte)VoxelType.WATER
                            : (byte)VoxelType.WATER;
                    }
                }
            }
        }

        private static bool CanFreeze(ushort x, ushort y, ushort distance) {
            for (sbyte x2 = -1; x2 <= 1; x2++) {
                for (sbyte y2 = -1; y2 <= 1; y2++) {

                    short x3 = (short)(x + (x2 * distance));
                    short y3 = (short)(y + (y2 * distance));
                    if (x3 < 0 || x3 >= Settings.WORLD_SIZE || y3 < 0 || y3 >= Settings.WORLD_SIZE) continue;

                    if (Global.BIOME_MAP[x3, y3, 0] < 30) return false; // water is too close to a subtropical or tropical biome to freeze
                    if (Global.BIOME_MAP[x3, y3, 0] >= 60 && Global.BIOME_MAP[x3, y3, 0] < 70) return true; // water is close to a polar biome
                }
            }
            return false;
        }
    }
}
