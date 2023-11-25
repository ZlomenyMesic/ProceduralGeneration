//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

using System;

namespace minecraft_kurwa {
    internal class TerrainGenerator {

        internal static ushort[,] GenerateHeightMap() {
            PerlinNoise perlinNoise = new(Settings.SEED);
            Random random = new Random(Settings.SEED);

            ushort[,] output = new ushort[Settings.WORLD_SIZE, Settings.WORLD_SIZE];

            // main generation handling
            for (ushort x = 0; x < Settings.WORLD_SIZE; x++) {
                for (ushort y = 0; y < Settings.WORLD_SIZE; y++) {

                    // primary height
                    ushort pHeight = (ushort) Math.Abs(perlinNoise.Noise((double) x / Settings.MAIN_NOISE_SCALE, (double) y / Settings.MAIN_NOISE_SCALE) * Settings.MAIN_NOISE_SHARPNESS * 5/2 + Settings.MAIN_NOISE_SHARPNESS * 3/2);
                    (string operation, float numerator)[] pOperations = Biome.GetTerrainGeneratorValues((BiomeType) Global.BIOME_MAP[x, y, 0]);

                    for (byte i = 0; i < pOperations.Length; i++) {
                        if (pOperations[i].operation == "*") {
                            pHeight = (ushort) (pHeight * pOperations[i].numerator);
                        }

                        if (pOperations[i].operation == "+") {
                            pHeight = (ushort) (pHeight + pOperations[i].numerator);
                        }
                    }
                    
                    output[x, y] = pHeight;
                }
            }
            
            // TODO collapse direction check
            // terrain collapse
            for (ushort i = 0; i < Settings.HEIGHT_LIMIT / Settings.TERRAIN_COLLAPSE_LIMIT; i++) {
                for (ushort x = 0; x < Settings.WORLD_SIZE; x++) {
                    for (ushort y = 0; y < Settings.WORLD_SIZE; y++) {
                        if (x < Settings.WORLD_SIZE - 1) { 
                            if (output[x, y] - output[x + 1, y] >  Settings.TERRAIN_COLLAPSE_LIMIT) output[x + 1, y] = (ushort) (output[x, y] - Settings.TERRAIN_COLLAPSE_LIMIT - random.Next(-Settings.TERRAIN_COLLAPSE_LIMIT, Settings.TERRAIN_COLLAPSE_LIMIT));
                            if (output[x, y] - output[x + 1, y] < -Settings.TERRAIN_COLLAPSE_LIMIT) output[x + 1, y] = (ushort) (output[x, y] + Settings.TERRAIN_COLLAPSE_LIMIT - random.Next(-Settings.TERRAIN_COLLAPSE_LIMIT, Settings.TERRAIN_COLLAPSE_LIMIT));
                        }

                        if (x > 0) {
                            if (output[x, y] - output[x - 1, y] >  Settings.TERRAIN_COLLAPSE_LIMIT) output[x - 1, y] = (ushort) (output[x, y] - Settings.TERRAIN_COLLAPSE_LIMIT - random.Next(-Settings.TERRAIN_COLLAPSE_LIMIT, Settings.TERRAIN_COLLAPSE_LIMIT));
                            if (output[x, y] - output[x - 1, y] < -Settings.TERRAIN_COLLAPSE_LIMIT) output[x - 1, y] = (ushort) (output[x, y] + Settings.TERRAIN_COLLAPSE_LIMIT - random.Next(-Settings.TERRAIN_COLLAPSE_LIMIT, Settings.TERRAIN_COLLAPSE_LIMIT));
                        }

                        if (y < Settings.WORLD_SIZE - 1) {
                            if (output[x, y] - output[x, y + 1] >  Settings.TERRAIN_COLLAPSE_LIMIT) output[x, y + 1] = (ushort) (output[x, y] - Settings.TERRAIN_COLLAPSE_LIMIT - random.Next(-Settings.TERRAIN_COLLAPSE_LIMIT, Settings.TERRAIN_COLLAPSE_LIMIT));
                            if (output[x, y] - output[x, y + 1] < -Settings.TERRAIN_COLLAPSE_LIMIT) output[x, y + 1] = (ushort) (output[x, y] + Settings.TERRAIN_COLLAPSE_LIMIT - random.Next(-Settings.TERRAIN_COLLAPSE_LIMIT, Settings.TERRAIN_COLLAPSE_LIMIT));
                        }

                        if (y > 0) {
                            if (output[x, y] - output[x, y - 1] >  Settings.TERRAIN_COLLAPSE_LIMIT) output[x, y - 1] = (ushort) (output[x, y] - Settings.TERRAIN_COLLAPSE_LIMIT - random.Next(-Settings.TERRAIN_COLLAPSE_LIMIT, Settings.TERRAIN_COLLAPSE_LIMIT));
                            if (output[x, y] - output[x, y - 1] < -Settings.TERRAIN_COLLAPSE_LIMIT) output[x, y - 1] = (ushort) (output[x, y] + Settings.TERRAIN_COLLAPSE_LIMIT - random.Next(-Settings.TERRAIN_COLLAPSE_LIMIT, Settings.TERRAIN_COLLAPSE_LIMIT));
                        }
                    }
                }
            }

            return output;
        }

        internal static void ShiftWorld() {
            for (ushort x = 0; x < Settings.WORLD_SIZE; x++) {
                for (ushort y = 0; y < Settings.WORLD_SIZE; y++) {
                    for (byte z = 0; z < Settings.WATER_LEVEL; z++) {
                        Global.VOXEL_MAP[x, y, z] = null;
                    }

                    for (ushort z = Settings.WATER_LEVEL; z < Settings.HEIGHT_LIMIT; z++) {
                        Global.VOXEL_MAP[x, y, z - Settings.WATER_LEVEL] = Global.VOXEL_MAP[x, y, z];
                        Global.VOXEL_MAP[x, y, z] = null;
                    }

                    Global.HEIGHT_MAP[x, y] -= Settings.WATER_LEVEL;
                }
            }
        }
    }
}
