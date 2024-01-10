//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

using minecraft_kurwa.src.global;
using minecraft_kurwa.src.global.functions;
using minecraft_kurwa.src.renderer.voxels;
using System;

namespace minecraft_kurwa.src.generator.feature.tree.types;

internal class KapokTree : Tree {
    internal KapokTree(ushort posX, ushort posY, ushort posZ, byte height) : base(posX, posY, posZ, height, VoxelType.KAPOK_LEAVES, VoxelType.KAPOK_WOOD) { }

    internal override void Build() {
        byte radius = (byte)(_height / 3);

        short crownPosX = (short)(_posX + Global.RANDOM.Next(-2, 3));
        short crownPosY = (short)(_posY + Global.RANDOM.Next(-2, 3));

        for (short x = (short)(-radius - 1); x <= radius + 1; x++) {
            if (!World.IsInRange(crownPosX + x)) continue;

            for (short y = (short)(-radius - 1); y <= radius + 1; y++) {
                if (!World.IsInRange(crownPosY + y)) continue;

                for (short z = (short)(-radius / 3 + 1); z <= radius / 1.5; z++) {
                    float distance = (float)Math.Sqrt(x * x + y * y);

                    if (z > _height / 10 && Geometry.Circle(x, y, radius) && distance + Math.Abs(z) < _height / 2 - 1 && Global.RANDOM.Next(0, 2) == 0) {
                        if (Global.VOXEL_MAP[crownPosX + x, crownPosY + y, _posZ + z + _height * 2 / 3] == null) {
                            Global.VOXEL_MAP[crownPosX + x, crownPosY + y, _posZ + z + _height * 2 / 3] = _leaveType;
                        }
                    }
                }
            }
        }

        BuildBranch((short)_posX, (short)_posY, (short)_posZ, crownPosX, crownPosY, (short)(_posZ + (_height * 2 / 3)));
    }
}
