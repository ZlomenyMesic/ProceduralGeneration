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
using minecraft_kurwa.src.gui;
using minecraft_kurwa.src.renderer.voxels;

namespace minecraft_kurwa.src.generator;

internal class WorldGenerator {
    internal static void GenerateWorld() {
        Time.StartProfiler();
        BiomeGenerator.GenerateBiomeMap();
        Time.StopProfiler("Biome map");

        Time.StartProfiler();
        BiomeGenerator.GenerateBiomeBlending();
        Time.StopProfiler("Biome blending");

        Time.StartProfiler();
        TerrainGenerator.GenerateHeightMap();
        Time.StopProfiler("Height map");

        Time.StartProfiler();
        WaterGenerator.GenerateRivers();
        Time.StopProfiler("Rivers");

        Time.StartProfiler();
        Erosion.GenerateErosion();
        Time.StopProfiler("Erosion");

        Time.StartProfiler();
        Creeks.generateCreeks();
        Time.StopProfiler("Creeks");

        for (ushort x = 0; x < Settings.WORLD_SIZE; x++) {
            for (ushort y = 0; y < Settings.WORLD_SIZE; y++) {
                // top block placement
                Global.VOXEL_MAP[x, y, Global.HEIGHT_MAP[x, y]] = Biome.GetTopBlock(Global.BIOME_MAP[x, y, 0]);

                for (ushort z = 0; z < Settings.WATER_LEVEL; z++) {
                    Global.VOXEL_MAP[x, y, z] = null;
                }

                // freeze water
                for (ushort z = 0; z < Ponds.highestWaterLevel; z++) {
                    if (Global.VOXEL_MAP[x, y, z] == (byte)VoxelType.WATER) {
                        Global.VOXEL_MAP[x, y, z] = WaterGenerator.ShouldFreeze(x, y, (ushort)Global.RANDOM.Next(Settings.FREEZING_DISTANCE, Settings.MAX_FREEZING_DISTANCE))
                            ? Global.RANDOM.Next(0, 100) > Settings.ICE_HOLES - 1
                                ? (byte)VoxelType.ICE
                                : (byte)VoxelType.WATER
                            : (byte)VoxelType.WATER;
                    }
                }
            }
        }

        Time.StartProfiler();
        WaterGenerator.GenerateOtherWaterThanRivers();
        Time.StopProfiler("Other water");

        TerrainFinalization.FillGaps();

        Time.StartProfiler();
        TreeGenerator.Generate();
        ShrubGenerator.Generate();
        Time.StopProfiler("Trees & Shrubs");
    }
}