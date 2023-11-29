//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

namespace minecraft_kurwa.src.global {
    internal static class Settings {
        internal static int WINDOW_HEIGHT = 1080;
        internal static int WINDOW_WIDTH = 1920;

        internal static float FIELD_OF_VIEW = 60; // in degrees
        internal static float RENDER_DISTANCE = 8000;
        internal static bool TRANSPARENT_TEXTURES = true;

        internal static float SENSIBILITY = 200; // higher value => faster mouse
        internal static float MOVEMENT_SPEED = 25; // higher value => faster movement

        internal static int WORLD_SIZE = 500;
        internal static int SEED = 21;

        internal static int MAIN_NOISE_SHARPNESS = 60;
        internal static int MAIN_NOISE_SCALE = 17;
        internal static int BIOME_SCALE = 80;
        internal static int SUBBIOME_SCALE = 12;

        internal static int WATER_LEVEL = 4;
        internal static int OCEAN_SCALE = 90;
        internal static int POND_DENSITY = 50;
        internal static int FREEZING_DISTANCE = 20;     // any water block closer to a polar biome than this will freeze
        internal static int MIN_FREEZING_DISTANCE = 45; // minimum distance from a polar biome to freeze
        internal static int ICE_HOLES = 0;              // 0 and less => no holes in ice; 100 and more => no ice

        internal static int HEIGHT_LIMIT = 220;

        internal static int TREE_DENSITY = 130;
        internal static int TREE_EDGE_OFFSET = 0; // trees on the edge will be cut

        internal static byte BIOME_BLENDING = 50;

        internal static ushort TERRAIN_COLLAPSE_LIMIT = 2;
        internal static ushort TERRAIN_SMOOTHING_RADIUS = 3;
    }
}