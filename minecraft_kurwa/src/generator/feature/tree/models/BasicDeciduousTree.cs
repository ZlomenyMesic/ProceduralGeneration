//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

using minecraft_kurwa.src.global;
using minecraft_kurwa.src.renderer.voxels;
using System;

namespace minecraft_kurwa.src.generator.feature.tree.models {
    internal class BasicDeciduousTree : Tree {
        internal BasicDeciduousTree(ushort posX, ushort posY, ushort posZ, byte height, VoxelType leaveType, VoxelType woodType) : base(posX, posY, posZ, height, leaveType, woodType) { }

        internal override void Build() {
            Random random = new(Settings.SEED * posX * posY * posZ * height);
            byte radius = (byte)(height / 2.5f);

            for (short x = (short)(-radius + 1); x < radius; x++) {
                if (posX + x < 0 || posX + x >= Settings.WORLD_SIZE) continue;

                for (short y = (short)(-radius + 1); y < radius; y++) {
                    if (posY + y < 0 || posY + y >= Settings.WORLD_SIZE) continue;

                    for (short z = (short)(-radius + 1); z < radius; z++) {
                        float distance = (float)Math.Sqrt(x * x + y * y + z * z);

                        if (distance < radius / 2 && random.Next(0, 2) == 0
                            || distance <= radius && random.Next(0, 4) == 0) {
                            if (Global.VOXEL_MAP[posX + x, posY + y, posZ + height * 2 / 3 + z] == null) {
                                Global.VOXEL_MAP[posX + x, posY + y, posZ + height * 2 / 3 + z] = leaveType;
                            }
                        }
                    }
                }
            }

            for (ushort z = 0; z < height * 2 / 3; z++) {
                Global.VOXEL_MAP[posX, posY, posZ + z] = woodType;
            }
        }
    }
}
