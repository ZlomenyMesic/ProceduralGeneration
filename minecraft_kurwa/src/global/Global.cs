//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace minecraft_kurwa.src.global;

internal static class Global {

    internal static GraphicsDevice GRAPHICS_DEVICE;

    internal const float UPDATES_PER_SECOND = 60; // how many times Update() gets called

    internal const string SKY_DOME_MODEL_SOURCE = "sky_model";
    internal const string SKY_DOME_TEXTURE_SOURCE = ""; // default skydome_0
    internal const float SKY_ROTATION_SPEED = 0.2f;

    internal static Vector3 CAM_POSITION; // camera position
    internal static Vector3 CAM_TARGET; // position the camera is pointed to

    internal static Random RANDOM;

    internal static ushort[,] HEIGHT_MAP = new ushort[Settings.WORLD_SIZE, Settings.WORLD_SIZE];
    internal static byte?[,,] VOXEL_MAP = new byte?[Settings.WORLD_SIZE, Settings.WORLD_SIZE, Settings.HEIGHT_LIMIT];

    // [x, y, mode]
    // mode == 0 -> primary biome
    // mode == 1 -> blending
    // mode == 2 -> secondary biome
    // mode == 3 -> tertiary biome
    internal static byte[,,] BIOME_MAP = new byte[Settings.WORLD_SIZE, Settings.WORLD_SIZE, 4];
}