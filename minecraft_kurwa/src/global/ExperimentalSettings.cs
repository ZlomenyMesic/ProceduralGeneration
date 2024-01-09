//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

namespace minecraft_kurwa.src.global;

internal static class ExperimentalSettings {
    internal readonly static int RENDER_TARGET_WIDTH = Settings.WINDOW_WIDTH * 2;   // resolution
    internal readonly static int RENDER_TARGET_HEIGHT = Settings.WINDOW_HEIGHT * 2; // window size * 2 for good quality (real)

    internal static float JOSH_TRANSPARENCY = 0f; // josh

    internal static bool TRANSPARENT_VOXELS = true; // support transparent textures?

    internal static float ASPECT_RATIO = 16 / 9; // default 16 / 9

    internal static float ANTI_RENDER_DISTANCE = 0.1f; // minimum distance to render

    internal static short ZOOM_LEVEL = 40; // zoom in

    internal static bool INVERT_COLORS = true; // nightmare fuel

    // if true, Settings.SEED is selected randomly
    internal static bool RANDOM_SEED = false;

    // perlin noise output near [0, 0] is always the same
    // must be AT LEAST 30 to not generate any gravel around [0, 0]
    internal static short NOISE_OFFSET = 30;

    internal static bool HITBOXES = false;
}