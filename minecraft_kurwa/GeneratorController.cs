//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

using System;
using System.Drawing;

namespace minecraft_kurwa {
    internal class GeneratorController {
        internal static void GenerateWorld() {
            Global.BIOME_MAP = BiomeGenerator.GenerateBiomeMap();
            BiomeGenerator.GenerateBiomeBlending();

            Global.HEIGHT_MAP = TerrainGenerator.GenerateHeightMap();
            
            Random random = new Random(Settings.SEED);

            for (ushort x = 0; x < Settings.WORLD_SIZE; x++) {
                for (ushort y = 0; y < Settings.WORLD_SIZE; y++) {
                    
                    // top block placement
                    byte[] topBlocks = Biome.GetTopBlocks(Global.BIOME_MAP[x, y, 0]);
                    Global.VOXEL_MAP[x, y, Global.HEIGHT_MAP[x, y]] = topBlocks[random.Next(0, topBlocks.Length)];

                    // filling in the gaps
                    if (x > 0 && Global.HEIGHT_MAP[x - 1, y] + 1 < Global.HEIGHT_MAP[x, y]) for (ushort z = (ushort)(1 + Global.HEIGHT_MAP[x - 1, y]); z < Global.HEIGHT_MAP[x, y]; z++) Global.VOXEL_MAP[x, y, z] = (byte) VoxelType.STONE;
                    if (y > 0 && Global.HEIGHT_MAP[x, y - 1] + 1 < Global.HEIGHT_MAP[x, y]) for (ushort z = (ushort)(1 + Global.HEIGHT_MAP[x, y - 1]); z < Global.HEIGHT_MAP[x, y]; z++) Global.VOXEL_MAP[x, y, z] = (byte) VoxelType.STONE;
                    if (x < Settings.WORLD_SIZE - 1 && Global.HEIGHT_MAP[x + 1, y] + 1 < Global.HEIGHT_MAP[x, y]) for (ushort z = (ushort)(1 + Global.HEIGHT_MAP[x + 1, y]); z < Global.HEIGHT_MAP[x, y]; z++) Global.VOXEL_MAP[x, y, z] = (byte) VoxelType.STONE;
                    if (y < Settings.WORLD_SIZE - 1 && Global.HEIGHT_MAP[x, y + 1] + 1 < Global.HEIGHT_MAP[x, y]) for (ushort z = (ushort)(1 + Global.HEIGHT_MAP[x, y + 1]); z < Global.HEIGHT_MAP[x, y]; z++) Global.VOXEL_MAP[x, y, z] = (byte) VoxelType.STONE;
                }
            }

            Water.Generate();
            Tree.GenerateTrees();
            TerrainGenerator.ShiftWorld();
        }
    }
}
