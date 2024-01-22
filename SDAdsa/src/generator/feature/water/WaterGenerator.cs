//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

using minecraft_kurwa.src.global;
using minecraft_kurwa.src.global.functions;
using minecraft_kurwa.src.renderer.voxels;

namespace minecraft_kurwa.src.generator.feature.water;

internal static class WaterGenerator {
    internal static void GenerateRivers() => Rivers.GenerateMap();

    internal static void GenerateOtherWaterThanRivers() {
        FillWaterLevel();
        Ponds.Generate();
    }
    internal static bool ShouldFreeze(ushort x, ushort y, ushort distance) {
        for (sbyte x2 = -1; x2 <= 1; x2++) {
            for (sbyte y2 = -1; y2 <= 1; y2++) {

                short x3 = (short)(x + x2 * distance);
                short y3 = (short)(y + y2 * distance);
                if (!World.IsInRange(x3, y3)) continue;

                if (Global.BIOME_MAP[x3, y3, 0] < 30) return false; // water is too close to a subtropical or tropical biome to freeze
                if (Global.BIOME_MAP[x3, y3, 0] >= 60 && Global.BIOME_MAP[x3, y3, 0] < 70) return true; // water is close to a polar biome
            }
        }
        return false;
    }

    private static void FillWaterLevel() {
        for (ushort x = 0; x < Settings.WORLD_SIZE; x++) {
            for (ushort y = 0; y < Settings.WORLD_SIZE; y++) {
                if (Global.HEIGHT_MAP[x, y] < Settings.WATER_LEVEL) {
                    Global.VOXEL_MAP[x, y, Settings.WATER_LEVEL] = (byte)VoxelType.WATER;
                    Global.HEIGHT_MAP[x, y] = (ushort)Settings.WATER_LEVEL;
                } 

                else if (Global.HEIGHT_MAP[x, y] == Settings.WATER_LEVEL || (Global.HEIGHT_MAP[x, y] == Settings.WATER_LEVEL + 1 && Global.RANDOM.Next(0, 3) != 0) || (Global.HEIGHT_MAP[x, y] == Settings.WATER_LEVEL + 2 && Global.RANDOM.Next(0, 3) == 0)) {
                    byte biome = Global.BIOME_MAP[x, y, 0];
                    if (biome > 4) Global.VOXEL_MAP[x, y, Global.HEIGHT_MAP[x, y]] = Global.RANDOM.Next(0, 6) != 0
                            ? (byte)(biome < 10 || (biome >= 30 && biome < 50) ? VoxelType.SAND : VoxelType.MUD)
                            : (byte)VoxelType.RIVER_ROCK;
                }
            }
        }
    }
}
