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
        internal const float MOVEMENT_SPEED = 40; // higher value => faster movement

        internal const float UPDATES_PER_SECOND = 60; // how many times Update() gets called

        internal const float START_POS_X = 0;
        internal const float START_POS_Y = 100;
        internal const float START_POS_Z = 0;

        internal const int WORLD_SIZE = 250;
        internal const int HEIGHT_LIMIT = 256;
        internal const int SEED = 4;

        internal const int MAIN_NOISE_SHARPNESS = 30;
        internal const int MAIN_NOISE_SCALE = 7;
        internal const int BIOME_SCALE = 65;
        internal const int SUBBIOME_SCALE = 8;

        internal const int TREE_DENSITY = 100;
        internal const int TREE_EDGE_OFFSET = 0; // trees on the edge will be cut

        internal static short[,] HEIGHT_MAP = new short[WORLD_SIZE, WORLD_SIZE];
        internal static VoxelType?[,,] VOXEL_MAP = new VoxelType?[WORLD_SIZE, WORLD_SIZE, HEIGHT_LIMIT];
        internal static byte[,] BIOME_MAP = new byte[WORLD_SIZE, WORLD_SIZE];
    }

    internal enum VoxelType {
        UNKNOWN = 0,
        GRASS = 1,
        STONE = 2,
        SAND = 3,
        ICE = 4,
        TERRACOTTA = 5,
        GRAVEL = 6,
        SNOW = 7,

        OAK_LEAVES = 8,
        OAK_WOOD = 9,

        KAPOK_LEAVES = 10,
        KAPOK_WOOD = 11,

        SPRUCE_LEAVES = 12,
        SPRUCE_WOOD = 13,

        WATER = 14,
    }
}
