//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

namespace minecraft_kurwa {
    internal static class Global {
        internal const int WINDOW_HEIGHT = 1400;
        internal const int WINDOW_WIDTH = 2400;

        internal const float FIELD_OF_VIEW = 60; // in degrees
        internal const float RENDER_DISTANCE = 2000;

        internal const float SENSIBILITY = 200; // higher value => faster mouse
        internal const float MOVEMENT_SPEED = 100; // higher value => faster movement

        internal const float UPDATES_PER_SECOND = 60; // how many times Update() gets called

        internal const int CHUNK_SIZE = 16;
        internal const int HEIGHT_LIMIT = 512;

        internal const int WORLD_SIZE = 250;
        internal const int SEED = 1;

        internal const int MAIN_NOISE_SHARPNESS = 50;
        internal const int MAIN_NOISE_SCALE = 10;
        internal const int SIDE_NOISE_SHARPNESS = 1;
        internal const int SIDE_NOISE_SCALE = 5;
        internal const int BIOME_SCALE = 40;
        internal const int SUBBIOME_SCALE = 5;

        internal static short[,] HEIGHT_MAP = new short[WORLD_SIZE, WORLD_SIZE];
        internal static VoxelType?[,,] VOXEL_MAP = new VoxelType?[WORLD_SIZE, WORLD_SIZE, HEIGHT_LIMIT];
        internal static byte[,] BIOME_MAP = new byte[WORLD_SIZE, WORLD_SIZE];
    }
}
