//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

using minecraft_kurwa.src.global;
using minecraft_kurwa.src.generator.terrain.biomes;
using minecraft_kurwa.src.generator.terrain;
using minecraft_kurwa.src.generator.feature.tree;
using minecraft_kurwa.src.generator.feature.water;
using minecraft_kurwa.src.generator.feature.shrub;

namespace minecraft_kurwa.src.generator;

internal class WorldGenerator {
    internal static void GenerateWorld() {
        BiomeGenerator.GenerateBiomeMap();
        BiomeGenerator.GenerateBiomeBlending();

        TerrainGenerator.GenerateHeightMap();

        WaterGenerator.GenerateRivers();

        Erosion.GenerateErosion();

        for (ushort x = 0; x < Settings.WORLD_SIZE; x++) {
            for (ushort y = 0; y < Settings.WORLD_SIZE; y++) {
                // top block placement
                Global.VOXEL_MAP[x, y, Global.HEIGHT_MAP[x, y]] = Biome.GetTopBlock(Global.BIOME_MAP[x, y, 0]);

                for (ushort z = 0; z < Settings.WATER_LEVEL; z++) {
                    Global.VOXEL_MAP[x, y, z] = null;
                }
            }
        }

        WaterGenerator.GenerateOtherWaterThanRivers();
        Creeks.GenerateCreeks();

        TerrainFinalization.FillGaps();

        TreeGenerator.Generate();
        ShrubGenerator.Generate();
    }
}
