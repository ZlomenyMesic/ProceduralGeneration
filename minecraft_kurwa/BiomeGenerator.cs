//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

namespace minecraft_kurwa {
    internal class BiomeGenerator {

        internal static byte[,] GenerateBiomeMap() {
            byte[,] biomeType = new byte[Global.WORLD_SIZE, Global.WORLD_SIZE];
            byte[,] subbiomeType = new byte[Global.WORLD_SIZE, Global.WORLD_SIZE];

            PerlinNoise p = new(Global.SEED);

            for (int x = 0; x < Global.WORLD_SIZE; x++) {
                for (int y = 0; y < Global.WORLD_SIZE; y++) {
                    biomeType[x, y] = (byte)((int)(p.Noise((double)x / Global.BIOME_SCALE, (double)y / Global.BIOME_SCALE) * 10) * -10);
                }
            }

            p.seed += 1;

            for (int x = 0; x < Global.WORLD_SIZE; x++) {
                for (int y = 0; y < Global.WORLD_SIZE; y++) {
                    subbiomeType[x, y] = (byte)((int)(p.Noise((double)x / Global.SUBBIOME_SCALE, (double)y / Global.SUBBIOME_SCALE) * 5) + 3);
                }
            }

            for (int x = 0; x < Global.WORLD_SIZE; x++) {
                for (int y = 0; y < Global.WORLD_SIZE; y++) {
                    biomeType[x, y] = (byte)(biomeType[x, y] + subbiomeType[x, y]);
                }
            }

            for (int x = 0; x < Global.WORLD_SIZE; x++) {
                for (int y = 0; y < Global.WORLD_SIZE; y++) {
                    if ((int)BiomeType.TROPICAL_DRY + Biome.TROPICAL_DRY_BIOME_COUNT < biomeType[x, y] && biomeType[x, y] < (int)BiomeType.TROPICAL_RAINY) biomeType[x, y] = 1;
                    if ((int)BiomeType.TROPICAL_RAINY + Biome.TROPICAL_RAINY_BIOME_COUNT < biomeType[x, y] && biomeType[x, y] < (int)BiomeType.SUBTROPICAL) biomeType[x, y] = 11;
                    if ((int)BiomeType.SUBTROPICAL + Biome.SUBTROPICAL_BIOME_COUNT < biomeType[x, y] && biomeType[x, y] < (int)BiomeType.TEMPERATE_OCEANIC) biomeType[x, y] = 21;
                    if ((int)BiomeType.TEMPERATE_OCEANIC + Biome.TEMPERATE_OCEANIC_BIOME_COUNT < biomeType[x,   y] && biomeType[x, y] < (int)BiomeType.TEMPERATE_INLAND) biomeType[x, y] = 31;
                    if ((int)BiomeType.TEMPERATE_INLAND + Biome.TEMPERATE_INLAND_BIOME_COUNT < biomeType[x, y] && biomeType[x, y] < (int)BiomeType.COLD) biomeType[x, y] = 41;
                    if ((int)BiomeType.COLD + Biome.COLD_BIOME_COUNT < biomeType[x, y]) biomeType[x, y] = 51;
                }
            }

            return biomeType;
        }
    }
}
