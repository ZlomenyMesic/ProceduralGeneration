//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

using System;
using minecraft_kurwa.src.global;
using minecraft_kurwa.src.renderer.voxels;

namespace minecraft_kurwa.debug;

public class Marker {
    internal static void GenerateMarker(int x, int y, VoxelType markerType) {
        for (int z = Global.HEIGHT_MAP[x, y] + 1; z < Settings.HEIGHT_LIMIT; z++) {
            Global.VOXEL_MAP[x, y, z] = (byte)markerType;
        }
    }
}