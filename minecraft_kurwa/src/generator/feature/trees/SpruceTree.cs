//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

using minecraft_kurwa.src.global;
using minecraft_kurwa.src.voxels;
using System;

namespace minecraft_kurwa.src.trees {
    internal static class SpruceTree {
        internal static void Build(int height, int posX, int posY, int posZ) {
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
                } else {
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
    }
}
