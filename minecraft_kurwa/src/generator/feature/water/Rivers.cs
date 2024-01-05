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

    internal static double[,] riverHeightMap;

    internal static void GenerateMap() {
        var noise = new SimplexNoise(Settings.SEED);

        riverHeightMap = new double[Settings.WORLD_SIZE, Settings.WORLD_SIZE];

        byte[,] map = new byte[Settings.WORLD_SIZE, Settings.WORLD_SIZE];
        bool[,] edges = new bool[Settings.WORLD_SIZE, Settings.WORLD_SIZE];
        float[,] distances = new float[Settings.WORLD_SIZE, Settings.WORLD_SIZE];

        for (int x = 0; x < Settings.WORLD_SIZE; x++) {
            for (int y = 0; y < Settings.WORLD_SIZE; y++) {
                int noiseValue = (int)Math.Round(noise.Calculate(x + ExperimentalSettings.NOISE_OFFSET, y + ExperimentalSettings.NOISE_OFFSET, Settings.BIOME_SCALE, 1), 0);

                map[x, y] = (byte)noiseValue;

                distances[x, y] = int.MaxValue;
            }
        }

        /* SLOW AS FUCK:

        int smudge = 8;
        for (int x = 0; x < Settings.WORLD_SIZE; x++) {
            for (int y = 0; y < Settings.WORLD_SIZE; y++) {
                byte blending = (byte)Math.Round(noise.Calculate(x + 5000 + ExperimentalSettings.NOISE_OFFSET, y + 5000 + ExperimentalSettings.NOISE_OFFSET, Settings.BIOME_SCALE / 30, 2), 0);
                if (blending != 0) continue;

                byte local = Global.BIOME_MAP[x, y, 0];
                byte[] neighbours = new byte[9];
                byte nCount = 0;

                for (int x2 = -smudge; x2 <= smudge; x2 += smudge) {
                    if (x + x2 < 0 || x + x2 >= Settings.WORLD_SIZE) continue;

                    for (int y2 = -smudge; y2 <= smudge; y2 += smudge) {
                        if (y + y2 < 0 || y + y2 >= Settings.WORLD_SIZE) continue;

                        neighbours[nCount++] = map[x + x2, y + y2];
                    }
                }

                for (int j = 0; j < nCount; j++) {
                    if (neighbours[j] != local) map[x, y] = neighbours[j];
                }
            }
        }*/

        // edging algorithm
        // (I've been already edging for 36 consecutive days)
        // (I can't break my streak now)
        int width = 10;
        for (int x = 0; x < Settings.WORLD_SIZE; x++) {
            for (int y = 0; y < Settings.WORLD_SIZE; y++) {
                edges[x, y] = (map[x, y] == 0) && ((x + width < Settings.WORLD_SIZE && map[x + width, y] == 1) || (x - width >= 0 && map[x - width, y] == 1) || (y + width < Settings.WORLD_SIZE && map[x, y + width] == 1) || (y - width >= 0 && map[x, y - width] == 1));
            }
        }

        for (int x = 0; x < Settings.WORLD_SIZE; x++) {
            for (int y = 0; y < Settings.WORLD_SIZE; y++) {
                if (!edges[x, y]) continue;

                distances[x, y] = 0;

                if (Global.RANDOM.Next(0, 20) != 0) continue;

                for (int x2 = 0; x2 < Settings.WORLD_SIZE; x2++) {
                    for (int y2 = 0; y2 < Settings.WORLD_SIZE; y2++) {
                        int distX = x - x2;
                        int distY = y - y2;

                        float dist = (float)Math.Sqrt(distX * distX + distY * distY);

                        distances[x2, y2] = Math.Min(distances[x2, y2], dist);
                    }
                }
            }
        }

        // I totally mog you (I'm looksmaxxing)
        // (Its over for you bro)
        // (I'm going insane)
        for (int x = 0; x < Settings.WORLD_SIZE; x++) {
            for (int y = 0; y < Settings.WORLD_SIZE; y++) {
                riverHeightMap[x, y] = Math.Round(Geometry.Sigmoid(distances[x, y] / 50f), 3);
            }
        }
    }
}
