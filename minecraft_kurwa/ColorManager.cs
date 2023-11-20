//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

using Microsoft.Xna.Framework;
using System;

namespace minecraft_kurwa {
    internal static class ColorManager {
        internal static readonly Vector3 FRONT_SHADOW = new(0.7f, 0.7f, 0.7f);
        internal static readonly Vector3 SIDE_SHADOW = new(0.8f, 0.8f, 0.8f);
        internal static readonly Vector3 BACK_SHADOW = new(0.7f, 0.7f, 0.7f);
        internal static readonly Vector3 BOTTOM_SHADOW = new(0.6f, 0.6f, 0.6f);
        internal static readonly Vector3 TOP_SHADOW = new(1.0f, 1.0f, 1.0f);

        private static readonly Color[] COLORS = {
            new(255, 0, 255),   // 0 - purple
            new(19, 133, 16),   // 1 - grass
            new(100, 110, 106), // 2 - rock
            new(194, 178, 128), // 3 - sand
            new(185, 232, 234), // 4 - ice
            new(179, 86, 66),   // 5 - terracotta
            new(83, 84, 78),    // 6 - gravel
            new(255, 255, 255), // 7 - snow
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
            new(24, 77, 134),   // 18 - water
        };

        internal static Color GetVoxelColor(VoxelType? voxelType, BiomeType biome, int altitude, int seed) {
            Vector3 color = COLORS[(int)voxelType].ToVector3() * new Vector3(255, 255, 255);

            if (voxelType == VoxelType.GRASS) {
                switch ((int)biome) {
                    case 0: case 1: case 2: case 3: case 4: case 5: case 6: case 23: color += new Vector3(70, 7, 20); break;  // super dry
                    case 20: case 21: case 22: case 24: case 25: color += new Vector3(50, 5, 15); break;                      // dry
                    case 10: case 11: case 12: color += new Vector3(15, 5, -2); break;                                        // rainy
                    case 50: case 51: case 52: case 53: case 54: color += new Vector3(0, -24, 5); break;                      // cold
                    case 31: case 41: color += new Vector3(57, -55, 10); break;                                               // dark soil
                    default: break;                                                                                           // no shade
                }
            }
            else if (voxelType == VoxelType.OAK_LEAVES) {
                switch (new Random(Global.SEED * seed).Next(0, 3)) {
                    case 0: color += new Vector3(-17, -15, -2); break;  // dark shade
                    case 1: color += new Vector3(15, 12, 3); break;     // light shade
                    default: break;                                     // no shade
                }
            }
            else if (voxelType == VoxelType.KAPOK_LEAVES) {
                switch (new Random(Global.SEED * seed).Next(0, 3)) {
                    case 0: color += new Vector3(-17, -15, -2); break;  // dark shade
                    case 1: color += new Vector3(8, 12, 3); break;     // light shade
                    default: break;                                     // no shade
                }
            }
            else if (voxelType == VoxelType.BEECH_LEAVES) {
                switch (new Random(Global.SEED * seed).Next(0, 4)) {
                    case 0: color += new Vector3(-15, 14, -20); break;  // green shade
                    case 1: color += new Vector3(-18, 20, -27); break;  // greener shade
                    default: break;                                     // no shade
                }
            } 
            else if (voxelType == VoxelType.MAPLE_LEAVES) {
                switch (new Random(Global.SEED * seed).Next(0, 3)) {
                    case 0: color += new Vector3(-30, 16, -3); break;  // green shade
                    case 1: color += new Vector3(-40, 23, -7); break;  // greener shade
                    default: break;                                    // no shade
                }
            }

            return new(color.X / 255, color.Y / 255, color.Z / 255);
        }

        internal static float GetVoxelTransparency(VoxelType? voxelType) {
            switch (voxelType) {
                case VoxelType.OAK_LEAVES: return 0.9f;
                case VoxelType.KAPOK_LEAVES: return 0.9f;
                case VoxelType.SPRUCE_LEAVES: return 0.9f;
                case VoxelType.BEECH_LEAVES: return 0.9f;
                case VoxelType.MAPLE_LEAVES: return 0.9f;
                default: return 1.0f;
            }
        }
    }
}
