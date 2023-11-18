//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

using System;

namespace minecraft_kurwa {
    internal static class Tree {
        internal static void GenerateTrees() {
            int amount = Global.WORLD_SIZE * Global.WORLD_SIZE * Global.TREE_DENSITY / 10_000;
            Random random = new(Global.SEED + 420);

            for (int i = 0; i < amount; i++) {
                int x = random.Next(10, Global.WORLD_SIZE - 10);
                int z = random.Next(10, Global.WORLD_SIZE - 10);

                if (Global.BIOME_MAP[x, z] == 11) {
                    BuildOakTree(random.Next(10, 14), x, z, Global.HEIGHT_MAP[x, z] + 1);
                }
            }
        }

        internal static void BuildOakTree(int height, int posX, int posY, int posZ) {
            Random random = new(Global.SEED * posX * posY * height);
            int crownRadius = (int)(height / 2.5);

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
    }
}
