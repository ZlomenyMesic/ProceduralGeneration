//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

using Microsoft.Xna.Framework;
using minecraft_kurwa.src.global;
using minecraft_kurwa.src.renderer.voxels;
using System;

namespace minecraft_kurwa.src.gui.colors;

internal static class ColorManager {
    private const byte DEFAULT_TRANSPARENCY = 100; // range 0-100
    private const byte LEAVES_TRANSPARENCY = 90;

    // original color is multiplied by shadow
    internal static readonly Vector3 FRONT_SHADOW = new(0.6f, 0.6f, 0.6f);
    internal static readonly Vector3 BACK_SHADOW = new(0.6f, 0.6f, 0.6f);
    internal static readonly Vector3 SIDE_SHADOW = new(0.8f, 0.8f, 0.8f);
    internal static readonly Vector3 TOP_SHADOW = new(1.0f, 1.0f, 1.0f);
    internal static readonly Vector3 BOTTOM_SHADOW = new(0.4f, 0.4f, 0.4f);

    private static readonly Color[] COLORS = {
            new(255, 0, 255),   // 0 - purple
            new(53, 131, 35),   // 1 - grass
            new(100, 110, 106), // 2 - rock
            new(194, 178, 128), // 3 - sand
            new(177, 222, 227), // 4 - ice
            new(203, 104, 67),  // 5 - terracotta
            new(83, 84, 78),    // 6 - gravel
            new(225, 225, 225), // 7 - snow
            new(105, 170, 5),   // 8 - oak leaves
            new(90, 85, 54),    // 9 - oak wood
            new(55, 124, 20),   // 10 - kapok leaves
            new(105, 75, 55),   // 11 - kapok wood
            new(31, 98, 39),    // 12 - spruce leaves
            new(99, 73, 43),    // 13 - spruce wood
            new(90, 112, 76),   // 14 - beech leaves
            new(125, 110, 86),  // 15 - beech wood
            new(209, 169, 7),   // 16 - maple leaves
            new(51, 42, 38),    // 17 - maple wood
            new(26, 84, 106),   // 18 - water
            new(93, 146, 37),   // 19 - poplar leaves
            new(120, 113, 100), // 20 - poplar wood
            new(255, 183, 197), // 21 - cherry leaves
            new(71, 52, 39),    // 22 - cherry wood
            new(189, 150, 79),  // 23 - sandstone
            new(74, 113, 20),   // 24 - acacia leaves
            new(110, 100, 77),  // 25 - acacia wood
            new(45, 121, 36),   // 26 - cherry laurel leaves
            new(104, 134, 37),  // 27 - jackalberry leaves
            new(85, 98, 58),    // 28 - jackalberry wood
            new(64, 101, 46),   // 29 - raisin leaves
            new(50, 100, 10),   // 30 - mahogany leaves
            new(112, 100, 65),  // 31 - mahogany wood
            new(118, 147, 27),  // 32 - dry grass
            new(86, 68, 54),    // 33 - mud
            new(0, 0, 0),       // 34 - river rock
            new(31, 110, 39),   // 35 - pine leaves
            new(110, 100, 66),  // 36 - pine wood
            new(255, 0, 0, 75), // 37 - marker block red
            new(0, 0, 255, 75), // 38 - marker block blue
            new(0, 255, 0, 75), // 39 - marker block green
        };

    internal static Color GetVoxelColor(byte? voxelType, byte biome) {
        Vector3 color = COLORS[(int)voxelType].ToVector3() * new Vector3(255, 255, 255);

        if (voxelType == (byte)VoxelType.GRASS) {
            switch (biome) {
                case 0 or 1 or 2 or 3 or 4 or 5 or 6 or 23: color = new Vector3(92, 140, 36); break;   // super dry
                case 20 or 22 or 24 or 25: color = new Vector3(77, 144, 30); break;                    // dry
                case 10 or 11 or 12: color = new Vector3(34, 138, 14); break;                          // rainy
                case 50 or 52 or 60 or 62 or 63 or 64: color = new(137, 192, 149); break;              // frozen
                case 51 or 61: color = new(173, 135, 101); break;                                      // frozen dark soil
                case 31 or 41: color = new Vector3(76, 78, 26); break;                                 // dark soil
                case 21: color = new Vector3(96, 98, 26); break;                                       // lighter dark soil
                default: break;                                                                        // no shade
            }
        } else if (voxelType == (byte)VoxelType.OAK_LEAVES) {
            switch (Global.RANDOM.Next(0, 3)) {
                case 0: color = new Vector3(88, 155, 3); break;    // dark shade
                case 1: color = new Vector3(120, 182, 8); break;   // light shade
                default: break;                                    // no shade
            }
        } else if (voxelType == (byte)VoxelType.KAPOK_LEAVES) {
            switch (Global.RANDOM.Next(0, 3)) {
                case 0: color = new Vector3(38, 109, 18); break;   // dark shade
                case 1: color = new Vector3(63, 136, 23); break;   // light shade
                default: break;                                    // no shade
            }
        } else if (voxelType == (byte)VoxelType.SPRUCE_LEAVES) {
            if (biome >= 50) color = new Vector3(11, 98, 59);      // blue-ish shade when in cold climate
        } else if (voxelType == (byte)VoxelType.BEECH_LEAVES) {
            switch (Global.RANDOM.Next(0, 4)) {
                case 0: color = new Vector3(75, 126, 56); break;   // green shade
                case 1: color = new Vector3(72, 132, 49); break;   // greener shade
                default: break;                                          // no shade
            }
        } else if (voxelType == (byte)VoxelType.MAPLE_LEAVES) {
            switch (Global.RANDOM.Next(0, 3)) {
                case 0: color = new Vector3(179, 185, 4); break;   // green shade
                case 1: color = new Vector3(169, 192, 0); break;   // greener shade
                default: break;                                          // no shade
            }
        } else if (voxelType == (byte)VoxelType.CHERRY_LEAVES) {
            switch (Global.RANDOM.Next(0, 3)) {
                case 0: color = new Vector3(255, 183, 209); break;  // more purple shade
                case 1: color = new Vector3(255, 176, 190); break;  // more reddish shade
                default: break;                                     // no shade
            }
        } else if (voxelType == (byte)VoxelType.WATER) {
            switch (biome) {
                case 50 or 51 or 52 or 60 or 61 or 62 or 63 or 64: color = new Vector3(36, 104, 116); break; // half-frozen water
                default: break;
            }
        } else if (voxelType == (byte)VoxelType.MUD) {
            switch (biome) {
                case 5 or 6 or 20 or 21 or 22 or 23 or 24 or 25: color = new(104, 86, 72); break;
                default: break;
            }
        } else if (voxelType == (byte)VoxelType.RIVER_ROCK) {
            color = Global.RANDOM.Next(0, 4) switch {
                0 => new(198, 191, 184),
                1 => new(158, 157, 156),
                2 => new(185, 156, 150),
                3 => new(104, 90, 96),
                _ => COLORS[0].ToVector3() // shouldn't happen
            };
        }

        if (ExperimentalSettings.INVERT_COLORS) color = new Vector3(255, 255, 255) - color;

        return new(color.X / 255, color.Y / 255, color.Z / 255);
    }

    internal static byte GetVoxelTransparency(byte? voxelType) {
        return voxelType switch {
            8 or 10 or 12 or 14 or 16 or 19 or 21 or 24 or 26 or 27 or 29 or 30 or 35 => LEAVES_TRANSPARENCY,
            _ => DEFAULT_TRANSPARENCY
        };
    }
}
