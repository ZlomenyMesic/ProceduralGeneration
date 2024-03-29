﻿//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

namespace minecraft_kurwa.src.global;

internal static class Settings {
    // gf: "I wish I never ever met you at all"
    // (Looksmaxxing reference)
    // (I have to breakupmaxx)
    // (gotta marrymaxx an A10 girl with positive canthal tilt she will save my bloodline)
    // (Smashmaxxed and created PSL gods)
    // (I saw her without makeup I wish I never ever met her at all)
    // (Looksmaxxing reference again)
    // (I need to divorcemaxx)
    // (children are ugly asf)
    // (ropemaxx will save me)
    // (I cut the rope with my jawline)
    // (There is no her)
    // (I'm schizophrenic)

    // DEFAULT VALUES FOR THE LAUNCHER

    internal static int WINDOW_HEIGHT = 1080;
    internal static int WINDOW_WIDTH = 1920;

    internal static int FIELD_OF_VIEW = 60; // in degrees
    internal static int RENDER_DISTANCE = 8000;

    internal static int SENSIBILITY = 200; // higher value => faster mouse
    internal static int MOVEMENT_SPEED = 25; // higher value => faster movement

    internal static int WORLD_SIZE = 500;
    internal static int HEIGHT_LIMIT = 250;
    internal static int SEED = 21;

    internal static int MAIN_NOISE_SHARPNESS = 60;
    internal static int MAIN_NOISE_SCALE = 17;
    internal static int BIOME_SCALE = 800;
    internal static int SUBBIOME_SCALE = 400;
    internal static byte BIOME_BLENDING_LEVEL = 50;

    internal static int WATER_LEVEL = 7;
    internal static int OCEAN_SCALE = 90;
    internal static int POND_DENSITY = 20;
    internal static int FREEZING_DISTANCE = 20;         // any water block closer to a polar biome than this will freeze
    internal static int MAX_FREEZING_DISTANCE = 45;     // maximum distance from a polar biome to freeze
    internal static int ICE_HOLES = 0;                  // 0 and less => no holes in ice; 100 and more => no ice
    internal static ushort CREEK_DENSITY = 20;          // amount per 1,000,000 blocks
    internal static ushort CREEK_EXPAND_TRY_LIMIT = 40; // max length it tries to find a lower point then the current
    internal static byte MIN_CREEK_LENGTH = 0;

    internal static int TREE_DENSITY = 130;
    internal static int BUSH_DENSITY = 50;
    internal static int WOODY_PLANTS_EDGE_OFFSET = 0; // trees and bushes on the edge of the map will be cut

    internal static bool ENABLE_TERRAIN_COLLAPSE = false;
    internal static ushort TERRAIN_COLLAPSE_LIMIT = 6;

    internal static bool FLAT_WORLD = false;
    internal static bool DEBUG_ENABLE_MARKERS = false;

    internal static byte BIOME_BLENDING = 50;
}
