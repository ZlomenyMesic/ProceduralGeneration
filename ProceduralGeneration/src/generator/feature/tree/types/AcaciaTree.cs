﻿//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

using minecraft_kurwa.src.global;
using minecraft_kurwa.src.global.functions;
using minecraft_kurwa.src.renderer.voxels;
using System.Numerics;

namespace minecraft_kurwa.src.generator.feature.tree.types;

internal class AcaciaTree : Tree {
    internal AcaciaTree(ushort posX, ushort posY, ushort posZ, byte height) : base(posX, posY, posZ, height, VoxelType.ACACIA_LEAVES, VoxelType.ACACIA_WOOD) { }

    internal override void Build() {
        short firstDivisionX = (short)(_posX + Global.RANDOM.Next(-1, 2));
        short firstDivisionY = (short)(_posY + Global.RANDOM.Next(-1, 2));
        short firstDivisionZ = (short)(_posZ + Global.RANDOM.Next(1, _height - 1));

        BuildBranch((short)_posX, (short)_posY, (short)_posZ, firstDivisionX, firstDivisionY, firstDivisionZ);

        Vector3 firstCrownPos = new(firstDivisionX + Global.RANDOM.Next(-3, 4), firstDivisionY + Global.RANDOM.Next(-3, 4), firstDivisionZ + Global.RANDOM.Next(1, _posZ + _height - firstDivisionZ - 1));
        BuildBranch(firstDivisionX, firstDivisionY, firstDivisionZ, (short)firstCrownPos.X, (short)firstCrownPos.Y, (short)firstCrownPos.Z);
        BuildCrown((ushort)firstCrownPos.X, (ushort)firstCrownPos.Y, (ushort)(firstCrownPos.Z + 1), (byte)(_height / 2 - Global.RANDOM.Next(0, 2)));

        if (Global.RANDOM.Next(0, 2) == 0) {
            short secondDivisionX = (short)(firstDivisionX + Global.RANDOM.Next(-1, 2));
            short secondDivisionY = (short)(firstDivisionY + Global.RANDOM.Next(-1, 2));
            short secondDivisionZ = (short)(firstDivisionZ + Global.RANDOM.Next(1, _posZ + _height - firstDivisionZ));

            BuildBranch(firstDivisionX, firstDivisionY, firstDivisionZ, secondDivisionX, secondDivisionY, secondDivisionZ);

            Vector3 secondCrownPos = new(secondDivisionX + Global.RANDOM.Next(-3, 4), secondDivisionY + Global.RANDOM.Next(-3, 4), secondDivisionZ + _posZ + _height - secondDivisionZ);
            BuildBranch(secondDivisionX, secondDivisionY, secondDivisionZ, (short)secondCrownPos.X, (short)secondCrownPos.Y, (short)secondCrownPos.Z);
            BuildCrown((ushort)secondCrownPos.X, (ushort)secondCrownPos.Y, (ushort)(secondCrownPos.Z + 1), (byte)(_height / 2 - Global.RANDOM.Next(0, 2)));

            Vector3 thirdCrownPos = new(secondDivisionX + Global.RANDOM.Next(-3, 4), secondDivisionY + Global.RANDOM.Next(-3, 4), secondDivisionZ + _posZ + _height - secondDivisionZ);
            BuildBranch(secondDivisionX, secondDivisionY, secondDivisionZ, (short)thirdCrownPos.X, (short)thirdCrownPos.Y, (short)thirdCrownPos.Z);
            BuildCrown((ushort)thirdCrownPos.X, (ushort)thirdCrownPos.Y, (ushort)(thirdCrownPos.Z + 1), (byte)(_height / 2 - Global.RANDOM.Next(0, 2)));
        } else {
            Vector3 secondCrownPos = new(firstDivisionX + Global.RANDOM.Next(-3, 4), firstDivisionY + Global.RANDOM.Next(-3, 4), Global.RANDOM.Next(firstDivisionZ, _posZ + _height));
            BuildBranch(firstDivisionX, firstDivisionY, firstDivisionZ, (short)secondCrownPos.X, (short)secondCrownPos.Y, (short)secondCrownPos.Z);
            BuildCrown((ushort)secondCrownPos.X, (ushort)secondCrownPos.Y, (ushort)(secondCrownPos.Z + 1), (byte)(_height / 2 - Global.RANDOM.Next(0, 2)));

        }
    }

    private void BuildCrown(ushort posX, ushort posY, ushort posZ, byte radius) {
        for (short layer = 0; layer < (_height / 10) + 2; layer++) {
            if (posZ + layer >= Settings.HEIGHT_LIMIT) break;

            for (short x = (short)(-radius - 1); x <= radius + 1; x++) {
                if (!World.IsInRange(posX + x)) continue;

                for (short y = (short)(-radius - 1); y <= radius + 1; y++) {
                    if (!World.IsInRange(posY + y)) continue;

                    if ((Geometry.Circle(x, y, radius) && Global.RANDOM.Next(0, 8) != 0) || (Geometry.Circle(x, y, radius + 1) && Global.RANDOM.Next(0, 4) != 0)) {
                        if (Global.VOXEL_MAP[posX + x, posY + y, posZ + layer] == null) {
                            Global.VOXEL_MAP[posX + x, posY + y, posZ + layer] = _leaveType;
                        }
                    }
                }
            }

            radius--;
        }
    }
}
