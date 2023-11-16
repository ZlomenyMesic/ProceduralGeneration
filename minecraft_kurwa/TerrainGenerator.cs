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

            for (int x = 0; x < Global.WORLD_SIZE; x++) {
                for (int y = 0; y < Global.WORLD_SIZE; y++) {
                    output[x, y] = (short)Math.Round(perlinNoise.Noise((double)x / Global.MAIN_NOISE_SCALE, (double)y / Global.MAIN_NOISE_SCALE) * Global.MAIN_NOISE_SHARPNESS * 5 / 2 + Global.MAIN_NOISE_SHARPNESS * 3 / 2);
                }
            }

            perlinNoise.seed += 1;

            short[,] side = new short[Global.WORLD_SIZE, Global.WORLD_SIZE];

            for (int x = 0; x < Global.WORLD_SIZE; x++) {
                for (int y = 0; y < Global.WORLD_SIZE; y++) {
                    side[x, y] = (short)Math.Round(perlinNoise.Noise((double)x / Global.SIDE_NOISE_SCALE, (double)y / Global.SIDE_NOISE_SCALE) * Global.SIDE_NOISE_SHARPNESS * 5 / 2 + Global.SIDE_NOISE_SHARPNESS * 3 / 2);
                }
            }

            for (int x = 0; x < Global.WORLD_SIZE; x++) {
                for (int y = 0; y < Global.WORLD_SIZE; y++) {
                    //output[x, y] += side[x, y];
                    output[x, y] = Math.Abs(output[x, y]);
                }
            }

            return output;
        }
    }
}
