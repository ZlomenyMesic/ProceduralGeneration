using minecraft_kurwa.src.generator.terrain.noise;
using minecraft_kurwa.src.global;
using minecraft_kurwa.src.renderer.voxels;
using System;

namespace minecraft_kurwa.src.generator.feature.water;

internal static class Rivers {
    internal static void GenerateMap() {
        var noise = new SimplexNoise(Settings.SEED);

        byte[,] map = new byte[Settings.WORLD_SIZE, Settings.WORLD_SIZE];
        bool[,] nothing = new bool[Settings.WORLD_SIZE, Settings.WORLD_SIZE];

        for (int x = 0; x < Settings.WORLD_SIZE; x++) {
            for (int y = 0; y < Settings.WORLD_SIZE; y++) {
                map[x, y] = (byte)Math.Round(noise.Calculate(x, y, Settings.BIOME_SCALE / 3, 1), 0);
            }
        }

        for (int x = 0; x < Settings.WORLD_SIZE; x++) {
            for (int y = 0; y < Settings.WORLD_SIZE; y++) {
                // hranit detection algorithm

                bool hranit = false;

                if (x < Settings.WORLD_SIZE - 1 && map[x, y] != map[x + 1, y]) hranit = true;
                if (x > 0 && map[x, y] != map[x - 1, y]) hranit = true;
                if (y < Settings.WORLD_SIZE - 1 && map[x, y] != map[x, y + 1]) hranit = true;
                if (y > 0 && map[x, y] != map[x, y - 1]) hranit = true;

                nothing[x, y] = hranit;
                if (hranit) Global.VOXEL_MAP[x, y, 150] = (byte)VoxelType.WATER;
            }
        }
    }
}
