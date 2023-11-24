//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

using System;

namespace minecraft_kurwa {
    internal class TerrainGenerator {

        internal static short[,] GenerateHeightMap() {
            PerlinNoise perlinNoise = new(Settings.SEED);

            short[,] output = new short[Settings.WORLD_SIZE, Settings.WORLD_SIZE];

            for (ushort x = 0; x < Settings.WORLD_SIZE; x++) {
                for (ushort y = 0; y < Settings.WORLD_SIZE; y++) {
                    output[x, y] = (short)Math.Abs(Math.Round(perlinNoise.Noise((double)x / Settings.MAIN_NOISE_SCALE, (double)y / Settings.MAIN_NOISE_SCALE) * Settings.MAIN_NOISE_SHARPNESS * 5 / 2 + Settings.MAIN_NOISE_SHARPNESS * 3 / 2));
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
