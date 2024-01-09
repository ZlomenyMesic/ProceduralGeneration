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
using minecraft_kurwa.src.renderer.voxels;

namespace minecraft_kurwa.src.generator;

internal class WorldGenerator {
    internal static void GenerateWorld() {
        
        if (Settings.FLAT_WORLD) {
            for (int x = 0; x < Settings.WORLD_SIZE; x++) {
                for (int y = 0; y < Settings.WORLD_SIZE; y++) {
                    Global.VOXEL_MAP[x, y, 5] = (byte)VoxelType.GRASS;
                }
            }

            return;
        } 
        
        
        BiomeGenerator.GenerateBiomeMap();
        BiomeGenerator.GenerateBiomeBlending();

        WaterGenerator.GenerateRivers();

        TerrainGenerator.GenerateHeightMap();
        Erosion.GenerateErosion();


        // top block placement
        for (ushort x = 0; x < Settings.WORLD_SIZE; x++) {
            for (ushort y = 0; y < Settings.WORLD_SIZE; y++) {
                Global.VOXEL_MAP[x, y, Global.HEIGHT_MAP[x, y]] = Biome.GetTopBlock(Global.BIOME_MAP[x, y, 0]);
            }
        }

        WaterGenerator.GenerateOtherWaterThanRivers();
        TerrainFinalization.FillGaps();

        TreeGenerator.Generate();
        ShrubGenerator.Generate();

        Creeks.generateCreeks();
        
        TerrainFinalization.ShiftWorld();
    }
}
