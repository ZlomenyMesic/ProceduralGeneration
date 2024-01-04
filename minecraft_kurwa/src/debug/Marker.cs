using minecraft_kurwa.src.global;
using minecraft_kurwa.src.renderer.voxels;

namespace minecraft_kurwa.debug;

public class Marker {
    internal static void generateMarker(int x, int y) {
        for (int z = Global.HEIGHT_MAP[x, y] + 1; z < Settings.HEIGHT_LIMIT; z++) {
            Global.VOXEL_MAP[x, y, z] = (byte)VoxelType.MARKER_BLOCK;
        }
    }
}