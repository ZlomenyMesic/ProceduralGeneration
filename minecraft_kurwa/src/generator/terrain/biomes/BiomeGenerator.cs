//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

using minecraft_kurwa.src.global;
using minecraft_kurwa.src.generator.terrain.noise;
using System;

namespace minecraft_kurwa.src.generator.terrain.biomes
{
    internal class BiomeGenerator {
        internal static void GenerateBiomeMap() {
            SimplexNoise s = new(Settings.SEED);

            for (int x = 0; x < Settings.WORLD_SIZE; x++) {
                for (int y = 0; y < Settings.WORLD_SIZE; y++) {
                    byte temperature = (byte)Math.Round(s.Calculate(x, y, Settings.BIOME_SCALE, 2), 0);
                    byte humidity = (byte)Math.Round(s.Calculate(x + 5000, y + 5000, Settings.BIOME_SCALE, 2), 0);

                    byte biome = (byte)BiomeType.UNKNOWN;

                    if (humidity == 0 && temperature == 0) biome = (byte)BiomeType.POLAR;
                    else if (humidity >= 1 && temperature == 0) biome = (byte)BiomeType.SUBPOLAR;
                    else if (humidity == 0 && temperature == 1) biome = (byte)BiomeType.TEMPERATE_INLAND;
                    else if (humidity >= 1 && temperature == 1) biome = (byte)BiomeType.TEMPERATE_OCEANIC;
                    else if (humidity == 0 && temperature == 2) biome = (byte)BiomeType.TROPICAL_DRY;
                    else if (humidity == 1 && temperature == 2) biome = (byte)BiomeType.SUBTROPICAL;
                    else if (humidity == 2 && temperature == 2) biome = (byte)BiomeType.TROPICAL_RAINY;

                    byte subbiome = (byte)Math.Round(s.Calculate(x + 10000, y + 10000, Settings.SUBBIOME_SCALE, Biome.GetSubbiomeCount((BiomeType)biome)), 0);
                    biome += subbiome;

                    Global.BIOME_MAP[x, y, 0] = biome;
                }
            }
        }


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
                    Global.BIOME_MAP[x, y, 2] = (byte)BiomeType.UNKNOWN;
                    Global.BIOME_MAP[x, y, 3] = (byte)BiomeType.UNKNOWN;
                }
            }

            // make the borders of the biomes blend 50 : 50 and save what is the secondary biome
            for (int x = 0; x < Settings.WORLD_SIZE; x++) {
                for (int y = 0; y < Settings.WORLD_SIZE; y++) {
                    if (x < Settings.WORLD_SIZE - 1) if (Global.BIOME_MAP[x, y, 0] / 10 != Global.BIOME_MAP[x + 1, y, 0] / 10) {
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
                                    if (Global.BIOME_MAP[x, y + 1, 2] == 0) Global.BIOME_MAP[x, y + 1, 2] = Global.BIOME_MAP[x, y, 2]; else Global.BIOME_MAP[x, y + 1, 3] = Global.BIOME_MAP[x, y, 2];
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
}