//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace minecraft_kurwa {
    internal static class Global {
        
        internal static GraphicsDevice GRAPHICS_DEVICE;

        internal const float UPDATES_PER_SECOND = 60; // how many times Update() gets called

        internal const float START_POS_X = Settings.WORLD_SIZE / 2;
        internal const float START_POS_Y = 100;
        internal const float START_POS_Z = Settings.WORLD_SIZE / 2;

        internal static ushort[,] HEIGHT_MAP = new ushort[Settings.WORLD_SIZE, Settings.WORLD_SIZE];
        internal static byte?[,,] VOXEL_MAP = new byte?[Settings.WORLD_SIZE, Settings.WORLD_SIZE, Settings.HEIGHT_LIMIT];
        
        // [x, y, mode]
        // mode == 0 -> primary biome
        // mode == 1 -> blending
        // mode == 2 -> secondary biome
        // mode == 3 -> tertiary biome
        internal static byte[,,] BIOME_MAP = new byte[Settings.WORLD_SIZE, Settings.WORLD_SIZE, 4];
    }

    internal static class Settings {
        internal const int WINDOW_HEIGHT = 1080;
        internal const int WINDOW_WIDTH = 1920;
        
        internal const float FIELD_OF_VIEW = 60; // in degrees
        internal const float RENDER_DISTANCE = 8000;
        internal const bool TRANSPARENT_TEXTURES = true;
        internal const bool INVERT_COLORS = false;

        internal const float SENSIBILITY = 200; // higher value => faster mouse
        internal const float MOVEMENT_SPEED = 25; // higher value => faster movement

        internal const int WORLD_SIZE = 550;
        internal const int SEED = 21;

        internal const int MAIN_NOISE_SHARPNESS = 60;
        internal const int MAIN_NOISE_SCALE = 17;
        internal const int BIOME_SCALE = 80;
        internal const int SUBBIOME_SCALE = 12;
        
        internal const int WATER_LEVEL = 4;
        internal const int OCEAN_SCALE = 90;
        
        internal const int HEIGHT_LIMIT = 200;

        internal const int TREE_DENSITY = 130;
        internal const int TREE_EDGE_OFFSET = 0; // trees on the edge will be cut

        internal const byte BIOME_BLENDING = 50;

        internal const ushort TERRAIN_COLLAPSE_LIMIT = 2;
        internal const ushort TERRAIN_SMOOTHING_RADIUS = 3;
    }

    internal static class ExperimentalSettings {
        internal const int RENDER_TARGET_WIDTH = Settings.WINDOW_WIDTH * 2; // resolution
        internal const int RENDER_TARGET_HEIGHT = Settings.WINDOW_HEIGHT * 2; // window size * 2 for good quality (real)

        internal const float ASPECT_RATIO = 16 / 9; // default 16 / 9

        internal const float ANTI_RENDER_DISTANCE = 1f; // minimum distance to render (1f is optimal)

        internal const bool INVERT_COLORS = false; // nightmare fuel
    }
}
