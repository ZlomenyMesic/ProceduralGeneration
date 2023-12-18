//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

using minecraft_kurwa.src.global;
using minecraft_kurwa.src.renderer.voxels;
using System;

namespace minecraft_kurwa.src.generator.feature.tree.types {
    internal class SpruceTree : Tree {
        internal SpruceTree(ushort posX, ushort posY, ushort posZ, byte height) : base(posX, posY, posZ, height, VoxelType.SPRUCE_LEAVES, VoxelType.SPRUCE_WOOD) { }

        internal override void Build() {
            float diameter = _height * 2 / 5;
            byte bottom = (byte)(_height / Global.RANDOM.Next(4, 6));

            for (short z = bottom; z < _height; z += 2) {
                for (short x = (short)(-diameter / 2); x <= diameter / 2; x++) {
                    if (_posX + x < 0 || _posX + x >= Settings.WORLD_SIZE) continue;

                    for (short y = (short)(-diameter / 2); y <= diameter / 2; y++) {
                        if (_posY + y < 0 || _posY + y >= Settings.WORLD_SIZE) continue;

                        float distance = (float)Math.Sqrt(x * x + y * y);
                        if (distance < diameter / 2 && Global.RANDOM.Next(0, 6) != 0) {
                            if (Global.VOXEL_MAP[_posX + x, _posY + y, _posZ + z] == null) {
                                Global.VOXEL_MAP[_posX + x, _posY + y, _posZ + z] = _leaveType;
                            }
                        }
                    }
                }
                diameter -= 0.8f;
            }

            for (byte z = 0; z <= _height; z++) {
                if (z >= _height * 4 / 5) Global.VOXEL_MAP[_posX, _posY, _posZ + z] = _leaveType;

                if (z >= bottom && z < _height * 4 / 5) {
                    if (_posX + 1 < Settings.WORLD_SIZE) Global.VOXEL_MAP[_posX + 1, _posY, _posZ + z] = _leaveType;
                    if (_posX - 1 >= 0) Global.VOXEL_MAP[_posX - 1, _posY, _posZ + z] = _leaveType;
                    if (_posY + 1 < Settings.WORLD_SIZE) Global.VOXEL_MAP[_posX, _posY + 1, _posZ + z] = _leaveType;
                    if (_posY - 1 >= 0) Global.VOXEL_MAP[_posX, _posY - 1, _posZ + z] = _leaveType;
                }
            }

            BuildBranch((short)_posX, (short)_posY, (short)_posZ, (short)_posX, (short)_posY, (short)(_posZ + (_height * 4 / 5)));
        }
    }
}
