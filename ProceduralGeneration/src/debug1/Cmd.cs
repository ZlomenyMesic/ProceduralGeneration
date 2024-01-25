//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

using minecraft_kurwa.src.renderer.voxels;
using minecraft_kurwa.src.global;

namespace minecraft_kurwa.debug;

internal static class Cmd {
    internal static void SetBlock(int x, int y, int z, VoxelType? block) {
        Global.VOXEL_MAP[x, y, z] = (byte)block;
    }

    internal static void Fill(int x1, int y1, int z1, int x2, int y2, int z2, VoxelType? block) {
        for (int x = x1; x < x2; x++) {
        for (int y = y1; y < y2; y++) {
        for (int z = z1; z < z2; z++) {
            Global.VOXEL_MAP[x, y, z] = (byte?)block;
        }}}
    }
}