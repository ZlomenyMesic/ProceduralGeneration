//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

using Microsoft.Xna.Framework;
using minecraft_kurwa.src.global;

namespace minecraft_kurwa.src.renderer.voxels {
    internal static class VoxelCulling {
        internal static readonly Vector3 defaultCTPosition = new(0, 300, -350); // default camera target position relative to camera position

        internal static ushort minRenderX = 0;
        internal static ushort maxRenderX = (ushort)(Settings.WORLD_SIZE - 1);

        internal static ushort minRenderY = 0;
        internal static ushort maxRenderY = (ushort)(Settings.WORLD_SIZE - 1);

        private static readonly ushort MAX_DIFFERENCE_VALUE = 300; // maximum possible xDiff and yDiff
        private static readonly ushort MIN_DIFFERENCE = (ushort)(25 + ((Settings.WORLD_SIZE - 500) / 50)); // minimum camPosition and camTarget coordinates difference to cull

        internal static void UpdateRenderCoordinates() {
            short xDiff = (short)(Global.CAM_POSITION.X - Global.CAM_TARGET.X);
            short yDiff = (short)(Global.CAM_POSITION.Z - Global.CAM_TARGET.Z);

            if (xDiff < -MIN_DIFFERENCE) {
                short minRenderX = (short)(Global.CAM_POSITION.X - MAX_DIFFERENCE_VALUE - xDiff - (Global.CAM_POSITION.Y / 2));
                VoxelCulling.minRenderX = (ushort)(minRenderX < 0 ? 0 : minRenderX);
            } else minRenderX = 0;

            if (xDiff > MIN_DIFFERENCE) {
                short maxRenderX = (short)(Global.CAM_POSITION.X + MAX_DIFFERENCE_VALUE - xDiff + (Global.CAM_POSITION.Y / 2));
                VoxelCulling.maxRenderX = (ushort)(maxRenderX < 0 ? 0 : maxRenderX);
            } else maxRenderX = (ushort)(Settings.WORLD_SIZE - 1);

            if (yDiff < -MIN_DIFFERENCE) {
                short minRenderY = (short)(Global.CAM_POSITION.Z - MAX_DIFFERENCE_VALUE - yDiff - (Global.CAM_POSITION.Y / 2));
                VoxelCulling.minRenderY = (ushort)(minRenderY < 0 ? 0 : minRenderY);
            } else minRenderY = 0;

            if (yDiff > MIN_DIFFERENCE) {
                short maxRenderY = (short)(Global.CAM_POSITION.Z + MAX_DIFFERENCE_VALUE - yDiff + (Global.CAM_POSITION.Y / 2));
                VoxelCulling.maxRenderY = (ushort)(maxRenderY < 0 ? 0 : maxRenderY);
            } else maxRenderY = (ushort)(Settings.WORLD_SIZE - 1);
        }
    }
}