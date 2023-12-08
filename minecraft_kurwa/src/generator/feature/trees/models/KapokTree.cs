//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

using minecraft_kurwa.src.global;
using minecraft_kurwa.src.renderer.voxels;
using System;

namespace minecraft_kurwa.src.generator.feature.trees.models {
    internal class KapokTree : Tree {
        internal KapokTree(ushort posX, ushort posY, ushort posZ, byte height) : base(posX, posY, posZ, height, VoxelType.KAPOK_LEAVES, VoxelType.KAPOK_WOOD) { }

        internal override void Build() {
            Random random = new(Settings.SEED * posX * posY * posZ * height);
            byte crownRadius = (byte)(height / 3);

            for (short x = (short)(-crownRadius - 1); x <= crownRadius + 1; x++) {
                if (posX + x < 0 || posX + x >= Settings.WORLD_SIZE) continue;

                for (short y = (short)(-crownRadius - 1); y <= crownRadius + 1; y++) {
                    if (posY + y < 0 || posY + y >= Settings.WORLD_SIZE) continue;

                    for (short z = (short)(-crownRadius / 3 + 1); z <= crownRadius / 1.5; z++) {
                        float distanceFromCenterXY = (float)Math.Sqrt(x * x + y * y);

                        if ((distanceFromCenterXY > crownRadius / 2 || distanceFromCenterXY <= crownRadius / 2 && z > height / 10)
                            && distanceFromCenterXY <= crownRadius && distanceFromCenterXY + Math.Abs(z) < height / 2 - 1 && random.Next(0, 2) == 0) {
                            if (Global.VOXEL_MAP[posX + x, posY + y, posZ + z + height * 2 / 3] == null) {
                                Global.VOXEL_MAP[posX + x, posY + y, posZ + z + height * 2 / 3] = leaveType;
                            }
                        }
                    }
                }
            }

            for (ushort z = 0; z <= height * 2 / 3; z++) {
                Global.VOXEL_MAP[posX, posY, posZ + z] = woodType;
            }

            for (sbyte x = -1; x <= 1; x++) {
                if (posX + x < 0 || posX + x >= Settings.WORLD_SIZE) continue;

                for (sbyte y = -1; y <= 1; y++) {
                    if (posY + y < 0 || posY + y >= Settings.WORLD_SIZE) continue;

                    for (sbyte z = 0; z <= 1; z++) {
                        if (random.Next(0, 3) == 0) {
                            Global.VOXEL_MAP[posX + x, posY + y, posZ + z + height * 2 / 3] = woodType;
                            if (posX + x - 1 >= 0 && x == -1 && y == 0 && z == 0 && random.Next(0, 2) == 0) Global.VOXEL_MAP[posX + x - 1, posY + y, posZ + z + height * 2 / 3] = woodType;
                            if (posX + x + 1 < Settings.WORLD_SIZE && x == 1 && y == 0 && z == 0 && random.Next(0, 2) == 0) Global.VOXEL_MAP[posX + x + 1, posY + y, posZ + z + height * 2 / 3] = woodType;
                            if (posY + y - 1 >= 0 && x == 0 && y == -1 && z == 0 && random.Next(0, 2) == 0) Global.VOXEL_MAP[posX + x, posY + y - 1, posZ + z + height * 2 / 3] = woodType;
                            if (posY + y + 1 < Settings.WORLD_SIZE && x == 0 && y == 1 && z == 0 && random.Next(0, 2) == 0) Global.VOXEL_MAP[posX + x - 1, posY + y + 1, posZ + z + height * 2 / 3] = woodType;
                        }
                    }
                }
            }
        }
    }
}
