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
                int x = random.Next(6, Global.WORLD_SIZE - 5);
                int z = random.Next(6, Global.WORLD_SIZE - 5);
                int biome = Global.BIOME_MAP[x, z];

                if (biome == 32 || biome == 42) {
                    BuildOakTree(random.Next(10, 18), x, z, Global.HEIGHT_MAP[x, z] + 1);
                }
                else if (biome == 11) {
                    BuildKapokTree(random.Next(15, 18), x, z, Global.HEIGHT_MAP[x, z] + 1);
                }
            }
        }

        internal static void BuildOakTree(int height, int posX, int posY, int posZ) {
            Random random = new(Global.SEED * posX * posY * posZ * height);
            int crownRadius = (int)(height / 2.5f);

            for (int x = -crownRadius + 1; x < crownRadius; x++) {
                for (int y = -crownRadius + 1; y < crownRadius; y++) {
                    for (int z = -crownRadius + 1; z < crownRadius; z++) {
                        float distanceFromCenter = (float)Math.Sqrt(x * x + y * y + z * z);
                        if ((distanceFromCenter < crownRadius / 2 && random.Next(0, 2) == 0)
                            || (distanceFromCenter <= crownRadius && random.Next(0, 4) == 0)) {
                            if (Global.VOXEL_MAP[posX + x, posY + y, posZ + (height * 2 / 3) + z] == null) {
                                Global.VOXEL_MAP[posX + x, posY + y, posZ + (height * 2 / 3) + z] = VoxelType.OAK_LEAVES;
                            }
                        }
                    }
                }
            }

            for (int z = posZ; z < posZ + (height * 2 / 3); z++) {
                Global.VOXEL_MAP[posX, posY, z] = VoxelType.OAK_WOOD;
            }
        }

        internal static void BuildKapokTree(int height, int posX, int posY, int posZ) {
            Random random = new(Global.SEED * posX * posY * posZ * height);
            int crownRadius = height / 3;

            for (int x = -crownRadius - 1; x <= crownRadius + 1; x++) {
                for (int y = -crownRadius - 1; y <= crownRadius + 1; y++) {
                    for (int z = -crownRadius / 3 + 1; z <= crownRadius / 1.5; z++) {
                        float distanceFromCenterXY = (float)Math.Sqrt(x * x + y * y);
                        if ((distanceFromCenterXY > crownRadius / 2 || (distanceFromCenterXY <= crownRadius / 2 && z > height / 10)) 
                            && distanceFromCenterXY <= crownRadius && distanceFromCenterXY + Math.Abs(z) < height / 2 - 1 && random.Next(0, 3) == 0) {
                            if (Global.VOXEL_MAP[posX + x, posY + y, posZ + z + (height * 2 / 3)] == null) {
                                Global.VOXEL_MAP[posX + x, posY + y, posZ + z + (height * 2 / 3)] = VoxelType.KAPOK_LEAVES;
                            }
                        }
                    }
                }
            }

            for (int z = posZ; z <= posZ + (height * 2 / 3); z++) {
                Global.VOXEL_MAP[posX, posY, z] = VoxelType.KAPOK_WOOD;
            }

            for (int x = -1; x <= 1; x++) {
                for (int y = -1; y <= 1; y++) {
                    for (int z = 0; z <= 1; z++) {
                        if (random.Next(0, 3) == 0) {
                            Global.VOXEL_MAP[posX + x, posY + y, posZ + z + (height * 2 / 3)] = VoxelType.KAPOK_WOOD;
                            if (x == -1 && y == 0 && z == 0 && random.Next(0, 2) == 0) Global.VOXEL_MAP[posX + x - 1, posY + y, posZ + z + (height * 2 / 3)] = VoxelType.KAPOK_WOOD;
                            if (x == 1 && y == 0 && z == 0 && random.Next(0, 2) == 0) Global.VOXEL_MAP[posX + x + 1, posY + y, posZ + z + (height * 2 / 3)] = VoxelType.KAPOK_WOOD;
                            if (x == 0 && y == -1 && z == 0 && random.Next(0, 2) == 0) Global.VOXEL_MAP[posX + x, posY + y - 1, posZ + z + (height * 2 / 3)] = VoxelType.KAPOK_WOOD;
                            if (x == 0 && y == 1 && z == 0 && random.Next(0, 2) == 0) Global.VOXEL_MAP[posX + x - 1, posY + y + 1, posZ + z + (height * 2 / 3)] = VoxelType.KAPOK_WOOD;
                        }
                    }
                }
            }
        }
    }
}
