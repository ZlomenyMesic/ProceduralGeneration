//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

namespace minecraft_kurwa.src.global {
    internal static class Settings {
        // DEFAULT VALUES FOR THE LAUNCHER

        internal static int WINDOW_HEIGHT = 1080;
        internal static int WINDOW_WIDTH = 1920;

        internal static int FIELD_OF_VIEW = 60; // in degrees
        internal static int RENDER_DISTANCE = 8000;

        internal static int SENSIBILITY = 200; // higher value => faster mouse
        internal static int MOVEMENT_SPEED = 25; // higher value => faster movement

        internal static int WORLD_SIZE = 500;
        internal static int HEIGHT_LIMIT = 250;
        internal static int SEED = 1;

        internal static int MAIN_NOISE_SHARPNESS = 60;
        internal static int MAIN_NOISE_SCALE = 17;
        internal static int BIOME_SCALE = 800;
        internal static int SUBBIOME_SCALE = 400;

        internal static int WATER_LEVEL = 7;
        internal static int OCEAN_SCALE = 90;
        internal static int POND_DENSITY = 20;
        internal static int FREEZING_DISTANCE = 20;     // any water block closer to a polar biome than this will freeze
        internal static int MAX_FREEZING_DISTANCE = 45; // maximum distance from a polar biome to freeze
        internal static int ICE_HOLES = 0;              // 0 and less => no holes in ice; 100 and more => no ice

        internal static int TREE_DENSITY = 130;
        internal static int BUSH_DENSITY = 50;
        internal static int WOODY_PLANTS_EDGE_OFFSET = 0; // trees and bushes on the edge of the map will be cut

        internal static byte BIOME_BLENDING = 50;

        internal static bool ENABLE_TERRAIN_COLLAPSE = false;
        internal static ushort TERRAIN_COLLAPSE_LIMIT = 6;

        internal static ushort ROUND_SMOOTH_RADIUS = 2;
        internal static ushort ROUND_SMOOTH_CHANGE_AMOUNT = 10;

        internal static ushort CREEK_EXPAND_TRY_LIMIT = 10;
        internal static ushort CREEK_DENSITY = 1; // amount per 10,000 blocks
    }
}