﻿//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

using minecraft_kurwa.src.global;
using minecraft_kurwa.src.global.functions;
using minecraft_kurwa.src.renderer.voxels;

namespace minecraft_kurwa.src.generator.feature.tree.types;

internal class SpruceTree : Tree {
    internal SpruceTree(ushort posX, ushort posY, ushort posZ, byte height) : base(posX, posY, posZ, height, VoxelType.SPRUCE_LEAVES, VoxelType.SPRUCE_WOOD) { }

    internal override void Build() {
        float radius = _height / 5;
        byte bottom = (byte)(_height / Global.RANDOM.Next(4, 6));

        for (short z = bottom; z < _height; z += 2) {
            for (short x = (short)-radius; x <= radius; x++) {
                if (!World.IsInRange(_posX + x)) continue;

                for (short y = (short)-radius; y <= radius; y++) {
                    if (!World.IsInRange(_posY + y)) continue;

                    if (Geometry.Circle(x, y, radius) && Global.RANDOM.Next(0, 6) != 0) {
                        if (Global.VOXEL_MAP[_posX + x, _posY + y, _posZ + z] == null) {
                            Global.VOXEL_MAP[_posX + x, _posY + y, _posZ + z] = _leaveType;
                        }
                    }
                }
            }
            radius -= 0.4f;
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

        BuildBranch((short)_posX, (short)_posY, (short)_posZ, (short)_posX, (short)_posY, (short)(_posZ + (_height * 4 / 5) - 1));
    }
}
