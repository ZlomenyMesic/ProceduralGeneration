//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

using minecraft_kurwa.src.global;
using minecraft_kurwa.src.renderer.voxels;
using System;

namespace minecraft_kurwa.src.generator.feature.tree.types {
    internal class AcaciaTree : Tree {
        internal AcaciaTree(ushort posX, ushort posY, ushort posZ, byte height) : base(posX, posY, posZ, height, VoxelType.ACACIA_LEAVES, VoxelType.ACACIA_WOOD) { }

        internal override void Build() {
            Random random = new(Settings.SEED * posX * posY * posZ * height);
            byte radius = (byte)((height / 2) + 1);

            for (short layer = 0; layer < (height / 10) + 2; layer++) {
                if (posZ + layer >= Settings.HEIGHT_LIMIT) break;

                for (short x = (short)(-radius - 1); x <= radius + 1; x++) {
                    if (posX + x < 0 || posX + x >= Settings.WORLD_SIZE) continue;

                    for (short y = (short)(-radius - 1); y <= radius + 1; y++) {
                        if (posY + y < 0 || posY + y >= Settings.WORLD_SIZE) continue;

                        byte distance = (byte)Math.Sqrt(x * x + y * y);
                        if ((distance < radius && random.Next(0, 8) != 0) || (distance >= radius && distance - 0.5f < radius && random.Next(0, 4) != 0)) {
                            if (Global.VOXEL_MAP[posX + x, posY + y, posZ + height + layer] == null) {
                                Global.VOXEL_MAP[posX + x, posY + y, posZ + height + layer] = leaveType;
                            }
                        }
                    }
                }

                radius--;
            }

            for (ushort z = 0; z < height; z++) {
                Global.VOXEL_MAP[posX, posY, posZ + z] = woodType;
            }
        }
    }
}
