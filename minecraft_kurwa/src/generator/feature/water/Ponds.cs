//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

using minecraft_kurwa.src.global;
using minecraft_kurwa.src.renderer.voxels;
using System.Linq;
using System;
using Microsoft.Xna.Framework;
using minecraft_kurwa.src.global.functions;

namespace minecraft_kurwa.src.generator.feature.water;

internal static class Ponds {
    private const byte MIN_POND_SIZE = 7;
    private const byte MAX_POND_SIZE = 10;

    internal static void Generate() {
        ushort maxCount = (ushort)(Settings.WORLD_SIZE * Settings.WORLD_SIZE * Settings.POND_DENSITY / 10_000);
        Vector2[] existing = new Vector2[maxCount];
        ushort existingCounter = 0;

        for (ushort i = 0; i < maxCount; i++) {
            ushort x = (ushort)Global.RANDOM.Next(0, Settings.WORLD_SIZE - 1);
            ushort y = (ushort)Global.RANDOM.Next(0, Settings.WORLD_SIZE - 1);

            // prevent ponds from generating close to each other
            for (ushort j = 0; j < existingCounter; j++) {
                if (Math.Abs(existing[j].X - x) <= MAX_POND_SIZE && Math.Abs(existing[j].Y - y) <= MAX_POND_SIZE) goto @continue;
            }

            ushort sizeX = (ushort)Global.RANDOM.Next(MIN_POND_SIZE, MAX_POND_SIZE + 1);
            ushort sizeY = (ushort)Global.RANDOM.Next(MIN_POND_SIZE, MAX_POND_SIZE + 1);

            (ushort, ushort) diff = GetPondHeightDifferences(x, y, sizeX, sizeY);

            if (diff.Item1 > 2) goto @continue;

            if (Global.RANDOM.Next(0, 2) == 0) diff.Item2--; // sometimes ponds will generate one block lower

            GeneratePond(x, y, sizeX, sizeY, diff.Item2);

            existing[existingCounter++] = new Vector2(x, y);

            @continue: continue;
        }
    }

    private static void GeneratePond(ushort posX, ushort posY, ushort sizeX, ushort sizeY, ushort waterLevel) {
        float a = (float)Math.Round((double)sizeX / 2, 0);
        float b = (float)Math.Round((double)sizeY / 2, 0);

        for (short x = (short)Math.Round(-a, 0); x <= a; x++) {
            if (!World.IsInRange(posX + x)) continue;

            for (short y = (short)Math.Round(-b, 0); y <= b; y++) {
                if (!World.IsInRange(posY + y)) continue;

                if (Geometry.Ellipse(x, y, a - 0.2f, b - 0.2f) && ((Math.Abs(x) < a - 1 && Math.Abs(y) < b - 1) || Global.RANDOM.Next(0, 4) != 0)) {
                    Global.VOXEL_MAP[posX + x, posY + y, waterLevel] = (byte?)VoxelType.WATER;

                    Global.VOXEL_MAP[posX + x, posY + y, waterLevel + 1] = null;
                    Global.VOXEL_MAP[posX + x, posY + y, waterLevel + 2] = null;
                    Global.VOXEL_MAP[posX + x, posY + y, waterLevel + 3] = null;

                    Global.HEIGHT_MAP[posX + x, posY + y] = waterLevel;
                }
            }
        }
    }

    private static (ushort, ushort) GetPondHeightDifferences(ushort x, ushort y, ushort sizeX, ushort sizeY) {
        ushort[] heights = new ushort[9];

        byte index = 0;
        int nx, ny;
        for (short x2 = -1; x2 <= 1; x2++) {
            nx = x + x2 * sizeX / 2;
            if (!World.IsInRange(nx)) continue;

            for (short y2 = -1; y2 <= 1; y2++) {
                ny = y + y2 * sizeY / 2;
                if (!World.IsInRange(nx, ny)) continue;

                heights[index++] = Global.HEIGHT_MAP[nx, ny];
            }
        }

        ushort waterLevel = heights.Min();
        if (waterLevel == Settings.WATER_LEVEL) goto cannotGenerate;

        for (short x2 = (short)(-sizeX / 2 - 1); x2 <= sizeX / 2 + 1; x2++) {
            if (!World.IsInRange(x + x2)) continue;

            for (short y2 = (short)(-sizeY / 2 - 1); y2 <= sizeY / 2 + 1; y2++) {
                if (!World.IsInRange(y + y2)) continue;

                if (Global.HEIGHT_MAP[x + x2, y + y2] < waterLevel) goto cannotGenerate;
            }
        }

        ushort maxDifference = (ushort)(heights.Max() - waterLevel);

        return (maxDifference, waterLevel);

        cannotGenerate: return (100, 0);
    }
}
