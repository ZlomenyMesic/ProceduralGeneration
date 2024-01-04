using System.Collections.Generic;
using global::minecraft_kurwa.src.global;
using minecraft_kurwa.src.renderer.voxels;

namespace minecraft_kurwa.src.generator.feature.water;

internal static class Creeks {

    internal static void generateCreeks () {
        (int x, int y)[] springs = generateSprings();

        for (int i = 0; i < springs.Length; i++) {
            
        }
    }

    private static (int x, int y)[] generateSprings () {
        List<(int x, int y)> list = new List<(int x, int y)>();
        
        for (int x = 0; x < Settings.WORLD_SIZE; x++) {
            for (int y = 0; y < Settings.WORLD_SIZE; y++) {
                if (Global.RANDOM.Next(0, 10000) <= Settings.CREEK_DENSITY) {
                    list.Add((x, y));
                }
            }
        }

        return list.ToArray();
    }

    private static (int x, int y)? getClosestSlope (int x, int y) {

        for (int i = 1; i <= Settings.CREEK_EXPAND_TRY_LIMIT;) {
            for (int tx = x - i; tx <= x + i; x++) {
                
            }
            
            for (int tx = x - i; tx <= x + i; x++) {
                
            }
        }

        return null;
    }
}