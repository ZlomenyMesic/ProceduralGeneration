//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

using System;

namespace minecraft_kurwa {
    internal static class Tree {
        internal static void GenerateTrees() {
            int amount = Global.WORLD_SIZE * Global.WORLD_SIZE * Global.TREE_DENSITY / 10_000;
            Random random = new(Global.SEED + 69);

            for (int i = 0; i < amount; i++) {
                int x = random.Next(Global.TREE_EDGE_OFFSET, Global.WORLD_SIZE - Global.TREE_EDGE_OFFSET - 1);
                int z = random.Next(Global.TREE_EDGE_OFFSET, Global.WORLD_SIZE - Global.TREE_EDGE_OFFSET - 1);
                int biome = Global.BIOME_MAP[x, z];

                if (Global.HEIGHT_MAP[x, z] <= Global.WATER_LEVEL) continue;

                if (biome == 32 || biome == 42) {
                    int treeType = random.Next(0, 12);
                    if (treeType == 0) BuildSpruceTree(random.Next(16, 22), x, z, Global.HEIGHT_MAP[x, z] + 1);
                    else if (treeType == 1) BuildBasicDeciduousTree(random.Next(13, 18), x, z, Global.HEIGHT_MAP[x, z] + 1, VoxelType.BEECH_LEAVES, VoxelType.BEECH_WOOD);
                    else if (treeType == 2 || treeType == 3) BuildBasicDeciduousTree(random.Next(13, 16), x, z, Global.HEIGHT_MAP[x, z] + 1, VoxelType.MAPLE_LEAVES, VoxelType.MAPLE_WOOD);
                    else if (treeType == 4) BuildPoplarTree(random.Next(22, 25), x, z, Global.HEIGHT_MAP[x, z] + 1);
                    else BuildBasicDeciduousTree(random.Next(10, 18), x, z, Global.HEIGHT_MAP[x, z] + 1, VoxelType.OAK_LEAVES, VoxelType.OAK_WOOD);
                }
                else if (biome == 11) {
                    BuildKapokTree(random.Next(15, 18), x, z, Global.HEIGHT_MAP[x, z] + 1);
                }
                else if (biome == 31 || biome == 41 || biome == 51) {
                    BuildSpruceTree(random.Next(16, 22), x, z, Global.HEIGHT_MAP[x, z] + 1);
                }
            }
        }

        internal static void BuildBasicDeciduousTree(int height, int posX, int posY, int posZ, VoxelType leaves, VoxelType wood) {
            Random random = new(Global.SEED * posX * posY * posZ * height);
            int crownRadius = (int)(height / 2.5f);

            for (int x = -crownRadius + 1; x < crownRadius; x++) {
                if (posX + x < 0 || posX + x >= Global.WORLD_SIZE) continue;

                for (int y = -crownRadius + 1; y < crownRadius; y++) {
                    if (posY + y < 0 || posY + y >= Global.WORLD_SIZE) continue;

                    for (int z = -crownRadius + 1; z < crownRadius; z++) {
                        float distanceFromCenter = (float)Math.Sqrt(x * x + y * y + z * z);

                        if ((distanceFromCenter < crownRadius / 2 && random.Next(0, 2) == 0)
                            || (distanceFromCenter <= crownRadius && random.Next(0, 4) == 0)) {
                            if (Global.VOXEL_MAP[posX + x, posY + y, posZ + (height * 2 / 3) + z] == null) {
                                Global.VOXEL_MAP[posX + x, posY + y, posZ + (height * 2 / 3) + z] = leaves;
                            }
                        }
                    }
                }
            }

            for (int z = 0; z < height * 2 / 3; z++) {
                Global.VOXEL_MAP[posX, posY, posZ + z] = wood;
            }
        }

        internal static void BuildKapokTree(int height, int posX, int posY, int posZ) {
            Random random = new(Global.SEED * posX * posY * posZ * height);
            int crownRadius = height / 3;

            for (int x = -crownRadius - 1; x <= crownRadius + 1; x++) {
                if (posX + x < 0 || posX + x >= Global.WORLD_SIZE) continue;

                for (int y = -crownRadius - 1; y <= crownRadius + 1; y++) {
                    if (posY + y < 0 || posY + y >= Global.WORLD_SIZE) continue;

                    for (int z = -crownRadius / 3 + 1; z <= crownRadius / 1.5; z++) {
                        float distanceFromCenterXY = (float)Math.Sqrt(x * x + y * y);

                        if ((distanceFromCenterXY > crownRadius / 2 || (distanceFromCenterXY <= crownRadius / 2 && z > height / 10)) 
                            && distanceFromCenterXY <= crownRadius && distanceFromCenterXY + Math.Abs(z) < height / 2 - 1 && random.Next(0, 2) == 0) {
                            if (Global.VOXEL_MAP[posX + x, posY + y, posZ + z + (height * 2 / 3)] == null) {
                                Global.VOXEL_MAP[posX + x, posY + y, posZ + z + (height * 2 / 3)] = VoxelType.KAPOK_LEAVES;
                            }
                        }
                    }
                }
            }

            for (int z = 0; z <= height * 2 / 3; z++) {
                Global.VOXEL_MAP[posX, posY, posZ + z] = VoxelType.KAPOK_WOOD;
            }

            for (int x = -1; x <= 1; x++) {
                if (posX + x < 0 || posX + x >= Global.WORLD_SIZE) continue;

                for (int y = -1; y <= 1; y++) {
                    if (posY + y < 0 || posY + y >= Global.WORLD_SIZE) continue;

                    for (int z = 0; z <= 1; z++) {
                        if (random.Next(0, 3) == 0) {
                            Global.VOXEL_MAP[posX + x, posY + y, posZ + z + (height * 2 / 3)] = VoxelType.KAPOK_WOOD;
                            if (posX + x - 1 >= 0 && x == -1 && y == 0 && z == 0 && random.Next(0, 2) == 0) Global.VOXEL_MAP[posX + x - 1, posY + y, posZ + z + (height * 2 / 3)] = VoxelType.KAPOK_WOOD;
                            if (posX + x + 1 < Global.WORLD_SIZE && x == 1 && y == 0 && z == 0 && random.Next(0, 2) == 0) Global.VOXEL_MAP[posX + x + 1, posY + y, posZ + z + (height * 2 / 3)] = VoxelType.KAPOK_WOOD;
                            if (posY + y - 1 >= 0 && x == 0 && y == -1 && z == 0 && random.Next(0, 2) == 0) Global.VOXEL_MAP[posX + x, posY + y - 1, posZ + z + (height * 2 / 3)] = VoxelType.KAPOK_WOOD;
                            if (posY + y + 1 < Global.WORLD_SIZE && x == 0 && y == 1 && z == 0 && random.Next(0, 2) == 0) Global.VOXEL_MAP[posX + x - 1, posY + y + 1, posZ + z + (height * 2 / 3)] = VoxelType.KAPOK_WOOD;
                        }
                    }
                }
            }
        }

        internal static void BuildSpruceTree(int height, int posX, int posY, int posZ) {
            Random random = new(Global.SEED * posX * posY * posZ * height);
            float diameter = height * 2 / 5;
            int bottomStart = height / random.Next(4, 6);

            for (int z = bottomStart; z < height; z += 2) {
                for (int x = (int)(-diameter / 2); x <= diameter / 2; x++) {
                    if (posX + x < 0 || posX + x >= Global.WORLD_SIZE) continue;

                    for (int y = (int)(-diameter / 2); y <= diameter / 2; y++) {
                        if (posY + y < 0 || posY + y >= Global.WORLD_SIZE) continue;

                        float distanceFromCenter = (float)Math.Sqrt(x * x + y * y);
                        if (distanceFromCenter < diameter / 2 && random.Next(0, 6) != 0) {
                            if (Global.VOXEL_MAP[posX + x, posY + y, posZ + z] == null) {
                                Global.VOXEL_MAP[posX + x, posY + y, posZ + z] = VoxelType.SPRUCE_LEAVES;
                            }
                        }
                    }
                }
                diameter -= 0.8f;
            }

            for (int z = 0; z <= height; z++) {
                if (z < height * 4 / 5) {
                    Global.VOXEL_MAP[posX, posY, posZ + z] = VoxelType.SPRUCE_WOOD;
                }
                else {
                    Global.VOXEL_MAP[posX, posY, posZ + z] = VoxelType.SPRUCE_LEAVES;
                }
                if (z >= bottomStart && z < height * 4 / 5) {
                    if (posX + 1 < Global.WORLD_SIZE) Global.VOXEL_MAP[posX + 1, posY, posZ + z] = VoxelType.SPRUCE_LEAVES;
                    if (posX - 1 >= 0) Global.VOXEL_MAP[posX - 1, posY, posZ + z] = VoxelType.SPRUCE_LEAVES;
                    if (posY + 1 < Global.WORLD_SIZE) Global.VOXEL_MAP[posX, posY + 1, posZ + z] = VoxelType.SPRUCE_LEAVES;
                    if (posY - 1 >= 0) Global.VOXEL_MAP[posX, posY - 1, posZ + z] = VoxelType.SPRUCE_LEAVES;
                }
            }
        }

        internal static void BuildPoplarTree(int height, int posX, int posY, int posZ) {
            Random random = new(Global.SEED * posX * posY * posZ * height);
            for (int z = height / random.Next(4, 6); z < height; z++) {
                if (posX + 1 < Global.WORLD_SIZE) Global.VOXEL_MAP[posX + 1, posY, posZ + z] = VoxelType.POPLAR_LEAVES;
                if (posX - 1 >= 0) Global.VOXEL_MAP[posX - 1, posY, posZ + z] = VoxelType.POPLAR_LEAVES;
                if (posY + 1 < Global.WORLD_SIZE) Global.VOXEL_MAP[posX, posY + 1, posZ + z] = VoxelType.POPLAR_LEAVES;
                if (posY - 1 >= 0) Global.VOXEL_MAP[posX, posY - 1, posZ + z] = VoxelType.POPLAR_LEAVES;

                if (z >= height * 1.5 / 5 && z <= height * 4.5 / 5) {
                    for (int x = -2; x <= 2; x++) {
                        for (int y = -2; y <= 2; y++) {
                            if (Math.Sqrt(x * x + y * y) <= 2 && Global.VOXEL_MAP[posX + x, posY + y, posZ + z] == null) {
                                if (random.Next(0, 4) != 0) Global.VOXEL_MAP[posX + x, posY + y, posZ + z] = VoxelType.POPLAR_LEAVES;
                            }
                        }
                    }
                }
            }

            for (int z = 0; z <= height; z++) {
                Global.VOXEL_MAP[posX, posY, posZ + z] = z != height ? VoxelType.POPLAR_WOOD : VoxelType.POPLAR_LEAVES;
            }
        }
    }
}
