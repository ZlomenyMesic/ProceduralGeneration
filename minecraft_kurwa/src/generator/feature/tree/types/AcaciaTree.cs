//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

using minecraft_kurwa.src.global;
using minecraft_kurwa.src.renderer.voxels;
using System;
using System.Numerics;

namespace minecraft_kurwa.src.generator.feature.tree.types {
    internal class AcaciaTree : Tree {
        internal AcaciaTree(ushort posX, ushort posY, ushort posZ, byte height) : base(posX, posY, posZ, height, VoxelType.ACACIA_LEAVES, VoxelType.ACACIA_WOOD) { }

        internal override void Build() {
            short firstDivisionX = (short)(posX + Global.RANDOM.Next(-1, 2));
            short firstDivisionY = (short)(posY + Global.RANDOM.Next(-1, 2));
            short firstDivisionZ = (short)(posZ + Global.RANDOM.Next(1, height - 1));

            BuildBranch((short)posX, (short)posY, (short)posZ, firstDivisionX, firstDivisionY, firstDivisionZ);

            Vector3 firstCrownPos = new(firstDivisionX + Global.RANDOM.Next(-3, 4), firstDivisionY + Global.RANDOM.Next(-3, 4), firstDivisionZ + Global.RANDOM.Next(1, posZ + height - firstDivisionZ - 1));
            BuildBranch(firstDivisionX, firstDivisionY, firstDivisionZ, (short)firstCrownPos.X, (short)firstCrownPos.Y, (short)firstCrownPos.Z);
            BuildCrown((ushort)firstCrownPos.X, (ushort)firstCrownPos.Y, (ushort)(firstCrownPos.Z + 1), (byte)(height / 2 - Global.RANDOM.Next(0, 2)));

            if (Global.RANDOM.Next(0, 2) == 0) {
                short secondDivisionX = (short)(firstDivisionX + Global.RANDOM.Next(-1, 2));
                short secondDivisionY = (short)(firstDivisionY + Global.RANDOM.Next(-1, 2));
                short secondDivisionZ = (short)(firstDivisionZ + Global.RANDOM.Next(1, posZ + height - firstDivisionZ));

                BuildBranch(firstDivisionX, firstDivisionY, firstDivisionZ, secondDivisionX, secondDivisionY, secondDivisionZ);

                Vector3 secondCrownPos = new(secondDivisionX + Global.RANDOM.Next(-3, 4), secondDivisionY + Global.RANDOM.Next(-3, 4), secondDivisionZ + posZ + height - secondDivisionZ);
                BuildBranch(secondDivisionX, secondDivisionY, secondDivisionZ, (short)secondCrownPos.X, (short)secondCrownPos.Y, (short)secondCrownPos.Z);
                BuildCrown((ushort)secondCrownPos.X, (ushort)secondCrownPos.Y, (ushort)(secondCrownPos.Z + 1), (byte)(height / 2 - Global.RANDOM.Next(0, 2)));

                Vector3 thirdCrownPos = new(secondDivisionX + Global.RANDOM.Next(-3, 4), secondDivisionY + Global.RANDOM.Next(-3, 4), secondDivisionZ + posZ + height - secondDivisionZ);
                BuildBranch(secondDivisionX, secondDivisionY, secondDivisionZ, (short)thirdCrownPos.X, (short)thirdCrownPos.Y, (short)thirdCrownPos.Z);
                BuildCrown((ushort)thirdCrownPos.X, (ushort)thirdCrownPos.Y, (ushort)(thirdCrownPos.Z + 1), (byte)(height / 2 - Global.RANDOM.Next(0, 2)));
            } else {
                Vector3 secondCrownPos = new(firstDivisionX + Global.RANDOM.Next(-3, 4), firstDivisionY + Global.RANDOM.Next(-3, 4), Global.RANDOM.Next(firstDivisionZ, posZ + height));
                BuildBranch(firstDivisionX, firstDivisionY, firstDivisionZ, (short)secondCrownPos.X, (short)secondCrownPos.Y, (short)secondCrownPos.Z);
                BuildCrown((ushort)secondCrownPos.X, (ushort)secondCrownPos.Y, (ushort)(secondCrownPos.Z + 1), (byte)(height / 2 - Global.RANDOM.Next(0, 2)));

            }
        }

        private void BuildCrown(ushort posX, ushort posY, ushort posZ, byte radius) {
            for (short layer = 0; layer < (height / 10) + 2; layer++) {
                if (posZ + layer >= Settings.HEIGHT_LIMIT) break;

                for (short x = (short)(-radius - 1); x <= radius + 1; x++) {
                    if (posX + x < 0 || posX + x >= Settings.WORLD_SIZE) continue;

                    for (short y = (short)(-radius - 1); y <= radius + 1; y++) {
                        if (posY + y < 0 || posY + y >= Settings.WORLD_SIZE) continue;

                        byte distance = (byte)Math.Sqrt(x * x + y * y);
                        if ((distance < radius && Global.RANDOM.Next(0, 8) != 0) || (distance >= radius && distance - 0.5f < radius && Global.RANDOM.Next(0, 4) != 0)) {
                            if (Global.VOXEL_MAP[posX + x, posY + y, posZ + layer] == null) {
                                Global.VOXEL_MAP[posX + x, posY + y, posZ + layer] = leaveType;
                            }
                        }
                    }
                }

                radius--;
            }
        }
    }
}
