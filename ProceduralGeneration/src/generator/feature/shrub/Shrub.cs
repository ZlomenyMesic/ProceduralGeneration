﻿//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

using minecraft_kurwa.src.renderer.voxels;
using minecraft_kurwa.src.global;
using minecraft_kurwa.src.global.functions;

namespace minecraft_kurwa.src.generator.feature.shrub;

internal static class Shrub {
    internal static void Build(ushort posX, ushort posY, ushort posZ, ushort sizeX, ushort sizeY, VoxelType leaves, bool main = true) {
        int sizeZ = (sizeX + sizeY) / 3;

        for (short x = (short)(-sizeX / 2); x <= sizeX / 2; x++) {
            if (!World.IsInRange(posX + x)) continue;

            for (short y = (short)(-sizeY / 2); y <= sizeY / 2; y++) {
                if (!World.IsInRange(posY + y)) continue;

                for (short z = (short)(-sizeZ / 2); z <= sizeZ / 2; z++) {
                    if (posZ + z < 0 || posZ + z >= Settings.HEIGHT_LIMIT) continue;

                    if (Geometry.Ellipsoid(x, y, z, sizeX / 2, sizeY / 2, sizeZ / 2) && Global.RANDOM.Next(0, 2) != 0) {
                        if (Global.HEIGHT_MAP[posX + x, posY + y] < posZ + z && Global.VOXEL_MAP[posX + x, posY + y, posZ + z] == null) {
                            Global.VOXEL_MAP[posX + x, posY + y, posZ + z] = (byte)leaves;
                        }
                    }
                }
            }
        }

        // 50 % chance to generate another bush for better shapes
        if (main && Global.RANDOM.Next(0, 2) == 0) Build((ushort)(posX + Global.RANDOM.Next(-2, 3)), (ushort)(posY + Global.RANDOM.Next(-2, 3)), posZ, (ushort)(Global.RANDOM.Next(ShrubGenerator.MIN_SHRUB_SIZE, ShrubGenerator.MAX_SHRUB_SIZE)), (ushort)(Global.RANDOM.Next(ShrubGenerator.MIN_SHRUB_SIZE, ShrubGenerator.MAX_SHRUB_SIZE)), leaves, false);
    }
}
