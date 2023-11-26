//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

using System;

namespace minecraft_kurwa {
    
    internal class BiomeGenerator {

        internal static byte[,,] GenerateBiomeMap() {
            byte[,,] biomeType = new byte[Settings.WORLD_SIZE, Settings.WORLD_SIZE, 4];
            byte[,,] subbiomeType = new byte[Settings.WORLD_SIZE, Settings.WORLD_SIZE, 4];

            PerlinNoise p = new(Settings.SEED);

            // main biome type
            for (ushort x = 0; x < Settings.WORLD_SIZE; x++) {
                for (ushort y = 0; y < Settings.WORLD_SIZE; y++) {
                    biomeType[x, y, 0] = (byte) ((int) (p.Noise((double) x / Settings.BIOME_SCALE, (double) y / Settings.BIOME_SCALE) * 10) * -10);
                }
            }

            // prevention of the noises being same
            p.seed += 1;

            // subbiome type
            for (ushort x = 0; x < Settings.WORLD_SIZE; x++) {
                for (ushort y = 0; y < Settings.WORLD_SIZE; y++) {
                    subbiomeType[x, y, 0] = (byte)((int)(p.Noise((double)x / Settings.SUBBIOME_SCALE, (double)y / Settings.SUBBIOME_SCALE) * 3) + 3);
                }
            }

            // merging
            for (ushort x = 0; x < Settings.WORLD_SIZE; x++) {
                for (ushort y = 0; y < Settings.WORLD_SIZE; y++) {
                    biomeType[x, y, 0] = (byte)(biomeType[x, y, 0] + subbiomeType[x, y, 0]);
                }
            }

            p.seed += 1;
            
            // ocean generation // TODO ocean
            // for (ushort x = 0; x < Settings.WORLD_SIZE; x++) {
            //     for (ushort y = 0; y < Settings.WORLD_SIZE; y++) {
            //         if (p.Noise((double) x / Settings.OCEAN_SCALE, (double) y / Settings.OCEAN_SCALE) > 0.5) biomeType[x, y, 0] = 70;
            //     }
            // }

            // prevention of setting of a not existing biome
            for (ushort x = 0; x < Settings.WORLD_SIZE; x++) {
                for (ushort y = 0; y < Settings.WORLD_SIZE; y++) {
                    if ((byte) BiomeType.TROPICAL_DRY      + Biome.TROPICAL_DRY_BIOME_COUNT      < biomeType[x, y, 0] && biomeType[x, y, 0] < (byte) BiomeType.TROPICAL_RAINY)    biomeType[x, y, 0] = 1;
                    if ((byte) BiomeType.TROPICAL_RAINY    + Biome.TROPICAL_RAINY_BIOME_COUNT    < biomeType[x, y, 0] && biomeType[x, y, 0] < (byte) BiomeType.SUBTROPICAL)       biomeType[x, y, 0] = 11;
                    if ((byte) BiomeType.SUBTROPICAL       + Biome.SUBTROPICAL_BIOME_COUNT       < biomeType[x, y, 0] && biomeType[x, y, 0] < (byte) BiomeType.TEMPERATE_OCEANIC) biomeType[x, y, 0] = 21;
                    if ((byte) BiomeType.TEMPERATE_OCEANIC + Biome.TEMPERATE_OCEANIC_BIOME_COUNT < biomeType[x, y, 0] && biomeType[x, y, 0] < (byte) BiomeType.TEMPERATE_INLAND)  biomeType[x, y, 0] = 31;
                    if ((byte) BiomeType.TEMPERATE_INLAND  + Biome.TEMPERATE_INLAND_BIOME_COUNT  < biomeType[x, y, 0] && biomeType[x, y, 0] < (byte) BiomeType.SUBPOLAR)          biomeType[x, y, 0] = 41;
                    if ((byte) BiomeType.SUBPOLAR          + Biome.SUBPOLAR_BIOME_COUNT          < biomeType[x, y, 0] && biomeType[x, y, 0] < (byte) BiomeType.POLAR)             biomeType[x, y, 0] = 51;
                    if ((byte) BiomeType.POLAR             + Biome.POLAR_BIOME_COUNT             < biomeType[x, y, 0] && biomeType[x, y, 0] < (byte) BiomeType.OCEAN)             biomeType[x, y, 0] = 61;
                    if ((byte) BiomeType.OCEAN                                                   < biomeType[x, y, 0])                                                            biomeType[x, y, 0] = 62;
                }
            }

            return biomeType;
        }

        public static void GenerateBiomeBlending() {
            byte[,] auxiliary = new byte[Settings.WORLD_SIZE, Settings.WORLD_SIZE];

            // make the borders of the biomes blend 50 : 50 and save what is the secondary biome
            for (int x = 0; x < Settings.WORLD_SIZE; x++) {
                for (int y = 0; y < Settings.WORLD_SIZE; y++) {
                    if (x < Settings.WORLD_SIZE - 1) if (Global.BIOME_MAP[x, y, 0] / 10 != Global.BIOME_MAP[x + 1, y, 0 ] / 10) {
                        Global.BIOME_MAP[x, y, 1] = Settings.BIOME_BLENDING;
                        Global.BIOME_MAP[x, y, 2] = Global.BIOME_MAP[x + 1, y, 0];
                    }

                    if (x > 0) if (Global.BIOME_MAP[x, y, 0] / 10 != Global.BIOME_MAP[x - 1, y, 0] / 10) {
                        Global.BIOME_MAP[x, y, 1] = Settings.BIOME_BLENDING;
                        Global.BIOME_MAP[x, y, 2] = Global.BIOME_MAP[x - 1, y, 0];
                    }

                    if (y < Settings.WORLD_SIZE - 1) if (Global.BIOME_MAP[x, y, 0] / 10 != Global.BIOME_MAP[x, y + 1, 0] / 10) {
                        Global.BIOME_MAP[x, y, 1] = Settings.BIOME_BLENDING;
                        Global.BIOME_MAP[x, y, 2] = Global.BIOME_MAP[x, y + 1, 0];
                    }

                    if (y > 0) if (Global.BIOME_MAP[x, y, 0] / 10 != Global.BIOME_MAP[x, y - 1, 0] / 10) {
                        Global.BIOME_MAP[x, y, 1] = Settings.BIOME_BLENDING;
                        Global.BIOME_MAP[x, y, 2] = Global.BIOME_MAP[x, y - 1, 0];
                    }
                }
            }

            // spread the biome blend values
            for (int i = 0; i < Settings.BIOME_BLENDING; i++) {
                for (int x = 0; x < Settings.WORLD_SIZE; x++) {
                    for (int y = 0; y < Settings.WORLD_SIZE; y++) {
                        if (Global.BIOME_MAP[x, y, 1] > 0) {
                            
                            if (x < Settings.WORLD_SIZE - 1) if (Global.BIOME_MAP[x + 1, y, 1] < Global.BIOME_MAP[x, y, 1] - 1) {
                                Global.BIOME_MAP[x + 1, y, 1] = (byte)(Global.BIOME_MAP[x, y, 1] - 1);
                                if (Global.BIOME_MAP[x + 1, y, 2] == 0) Global.BIOME_MAP[x + 1, y, 2] = Global.BIOME_MAP[x, y, 2]; else Global.BIOME_MAP[x + 1, y, 3] = Global.BIOME_MAP[x, y, 2];
                            }
                        
                            if (x > 0) if (Global.BIOME_MAP[x - 1, y, 1] < Global.BIOME_MAP[x, y, 1] - 1) {
                                Global.BIOME_MAP[x - 1, y, 1] = (byte)(Global.BIOME_MAP[x, y, 1] - 1);  
                                if (Global.BIOME_MAP[x - 1, y, 2] == 0) Global.BIOME_MAP[x - 1, y, 2] = Global.BIOME_MAP[x, y, 2]; else Global.BIOME_MAP[x - 1, y, 3] = Global.BIOME_MAP[x, y, 2];
                            }
                        
                            if (y < Settings.WORLD_SIZE - 1) if (Global.BIOME_MAP[x, y + 1, 1] < Global.BIOME_MAP[x, y, 1] - 1) {
                                Global.BIOME_MAP[x, y + 1, 1] = (byte)(Global.BIOME_MAP[x, y, 1] - 1);
                                if (Global.BIOME_MAP[x, y + 1, 2] == 0) Global.BIOME_MAP[x, y + 1, 2] = Global.BIOME_MAP[x, y, 2];else Global.BIOME_MAP[x, y + 1, 3] = Global.BIOME_MAP[x, y, 2];
                            }
                        
                            if (y > 0) if (Global.BIOME_MAP[x, y - 1, 1] < Global.BIOME_MAP[x, y, 1] - 1) {
                                Global.BIOME_MAP[x, y - 1, 1] = (byte)(Global.BIOME_MAP[x, y, 1] - 1);
                                if (Global.BIOME_MAP[x, y - 1, 2] == 0) Global.BIOME_MAP[x, y - 1, 2] = Global.BIOME_MAP[x, y, 2]; else Global.BIOME_MAP[x, y - 1, 3] = Global.BIOME_MAP[x, y, 2];
                            }
                        }
                    }
                }
            }
        }
    }
    
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
                ? (BiomeType)(Global.BIOME_MAP[x, z, 0] - (Global.BIOME_MAP[x, z, 0] % 10))
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
            if (biome == (byte) BiomeType.TROPICAL_DRY_DESERT_SANDY) return new[] {(byte) VoxelType.SAND, (byte) VoxelType.SAND, (byte) VoxelType.SANDSTONE};
            if (biome == (byte) BiomeType.TROPICAL_DRY_DESERT_STONY) return new[] {(byte) VoxelType.STONE, (byte) VoxelType.STONE, (byte) VoxelType.GRAVEL};
            if (biome == (byte) BiomeType.TROPICAL_DRY_DESERT_GRAVEL) return new[] {(byte) VoxelType.STONE, (byte) VoxelType.GRAVEL, (byte) VoxelType.GRAVEL};
            if (biome == (byte) BiomeType.TROPICAL_DRY_DESERT_TERRACOTTA) return new[] {(byte) VoxelType.TERRACOTTA, (byte) VoxelType.SAND};
            if (biome is (byte) BiomeType.SUBPOLAR_FOREST or (byte) BiomeType.SUBPOLAR_PLAINS or (byte) BiomeType.SUBPOLAR) return new[] {(byte) VoxelType.GRASS, (byte) VoxelType.SNOW};
            if (biome == (byte) BiomeType.POLAR_HIGHLAND) return new[] {(byte) VoxelType.SNOW, (byte) VoxelType.ICE};
            if (biome >= 50) return new[] {(byte) VoxelType.SNOW};

            return new[] { (byte)VoxelType.GRASS };
        }

        /// <summary>
        /// retuns values that tell the generator how to process the biomes
        /// </summary>
        /// <param name="biome" />
        /// <returns>an array of strings with a operation and numbers associated with the operation</returns>
        internal static (string, float)[] GetTerrainGeneratorValues(BiomeType biome) {
            switch (biome) {
                case BiomeType.OCEAN: return new []{("*", 0f)};
                case BiomeType.POLAR_HIGHLAND: return new []{("*", 0.1f), ("+", 100f)};
                default: return new[] { ("*", 1f) };
            }
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
        POLAR_ICEBERG = 64,
        
        OCEAN = 70
    }
}
