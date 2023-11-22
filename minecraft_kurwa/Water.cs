//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

namespace minecraft_kurwa {
    internal static class Water {
        internal static void Generate() {
            for (ushort x = 0; x < Global.WORLD_SIZE; x++) {
                for (ushort y = 0; y < Global.WORLD_SIZE; y++) {
                    if (Global.HEIGHT_MAP[x, y] < Global.WATER_LEVEL) {
                        Global.VOXEL_MAP[x, y, Global.WATER_LEVEL] = VoxelType.WATER;
                    }
                }
            }
        }
    }
}
