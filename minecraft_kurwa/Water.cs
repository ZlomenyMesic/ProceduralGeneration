//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

using System;

namespace minecraft_kurwa {
    internal static class Water {
        internal static void Generate() {
            Random random = new(Settings.SEED + 69);
            for (ushort x = 0; x < Settings.WORLD_SIZE; x++) {
                for (ushort y = 0; y < Settings.WORLD_SIZE; y++) {
                    if (Global.HEIGHT_MAP[x, y] < Settings.WATER_LEVEL) {
                        Global.VOXEL_MAP[x, y, Settings.WATER_LEVEL] = Global.BIOME_MAP[x, y, 0] >= 50 && Global.BIOME_MAP[x, y, 0] < 70 && Global.BIOME_MAP[x, y, 2] >= 50 && Global.BIOME_MAP[x, y, 2] < 70
                            ? random.Next(0, 5) != 0
                                ? (byte)VoxelType.ICE 
                                : (byte)VoxelType.WATER
                            : (byte)VoxelType.WATER;
                    }
                }
            }
        }
    }
}
