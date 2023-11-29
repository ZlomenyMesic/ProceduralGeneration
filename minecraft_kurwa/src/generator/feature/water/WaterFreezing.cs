//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

using minecraft_kurwa.src.global;
using minecraft_kurwa.src.generator.voxels;
using System;

namespace minecraft_kurwa.src.generator.feature.water {
    internal static class WaterFreezing {
        internal static void Freeze(Random random) {
            for (ushort x = 0; x < Settings.WORLD_SIZE; x++) {
                for (ushort y = 0; y < Settings.WORLD_SIZE; y++) {
                    for (ushort z = 0; z < Settings.HEIGHT_LIMIT; z++) {
                        if (Global.VOXEL_MAP[x, y, z] == (byte)VoxelType.WATER) {
                            Global.VOXEL_MAP[x, y, z] = CanFreeze(x, y, (ushort)random.Next(Settings.FREEZING_DISTANCE, Settings.MIN_FREEZING_DISTANCE))
                                ? random.Next(0, 100) > Settings.ICE_HOLES - 1
                                    ? (byte)VoxelType.ICE
                                    : (byte)VoxelType.WATER
                                : (byte)VoxelType.WATER;
                        }
                    }
                }
            }
        }

        private static bool CanFreeze(ushort x, ushort y, ushort distance) {
            for (sbyte x2 = -1; x2 <= 1; x2++) {
                for (sbyte y2 = -1; y2 <= 1; y2++) {

                    short x3 = (short)(x + x2 * distance);
                    short y3 = (short)(y + y2 * distance);
                    if (x3 < 0 || x3 >= Settings.WORLD_SIZE || y3 < 0 || y3 >= Settings.WORLD_SIZE) continue;

                    if (Global.BIOME_MAP[x3, y3, 0] < 30) return false; // water is too close to a subtropical or tropical biome to freeze
                    if (Global.BIOME_MAP[x3, y3, 0] >= 60 && Global.BIOME_MAP[x3, y3, 0] < 70) return true; // water is close to a polar biome
                }
            }
            return false;
        }
    }
}
