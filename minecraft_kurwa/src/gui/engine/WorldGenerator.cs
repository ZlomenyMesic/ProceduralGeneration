//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

using System;
using minecraft_kurwa.src.biomes;
using minecraft_kurwa.src.global;
using minecraft_kurwa.src.terrain;
using minecraft_kurwa.src.trees;
using minecraft_kurwa.src.water;

namespace minecraft_kurwa.src.engine {
    internal class WorldGenerator {
        internal static void GenerateWorld() {
            BiomeGenerator.GenerateBiomeMap();
            BiomeGenerator.GenerateBiomeBlending();

            TerrainGenerator.GenerateHeightMap();

            Random random = new(Settings.SEED);

            for (ushort x = 0; x < Settings.WORLD_SIZE; x++) {
                for (ushort y = 0; y < Settings.WORLD_SIZE; y++) {
                    // top block placement
                    byte[] topBlocks = Biome.GetTopBlocks(Global.BIOME_MAP[x, y, 0]);
                    Global.VOXEL_MAP[x, y, Global.HEIGHT_MAP[x, y]] = topBlocks[random.Next(0, topBlocks.Length)];
                }
            }

            WaterGenerator.Generate();
            TerrainFinalization.FillGaps();

            TreeGenerator.Generate();
            TerrainFinalization.ShiftWorld();
        }
    }
}
