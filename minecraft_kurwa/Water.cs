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
                        for (ushort z = (ushort)(Global.HEIGHT_MAP[x, y] + 1); z <= Global.WATER_LEVEL; z++) {
                            Global.VOXEL_MAP[x, y, z] = VoxelType.WATER;
                        }
                    }
                }
            }
        }
    }
}
