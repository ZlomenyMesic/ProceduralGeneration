//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

using System;

namespace minecraft_kurwa {
    internal class TerrainGenerator {

        internal static short[,] GenerateHeightMap() {
            PerlinNoise perlinNoise = new(Global.SEED);

            short[,] output = new short[Global.WORLD_SIZE, Global.WORLD_SIZE];

            for (ushort x = 0; x < Global.WORLD_SIZE; x++) {
                for (ushort y = 0; y < Global.WORLD_SIZE; y++) {
                    output[x, y] = (short)Math.Abs(Math.Round(perlinNoise.Noise((double)x / Global.MAIN_NOISE_SCALE, (double)y / Global.MAIN_NOISE_SCALE) * Global.MAIN_NOISE_SHARPNESS * 5 / 2 + Global.MAIN_NOISE_SHARPNESS * 3 / 2));
                }
            }

            return output;
        }

        internal static void ShiftWorld() {
            for (ushort x = 0; x < Global.WORLD_SIZE; x++) {
                for (ushort y = 0; y < Global.WORLD_SIZE; y++) {
                    for (byte z = 0; z < Global.WATER_LEVEL; z++) {
                        Global.VOXEL_MAP[x, y, z] = null;
                    }

                    for (ushort z = Global.WATER_LEVEL; z < Global.HEIGHT_LIMIT; z++) {
                        Global.VOXEL_MAP[x, y, z - Global.WATER_LEVEL] = Global.VOXEL_MAP[x, y, z];
                        Global.VOXEL_MAP[x, y, z] = null;
                    }

                    Global.HEIGHT_MAP[x, y] -= Global.WATER_LEVEL;
                }
            }
        }
    }
}
