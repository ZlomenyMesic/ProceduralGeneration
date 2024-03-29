﻿//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

using minecraft_kurwa.src.global;
using minecraft_kurwa.src.global.functions;
using minecraft_kurwa.src.renderer.voxels;

namespace minecraft_kurwa.src.generator.feature.tree.types;

internal class PoplarTree : Tree {
    internal PoplarTree(ushort posX, ushort posY, ushort posZ, byte height) : base(posX, posY, posZ, height, VoxelType.POPLAR_LEAVES, VoxelType.POPLAR_WOOD) { }

    internal override void Build() {
        for (byte z = (byte)(_height / Global.RANDOM.Next(4, 6)); z < _height; z++) {
            if (_posX + 1 < Settings.WORLD_SIZE) Global.VOXEL_MAP[_posX + 1, _posY, _posZ + z] = _leaveType;
            if (_posX - 1 >= 0) Global.VOXEL_MAP[_posX - 1, _posY, _posZ + z] = _leaveType;
            if (_posY + 1 < Settings.WORLD_SIZE) Global.VOXEL_MAP[_posX, _posY + 1, _posZ + z] = _leaveType;
            if (_posY - 1 >= 0) Global.VOXEL_MAP[_posX, _posY - 1, _posZ + z] = _leaveType;

            if (z >= _height * 1.5 / 5 && z <= _height * 4.5 / 5) {
                for (sbyte x = -2; x <= 2; x++) {
                    if (!World.IsInRange(_posX + x)) continue;

                    for (sbyte y = -2; y <= 2; y++) {
                        if (!World.IsInRange(_posY + y)) continue;

                        if (Geometry.Circle(x, y, 2) && Global.VOXEL_MAP[_posX + x, _posY + y, _posZ + z] == null) {
                            if (Global.RANDOM.Next(0, 4) != 0) Global.VOXEL_MAP[_posX + x, _posY + y, _posZ + z] = _leaveType;
                        }
                    }
                }
            }
        }

        BuildBranch((short)_posX, (short)_posY, (short)_posZ, (short)_posX, (short)_posY, (short)(_posZ + _height - 2));
        Global.VOXEL_MAP[_posX, _posY, _posZ + _height - 1] = _leaveType;
        Global.VOXEL_MAP[_posX, _posY, _posZ + _height] = _leaveType;
    }
}
