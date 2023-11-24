//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

namespace minecraft_kurwa {
    internal class BiomeGenerator {

        internal static byte[,] GenerateBiomeMap() {
            byte[,] biomeType = new byte[Settings.WORLD_SIZE, Settings.WORLD_SIZE];
            byte[,] subbiomeType = new byte[Settings.WORLD_SIZE, Settings.WORLD_SIZE];

            PerlinNoise p = new(Settings.SEED);

            // main biome type
            for (ushort x = 0; x < Settings.WORLD_SIZE; x++) {
                for (ushort y = 0; y < Settings.WORLD_SIZE; y++) {
                    biomeType[x, y] = (byte) ((int) (p.Noise((double) x / Settings.BIOME_SCALE, (double) y / Settings.BIOME_SCALE) * 10) * -10);
                }
            }

            // prevention of the noises being same
            p.seed += 1;

            // subbiome type
            for (ushort x = 0; x < Settings.WORLD_SIZE; x++) {
                for (ushort y = 0; y < Settings.WORLD_SIZE; y++) {
                    subbiomeType[x, y] = (byte)((int)(p.Noise((double)x / Settings.SUBBIOME_SCALE, (double)y / Settings.SUBBIOME_SCALE) * 5) + 3);
                }
            }

            // merging
            for (ushort x = 0; x < Settings.WORLD_SIZE; x++) {
                for (ushort y = 0; y < Settings.WORLD_SIZE; y++) {
                    biomeType[x, y] = (byte)(biomeType[x, y] + subbiomeType[x, y]);
                }
            }

            // prevention of setting of a not existing biome
            for (int x = 0; x < Settings.WORLD_SIZE; x++) {
                for (int y = 0; y < Settings.WORLD_SIZE; y++) {
                    if ((byte) BiomeType.TROPICAL_DRY      + Biome.TROPICAL_DRY_BIOME_COUNT      < biomeType[x, y] && biomeType[x, y] < (byte) BiomeType.TROPICAL_RAINY)    biomeType[x, y] = 1;
                    if ((byte) BiomeType.TROPICAL_RAINY    + Biome.TROPICAL_RAINY_BIOME_COUNT    < biomeType[x, y] && biomeType[x, y] < (byte) BiomeType.SUBTROPICAL)       biomeType[x, y] = 11;
                    if ((byte) BiomeType.SUBTROPICAL       + Biome.SUBTROPICAL_BIOME_COUNT       < biomeType[x, y] && biomeType[x, y] < (byte) BiomeType.TEMPERATE_OCEANIC) biomeType[x, y] = 21;
                    if ((byte) BiomeType.TEMPERATE_OCEANIC + Biome.TEMPERATE_OCEANIC_BIOME_COUNT < biomeType[x, y] && biomeType[x, y] < (byte) BiomeType.TEMPERATE_INLAND)  biomeType[x, y] = 31;
                    if ((byte) BiomeType.TEMPERATE_INLAND  + Biome.TEMPERATE_INLAND_BIOME_COUNT  < biomeType[x, y] && biomeType[x, y] < (byte) BiomeType.SUBPOLAR)          biomeType[x, y] = 41;
                    if ((byte) BiomeType.SUBPOLAR          + Biome.SUBPOLAR_BIOME_COUNT          < biomeType[x, y] && biomeType[x, y] < (byte) BiomeType.POLAR)             biomeType[x, y] = 51;
                    if ((byte) BiomeType.POLAR             + Biome.POLAR_BIOME_COUNT             < biomeType[x, y] )                                                        biomeType[x, y] = 61;
                }
            }

            return biomeType;
        }
    }
}
