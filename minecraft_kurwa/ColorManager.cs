//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

using Microsoft.Xna.Framework;

namespace minecraft_kurwa {
    internal static class ColorManager {
        internal static readonly Vector3 FRONT_SHADOW = new(0.7f, 0.7f, 0.7f);
        internal static readonly Vector3 SIDE_SHADOW = new(0.8f, 0.8f, 0.8f);
        internal static readonly Vector3 BACK_SHADOW = new(0.7f, 0.7f, 0.7f);
        internal static readonly Vector3 BOTTOM_SHADOW = new(0.6f, 0.6f, 0.6f);
        internal static readonly Vector3 TOP_SHADOW = new(1.0f, 1.0f, 1.0f);

        private static readonly Color[] BASE_COLORS = {
            new(255, 0, 255),   // 0 - purple
            new(19, 133, 16),   // 1 - grass
            new(100, 110, 106), // 2 - rock
            new(242, 210, 169), // 3 - sand
            new(185, 232, 234), // 4 - ice
            new(179, 86, 66),   // 5 - terracotta
            new(83, 84, 78),    // 6 - gravel
            new(255, 255, 255), // 7 - snow
            new(0, 255, 0),     // 8 - oak leaves
        };

        private static readonly Color[] SHADES = {
            new(0, 0, 0),       // 0 - no shade
            new(70, 7, 20),     // 1 - grass - super dry
            new(50, 5, 15),     // 2 - grass - dry
            new(15, 5, -2),     // 3 - grass - rainy
            new(0, -24, 5),     // 4 - grass - cold
        };

        internal static Color GetVoxelColor(VoxelType? voxelType, BiomeType biome, int altitude) {
            Color @base = BASE_COLORS[(int)voxelType];
            Color shade = SHADES[0];

            if (voxelType == VoxelType.GRASS) {
                switch ((int)biome) {
                    case 1: case 2: case 3: case 4: case 5: case 6: case 23: shade = SHADES[1]; break;  // super dry
                    case 21: case 22: case 24: case 25: shade = SHADES[2]; break;                       // dry
                    case 11: case 12: shade = SHADES[3]; break;                                         // rainy
                    case 51: case 52: case 53: case 54: shade = SHADES[4]; break;                       // cold
                    default: shade = SHADES[0]; break;                                                  // normal
                }
            }

            Vector3 result = @base.ToVector3() + shade.ToVector3();
            return new(result.X, result.Y, result.Z);
        }
    }
}
