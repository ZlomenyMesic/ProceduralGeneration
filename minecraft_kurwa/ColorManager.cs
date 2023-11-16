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

        internal static readonly Color[] LIST = {
            new(255, 0, 255),   // NONE - purple, default color
            new(19, 109, 21),   // GRASS_COLD
            new(19, 133, 16),   // GRASS_NORMAL
            new(65, 152, 10),   // GRASS_WARM
            new(120, 144, 48),  // GRASS_DRY
            new(144, 120, 48),  // DIRT_DRY
            new(103, 146, 125), // ROCK_NORMAL
            new(127, 131, 134), // ROCK_MOSSY
            new(255, 255, 255)  // SNOW
        };
        internal static Colors GetTypeColor(VoxelType? voxelType, BiomeType biomeType, int altitude) {
            // TODO: colors based on biome type

            if (voxelType == VoxelType.GRASS) {
                if (altitude > 70) return Colors.SNOW;
                else if (altitude > 66) return new System.Random().Next(0, 2) == 0 ? Colors.SNOW : Colors.GRASS_COLD;
                else if (altitude < 10) return Colors.GRASS_NORMAL;
                return Colors.GRASS_COLD;
            } 
            else if (voxelType == VoxelType.STONE) {
                return Colors.ROCK_NORMAL;
            }

            return Colors.NONE;
        }
    }

    internal enum Colors {
        NONE = 0,
        GRASS_COLD = 1,
        GRASS_NORMAL = 2,
        GRASS_WARM = 3,
        GRASS_DRY = 4,
        DIRT_DRY = 5,
        ROCK_NORMAL = 6,
        ROCK_MOSSY = 7,
        SNOW = 8,
    }
}
