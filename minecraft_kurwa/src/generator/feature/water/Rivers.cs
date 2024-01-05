//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

using minecraft_kurwa.src.generator.terrain.noise;
using minecraft_kurwa.src.global;
using minecraft_kurwa.src.global.geometry;
using System;

namespace minecraft_kurwa.src.generator.feature.water;

internal static class Rivers {
    // Me when a girl asks me for my phone number: (°_°)
    // (I can't talk I'm mewing)
    // (I brutally mog her)
    // (I have a positive canthal tilt, A10 hunter eyes and a sharp jawline)
    // (I don't)
    // (I'm delusional)
    // (It's over for me)
    // (I should try ropemaxxing)

    private const float RIVER_RARITY = 1.5f;
    private const int MAX_RIVER_WIDTH = 20; // only for the generation part, the actual rivers can be much wider
    private const int WATER_EROSION_DISTANCE = 130;

    internal static void GenerateMap() {
        var noise = new SimplexNoise(Settings.SEED + 1);

        byte[,] map = new byte[Settings.WORLD_SIZE, Settings.WORLD_SIZE];
        bool[,] edges = new bool[Settings.WORLD_SIZE, Settings.WORLD_SIZE];
        float[,] distances = new float[Settings.WORLD_SIZE, Settings.WORLD_SIZE];

        for (int x = 0; x < Settings.WORLD_SIZE; x++) {
            for (int y = 0; y < Settings.WORLD_SIZE; y++) {
                int noiseValue = (int)Math.Round(noise.Calculate(x + ExperimentalSettings.NOISE_OFFSET, y + ExperimentalSettings.NOISE_OFFSET, Settings.BIOME_SCALE, RIVER_RARITY), 0);

                map[x, y] = (byte)noiseValue;

                distances[x, y] = WATER_EROSION_DISTANCE;
            }
        }

        // edging algorithm
        // (I've been already edging for 36 consecutive days)
        // (I can't break my streak now)
        for (int x = 0; x < Settings.WORLD_SIZE; x++) {
            for (int y = 0; y < Settings.WORLD_SIZE; y++) {
                if (map[x, y] != 0) continue;

                int width = Global.HEIGHT_MAP[x, y] != 0 
                    ? Math.Min((250 / Global.HEIGHT_MAP[x, y]) + 1, MAX_RIVER_WIDTH) : MAX_RIVER_WIDTH;

                edges[x, y] = (x + width < Settings.WORLD_SIZE && map[x + width, y] == 1) || (x - width >= 0 && map[x - width, y] == 1) || (y + width < Settings.WORLD_SIZE && map[x, y + width] == 1) || (y - width >= 0 && map[x, y - width] == 1);
            }
        }

        for (int x = 0; x < Settings.WORLD_SIZE; x++) {
            for (int y = 0; y < Settings.WORLD_SIZE; y++) {
                if (!edges[x, y]) continue;

                distances[x, y] = 0;

                if (Global.RANDOM.Next(0, 2) == 0 && ((x + 1 < Settings.WORLD_SIZE && !edges[x + 1, y]) || (x - 1 >= 0 && !edges[x - 1, y]) || (y + 1 < Settings.WORLD_SIZE && !edges[x, y + 1]) || (y - 1 >= 0 && !edges[x, y - 1]))) {
                    for (int x2 = Math.Max(0, x - WATER_EROSION_DISTANCE); x2 <= Math.Min(Settings.WORLD_SIZE - 1, x + WATER_EROSION_DISTANCE); x2++) {
                        for (int y2 = Math.Max(0, y - WATER_EROSION_DISTANCE); y2 <= Math.Min(Settings.WORLD_SIZE - 1, y + WATER_EROSION_DISTANCE); y2++) {
                            int distX = x - x2;
                            int distY = y - y2;

                            float dist = (float)Math.Sqrt(distX * distX + distY * distY);

                            distances[x2, y2] = Math.Min(distances[x2, y2], dist);
                        }
                    }
                }
            }
        }

        // I totally mog you (I'm looksmaxxing)
        // (Its over for you bro)
        // (I'm going insane)
        for (int x = 0; x < Settings.WORLD_SIZE; x++) {
            for (int y = 0; y < Settings.WORLD_SIZE; y++) {
                Global.HEIGHT_MAP[x, y] = (ushort)(Global.HEIGHT_MAP[x, y] * Math.Round(Geometry.Sigmoid(distances[x, y] / 40f), 3));
            }
        }
    }
}
