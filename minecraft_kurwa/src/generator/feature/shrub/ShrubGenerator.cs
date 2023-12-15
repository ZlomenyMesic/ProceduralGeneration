//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

using minecraft_kurwa.src.global;
using minecraft_kurwa.src.renderer.voxels;
using System;

namespace minecraft_kurwa.src.generator.feature.shrub {
    internal static class ShrubGenerator {
        internal static void Generate() {
            ushort maxCount = (ushort)(Settings.WORLD_SIZE * Settings.WORLD_SIZE * Settings.BUSH_DENSITY / 10_000);
            Random random = new(Settings.SEED >> 1);

            for (ushort i = 0; i < maxCount; i++) {
                ushort x = (ushort)random.Next(Settings.WOODY_PLANTS_EDGE_OFFSET, Settings.WORLD_SIZE - Settings.WOODY_PLANTS_EDGE_OFFSET - 1);
                ushort y = (ushort)random.Next(Settings.WOODY_PLANTS_EDGE_OFFSET, Settings.WORLD_SIZE - Settings.WOODY_PLANTS_EDGE_OFFSET - 1);
                byte biome = Global.BIOME_MAP[x, y, 0];

                // shrubs can't generate under the water level, on water, or on ice
                if (Global.HEIGHT_MAP[x, y] <= Settings.WATER_LEVEL) continue;
                if (Global.VOXEL_MAP[x, y, Global.HEIGHT_MAP[x, y]] == (byte)VoxelType.WATER || Global.VOXEL_MAP[x, y, Global.HEIGHT_MAP[x, y]] == (byte)VoxelType.ICE) continue;

                VoxelType leaves = biome switch {
                    10 or 21 or 22 or 24 => VoxelType.CHERRY_LAUREL_LEAVES,                          // subtropical
                    5 or 23 => random.Next(0, 8) == 0 ? VoxelType.RAISIN_LEAVES : VoxelType.UNKNOWN, // savanna
                    _ => VoxelType.UNKNOWN
                };

                if (leaves != VoxelType.UNKNOWN) Shrub.Build(x, y, (ushort)(Global.HEIGHT_MAP[x, y] + 1), (ushort)random.Next(Shrub.MIN_SHRUB_SIZE, Shrub.MAX_SHRUB_SIZE), (ushort)random.Next(Shrub.MIN_SHRUB_SIZE, Shrub.MAX_SHRUB_SIZE), leaves);
            }
        }
    }
}
