//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

namespace minecraft_kurwa {
    internal static class Water {
        internal static void Generate() {
            for (ushort x = 0; x < Settings.WORLD_SIZE; x++) {
                for (ushort y = 0; y < Settings.WORLD_SIZE; y++) {
                    if (Global.HEIGHT_MAP[x, y] < Settings.WATER_LEVEL) {
                        Global.VOXEL_MAP[x, y, Settings.WATER_LEVEL] = (byte)VoxelType.WATER;
                    }
                }
            }
        }
    }
}
