//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

using SharpDX.DirectWrite;
using System;

namespace minecraft_kurwa {
    internal static class Tree {
        internal static void GenerateTrees() {
            ushort amount = Settings.WORLD_SIZE * Settings.WORLD_SIZE * Settings.TREE_DENSITY / 10_000;
            Random random = new(Settings.SEED + 69);

            for (ushort i = 0; i < amount; i++) {
                ushort x = (ushort)random.Next(Global.TREE_EDGE_OFFSET, Settings.WORLD_SIZE - Global.TREE_EDGE_OFFSET - 1);
                ushort z = (ushort)random.Next(Global.TREE_EDGE_OFFSET, Settings.WORLD_SIZE - Global.TREE_EDGE_OFFSET - 1);
                byte biome = Global.BIOME_MAP[x, z];

                if (Global.HEIGHT_MAP[x, z] <= Settings.WATER_LEVEL) continue;

                if (biome == 32 || biome == 42) {
                    switch (random.Next(0, 50)) {
                        case 0: BuildBasicDeciduousTree(random.Next(15, 19), x, z, Global.HEIGHT_MAP[x, z] + 1, VoxelType.CHERRY_LEAVES, VoxelType.CHERRY_WOOD); break;
                        case 1 or 2 or 3: BuildSpruceTree(random.Next(16, 22), x, z, Global.HEIGHT_MAP[x, z] + 1); break;
                        case 4 or 5 or 6 or 7: BuildBasicDeciduousTree(random.Next(13, 18), x, z, Global.HEIGHT_MAP[x, z] + 1, VoxelType.BEECH_LEAVES, VoxelType.BEECH_WOOD); break;
                        case 8 or 9 or 10 or 11: BuildPoplarTree(random.Next(22, 25), x, z, Global.HEIGHT_MAP[x, z] + 1); break;
                        case 12 or 13 or 14 or 15 or 16 or 17: BuildBasicDeciduousTree(random.Next(13, 16), x, z, Global.HEIGHT_MAP[x, z] + 1, VoxelType.MAPLE_LEAVES, VoxelType.MAPLE_WOOD); break;
                        default: BuildBasicDeciduousTree(random.Next(10, 18), x, z, Global.HEIGHT_MAP[x, z] + 1, VoxelType.OAK_LEAVES, VoxelType.OAK_WOOD); break;
                    }
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
            Random random = new(Settings.SEED * posX * posY * posZ * height);
            byte crownRadius = (byte)(height / 2.5f);

            for (short x = (short)(-crownRadius + 1); x < crownRadius; x++) {
                if (posX + x < 0 || posX + x >= Settings.WORLD_SIZE) continue;

                for (short y = (short)(-crownRadius + 1); y < crownRadius; y++) {
                    if (posY + y < 0 || posY + y >= Settings.WORLD_SIZE) continue;

                    for (short z = (short)(-crownRadius + 1); z < crownRadius; z++) {
                        float distanceFromCenter = (float)Math.Sqrt(x * x + y * y + z * z);

                        if ((distanceFromCenter < crownRadius / 2 && random.Next(0, 2) == 0)
                            || (distanceFromCenter <= crownRadius && random.Next(0, 4) == 0)) {
                            if (Global.VOXEL_MAP[posX + x, posY + y, posZ + (height * 2 / 3) + z] == null) {
                                Global.VOXEL_MAP[posX + x, posY + y, posZ + (height * 2 / 3) + z] = (byte) leaves;
                            }
                        }
                    }
                }
            }

            for (ushort z = 0; z < height * 2 / 3; z++) {
                Global.VOXEL_MAP[posX, posY, posZ + z] = (byte) wood;
            }
        }

        internal static void BuildKapokTree(int height, int posX, int posY, int posZ) {
            Random random = new(Settings.SEED * posX * posY * posZ * height);
            byte crownRadius = (byte)(height / 3);

            for (short x = (short)(-crownRadius - 1); x <= crownRadius + 1; x++) {
                if (posX + x < 0 || posX + x >= Settings.WORLD_SIZE) continue;

                for (short y = (short)(-crownRadius - 1); y <= crownRadius + 1; y++) {
                    if (posY + y < 0 || posY + y >= Settings.WORLD_SIZE) continue;

                    for (short z = (short)(-crownRadius / 3 + 1); z <= crownRadius / 1.5; z++) {
                        float distanceFromCenterXY = (float)Math.Sqrt(x * x + y * y);

                        if ((distanceFromCenterXY > crownRadius / 2 || (distanceFromCenterXY <= crownRadius / 2 && z > height / 10)) 
                            && distanceFromCenterXY <= crownRadius && distanceFromCenterXY + Math.Abs(z) < height / 2 - 1 && random.Next(0, 2) == 0) {
                            if (Global.VOXEL_MAP[posX + x, posY + y, posZ + z + (height * 2 / 3)] == null) {
                                Global.VOXEL_MAP[posX + x, posY + y, posZ + z + (height * 2 / 3)] = (byte)VoxelType.KAPOK_LEAVES;
                            }
                        }
                    }
                }
            }

            for (ushort z = 0; z <= height * 2 / 3; z++) {
                Global.VOXEL_MAP[posX, posY, posZ + z] = (byte)VoxelType.KAPOK_WOOD;
            }

            for (sbyte x = -1; x <= 1; x++) {
                if (posX + x < 0 || posX + x >= Settings.WORLD_SIZE) continue;

                for (sbyte y = -1; y <= 1; y++) {
                    if (posY + y < 0 || posY + y >= Settings.WORLD_SIZE) continue;

                    for (sbyte z = 0; z <= 1; z++) {
                        if (random.Next(0, 3) == 0) {
                            Global.VOXEL_MAP[posX + x, posY + y, posZ + z + (height * 2 / 3)] = (byte)VoxelType.KAPOK_WOOD;
                            if (posX + x - 1 >= 0 && x == -1 && y == 0 && z == 0 && random.Next(0, 2) == 0) Global.VOXEL_MAP[posX + x - 1, posY + y, posZ + z + (height * 2 / 3)] = (byte)VoxelType.KAPOK_WOOD;
                            if (posX + x + 1 < Settings.WORLD_SIZE && x == 1 && y == 0 && z == 0 && random.Next(0, 2) == 0) Global.VOXEL_MAP[posX + x + 1, posY + y, posZ + z + (height * 2 / 3)] = (byte)VoxelType.KAPOK_WOOD;
                            if (posY + y - 1 >= 0 && x == 0 && y == -1 && z == 0 && random.Next(0, 2) == 0) Global.VOXEL_MAP[posX + x, posY + y - 1, posZ + z + (height * 2 / 3)] = (byte)VoxelType.KAPOK_WOOD;
                            if (posY + y + 1 < Settings.WORLD_SIZE && x == 0 && y == 1 && z == 0 && random.Next(0, 2) == 0) Global.VOXEL_MAP[posX + x - 1, posY + y + 1, posZ + z + (height * 2 / 3)] = (byte)VoxelType.KAPOK_WOOD;
                        }
                    }
                }
            }
        }

        internal static void BuildSpruceTree(int height, int posX, int posY, int posZ) {
            Random random = new(Settings.SEED * posX * posY * posZ * height);
            float diameter = height * 2 / 5;
            byte bottomStart = (byte)(height / random.Next(4, 6));

            for (short z = bottomStart; z < height; z += 2) {
                for (short x = (short)(-diameter / 2); x <= diameter / 2; x++) {
                    if (posX + x < 0 || posX + x >= Settings.WORLD_SIZE) continue;

                    for (short y = (short)(-diameter / 2); y <= diameter / 2; y++) {
                        if (posY + y < 0 || posY + y >= Settings.WORLD_SIZE) continue;

                        float distanceFromCenter = (float)Math.Sqrt(x * x + y * y);
                        if (distanceFromCenter < diameter / 2 && random.Next(0, 6) != 0) {
                            if (Global.VOXEL_MAP[posX + x, posY + y, posZ + z] == null) {
                                Global.VOXEL_MAP[posX + x, posY + y, posZ + z] = (byte)VoxelType.SPRUCE_LEAVES;
                            }
                        }
                    }
                }
                diameter -= 0.8f;
            }

            for (byte z = 0; z <= height; z++) {
                if (z < height * 4 / 5) {
                    Global.VOXEL_MAP[posX, posY, posZ + z] = (byte)VoxelType.SPRUCE_WOOD;
                }
                else {
                    Global.VOXEL_MAP[posX, posY, posZ + z] = (byte)VoxelType.SPRUCE_LEAVES;
                }
                if (z >= bottomStart && z < height * 4 / 5) {
                    if (posX + 1 < Settings.WORLD_SIZE) Global.VOXEL_MAP[posX + 1, posY, posZ + z] = (byte)VoxelType.SPRUCE_LEAVES;
                    if (posX - 1 >= 0) Global.VOXEL_MAP[posX - 1, posY, posZ + z] = (byte)VoxelType.SPRUCE_LEAVES;
                    if (posY + 1 < Settings.WORLD_SIZE) Global.VOXEL_MAP[posX, posY + 1, posZ + z] = (byte)VoxelType.SPRUCE_LEAVES;
                    if (posY - 1 >= 0) Global.VOXEL_MAP[posX, posY - 1, posZ + z] = (byte)VoxelType.SPRUCE_LEAVES;
                }
            }
        }

        internal static void BuildPoplarTree(int height, int posX, int posY, int posZ) {
            Random random = new(Settings.SEED * posX * posY * posZ * height);
            for (byte z = (byte)(height / random.Next(4, 6)); z < height; z++) {
                if (posX + 1 < Settings.WORLD_SIZE) Global.VOXEL_MAP[posX + 1, posY, posZ + z] = (byte)VoxelType.POPLAR_LEAVES;
                if (posX - 1 >= 0) Global.VOXEL_MAP[posX - 1, posY, posZ + z] = (byte)VoxelType.POPLAR_LEAVES;
                if (posY + 1 < Settings.WORLD_SIZE) Global.VOXEL_MAP[posX, posY + 1, posZ + z] = (byte)VoxelType.POPLAR_LEAVES;
                if (posY - 1 >= 0) Global.VOXEL_MAP[posX, posY - 1, posZ + z] = (byte)VoxelType.POPLAR_LEAVES;

                if (z >= height * 1.5 / 5 && z <= height * 4.5 / 5) {
                    for (sbyte x = -2; x <= 2; x++) {
                        if (posX + x < 0 || posX + x >= Settings.WORLD_SIZE) continue;

                        for (sbyte y = -2; y <= 2; y++) {
                            if (posY + y < 0 || posY + y >= Settings.WORLD_SIZE) continue;

                            if (Math.Sqrt(x * x + y * y) <= 2 && Global.VOXEL_MAP[posX + x, posY + y, posZ + z] == null) {
                                if (random.Next(0, 4) != 0) Global.VOXEL_MAP[posX + x, posY + y, posZ + z] = (byte)VoxelType.POPLAR_LEAVES;
                            }
                        }
                    }
                }
            }

            for (byte z = 0; z <= height; z++) {
                Global.VOXEL_MAP[posX, posY, posZ + z] = z != height ? (byte)VoxelType.POPLAR_WOOD : (byte)VoxelType.POPLAR_LEAVES;
            }
        }
    }
}
