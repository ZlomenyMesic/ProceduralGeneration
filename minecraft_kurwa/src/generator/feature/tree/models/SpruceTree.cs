//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

using minecraft_kurwa.src.global;
using minecraft_kurwa.src.renderer.voxels;
using System;

namespace minecraft_kurwa.src.generator.feature.tree.models {
    internal class SpruceTree : Tree {
        internal SpruceTree(ushort posX, ushort posY, ushort posZ, byte height) : base(posX, posY, posZ, height, VoxelType.SPRUCE_LEAVES, VoxelType.SPRUCE_WOOD) { }

        internal override void Build() {
            Random random = new(Settings.SEED * posX * posY * posZ * height);
            float diameter = height * 2 / 5;
            byte bottom = (byte)(height / random.Next(4, 6));

            for (short z = bottom; z < height; z += 2) {
                for (short x = (short)(-diameter / 2); x <= diameter / 2; x++) {
                    if (posX + x < 0 || posX + x >= Settings.WORLD_SIZE) continue;

                    for (short y = (short)(-diameter / 2); y <= diameter / 2; y++) {
                        if (posY + y < 0 || posY + y >= Settings.WORLD_SIZE) continue;

                        float distance = (float)Math.Sqrt(x * x + y * y);
                        if (distance < diameter / 2 && random.Next(0, 6) != 0) {
                            if (Global.VOXEL_MAP[posX + x, posY + y, posZ + z] == null) {
                                Global.VOXEL_MAP[posX + x, posY + y, posZ + z] = leaveType;
                            }
                        }
                    }
                }
                diameter -= 0.8f;
            }

            for (byte z = 0; z <= height; z++) {
                if (z < height * 4 / 5) {
                    Global.VOXEL_MAP[posX, posY, posZ + z] = woodType;
                } else {
                    Global.VOXEL_MAP[posX, posY, posZ + z] = leaveType;
                }
                if (z >= bottom && z < height * 4 / 5) {
                    if (posX + 1 < Settings.WORLD_SIZE) Global.VOXEL_MAP[posX + 1, posY, posZ + z] = leaveType;
                    if (posX - 1 >= 0) Global.VOXEL_MAP[posX - 1, posY, posZ + z] = leaveType;
                    if (posY + 1 < Settings.WORLD_SIZE) Global.VOXEL_MAP[posX, posY + 1, posZ + z] = leaveType;
                    if (posY - 1 >= 0) Global.VOXEL_MAP[posX, posY - 1, posZ + z] = leaveType;
                }
            }
        }
    }
}
