//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

using Microsoft.Xna.Framework;
using minecraft_kurwa.src.global;

namespace minecraft_kurwa.src.voxels {
    internal static class VoxelCulling {
        internal static readonly Vector3 defaultCTPosition = new(0, 300, -350); // default camera target position relative to camera position

        internal static ushort MIN_RENDER_X = 0;
        internal static ushort MAX_RENDER_X = (ushort)(Settings.WORLD_SIZE - 1);

        internal static ushort MIN_RENDER_Y = 0;
        internal static ushort MAX_RENDER_Y = (ushort)(Settings.WORLD_SIZE - 1);

        private const ushort MAX_DIFFERENCE_VALUE = 300; // maximal possible xDiff and yDiff
        private const ushort MIN_DIFFERENCE = 25; // minimal camPosition and camTarget coordinates difference to cull

        internal static void UpdateRenderCoordinates(Vector3 camPosition, Vector3 camTarget) {
            short xDiff = (short)(camPosition.X - camTarget.X);
            short yDiff = (short)(camPosition.Z - camTarget.Z);

            if (xDiff < -MIN_DIFFERENCE) {
                short minRenderX = (short)(camPosition.X - MAX_DIFFERENCE_VALUE - xDiff - (camPosition.Y / 2));
                MIN_RENDER_X = (ushort)(minRenderX < 0 ? 0 : minRenderX);
            } else MIN_RENDER_X = 0;

            if (xDiff > MIN_DIFFERENCE) {
                short maxRenderX = (short)(camPosition.X + MAX_DIFFERENCE_VALUE - xDiff + (camPosition.Y / 2));
                MAX_RENDER_X = (ushort)(maxRenderX < 0 ? 0 : maxRenderX);
            } else MAX_RENDER_X = (ushort)(Settings.WORLD_SIZE - 1);

            if (yDiff < -MIN_DIFFERENCE) {
                short minRenderY = (short)(camPosition.Z - MAX_DIFFERENCE_VALUE - yDiff - (camPosition.Y / 2));
                MIN_RENDER_Y = (ushort)(minRenderY < 0 ? 0 : minRenderY);
            } else MIN_RENDER_Y = 0;

            if (yDiff > MIN_DIFFERENCE) {
                short maxRenderY = (short)(camPosition.Z + MAX_DIFFERENCE_VALUE - yDiff + (camPosition.Y / 2));
                MAX_RENDER_Y = (ushort)(maxRenderY < 0 ? 0 : maxRenderY);
            } else MAX_RENDER_Y = (ushort)(Settings.WORLD_SIZE - 1);
        }
    }
}