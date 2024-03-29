﻿//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

using minecraft_kurwa.src.global;
using minecraft_kurwa.src.global.functions;
using minecraft_kurwa.src.renderer.voxels;

namespace minecraft_kurwa.src.generator.feature.tree.types;

internal class BasicDeciduousTree : Tree {
    internal BasicDeciduousTree(ushort posX, ushort posY, ushort posZ, byte height, VoxelType leaveType, VoxelType woodType) : base(posX, posY, posZ, height, leaveType, woodType) { }

    internal override void Build() {
        byte radius = (byte)(_height / 2.5f);

        for (short x = (short)(-radius + 1); x < radius; x++) {
            if (!World.IsInRange(_posX + x)) continue;

            for (short y = (short)(-radius + 1); y < radius; y++) {
                if (!World.IsInRange(_posY + y)) continue;

                for (short z = (short)(-radius + 1); z < radius; z++) {
                    if (Geometry.Sphere(x, y, z, radius / 2) && Global.RANDOM.Next(0, 2) == 0
                        || Geometry.Sphere(x, y, z, radius) && Global.RANDOM.Next(0, 4) == 0) {
                        if (Global.VOXEL_MAP[_posX + x, _posY + y, _posZ + _height * 2 / 3 + z] == null) {
                            Global.VOXEL_MAP[_posX + x, _posY + y, _posZ + _height * 2 / 3 + z] = _leaveType;
                        }
                    }
                }
            }
        }

        BuildBranch((short)_posX, (short)_posY, (short)_posZ, (short)_posX, (short)_posY, (short)(_posZ + (_height * 2 / 3)));
    }
}
