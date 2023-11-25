//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

namespace minecraft_kurwa {
    internal static class Biome {
        internal const int TROPICAL_DRY_BIOME_COUNT = 6;
        internal const int TROPICAL_RAINY_BIOME_COUNT = 3;
        internal const int SUBTROPICAL_BIOME_COUNT = 5;
        internal const int TEMPERATE_OCEANIC_BIOME_COUNT = 4;
        internal const int TEMPERATE_INLAND_BIOME_COUNT = 4;
        internal const int SUBPOLAR_BIOME_COUNT = 2;
        internal const int POLAR_BIOME_COUNT = 4;

        internal static BiomeType GetBiome(ushort x, ushort z) {
            return x < Settings.WORLD_SIZE && z < Settings.WORLD_SIZE 
                ? (BiomeType)(Global.BIOME_MAP[x, z] - (Global.BIOME_MAP[x, z] % 10))
                : BiomeType.VOID;
        }

        internal static BiomeType GetSubbiome(ushort x, ushort z) {
            return x < Settings.WORLD_SIZE && z < Settings.WORLD_SIZE
                ? (BiomeType)Global.BIOME_MAP[x, z]
                : BiomeType.VOID;
        }

        internal static byte[] GetTopBlocks(byte biome) {
            if (biome == (byte) BiomeType.TROPICAL_DRY_DESERT_SANDY) return new[] {(byte) VoxelType.SAND, (byte) VoxelType.SANDSTONE};
            if (biome == (byte) BiomeType.TROPICAL_DRY_DESERT_STONY) return new[] {(byte) VoxelType.STONE, (byte) VoxelType.STONE, (byte) VoxelType.GRAVEL};
            if (biome == (byte) BiomeType.TROPICAL_DRY_DESERT_GRAVEL) return new[] {(byte) VoxelType.STONE, (byte) VoxelType.GRAVEL, (byte) VoxelType.GRAVEL};
            if (biome == (byte) BiomeType.TROPICAL_DRY_DESERT_TERRACOTTA) return new[] {(byte) VoxelType.TERRACOTTA, (byte) VoxelType.SAND};
            if (biome is (byte) BiomeType.SUBPOLAR_FOREST or (byte) BiomeType.SUBPOLAR_PLAINS or (byte) BiomeType.SUBPOLAR) return new[] {(byte) VoxelType.GRASS, (byte) VoxelType.SNOW};
            if (biome == (byte) BiomeType.POLAR_HIGHLAND) return new[] {(byte) VoxelType.SNOW, (byte) VoxelType.ICE};
            if (biome >= 50) return new[] {(byte) VoxelType.SNOW};

            return new[] { (byte)VoxelType.GRASS };
        }
    }

    internal enum BiomeType {
        VOID = -1,

        TROPICAL_DRY = 0,
        TROPICAL_DRY_DESERT_SANDY = 1,
        TROPICAL_DRY_DESERT_STONY = 2,
        TROPICAL_DRY_DESERT_GRAVEL = 3,
        TROPICAL_DRY_DESERT_TERRACOTTA = 4,
        TROPICAL_DRY_SAVANNA = 5,
        TROPICAL_DRY_WASTELAND = 6,

        TROPICAL_RAINY = 10,
        TROPICAL_RAINY_JUNGLE = 11,
        TROPICAL_RAINY_BAMBOO_JUNGLE = 12,
        TROPICAL_RAINY_PLAINS = 13,

        SUBTROPICAL = 20,
        SUBTROPICAL_CONIFEROUS_FOREST = 21,
        SUBTROPICAL_BROADLEAF_FOREST = 22,
        SUBTROPICAL_SAVANNA = 23,
        SUBTROPICAL_BUSH = 24,
        SUBTROPICAL_WASTELAND = 25,

        TEMPERATE_OCEANIC = 30,
        TEMPERATE_OCEANIC_CONIFEROUS_FOREST = 31,
        TEMPERATE_OCEANIC_BROADLEAF_FOREST = 32,
        TEMPERATE_OCEANIC_PLAINS = 33,
        TEMPERATE_OCEANIC_THORNS = 34,

        TEMPERATE_INLAND = 40,
        TEMPERATE_INLAND_CONIFEROUS_FOREST = 41,
        TEMPERATE_INLAND_BROADLEAF_FOREST = 42,
        TEMPERATE_INLAND_PLAINS = 43,
        TEMPERATE_INLAND_MEADOW = 44,
        
        SUBPOLAR = 50,
        SUBPOLAR_FOREST = 51,
        SUBPOLAR_PLAINS = 52,

        POLAR = 60,
        POLAR_TAIGA = 61,
        POLAR_HIGHLAND = 62,
        POLAR_TUNDRA = 63,
        POLAR_ICEBERG = 64
    }
}
