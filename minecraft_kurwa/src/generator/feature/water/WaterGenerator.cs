//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

using System;
using minecraft_kurwa.src.global;
using minecraft_kurwa.src.renderer.voxels;

namespace minecraft_kurwa.src.generator.feature.water {
    internal static class WaterGenerator {


        internal static void Generate() {
            Random random = new(Settings.SEED);

            FillWaterLevel();
            Ponds.Generate(random);
            WaterFreezing.Freeze(random);
        }

        private static void FillWaterLevel() {
            for (ushort x = 0; x < Settings.WORLD_SIZE; x++) {
                for (ushort y = 0; y < Settings.WORLD_SIZE; y++) {
                    if (Global.HEIGHT_MAP[x, y] < Settings.WATER_LEVEL) {
                        Global.VOXEL_MAP[x, y, Settings.WATER_LEVEL] = (byte)VoxelType.WATER;
                        Global.HEIGHT_MAP[x, y] = (ushort)Settings.WATER_LEVEL;
                    }
                }
            }
        }
    }
}
