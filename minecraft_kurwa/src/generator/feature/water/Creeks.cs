
//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

using System;
using System.Collections.Generic;
using minecraft_kurwa.debug;
using minecraft_kurwa.src.global;
using minecraft_kurwa.src.renderer.voxels;

namespace minecraft_kurwa.src.generator.feature.water;

internal static class Creeks {
    internal static void GenerateCreeks () {
        (int x, int y)[] springs = GenerateSprings();

        List<(int x, int y)>[] creekPaths = new List<(int x, int y)>[springs.Length];

        byte[] failLength = new byte[springs.Length];

        for (int i = 0; i < springs.Length; i++) {
            
            (int x, int y)? next = GetSlope(springs[i].x, springs[i].y);
            
            if (next == null) {
                failLength[i] = 0;
                continue;
            }
            
            do {
                next = GetSlope(next.Value.x, next.Value.y);
                failLength[i]++;
            } while (next != null);
        }
    }

    private static (int x, int y)[] GenerateSprings() {
        List<(int x, int y)> list = new();
        
        for (int x = 0; x < Settings.WORLD_SIZE; x++) {
            for (int y = 0; y < Settings.WORLD_SIZE; y++) {
                if (Global.RANDOM.Next(0, 1000000) <= Settings.CREEK_DENSITY) {
                    list.Add((x, y));
                    Marker.GenerateMarker(x, y, VoxelType.MARKER_BLOCK_RED);
                }
            }
        }

        return list.ToArray();
    }

    private static (int x, int y)? GetSlope(int x, int y) {

        (int x, int y, int z)? lowest = (-1, -1, Settings.HEIGHT_LIMIT);

        for (int i = Settings.CREEK_EXPAND_TRY_LIMIT; i >= 1; i--) {
            for (int tx = x - i; tx <= x + i; tx++) {
                int ty = tx - x + y;

                //Console.WriteLine("[" + tx + "; " + ty + "]");

                if (tx >= 0 && ty >= 0 && tx < Settings.WORLD_SIZE && ty < Settings.WORLD_SIZE) {
                    if (Global.HEIGHT_MAP[tx, ty] < Global.HEIGHT_MAP[x, y] && Global.HEIGHT_MAP[tx, ty] < lowest.Value.z) {
                        lowest = (tx, ty, Global.HEIGHT_MAP[tx, ty]);
                    }
                }
            }
            
            for (int tx = x - i; tx <= x + i; tx++) {
                int ty = x - tx + y;

                //Console.WriteLine("[" + tx + "; " + ty + "]");

                if (tx >= 0 && ty >= 0 && tx < Settings.WORLD_SIZE && ty < Settings.WORLD_SIZE) {
                    if (Global.HEIGHT_MAP[tx, ty] < Global.HEIGHT_MAP[x, y] && Global.HEIGHT_MAP[tx, ty] < lowest.Value.z) {
                        lowest = (tx, ty, Global.HEIGHT_MAP[tx, ty]);
                    }
                }
            }
        }

        if (lowest.Value.x != -1) {
            Marker.GenerateMarker(lowest.Value.x, lowest.Value.y, VoxelType.MARKER_BLOCK_GREEN);
            return (lowest.Value.x, lowest.Value.y);
        }

        return null;
    }
}