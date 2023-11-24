﻿//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

using Microsoft.Xna.Framework.Graphics;

namespace minecraft_kurwa {
    internal static class Global {
        
        internal static GraphicsDevice GRAPHICS_DEVICE;

        internal const float UPDATES_PER_SECOND = 60; // how many times Update() gets called

        internal const float START_POS_X = 0;
        internal const float START_POS_Y = 100;
        internal const float START_POS_Z = 0;

        internal const bool INVERT_COLORS = false;
        internal const int TREE_EDGE_OFFSET = 0; // trees on the edge will be cut

        internal static short[,] HEIGHT_MAP = new short[Settings.WORLD_SIZE, Settings.WORLD_SIZE];
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

        internal const int MAIN_NOISE_SHARPNESS = 30;
        internal const int MAIN_NOISE_SCALE = 7;
        internal const int BIOME_SCALE = 65;
        internal const int SUBBIOME_SCALE = 8;
        
        internal const int WATER_LEVEL = 4;
        internal const int HEIGHT_LIMIT = 140;

        internal const int TREE_DENSITY = 130;
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

        BEECH_LEAVES = 14,
        BEECH_WOOD = 15,

        MAPLE_LEAVES = 16,
        MAPLE_WOOD = 17,

        WATER = 18,

        POPLAR_LEAVES = 19,
        POPLAR_WOOD = 20,

        CHERRY_LEAVES = 21,
        CHERRY_WOOD = 22,
    }
}
