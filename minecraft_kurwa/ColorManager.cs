//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

using Microsoft.Xna.Framework;

namespace minecraft_kurwa {
    internal static class ColorManager {
        internal static readonly Vector3 FRONT_SHADOW = new(0.9f, 0.9f, 0.9f);
        internal static readonly Vector3 SIDE_SHADOW = new(0.8f, 0.8f, 0.8f);
        internal static readonly Vector3 BACK_SHADOW = new(0.7f, 0.7f, 0.7f);
        internal static readonly Vector3 BOTTOM_SHADOW = new(0.6f, 0.6f, 0.6f);
        internal static readonly Vector3 TOP_SHADOW = new(1.0f, 1.0f, 1.0f);

        private static readonly Color[] BASE_COLORS = {
            new(255, 0, 255),   // 0 - purple
            new(19, 133, 16),   // 1 - grass
            new(103, 146, 125), // 2 - rock
            new(255, 255, 255), // 3 - snow
        };

        private static readonly Color[] SHADES = {
            new(0, 0, 0),       // 0 - no shade
            new(0, -24, 5),     // 1 - grass - cold
            new(15, 5, -2),     // 2 - grass - warm
            new(50, 5, 15),     // 3 - grass - dry
            new(24, -15, 9),    // 4 - rock - mossy
        };

        internal static Color GetVoxelColor(VoxelType? voxelType, BiomeType biomeType, int altitude) {
            Color baseColor = BASE_COLORS[0];
            Color shade = SHADES[0];

            if (voxelType == VoxelType.GRASS) {
                baseColor = BASE_COLORS[1];

                if ((int)biomeType < 10) shade = SHADES[3];        // tropical dry
                else if ((int)biomeType < 20) shade = SHADES[2];   // tropical rainy
                else if ((int)biomeType < 50) shade = SHADES[0];   // subtropical, temperate oceanic, temperate inland
                else shade = SHADES[1];                            // cold
            }
            else if (voxelType == VoxelType.STONE) {
                baseColor = BASE_COLORS[2];

                //if ((int)biomeType >= 10 && (int)biomeType < 20) shade = SHADES[4]; // tropical rainy
            }

            Vector3 result = baseColor.ToVector3() + shade.ToVector3();
            return new(result.X, result.Y, result.Z);
        }
    }
}
