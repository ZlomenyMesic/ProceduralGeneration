//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

namespace minecraft_kurwa.src.global {
    internal static class ExperimentalSettings {
        internal readonly static int RENDER_TARGET_WIDTH = Settings.WINDOW_WIDTH * 2;   // resolution
        internal readonly static int RENDER_TARGET_HEIGHT = Settings.WINDOW_HEIGHT * 2; // window size * 2 for good quality (real)

        internal static float JOSH_TRANSPARENCY = 0.2f; // josh

        internal static bool TRANSPARENT_VOXELS = true; // support transparent textures?

        internal static float ASPECT_RATIO = 16 / 9; // default 16 / 9

        internal static float ANTI_RENDER_DISTANCE = 1f; // minimum distance to render (1f is optimal)

        internal static short ZOOM_LEVEL = 40; // zoom in

        internal static bool INVERT_COLORS = true; // nightmare fuel

        internal static short NOISE_OFFSET = 30; // perlin noise output near [0, 0] is always the same
    }
}