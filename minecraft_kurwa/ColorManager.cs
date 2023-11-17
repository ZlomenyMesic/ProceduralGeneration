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
            new(100, 110, 106), // 2 - rock
            new(242, 210, 169), // 3 - sand
            new(185, 232, 234), // 4 - ice
            new(179, 86, 66),   // 5 - terracotta
            new(83, 84, 78),    // 6 - gravel
            new(255, 255, 255), // 7 - snow
        };

        private static readonly Color[] SHADES = {
            new(0, 0, 0),       // 0 - no shade
            new(0, -24, 5),     // 1 - grass - cold
            new(15, 5, -2),     // 2 - grass - rainy
            new(50, 5, 15),     // 3 - grass - dry
            new(70, 7, 20),     // 4 - grass - super dry
        };

        internal static Color GetVoxelColor(VoxelType? voxelType, BiomeType biome, int altitude) {
            Color @base = BASE_COLORS[(int)voxelType];
            Color shade = SHADES[0];

            if (voxelType == VoxelType.GRASS) {
                if ((int)biome < 10 || (int)biome == 23) shade = SHADES[4];  // super dry
                else if ((int)biome < 20) shade = SHADES[2];                 // rainy
                else if ((int)biome < 30) shade = SHADES[3];                 // dry
                else if ((int)biome < 50) shade = SHADES[0];                 // normal
                else shade = SHADES[1];                                      // cold
            }
            else if (voxelType == VoxelType.STONE) {
                
            }
            else if (voxelType == VoxelType.SAND) {
                
            }
            else if (voxelType == VoxelType.ICE) {
                
            }
            else if (voxelType == VoxelType.TERRACOTTA) {

            }
            else if (voxelType == VoxelType.GRAVEL) {

            }
            else if (voxelType == VoxelType.SNOW) {

            }

            Vector3 result = @base.ToVector3() + shade.ToVector3();
            return new(result.X, result.Y, result.Z);
        }
    }
}
