//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

using minecraft_kurwa.src.global;
using minecraft_kurwa.src.renderer.voxels;

namespace minecraft_kurwa.src.generator.terrain.biomes {
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
                ? (BiomeType)(Global.BIOME_MAP[x, z, 0] - Global.BIOME_MAP[x, z, 0] % 10)
                : BiomeType.VOID;
        }

        internal static BiomeType GetSubbiome(ushort x, ushort z) {
            return x < Settings.WORLD_SIZE && z < Settings.WORLD_SIZE
                ? (BiomeType)Global.BIOME_MAP[x, z, 0]
                : BiomeType.VOID;
        }

        internal static BiomeType GetSecondaryBiome(ushort x, ushort z) {
            return x < Settings.WORLD_SIZE && z < Settings.WORLD_SIZE
                ? (BiomeType)Global.BIOME_MAP[x, z, 2]
                : BiomeType.VOID;
        }

        internal static BiomeType GetTertiaryBiome(ushort x, ushort z) {
            return x < Settings.WORLD_SIZE && z < Settings.WORLD_SIZE
                ? (BiomeType)Global.BIOME_MAP[x, z, 3]
                : BiomeType.VOID;
        }

        internal static byte GetBiomeBlending(ushort x, ushort z) {
            return (byte)(x < Settings.WORLD_SIZE && z < Settings.WORLD_SIZE
                ? Global.BIOME_MAP[x, z, 1]
                : 0);
        }

        internal static byte[] GetTopBlocks(byte biome) {
            if (biome == (byte)BiomeType.TROPICAL_DRY_DESERT_SANDY) return new[] { (byte)VoxelType.SAND, (byte)VoxelType.SAND, (byte)VoxelType.SANDSTONE };
            if (biome == (byte)BiomeType.TROPICAL_DRY_DESERT_STONY) return new[] { (byte)VoxelType.STONE, (byte)VoxelType.STONE, (byte)VoxelType.GRAVEL };
            if (biome == (byte)BiomeType.TROPICAL_DRY_DESERT_GRAVEL) return new[] { (byte)VoxelType.STONE, (byte)VoxelType.GRAVEL, (byte)VoxelType.GRAVEL };
            if (biome == (byte)BiomeType.TROPICAL_DRY_DESERT_TERRACOTTA) return new[] { (byte)VoxelType.TERRACOTTA, (byte)VoxelType.SAND };
            if (biome is (byte)BiomeType.SUBPOLAR_FOREST or (byte)BiomeType.SUBPOLAR_PLAINS or (byte)BiomeType.SUBPOLAR) return new[] { (byte)VoxelType.GRASS, (byte)VoxelType.SNOW };
            if (biome == (byte)BiomeType.POLAR_HIGHLAND) return new[] { (byte)VoxelType.SNOW, (byte)VoxelType.ICE };
            if (biome >= 50) return new[] { (byte)VoxelType.SNOW };

            return new[] { (byte)VoxelType.GRASS };
        }

        /// <summary>
        /// retuns values that tell the generator how to process the biomes
        /// </summary>
        /// <param name="biome" />
        /// <returns>an array of strings with a operation and numbers associated with the operation</returns>
        internal static (string, float)[] GetTerrainGeneratorValues(BiomeType biome) {
            switch (biome) {
                case BiomeType.OCEAN: return new[] { ("*", 0f) };
                case BiomeType.POLAR_HIGHLAND: return new[] { ("*", 0.1f), ("+", 100f) };
                default: return new[] { ("*", 1f) };
            }
        }
    }
}
