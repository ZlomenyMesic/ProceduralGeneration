//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

using minecraft_kurwa.src.generator.terrain.noise;
using minecraft_kurwa.src.global;
using minecraft_kurwa.src.global.functions;
using System;
using System.Collections.Generic;

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
    private const byte MAX_RIVER_WIDTH = 20; // only for the generation part, the actual rivers can be much wider

    private static readonly short[] dx = { 1, -1, 0, 0, 1, 1, -1, -1 };
    private static readonly short[] dy = { 0, 0, 1, -1, 1, -1, 1, -1 };

    internal static void GenerateMap() {
        SimplexNoise noise = new(Settings.SEED + 1);

        byte[,] map = new byte[Settings.WORLD_SIZE, Settings.WORLD_SIZE];
        bool[,] edges = new bool[Settings.WORLD_SIZE, Settings.WORLD_SIZE];
        short[,] distances = new short[Settings.WORLD_SIZE, Settings.WORLD_SIZE];

        for (ushort x = 0; x < Settings.WORLD_SIZE; x++) {
            for (ushort y = 0; y < Settings.WORLD_SIZE; y++) {
                map[x, y] = (byte)Math.Round(noise.Calculate(x + ExperimentalSettings.NOISE_OFFSET, y + ExperimentalSettings.NOISE_OFFSET, Settings.BIOME_SCALE, RIVER_RARITY), 0);
            }
        }

        // edging algorithm
        // (I've been already edging for 36 consecutive days)
        // (I can't break my streak now)
        for (ushort x = 0; x < Settings.WORLD_SIZE; x++) {
            for (ushort y = 0; y < Settings.WORLD_SIZE; y++) {
                if (map[x, y] != 0) continue;

                ushort width = (ushort)(Global.HEIGHT_MAP[x, y] != 0 
                    ? Math.Min((250 / Global.HEIGHT_MAP[x, y]) + 1, MAX_RIVER_WIDTH) : MAX_RIVER_WIDTH);

                edges[x, y] = (World.IsInRange(x + width) && map[x + width, y] == 1) || (World.IsInRange(x - width) && map[x - width, y] == 1) || (World.IsInRange(y + width) && map[x, y + width] == 1) || (World.IsInRange(y - width) && map[x, y - width] == 1);
            }
        }

        Queue<(short, short)> queue = new();

        for (short x = 0; x < Settings.WORLD_SIZE; x++) {
            for (short y = 0; y < Settings.WORLD_SIZE; y++) {
                if (edges[x, y]) {
                    distances[x, y] = 0;

                    if ((World.IsInRange(x + 1) && !edges[x + 1, y]) || (World.IsInRange(x - 1) && !edges[x - 1, y]) || (World.IsInRange(y + 1) && !edges[x, y + 1]) || (World.IsInRange(y - 1) && !edges[x, y - 1])) {
                        queue.Enqueue((x, y));
                    }
                } 
                else distances[x, y] = short.MaxValue;
            }
        }

        // BFS algorithm
        while (queue.Count > 0) {
            (short, short) current = queue.Dequeue();

            for (byte i = 0; i < 8; i++) {
                short nx = (short)(current.Item1 + dx[i]);
                short ny = (short)(current.Item2 + dy[i]);

                if (World.IsInRange(nx, ny) && distances[nx, ny] > distances[current.Item1, current.Item2] + 1) {
                    distances[nx, ny] = (short)(distances[current.Item1, current.Item2] + 1);
                    queue.Enqueue((nx, ny));
                }
            }
        }

        // I totally mog you (I'm looksmaxxing)
        // (Its over for you)
        // (I'm going insane)
        for (ushort x = 0; x < Settings.WORLD_SIZE; x++) {
            for (ushort y = 0; y < Settings.WORLD_SIZE; y++) {
                Global.HEIGHT_MAP[x, y] = (ushort)(Global.HEIGHT_MAP[x, y] * Geometry.Sigmoid(distances[x, y] / 40f));
            }
        }
    }
}
