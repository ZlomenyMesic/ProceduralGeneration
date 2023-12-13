//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

using Microsoft.Xna.Framework;
using minecraft_kurwa.src.global;
using minecraft_kurwa.src.renderer.voxels;
using System;

namespace minecraft_kurwa.src.gui.colors {
    internal static class ColorManager {
        private const byte DEFAULT_TRANSPARENCY = 100; // range 0-100
        private const byte LEAVES_TRANSPARENCY = 90;

        // original color is multiplied by shadow
        internal static readonly Vector3 FRONT_SHADOW = new(0.7f, 0.7f, 0.7f);
        internal static readonly Vector3 BACK_SHADOW = new(0.7f, 0.7f, 0.7f);
        internal static readonly Vector3 SIDE_SHADOW = new(0.8f, 0.8f, 0.8f);
        internal static readonly Vector3 TOP_SHADOW = new(1.0f, 1.0f, 1.0f);
        internal static readonly Vector3 BOTTOM_SHADOW = new(0.6f, 0.6f, 0.6f);

        private static readonly Color[] COLORS = {
            new(255, 0, 255),   // 0 - purple
            new(19, 133, 16),   // 1 - grass
            new(100, 110, 106), // 2 - rock
            new(194, 178, 128), // 3 - sand
            new(177, 222, 227), // 4 - ice
            new(179, 86, 66),   // 5 - terracotta
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
            new(23, 75, 144),   // 18 - water
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
        };

        internal static Color GetVoxelColor(byte? voxelType, byte biome, ushort altitude, int seed) {
            Vector3 color = COLORS[(int)voxelType].ToVector3() * new Vector3(255, 255, 255);

            if (voxelType == (byte)VoxelType.GRASS) {
                switch (biome) {
                    case 0 or 1 or 2 or 3 or 4 or 5 or 6 or 23: color += new Vector3(73, 7, 20); break;    // super dry
                    case 20 or 21 or 22 or 24 or 25: color += new Vector3(50, 5, 15); break;               // dry
                    case 10 or 11 or 12: color += new Vector3(15, 5, -2); break;                           // rainy
                    case 50 or 52 or 60 or 62 or 63 or 64: color = new(147, 192, 139); break;              // frozen
                    case 51 or 61: color = new(173, 135, 101); break;                                      // frozen dark soil
                    case 31 or 41: color += new Vector3(57, -55, 10); break;                               // dark soil
                    default: break;                                                                        // no shade
                }
            }
            else if (voxelType == (byte)VoxelType.KAPOK_LEAVES) {
                switch (new Random(Settings.SEED * seed).Next(0, 3)) {
                    case 0: color += new Vector3(-17, -15, -2); break;  // dark shade
                    case 1: color += new Vector3(15, 12, 3); break;     // light shade
                    default: break;                                     // no shade
                }
            }
            else if (voxelType == (byte)VoxelType.KAPOK_LEAVES) {
                switch (new Random(Settings.SEED * seed).Next(0, 3)) {
                    case 0: color += new Vector3(-17, -15, -2); break;  // dark shade
                    case 1: color += new Vector3(8, 12, 3); break;      // light shade
                    default: break;                                     // no shade
                }
            }
            else if (voxelType == (byte)VoxelType.BEECH_LEAVES) {
                switch (new Random(Settings.SEED * seed).Next(0, 4)) {
                    case 0: color += new Vector3(-15, 14, -20); break;  // green shade
                    case 1: color += new Vector3(-18, 20, -27); break;  // greener shade
                    default: break;                                     // no shade
                }
            }
            else if (voxelType == (byte)VoxelType.MAPLE_LEAVES) {
                switch (new Random(Settings.SEED * seed).Next(0, 3)) {
                    case 0: color += new Vector3(-30, 16, -3); break;   // green shade
                    case 1: color += new Vector3(-40, 23, -7); break;   // greener shade
                    default: break;                                     // no shade
                }
            }
            else if (voxelType == (byte)VoxelType.CHERRY_LEAVES) {
                switch (new Random(Settings.SEED * seed).Next(0, 3)) {
                    case 0: color += new Vector3(0, 0, 12); break;      // more purple shade
                    case 1: color += new Vector3(0, -7, -7); break;     // more reddish shade
                    default: break;                                     // no shade
                }
            }
            else if (voxelType == (byte)VoxelType.WATER) {
                switch (biome) {
                    case 50 or 51 or 52 or 60 or 61 or 62 or 63 or 64: color += new Vector3(20, 40, 20); break; // half-frozen water
                    default: break;
                }
            }

            return new(color.X / 255, color.Y / 255, color.Z / 255);
        }

        internal static byte GetVoxelTransparency(byte? voxelType) {
            return voxelType switch {
                8 or 10 or 12 or 14 or 16 or 19 or 21 or 24 or 26 or 27 or 29 or 30 => LEAVES_TRANSPARENCY,
                _ => DEFAULT_TRANSPARENCY
            };
        }
    }
}
