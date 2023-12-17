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
            for (byte z = (byte)(_height / Global.RANDOM.Next(4, 6)); z < _height; z++) {
                if (_posX + 1 < Settings.WORLD_SIZE) Global.VOXEL_MAP[_posX + 1, _posY, _posZ + z] = _leaveType;
                if (_posX - 1 >= 0) Global.VOXEL_MAP[_posX - 1, _posY, _posZ + z] = _leaveType;
                if (_posY + 1 < Settings.WORLD_SIZE) Global.VOXEL_MAP[_posX, _posY + 1, _posZ + z] = _leaveType;
                if (_posY - 1 >= 0) Global.VOXEL_MAP[_posX, _posY - 1, _posZ + z] = _leaveType;

                if (z >= _height * 1.5 / 5 && z <= _height * 4.5 / 5) {
                    for (sbyte x = -2; x <= 2; x++) {
                        if (_posX + x < 0 || _posX + x >= Settings.WORLD_SIZE) continue;

                        for (sbyte y = -2; y <= 2; y++) {
                            if (_posY + y < 0 || _posY + y >= Settings.WORLD_SIZE) continue;

                            if (Math.Sqrt(x * x + y * y) <= 2 && Global.VOXEL_MAP[_posX + x, _posY + y, _posZ + z] == null) {
                                if (Global.RANDOM.Next(0, 4) != 0) Global.VOXEL_MAP[_posX + x, _posY + y, _posZ + z] = _leaveType;
                            }
                        }
                    }
                }
            }

            for (byte z = 0; z <= _height; z++) {
                Global.VOXEL_MAP[_posX, _posY, _posZ + z] = z != _height ? _woodType : _leaveType;
            }
        }
    }
}
