using System;
using System.Collections.Generic;
using minecraft_kurwa.debug;
using minecraft_kurwa.src.global;
using minecraft_kurwa.src.renderer.voxels;
using Console = System.Console;

namespace minecraft_kurwa.src.generator.feature.water;

internal static class Creeks {
    internal static void generateCreeks () {
        (int x, int y)[] springs = generateSprings();

        List<(int x, int y)>[] creekPaths = new List<(int x, int y)>[springs.Length];

        byte[] failed = new byte[springs.Length];

        for (int i = 0; i < springs.Length; i++) {
            getClosestSlope(springs[i].x, springs[i].y);
        }
    }

    private static (int x, int y)[] generateSprings () {
        List<(int x, int y)> list = new List<(int x, int y)>();
        
        for (int x = 0; x < Settings.WORLD_SIZE; x++) {
            for (int y = 0; y < Settings.WORLD_SIZE; y++) {
                if (Global.RANDOM.Next(0, 1000000) <= Settings.CREEK_DENSITY) {
                    list.Add((x, y));
                    Marker.generateMarker(x, y, VoxelType.MARKER_BLOCK_RED);
                }
            }
        }

        return list.ToArray();
    }

    private static (int x, int y)? getClosestSlope (int x, int y) {

        for (int i = Settings.CREEK_EXPAND_TRY_LIMIT; i >= 10; i--) {
            for (int tx = x - i; tx <= x + i; tx++) {
                int ty = tx - x + y;

                Console.WriteLine("[" + tx + "; " + ty + "]");

                try {
                    if (Global.HEIGHT_MAP[tx, ty] < Global.HEIGHT_MAP[x, y]) {
                        Marker.generateMarker(tx, ty, VoxelType.MARKER_BLOCK_GREEN);
                        return (tx, ty);
                    }
                } catch {}
            }
            
            for (int tx = x - i; tx <= x + i; tx++) {
                int ty = x - tx + y;

                Console.WriteLine("[" + tx + "; " + ty + "]");
                
                try {
                    if (Global.HEIGHT_MAP[tx, ty] < Global.HEIGHT_MAP[x, y]) {
                        Marker.generateMarker(tx, ty, VoxelType.MARKER_BLOCK_GREEN);
                        return (tx, ty);
                    }
                } catch {}
            }
        }

        return null;
    }
}