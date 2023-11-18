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
                    output[x, y] = (short)Math.Abs(Math.Round(perlinNoise.Noise((double)x / Global.MAIN_NOISE_SCALE, (double)y / Global.MAIN_NOISE_SCALE) * Global.MAIN_NOISE_SHARPNESS * 5 / 2 + Global.MAIN_NOISE_SHARPNESS * 3 / 2));
                }
            }

            return output;
        }
    }
}
