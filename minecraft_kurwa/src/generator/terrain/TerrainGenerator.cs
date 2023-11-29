//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

using System;
using minecraft_kurwa.src.generator.terrain.biomes;
using minecraft_kurwa.src.global;
using minecraft_kurwa.src.generator.terrain.noise;

namespace minecraft_kurwa.src.generator.terrain
{
    internal class TerrainGenerator {

        internal static void GenerateHeightMap() {
            PerlinNoise perlinNoise = new(Settings.SEED);
            Random random = new(Settings.SEED);


            // main generation handling
            for (ushort x = 0; x < Settings.WORLD_SIZE; x++) {
                for (ushort y = 0; y < Settings.WORLD_SIZE; y++) {

                    // primary height
                    ushort pHeight = (ushort)Math.Abs(perlinNoise.Noise((double)x / Settings.MAIN_NOISE_SCALE, (double)y / Settings.MAIN_NOISE_SCALE) * Settings.MAIN_NOISE_SHARPNESS * 5 / 2 + Settings.MAIN_NOISE_SHARPNESS * 3 / 2);
                    (string operation, float numerator)[] pOperations = Biome.GetTerrainGeneratorValues((BiomeType)Global.BIOME_MAP[x, y, 0]);

                    for (byte i = 0; i < pOperations.Length; i++) {
                        if (pOperations[i].operation == "*") {
                            pHeight = (ushort)(pHeight * pOperations[i].numerator);
                        }

                        if (pOperations[i].operation == "+") {
                            pHeight = (ushort)(pHeight + pOperations[i].numerator);
                        }
                    }

                    ushort sHeight = (ushort)Math.Abs(perlinNoise.Noise((double)x / Settings.MAIN_NOISE_SCALE, (double)y / Settings.MAIN_NOISE_SCALE) * Settings.MAIN_NOISE_SHARPNESS * 5 / 2 + Settings.MAIN_NOISE_SHARPNESS * 3 / 2);
                    (string operation, float numerator)[] sOperations = Biome.GetTerrainGeneratorValues((BiomeType)Global.BIOME_MAP[x, y, 2]);

                    for (byte i = 0; i < sOperations.Length; i++) {
                        if (sOperations[i].operation == "*") {
                            sHeight = (ushort)(sHeight * sOperations[i].numerator);
                        }

                        if (sOperations[i].operation == "+") {
                            sHeight = (ushort)(sHeight + sOperations[i].numerator);
                        }
                    }

                    ushort tHeight = (ushort)Math.Abs(perlinNoise.Noise((double)x / Settings.MAIN_NOISE_SCALE, (double)y / Settings.MAIN_NOISE_SCALE) * Settings.MAIN_NOISE_SHARPNESS * 5 / 2 + Settings.MAIN_NOISE_SHARPNESS * 3 / 2);
                    (string operation, float numerator)[] tOperations = Biome.GetTerrainGeneratorValues((BiomeType)Global.BIOME_MAP[x, y, 3]);

                    for (byte i = 0; i < tOperations.Length; i++) {
                        if (tOperations[i].operation == "*") {
                            tHeight = (ushort)(tHeight * tOperations[i].numerator);
                        }

                        if (tOperations[i].operation == "+") {
                            tHeight = (ushort)(tHeight + tOperations[i].numerator);
                        }
                    }

                    if (Global.BIOME_MAP[x, y, 2] == (byte)BiomeType.UNKNOWN && Global.BIOME_MAP[x, y, 3] == (byte)BiomeType.UNKNOWN)
                        Global.HEIGHT_MAP[x, y] = pHeight;

                    if (Global.BIOME_MAP[x, y, 2] != (byte)BiomeType.UNKNOWN && Global.BIOME_MAP[x, y, 3] == (byte)BiomeType.UNKNOWN)
                        Global.HEIGHT_MAP[x, y] = (ushort)((pHeight * (100 - Global.BIOME_MAP[x, y, 1]) + sHeight * Global.BIOME_MAP[x, y, 1]) / 200);

                    if (Global.BIOME_MAP[x, y, 2] != (byte)BiomeType.UNKNOWN && Global.BIOME_MAP[x, y, 3] != (byte)BiomeType.UNKNOWN)
                        Global.HEIGHT_MAP[x, y] = (ushort)((pHeight * (100 - Global.BIOME_MAP[x, y, 1]) + sHeight * Global.BIOME_MAP[x, y, 1] / 2 + tHeight * Global.BIOME_MAP[x, y, 1] / 2) / 300);
                }
            }

            // terrain collapse
            for (ushort i = 0; i < Settings.HEIGHT_LIMIT / Settings.TERRAIN_COLLAPSE_LIMIT; i++) {
                for (ushort x = 0; x < Settings.WORLD_SIZE; x++) {
                    for (ushort y = 0; y < Settings.WORLD_SIZE; y++) {
                        if (x < Settings.WORLD_SIZE - 1) {
                            if (Global.HEIGHT_MAP[x, y] - Global.HEIGHT_MAP[x + 1, y] > Settings.TERRAIN_COLLAPSE_LIMIT) {
                                Global.HEIGHT_MAP[x + 1, y] = (ushort)(Global.HEIGHT_MAP[x, y] - Settings.TERRAIN_COLLAPSE_LIMIT + random.Next(-1, 1));
                            }

                            if (Global.HEIGHT_MAP[x, y] - Global.HEIGHT_MAP[x + 1, y] < -Settings.TERRAIN_COLLAPSE_LIMIT) {
                                Global.HEIGHT_MAP[x + 1, y] = (ushort)(Global.HEIGHT_MAP[x, y] + Settings.TERRAIN_COLLAPSE_LIMIT + random.Next(-1, 1));
                            }
                        }

                        if (x > 0) {
                            if (Global.HEIGHT_MAP[x, y] - Global.HEIGHT_MAP[x - 1, y] > Settings.TERRAIN_COLLAPSE_LIMIT) {
                                Global.HEIGHT_MAP[x - 1, y] = (ushort)(Global.HEIGHT_MAP[x, y] - Settings.TERRAIN_COLLAPSE_LIMIT + random.Next(-1, 1));
                            }

                            if (Global.HEIGHT_MAP[x, y] - Global.HEIGHT_MAP[x - 1, y] < -Settings.TERRAIN_COLLAPSE_LIMIT) {
                                Global.HEIGHT_MAP[x - 1, y] = (ushort)(Global.HEIGHT_MAP[x, y] + Settings.TERRAIN_COLLAPSE_LIMIT + random.Next(-1, 1));
                            }
                        }

                        if (y < Settings.WORLD_SIZE - 1) {
                            if (Global.HEIGHT_MAP[x, y] - Global.HEIGHT_MAP[x, y + 1] > Settings.TERRAIN_COLLAPSE_LIMIT) {
                                Global.HEIGHT_MAP[x, y + 1] = (ushort)(Global.HEIGHT_MAP[x, y] - Settings.TERRAIN_COLLAPSE_LIMIT + random.Next(-1, 1));
                            }

                            if (Global.HEIGHT_MAP[x, y] - Global.HEIGHT_MAP[x, y + 1] < -Settings.TERRAIN_COLLAPSE_LIMIT) {
                                Global.HEIGHT_MAP[x, y + 1] = (ushort)(Global.HEIGHT_MAP[x, y] + Settings.TERRAIN_COLLAPSE_LIMIT + random.Next(-1, 1));
                            }
                        }

                        if (y > 0) {
                            if (Global.HEIGHT_MAP[x, y] - Global.HEIGHT_MAP[x, y - 1] > Settings.TERRAIN_COLLAPSE_LIMIT) {
                                Global.HEIGHT_MAP[x, y - 1] = (ushort)(Global.HEIGHT_MAP[x, y] - Settings.TERRAIN_COLLAPSE_LIMIT + random.Next(-1, 1));
                            }

                            if (Global.HEIGHT_MAP[x, y] - Global.HEIGHT_MAP[x, y - 1] < -Settings.TERRAIN_COLLAPSE_LIMIT) {
                                Global.HEIGHT_MAP[x, y - 1] = (ushort)(Global.HEIGHT_MAP[x, y] + Settings.TERRAIN_COLLAPSE_LIMIT + random.Next(-1, 1));
                            }
                        }
                    }
                }
            }

            // terrain smoothing
            // for (ushort x = Settings.TERRAIN_SMOOTHING_RADIUS - 1; x < Settings.WORLD_SIZE - Settings.TERRAIN_SMOOTHING_RADIUS + 2; x++) {
            //     for (ushort y = Settings.TERRAIN_SMOOTHING_RADIUS - 1; y < Settings.WORLD_SIZE - Settings.TERRAIN_SMOOTHING_RADIUS + 2; y++) {
            //         ushort avarage = 0;
            //         ushort iterationCount = 0;
            //         
            //         for (ushort tx = (ushort)(x - Settings.TERRAIN_SMOOTHING_RADIUS + 1); tx < x + Settings.TERRAIN_SMOOTHING_RADIUS - 1; tx++) {
            //             for (ushort ty = (ushort)(y - Settings.TERRAIN_SMOOTHING_RADIUS + 1); ty < y + Settings.TERRAIN_SMOOTHING_RADIUS - 1; ty++) {
            //                 avarage += Global.HEIGHT_MAP[tx, ty];
            //                 iterationCount++;
            //             }
            //         }
            //
            //         avarage = (ushort) ((avarage / iterationCount) / 2);
            //         
            //         for (ushort tx = (ushort)(x - Settings.TERRAIN_SMOOTHING_RADIUS + 1); tx < x + Settings.TERRAIN_SMOOTHING_RADIUS - 1; tx++) {
            //             for (ushort ty = (ushort)(y - Settings.TERRAIN_SMOOTHING_RADIUS + 1); ty < y + Settings.TERRAIN_SMOOTHING_RADIUS - 1; ty++) {
            //                 Global.HEIGHT_MAP[tx, ty] = (ushort)((Global.HEIGHT_MAP[tx, ty] * 2 + avarage) / 3);
            //             }
            //         }
            //     }
            // }
        }
    }
}
