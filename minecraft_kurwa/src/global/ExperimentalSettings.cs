//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

namespace minecraft_kurwa.src.global {
    internal static class ExperimentalSettings {
        internal readonly static int RENDER_TARGET_WIDTH = Settings.WINDOW_WIDTH * 2;   // resolution
        internal readonly static int RENDER_TARGET_HEIGHT = Settings.WINDOW_HEIGHT * 2; // window size * 2 for good quality (real)

        internal readonly static float ASPECT_RATIO = 16 / 9; // default 16 / 9

        internal readonly static float ANTI_RENDER_DISTANCE = 1f; // minimum distance to render (1f is optimal)

        internal readonly static bool INVERT_COLORS = false; // nightmare fuel
    }
}