//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

using minecraft_kurwa.src.global;
using minecraft_kurwa.src.renderer.voxels;
using System;

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

        internal static byte GetTopBlock(byte biome) {
            return biome switch {
                1 => Global.RANDOM.Next(0, 3) == 0 ? (byte)VoxelType.SANDSTONE : (byte)VoxelType.SAND,
                2 => Global.RANDOM.Next(0, 4) == 0 ? (byte)VoxelType.GRAVEL : (byte)VoxelType.STONE,
                3 => Global.RANDOM.Next(0, 4) == 0 ? (byte)VoxelType.STONE : (byte)VoxelType.GRAVEL,
                4 => Global.RANDOM.Next(0, 3) == 0 ? (byte)VoxelType.TERRACOTTA : (byte)VoxelType.SAND,
                62 => Global.RANDOM.Next(0, 2) == 0 ? (byte)VoxelType.ICE : (byte)VoxelType.SNOW,
                5 or 23 => Global.RANDOM.Next(0, 15) == 0 ? (byte)VoxelType.DRY_GRASS : (byte)VoxelType.GRASS,
                21 or 31 or 32 => Global.RANDOM.Next(0, 250) == 0 ? (byte)VoxelType.RIVER_ROCK : (byte)VoxelType.GRASS,
                50 or 51 or 52 => Global.RANDOM.Next(0, 2) == 0 ? (byte)VoxelType.GRASS : (byte)VoxelType.SNOW,
                60 or 61 or 63 or 64 => (byte)VoxelType.SNOW,
                _ => (byte)VoxelType.GRASS
            };
        }

        /// <summary>
        /// retuns values that tell the generator how to process the biomes
        /// </summary>
        /// <param name="biome" />
        /// <returns>an array of strings with a operation and numbers associated with the operation</returns>
        internal static (string, float)[] GetTerrainGeneratorValues(BiomeType biome) {
            return biome switch {
                BiomeType.OCEAN => new[] { ("*", 0f) },
                BiomeType.POLAR_HIGHLAND => new[] { ("*", 0.1f), ("+", 100f) },
                _ => new[] { ("*", 1f) },
            };
        }
    }
}
