﻿//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

using minecraft_kurwa.src.global;
using minecraft_kurwa.src.generator.terrain.noise;
using System;
using System.Collections.Generic;
using minecraft_kurwa.src.global.functions;
using System.Collections;

namespace minecraft_kurwa.src.generator.terrain.biomes;

internal static class BiomeGenerator {
    private static readonly short[] dx = { 1, -1, 0, 0, 1, 1, -1, -1 };
    private static readonly short[] dy = { 0, 0, 1, -1, 1, -1, 1, -1 };

    internal static void GenerateBiomeMap() {
        SimplexNoise noise = new(Settings.SEED);

        for (int x = 0; x < Settings.WORLD_SIZE; x++) {
            for (int y = 0; y < Settings.WORLD_SIZE; y++) {
                byte temperature = (byte)Math.Round(noise.Calculate(x + ExperimentalSettings.NOISE_OFFSET, y + ExperimentalSettings.NOISE_OFFSET, Settings.BIOME_SCALE, 2), 0);
                byte rainfall = (byte)Math.Round(noise.Calculate(x + 5000 + ExperimentalSettings.NOISE_OFFSET, y + 5000 + ExperimentalSettings.NOISE_OFFSET, Settings.BIOME_SCALE, 2), 0);

                byte biome = (byte)BiomeType.UNKNOWN;

                if (rainfall == 0 && temperature == 0) biome = (byte)BiomeType.POLAR;
                else if (rainfall >= 1 && temperature == 0) biome = (byte)BiomeType.SUBPOLAR;
                else if (rainfall == 0 && temperature == 1) biome = (byte)BiomeType.TEMPERATE_INLAND;
                else if (rainfall >= 1 && temperature == 1) biome = (byte)BiomeType.TEMPERATE_OCEANIC;
                else if (rainfall == 0 && temperature == 2) biome = (byte)BiomeType.TROPICAL_DRY;
                else if (rainfall == 1 && temperature == 2) biome = (byte)BiomeType.SUBTROPICAL;
                else if (rainfall == 2 && temperature == 2) biome = (byte)BiomeType.TROPICAL_RAINY;

                byte subbiome = (byte)Math.Round(noise.Calculate(x + 10000 + ExperimentalSettings.NOISE_OFFSET, y + 10000 + ExperimentalSettings.NOISE_OFFSET, Settings.SUBBIOME_SCALE, Biome.GetSubbiomeCount((BiomeType)biome)), 0);
                biome += subbiome;

                Global.BIOME_MAP[x, y, 0] = biome;
            }
        }

        // spreading biomes over edges for a more random look
        for (int x = 0; x < Settings.WORLD_SIZE; x++) {
            for (int y = 0; y < Settings.WORLD_SIZE; y++) {
                byte blending = (byte)Math.Round(noise.Calculate(x + 15000 + ExperimentalSettings.NOISE_OFFSET, y + 15000 + ExperimentalSettings.NOISE_OFFSET, Settings.SUBBIOME_SCALE / 15, 1.8f), 0);
                if (blending != 0) continue;

                byte local = Global.BIOME_MAP[x, y, 0];
                byte[] neighbours = new byte[9];
                byte nCount = 0;

                for (int x2 = -6; x2 < 7; x2 += 6) {
                    if (!World.IsInRange(x + x2)) continue;

                    for (int y2 = -6; y2 < 7; y2 += 6) {
                        if (!World.IsInRange(y + y2)) continue;

                        neighbours[nCount++] = Global.BIOME_MAP[x + x2, y + y2, 0];
                    }
                }

                for (int j = 0; j < nCount; j++) {
                    if (neighbours[j] != local) Global.BIOME_MAP[x, y, 0] = neighbours[j];
                }
            }
        }

        // rougher biome edges
        for (int i = 0; i < 5; i++) {
            for (int x = 0; x < Settings.WORLD_SIZE; x++) {
                for (int y = 0; y < Settings.WORLD_SIZE; y++) {
                    byte local = Global.BIOME_MAP[x, y, 0];
                    Dictionary<byte, byte> neighbours = new();

                    for (int x2 = -1; x2 < 2; x2++) {
                        if (!World.IsInRange(x + x2)) continue;

                        for (int y2 = -1; y2 < 2; y2++) {
                            if (!World.IsInRange(y + y2)) continue;

                            byte neighbour = Global.BIOME_MAP[x + x2, y + y2, 0];

                            if (local != neighbour) {
                                if (neighbours.ContainsKey(neighbour)) {
                                    neighbours[neighbour]++;
                                } else neighbours.Add(neighbour, 1);
                            }
                        }
                    }

                    foreach (var key in neighbours.Keys) {
                        if (neighbours[key] > 2 && Global.RANDOM.Next(0, 3) != 0) Global.BIOME_MAP[x, y, 0] = key;
                    }
                }
            }
        }
    }

    // --------- old biome generation
    //internal static void GenerateBiomeMap() {
    //    byte[,,] subbiomeType = new byte[Settings.WORLD_SIZE, Settings.WORLD_SIZE, 4];

    //    PerlinNoise p = new(Settings.SEED);

    //    // main biome type
    //    for (ushort x = 0; x < Settings.WORLD_SIZE; x++) {
    //        for (ushort y = 0; y < Settings.WORLD_SIZE; y++) {
    //            //Global.BIOME_MAP[x, y, 0] = (byte)(Math.Round(p.Noise((double)(x + ExperimentalSettings.NOISE_OFFSET) / Settings.BIOME_SCALE, (double)(y + ExperimentalSettings.NOISE_OFFSET) / Settings.BIOME_SCALE) * 6) * -10);
    //        }
    //    }

    //    // prevention of the noises being identical
    //    p.seed += 1;

    //    // subbiome type
    //    for (ushort x = 0; x < Settings.WORLD_SIZE; x++) {
    //        for (ushort y = 0; y < Settings.WORLD_SIZE; y++) {
    //            //subbiomeType[x, y, 0] = (byte)((int)(p.Noise((double)(x + ExperimentalSettings.NOISE_OFFSET) / Settings.SUBBIOME_SCALE, (double)(y + ExperimentalSettings.NOISE_OFFSET) / Settings.SUBBIOME_SCALE) * 3) + 3);                    subbiomeType[x, y, 0] = 0;
    //        }
    //    }

    //    // merging
    //    for (ushort x = 0; x < Settings.WORLD_SIZE; x++) {
    //        for (ushort y = 0; y < Settings.WORLD_SIZE; y++) {
    //            Global.BIOME_MAP[x, y, 0] = (byte)(Global.BIOME_MAP[x, y, 0] + subbiomeType[x, y, 0]);
    //        }
    //    }

    //    p.seed += 1;

    //    // ocean generation // TODO ocean
    //    // for (ushort x = 0; x < Settings.WORLD_SIZE; x++) {
    //    //     for (ushort y = 0; y < Settings.WORLD_SIZE; y++) {
    //    //         if (p.Noise((double) x / Settings.OCEAN_SCALE, (double) y / Settings.OCEAN_SCALE) > 0.5) Global.BIOME_MAP[x, y, 0] = 70;
    //    //     }
    //    // }

    //    // prevention of setting of a not existing biome
    //    for (ushort x = 0; x < Settings.WORLD_SIZE; x++) {
    //        for (ushort y = 0; y < Settings.WORLD_SIZE; y++) {
    //            if ((byte)BiomeType.TROPICAL_DRY + Biome.TROPICAL_DRY_BIOME_COUNT < Global.BIOME_MAP[x, y, 0] && Global.BIOME_MAP[x, y, 0] < (byte)BiomeType.TROPICAL_RAINY) Global.BIOME_MAP[x, y, 0] = 1;
    //            if ((byte)BiomeType.TROPICAL_RAINY + Biome.TROPICAL_RAINY_BIOME_COUNT < Global.BIOME_MAP[x, y, 0] && Global.BIOME_MAP[x, y, 0] < (byte)BiomeType.SUBTROPICAL) Global.BIOME_MAP[x, y, 0] = 11;
    //            if ((byte)BiomeType.SUBTROPICAL + Biome.SUBTROPICAL_BIOME_COUNT < Global.BIOME_MAP[x, y, 0] && Global.BIOME_MAP[x, y, 0] < (byte)BiomeType.TEMPERATE_OCEANIC) Global.BIOME_MAP[x, y, 0] = 21;
    //            if ((byte)BiomeType.TEMPERATE_OCEANIC + Biome.TEMPERATE_OCEANIC_BIOME_COUNT < Global.BIOME_MAP[x, y, 0] && Global.BIOME_MAP[x, y, 0] < (byte)BiomeType.TEMPERATE_INLAND) Global.BIOME_MAP[x, y, 0] = 31;
    //            if ((byte)BiomeType.TEMPERATE_INLAND + Biome.TEMPERATE_INLAND_BIOME_COUNT < Global.BIOME_MAP[x, y, 0] && Global.BIOME_MAP[x, y, 0] < (byte)BiomeType.SUBPOLAR) Global.BIOME_MAP[x, y, 0] = 41;
    //            if ((byte)BiomeType.SUBPOLAR + Biome.SUBPOLAR_BIOME_COUNT < Global.BIOME_MAP[x, y, 0] && Global.BIOME_MAP[x, y, 0] < (byte)BiomeType.POLAR) Global.BIOME_MAP[x, y, 0] = 51;
    //            if ((byte)BiomeType.POLAR + Biome.POLAR_BIOME_COUNT < Global.BIOME_MAP[x, y, 0] && Global.BIOME_MAP[x, y, 0] < (byte)BiomeType.OCEAN) Global.BIOME_MAP[x, y, 0] = 61;
    //            if ((byte)BiomeType.OCEAN < Global.BIOME_MAP[x, y, 0]) Global.BIOME_MAP[x, y, 0] = 62;
    //        }
    //    }
    //}

    public static void GenerateBiomeBlending() {

        for (ushort x = 0; x < Settings.WORLD_SIZE; x++) {
            for (ushort y = 0; y < Settings.WORLD_SIZE; y++) {
                Global.BIOME_MAP[x, y, 1] = 0;
                Global.BIOME_MAP[x, y, 2] = (byte)BiomeType.UNKNOWN;
                Global.BIOME_MAP[x, y, 3] = (byte)BiomeType.UNKNOWN;
            }
        }

        Queue<(int, int)> queue = new();

        // make the borders of the biomes blend 50 : 50 and save what is the secondary biome
        // (hranit detection algorithm)
        for (int x = 0; x < Settings.WORLD_SIZE; x++) {
            for (int y = 0; y < Settings.WORLD_SIZE; y++) {

                if (x < Settings.WORLD_SIZE - 1 && Global.BIOME_MAP[x, y, 0] != Global.BIOME_MAP[x + 1, y, 0]) {
                    Global.BIOME_MAP[x, y, 1] = Settings.BIOME_BLENDING;
                    Global.BIOME_MAP[x, y, 2] = Global.BIOME_MAP[x + 1, y, 0];
                    queue.Enqueue((x, y));
                }

                if (x > 0 && Global.BIOME_MAP[x, y, 0] != Global.BIOME_MAP[x - 1, y, 0]) {
                    Global.BIOME_MAP[x, y, 1] = Settings.BIOME_BLENDING;
                    Global.BIOME_MAP[x, y, 2] = Global.BIOME_MAP[x - 1, y, 0];
                    queue.Enqueue((x, y));
                }

                if (y < Settings.WORLD_SIZE - 1 && Global.BIOME_MAP[x, y, 0] != Global.BIOME_MAP[x, y + 1, 0]) {
                    Global.BIOME_MAP[x, y, 1] = Settings.BIOME_BLENDING;
                    Global.BIOME_MAP[x, y, 2] = Global.BIOME_MAP[x, y + 1, 0];
                    queue.Enqueue((x, y));
                }

                if (y > 0 && Global.BIOME_MAP[x, y, 0] != Global.BIOME_MAP[x, y - 1, 0]) {
                    Global.BIOME_MAP[x, y, 1] = Settings.BIOME_BLENDING;
                    Global.BIOME_MAP[x, y, 2] = Global.BIOME_MAP[x, y - 1, 0];
                    queue.Enqueue((x, y));
                }
            }
        }

        // BFS algorithm
        while (queue.Count > 0) {
            var current = queue.Dequeue();

            for (byte i = 0; i < 8; i++) {
                short x = (short)current.Item1;
                short y = (short)current.Item2;
                short nx = (short)(current.Item1 + dx[i]);
                short ny = (short)(current.Item2 + dy[i]);

                if (World.IsInRange(nx, ny) && Global.BIOME_MAP[nx, ny, 1] + 1 < Global.BIOME_MAP[x, y, 1]) {
                    Global.BIOME_MAP[nx, ny, 1] = (byte)(Global.BIOME_MAP[x, y, 1] - 1);
                    if (Global.BIOME_MAP[nx, ny, 1] > 1) queue.Enqueue((nx, ny));
                }

                if (World.IsInRange(nx, ny)) {
                    // inherit the secondary biome if it is not the same as primary or tertiary
                    if (Global.BIOME_MAP[x, y, 0] != Global.BIOME_MAP[nx, ny, 2] &&
                        Global.BIOME_MAP[x, y, 2] != Global.BIOME_MAP[nx, ny, 2] &&
                        Global.BIOME_MAP[x, y, 3] != Global.BIOME_MAP[nx, ny, 2]) {
                        if (Global.BIOME_MAP[x, y, 2] == (byte)BiomeType.UNKNOWN) {
                            Global.BIOME_MAP[x, y, 2] = Global.BIOME_MAP[nx, ny, 2];
                        }
                        else if (Global.BIOME_MAP[x, y, 3] == (byte)BiomeType.UNKNOWN) {
                            Global.BIOME_MAP[x, y, 3] = Global.BIOME_MAP[nx, ny, 2];
                        }
                    }

                    // inherit the terciary biome if it is not the same as primary or secondary
                    if (Global.BIOME_MAP[x, y, 0] != Global.BIOME_MAP[nx, ny, 3] &&
                        Global.BIOME_MAP[x, y, 2] != Global.BIOME_MAP[nx, ny, 3] &&
                        Global.BIOME_MAP[x, y, 3] != Global.BIOME_MAP[nx, ny, 3]) {
                        if (Global.BIOME_MAP[x, y, 3] == (byte)BiomeType.UNKNOWN) {
                            Global.BIOME_MAP[x, y, 3] = Global.BIOME_MAP[nx, ny, 3];
                        }
                    }
                }
            }
        }
    }
}