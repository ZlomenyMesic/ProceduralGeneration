//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

using minecraft_kurwa.src.global;
using minecraft_kurwa.src.global.functions;
using minecraft_kurwa.src.renderer.voxels;
using System;
using System.Numerics;

namespace minecraft_kurwa.src.generator.feature.tree.types;

internal class PineTree : Tree {
    internal PineTree(ushort posX, ushort posY, ushort posZ, byte height) : base(posX, posY, posZ, height, VoxelType.PINE_LEAVES, VoxelType.PINE_WOOD) { }

    internal override void Build() {
        Vector3[] crowns = new Vector3[_height * 2];
        byte crownCount = 0;

        BuildBranch((short)_posX, (short)_posY, (short)_posZ, (short)_posX, (short)_posY, (short)(_posZ + _height - 3));

        crowns[crownCount++] = new Vector3(_posX, _posY, _posZ + _height);
        crowns[crownCount++] = new Vector3(_posX, _posY, _posZ + _height - 2);

        for (ushort z = (ushort)(_height / 2); z < _height; z++) {
            for (byte i = 0; i < 2; i++) {
                if (Global.RANDOM.Next(0, 3) != 0) {
                    crowns[crownCount] = new Vector3(_posX + Global.RANDOM.Next(-2, 3), _posY + Global.RANDOM.Next(-2, 3), _posZ + z + Global.RANDOM.Next(-1, 2));
                    BuildBranch((short)_posX, (short)_posY, (short)(_posZ + z), (short)crowns[crownCount].X, (short)crowns[crownCount].Y, (short)crowns[crownCount].Z);
                    crownCount++;
                }
            }
        }

        for (byte i = 0; i < crownCount; i++) {
            BuildCrown((ushort)crowns[i].X, (ushort)crowns[i].Y, (ushort)crowns[i].Z);
        }
    }

    private void BuildCrown(ushort posX, ushort posY, ushort posZ) {
        short sizeX = (short)(Global.RANDOM.Next(3, 8) - 1);
        short sizeY = (short)(Global.RANDOM.Next(3, 8) - 1);

        for (short x = (short)(-sizeX / 2); x <= sizeX / 2; x++) {
            if (!World.IsInRange(posX + x)) continue;

            for (short y = (short)(-sizeY / 2); y <= sizeY / 2; y++) {
                if (!World.IsInRange(posY + y)) continue;

                for (short z = -1; z <= 1; z++) {
                    if (posZ + z < 0 || posZ + z >= Settings.HEIGHT_LIMIT) continue;

                    if (Geometry.Ellipsoid(x - 1, y - 1, z, sizeX, sizeY, 1)) {
                        if (Global.VOXEL_MAP[posX + x, posY + y, posZ + z] == null && Global.RANDOM.Next(0, 4) == 0) {
                            Global.VOXEL_MAP[posX + x, posY + y, posZ + z] = _leaveType;
                        }
                    }
                }
            }
        }
    }
}
