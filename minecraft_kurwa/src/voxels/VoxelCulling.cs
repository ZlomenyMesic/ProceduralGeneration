//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

using Microsoft.Xna.Framework;
using minecraft_kurwa.src.global;
using System;

namespace minecraft_kurwa.src.voxels {
    internal static class VoxelCulling {
        internal static readonly Vector3 defaultCTPosition = new(0, 300, -350); // default camera target position relatively to camera position

        internal static ushort MIN_RENDER_X = 0;
        internal static ushort MAX_RENDER_X = (ushort)(Settings.WORLD_SIZE - 1);

        internal static ushort MIN_RENDER_Y = 0;
        internal static ushort MAX_RENDER_Y = (ushort)(Settings.WORLD_SIZE - 1);

        internal static ushort MIN_RENDER_Z = 0;
        internal static ushort MAX_RENDER_Z = (ushort)(Settings.HEIGHT_LIMIT - 1);

        internal static void UpdateRenderCoordinates(Vector3 camPosition, Vector3 camTarget) {
            short xDiff = (short)(camPosition.X - camTarget.X);
            short yDiff = (short)(camPosition.Z - camTarget.Z);
            //short zDiff = (short)(camPosition.Y - camTarget.Y);

            if (xDiff < 0) {
                short minRenderX = (short)(camPosition.X - 300 - xDiff - (camPosition.Y / 2));
                MIN_RENDER_X = (ushort)(minRenderX < 0 ? 0 : minRenderX);
            } else MIN_RENDER_X = 0;

            if (xDiff > 0) {
                short maxRenderX = (short)(camPosition.X + 300 - xDiff + (camPosition.Y / 2));
                MAX_RENDER_X = (ushort)(maxRenderX < 0 ? 0 : maxRenderX);
            } else MAX_RENDER_X = (ushort)(Settings.WORLD_SIZE - 1);

            if (yDiff < 0) {
                short minRenderY = (short)(camPosition.Z - 300 - yDiff - (camPosition.Y / 2));
                MIN_RENDER_Y = (ushort)(minRenderY < 0 ? 0 : minRenderY);
            } else MIN_RENDER_Y = 0;

            if (yDiff > 0) {
                short maxRenderY = (short)(camPosition.Z + 300 - yDiff + (camPosition.Y / 2));
                MAX_RENDER_Y = (ushort)(maxRenderY < 0 ? 0 : maxRenderY);
            } else MAX_RENDER_Y = (ushort)(Settings.WORLD_SIZE - 1);
        }
    }
}
