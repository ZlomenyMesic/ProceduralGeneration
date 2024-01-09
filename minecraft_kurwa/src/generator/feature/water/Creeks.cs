//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

using System;
using System.Collections.Generic;
using Microsoft.VisualBasic;
using minecraft_kurwa.debug;
using minecraft_kurwa.src.global;
using minecraft_kurwa.src.renderer.voxels;

namespace minecraft_kurwa.src.generator.feature.water;

internal static class Creeks {

    internal static void generateCreeks () {
        
        // get springs
        (int x, int y)[] springs = generateSprings();

        // creek paths
        List<(int x, int y)>[] creekPaths = new List<(int x, int y)>[springs.Length];
        
        for (int i = 0; i < springs.Length; i++) {
            
            // init
            creekPaths[i] = new List<(int x, int y)>();
            
            creekPaths[i].Add(springs[i]);
            
            // get next step for the first part of the creek
            (int x, int y)? next = getSlope(springs[i].x, springs[i].y);
            
            // if it does not exist, skip
            if (next == null) continue;
            creekPaths[i].Add((next.Value.x, next.Value.y));
            
            // get next step if it exsits 
            do {
                next = getSlope(next.Value.x, next.Value.y);
                
                // add it to the list of steps
                if (next != null) creekPaths[i].Add((next.Value.x, next.Value.y));
                
            } while (next != null);
            
            generateCreek(creekPaths[i].ToArray());
        }
    }

    private static void generateCreek ((int x, int y)[] pathPoints) {

        if (pathPoints == null) return;
        if (pathPoints.Length <= Settings.MIN_CREEK_LENGTH) return;

        for (int i = 0; i < pathPoints.Length - 1; i++) {
            
            if (Math.Abs(pathPoints[i].y - pathPoints[i + 1].y) < Math.Abs(pathPoints[i].x - pathPoints[i + 1].x)) {

                if (pathPoints[i].x < pathPoints[i + 1].x) {
                    generateCreekPartLow(pathPoints[i], pathPoints[i + 1]);
                } else {
                    generateCreekPartLow(pathPoints[i + 1], pathPoints[i]);
                }
                
            } else {
                
                if (pathPoints[i].y < pathPoints[i + 1].y) {
                    generateCreekPartHigh(pathPoints[i], pathPoints[i + 1]);
                } else {
                    generateCreekPartHigh(pathPoints[i + 1], pathPoints[i]);
                }
            }
        }
    }

    private static void generateCreekPartLow((int x, int y) p1, (int x, int y) p2) {

        int deltaX = p2.x - p1.x;
        int deltaY = p2.y - p1.y;

        int yi = 1;

        if (deltaY < 0) {
            yi = -1;
            deltaY *= -1;
        }

        int D = 2 * deltaY - deltaX;
        int y = p1.y;

        for (int x = p1.x; x < p2.x; x++) {
            placeCreekPart(x, y);

            if (D > 0) {
                y += yi;
                D += 2 * (deltaY - deltaX);
            } else {
                D += 2 * deltaY;
            }
        }

    }
    
    private static void generateCreekPartHigh((int x, int y) p1, (int x, int y) p2) {
        
        int deltaX = p2.x - p1.x;
        int deltaY = p2.y - p1.y;

        int xi = 1;

        if (deltaX < 0) {
            xi = -1;
            deltaX *= -1;
        }

        int D = 2 * deltaY - deltaX;
        int x = p1.x;

        for (int y = p1.y; y < p2.y; y++) {
            placeCreekPart(x, y);

            if (D > 0) {
                x += xi;
                D += 2 * (deltaX - deltaY);
            } else {
                D += 2 * deltaX;
            }
        }
    }

    private static readonly ushort[,] heightPattern = { 
        {0, 1, 1, 1, 0},
        {1, 2, 2, 2, 1},
        {1, 2, 5, 2, 1},
        {1, 2, 2, 2, 1},
        {0, 1, 1, 1, 0}
    };

    private static readonly byte[,] waterPattern = {
        {0, 0, 0, 0, 0},
        {0, 1, 1, 1, 0},
        {0, 1, 1, 1, 0},
        {0, 1, 1, 1, 0},
        {0, 0, 0, 0, 0}
    };

    private static readonly int patternSize = 5;

    private static void placeCreekPart(int x, int y) {

        int px = 0;
        int py = 0;
        
        for (int tx = x - (patternSize - 1) / 2; tx <= x + (patternSize - 1) / 2; tx++) {
            for (int ty = y - (patternSize - 1) / 2; ty <= y + (patternSize - 1) / 2; ty++) {
                try {
                    Global.HEIGHT_MAP[tx, ty] = (ushort)(Global.HEIGHT_MAP[tx, ty] - heightPattern[px, py]);

                    if (waterPattern[px, py] == 1) {
                        Global.VOXEL_MAP[tx, ty, Global.HEIGHT_MAP[tx, ty] + heightPattern[px, py]] = (byte)VoxelType.WATER;
                    }
                    
                } catch (IndexOutOfRangeException e) {}

                py++;
            }

            px++;
        }
    }

    private static (int x, int y)[] generateSprings () {
        
        List<(int x, int y)> list = new List<(int x, int y)>();
        
        for (int x = 0; x < Settings.WORLD_SIZE; x++) {
            for (int y = 0; y < Settings.WORLD_SIZE; y++) {
                if (Global.RANDOM.Next(0, 1000000) <= Settings.CREEK_DENSITY) {
                    list.Add((x, y));
                    // Marker.generateMarker(x, y, VoxelType.MARKER_BLOCK_RED);
                }
            }
        }

        return list.ToArray();
    }

    private static (int x, int y)? getSlope (int x, int y) {

        (int x, int y, int z)? lowest = (-1, -1, Settings.HEIGHT_LIMIT);

        for (int i = Settings.CREEK_EXPAND_TRY_LIMIT; i >= 1; i--) {
            for (int tx = x - i; tx <= x + i; tx++) {
                int ty = y + Math.Abs(tx - x) - i;

                try {
                    
                    if (Global.HEIGHT_MAP[tx, ty] < Global.HEIGHT_MAP[x, y] && Global.HEIGHT_MAP[tx, ty] < lowest.Value.z) {
                        lowest = (tx, ty, Global.HEIGHT_MAP[tx, ty]);
                    }
                } catch {}
            }
            
            for (int tx = x - i; tx <= x + i; tx++) {
                int ty = y - Math.Abs(x - tx) + i;
                
                try {
                    
                    if (Global.HEIGHT_MAP[tx, ty] < Global.HEIGHT_MAP[x, y] && Global.HEIGHT_MAP[tx, ty] < lowest.Value.z) {
                        lowest = (tx, ty, Global.HEIGHT_MAP[tx, ty]);
                    }
                } catch {}
            }
        }

        if (lowest.Value.x != -1) {
            // Marker.generateMarker(lowest.Value.x, lowest.Value.y, VoxelType.MARKER_BLOCK_GREEN);
            return (lowest.Value.x, lowest.Value.y);
        }

        return null;
    }
}