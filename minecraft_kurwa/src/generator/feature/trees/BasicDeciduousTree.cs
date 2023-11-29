//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

using minecraft_kurwa.src.global;
using minecraft_kurwa.src.generator.voxels;
using System;

namespace minecraft_kurwa.src.generator.feature.trees {
    internal static class BasicDeciduousTree {
        internal static void Build(int height, int posX, int posY, int posZ, VoxelType leaves, VoxelType wood) {
            Random random = new(Settings.SEED * posX * posY * posZ * height);
            byte crownRadius = (byte)(height / 2.5f);

            for (short x = (short)(-crownRadius + 1); x < crownRadius; x++) {
                if (posX + x < 0 || posX + x >= Settings.WORLD_SIZE) continue;

                for (short y = (short)(-crownRadius + 1); y < crownRadius; y++) {
                    if (posY + y < 0 || posY + y >= Settings.WORLD_SIZE) continue;

                    for (short z = (short)(-crownRadius + 1); z < crownRadius; z++) {
                        float distanceFromCenter = (float)Math.Sqrt(x * x + y * y + z * z);

                        if (distanceFromCenter < crownRadius / 2 && random.Next(0, 2) == 0
                            || distanceFromCenter <= crownRadius && random.Next(0, 4) == 0) {
                            if (Global.VOXEL_MAP[posX + x, posY + y, posZ + height * 2 / 3 + z] == null) {
                                Global.VOXEL_MAP[posX + x, posY + y, posZ + height * 2 / 3 + z] = (byte)leaves;
                            }
                        }
                    }
                }
            }

            for (ushort z = 0; z < height * 2 / 3; z++) {
                Global.VOXEL_MAP[posX, posY, posZ + z] = (byte)wood;
            }
        }
    }
}
