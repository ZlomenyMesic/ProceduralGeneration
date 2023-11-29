//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

using minecraft_kurwa.src.global;
using minecraft_kurwa.src.generator.voxels;
using System;

namespace minecraft_kurwa.src.generator.feature.trees {
    internal static class TreeGenerator {
        internal static void Generate() {
            ushort maxCount = (ushort)(Settings.WORLD_SIZE * Settings.WORLD_SIZE * Settings.TREE_DENSITY / 10_000);
            Random random = new(Settings.SEED + 69);

            for (ushort i = 0; i < maxCount; i++) {
                ushort x = (ushort)random.Next(Settings.TREE_EDGE_OFFSET, Settings.WORLD_SIZE - Settings.TREE_EDGE_OFFSET - 1);
                ushort y = (ushort)random.Next(Settings.TREE_EDGE_OFFSET, Settings.WORLD_SIZE - Settings.TREE_EDGE_OFFSET - 1);
                byte biome = Global.BIOME_MAP[x, y, 0];

                // trees can't generate under the water level, on water, or on ice
                if (Global.HEIGHT_MAP[x, y] <= Settings.WATER_LEVEL) continue;
                if (Global.VOXEL_MAP[x, y, Global.HEIGHT_MAP[x, y]] == (byte)VoxelType.WATER || Global.VOXEL_MAP[x, y, Global.HEIGHT_MAP[x, y]] == (byte)VoxelType.ICE) continue;

                if (biome == 32 || biome == 42) {
                    switch (random.Next(0, 50)) {
                        case 0: BasicDeciduousTree.Build(random.Next(15, 19), x, y, Global.HEIGHT_MAP[x, y] + 1, VoxelType.CHERRY_LEAVES, VoxelType.CHERRY_WOOD); break;
                        case 1 or 2 or 3: SpruceTree.Build(random.Next(16, 22), x, y, Global.HEIGHT_MAP[x, y] + 1); break;
                        case 4 or 5 or 6 or 7: BasicDeciduousTree.Build(random.Next(13, 18), x, y, Global.HEIGHT_MAP[x, y] + 1, VoxelType.BEECH_LEAVES, VoxelType.BEECH_WOOD); break;
                        case 8 or 9 or 10 or 11: PoplarTree.Build(random.Next(22, 25), x, y, Global.HEIGHT_MAP[x, y] + 1); break;
                        case 12 or 13 or 14 or 15 or 16 or 17: BasicDeciduousTree.Build(random.Next(13, 16), x, y, Global.HEIGHT_MAP[x, y] + 1, VoxelType.MAPLE_LEAVES, VoxelType.MAPLE_WOOD); break;
                        default: BasicDeciduousTree.Build(random.Next(10, 18), x, y, Global.HEIGHT_MAP[x, y] + 1, VoxelType.OAK_LEAVES, VoxelType.OAK_WOOD); break;
                    }
                }
                else if (biome == 11) {
                    KapokTree.Build(random.Next(15, 18), x, y, Global.HEIGHT_MAP[x, y] + 1);
                }
                else if (biome == 31 || biome == 41 || biome == 51 || biome == 61 || biome == 63) {
                    SpruceTree.Build(random.Next(16, 22), x, y, Global.HEIGHT_MAP[x, y] + 1);
                }
            }
        }
    }
}
