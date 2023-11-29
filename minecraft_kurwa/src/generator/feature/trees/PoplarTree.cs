//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

using minecraft_kurwa.src.global;
using minecraft_kurwa.src.voxels;
using System;

namespace minecraft_kurwa.src.generator.feature.trees {
    internal static class PoplarTree {
        internal static void Build(int height, int posX, int posY, int posZ) {
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
