//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

using Microsoft.Xna.Framework.Graphics;

namespace minecraft_kurwa {
    internal static class Global {
        
        internal static GraphicsDevice GRAPHICS_DEVICE;

        internal const float UPDATES_PER_SECOND = 60; // how many times Update() gets called

        internal const float START_POS_X = Settings.WORLD_SIZE / 2;
        internal const float START_POS_Y = 100;
        internal const float START_POS_Z = Settings.WORLD_SIZE / 2;

        internal const bool INVERT_COLORS = false;

        internal static ushort[,] HEIGHT_MAP = new ushort[Settings.WORLD_SIZE, Settings.WORLD_SIZE];
        internal static byte?[,,] VOXEL_MAP = new byte?[Settings.WORLD_SIZE, Settings.WORLD_SIZE, Settings.WORLD_SIZE];
        internal static byte[,] BIOME_MAP = new byte[Settings.WORLD_SIZE, Settings.WORLD_SIZE];
    }

    internal static class Settings {
        internal const int WINDOW_HEIGHT = 1400;
        internal const int WINDOW_WIDTH = 2400;
        
        internal const float FIELD_OF_VIEW = 60; // in degrees
        internal const float RENDER_DISTANCE = 8000;
        
        internal const float SENSIBILITY = 200; // higher value => faster mouse
        internal const float MOVEMENT_SPEED = 40; // higher value => faster movement

        internal const int WORLD_SIZE = 250;
        internal const int SEED = 21;

        internal const int MAIN_NOISE_SHARPNESS = 60;
        internal const int MAIN_NOISE_SCALE = 17;
        internal const int BIOME_SCALE = 80;
        internal const int SUBBIOME_SCALE = 12;
        
        internal const int WATER_LEVEL = 4;
        internal const int HEIGHT_LIMIT = 200;

        internal const int TREE_DENSITY = 130;
        internal const int TREE_EDGE_OFFSET = 0; // trees on the edge will be cut
    }
}
