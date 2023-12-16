//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

using minecraft_kurwa.src.global;
using minecraft_kurwa.src.renderer.voxels;
using System;

namespace minecraft_kurwa.src.generator.feature.tree.types {
    internal class PoplarTree : Tree {
        internal PoplarTree(ushort posX, ushort posY, ushort posZ, byte height) : base(posX, posY, posZ, height, VoxelType.POPLAR_LEAVES, VoxelType.POPLAR_WOOD) { }

        internal override void Build() {
            for (byte z = (byte)(height / Global.RANDOM.Next(4, 6)); z < height; z++) {
                if (posX + 1 < Settings.WORLD_SIZE) Global.VOXEL_MAP[posX + 1, posY, posZ + z] = leaveType;
                if (posX - 1 >= 0) Global.VOXEL_MAP[posX - 1, posY, posZ + z] = leaveType;
                if (posY + 1 < Settings.WORLD_SIZE) Global.VOXEL_MAP[posX, posY + 1, posZ + z] = leaveType;
                if (posY - 1 >= 0) Global.VOXEL_MAP[posX, posY - 1, posZ + z] = leaveType;

                if (z >= height * 1.5 / 5 && z <= height * 4.5 / 5) {
                    for (sbyte x = -2; x <= 2; x++) {
                        if (posX + x < 0 || posX + x >= Settings.WORLD_SIZE) continue;

                        for (sbyte y = -2; y <= 2; y++) {
                            if (posY + y < 0 || posY + y >= Settings.WORLD_SIZE) continue;

                            if (Math.Sqrt(x * x + y * y) <= 2 && Global.VOXEL_MAP[posX + x, posY + y, posZ + z] == null) {
                                if (Global.RANDOM.Next(0, 4) != 0) Global.VOXEL_MAP[posX + x, posY + y, posZ + z] = leaveType;
                            }
                        }
                    }
                }
            }

            for (byte z = 0; z <= height; z++) {
                Global.VOXEL_MAP[posX, posY, posZ + z] = z != height ? woodType : leaveType;
            }
        }
    }
}
