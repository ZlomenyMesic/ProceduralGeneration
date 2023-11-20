using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace minecraft_kurwa {
    internal static class Water {
        internal static void Generate() {
            for (int x = 0; x < Global.WORLD_SIZE; x++) {
                for (int y = 0; y < Global.WORLD_SIZE; y++) {
                    if (Global.HEIGHT_MAP[x, y] < Global.WATER_LEVEL) {
                        for (int z = Global.HEIGHT_MAP[x, y] + 1; z <= Global.WATER_LEVEL; z++) {
                            Global.VOXEL_MAP[x, y, z] = VoxelType.WATER;
                        }
                    }
                }
            }
        }
    }
}
