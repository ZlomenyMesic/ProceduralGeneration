//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

using System;
using System.Linq;
using System.Numerics;

namespace minecraft_kurwa {
    internal static class Water {
        private const byte MIN_POND_SIZE = 6;
        private const byte MAX_POND_SIZE = 13;

        internal static void Generate() {
            Random random = new(Settings.SEED);

            FillWaterLevel();
            GeneratePonds(random);
            Freeze(random);
        }

        private static void FillWaterLevel() {
            for (ushort x = 0; x < Settings.WORLD_SIZE; x++) {
                for (ushort y = 0; y < Settings.WORLD_SIZE; y++) {
                    if (Global.HEIGHT_MAP[x, y] < Settings.WATER_LEVEL) {
                        Global.VOXEL_MAP[x, y, Settings.WATER_LEVEL] = (byte)VoxelType.WATER;
                        Global.HEIGHT_MAP[x, y] = Settings.WATER_LEVEL;
                    }
                }
            }
        }

        private static void GeneratePonds(Random random) {
            ushort maxCount = Settings.WORLD_SIZE * Settings.WORLD_SIZE * Settings.POND_DENSITY / 10_000;
            Vector2[] existing = new Vector2[maxCount];
            ushort existingCounter = 0;

            for (ushort i = 0; i < maxCount; i++) {
                ushort x = (ushort)random.Next(0, Settings.WORLD_SIZE - 1);
                ushort y = (ushort)random.Next(0, Settings.WORLD_SIZE - 1);

                for (ushort j = 0; j < existingCounter; j++) {
                    if (Math.Abs(existing[j].X - x) <= MAX_POND_SIZE && Math.Abs(existing[j].Y - y) <= MAX_POND_SIZE) goto exit;
                }

                ushort sizeX = (ushort)random.Next(MIN_POND_SIZE, MAX_POND_SIZE + 1);
                ushort sizeY = (ushort)random.Next(MIN_POND_SIZE, MAX_POND_SIZE + 1);

                ushort x2 = (ushort)(x + random.Next(-1, 2));
                ushort y2 = (ushort)(y + random.Next(-1, 2));

                (ushort, ushort) diff = GetPondHeightDifferences(x, y, sizeX, sizeY);
                (ushort, ushort) diff2 = GetPondHeightDifferences(x2, y2, sizeY, sizeX);

                if (diff.Item1 > 2 || diff2.Item1 > 2) goto exit;

                if (random.Next(0, 2) == 0) diff.Item2--; // sometimes ponds will generate one block lower

                GeneratePond(x, y, sizeX, sizeY, diff.Item2, random);
                GeneratePond(x2, y2, sizeY, sizeX, diff.Item2, random);

                existing[existingCounter++] = new Vector2(x, y);

                exit: continue;
            }
        }

        private static void GeneratePond(ushort posX, ushort posY, ushort sizeX, ushort sizeY, ushort waterLevel, Random random) {
            for (short x = (short)(-sizeX / 2); x < sizeX / 2; x++) {
                if (posX + x < 0 || posX + x >= Settings.WORLD_SIZE) continue;

                for (short y = (short)(-sizeY / 2); y < sizeY / 2; y++) {
                    if (posY + y < 0 || posY + y >= Settings.WORLD_SIZE) continue;

                    if (x - 0.5f >= sizeX / 2 * 3 / 5 && y - 0.5f >= sizeY / 2 * 3 / 5) continue;
                    if (x + 1.0f < -sizeX / 2 * 3 / 5 && y - 0.5f >= sizeY / 2 * 3 / 5) continue;
                    if (x - 0.5f >= sizeX / 2 * 3 / 5 && y + 1.0f < -sizeY / 2 * 3 / 5) continue;
                    if (x + 1.0f < -sizeX / 2 * 3 / 5 && y + 1.0f < -sizeY / 2 * 3 / 5) continue;

                    if ((x == -sizeX / 2 || x == (sizeX / 2) - 1) && random.Next(0, 3) == 0) continue;
                    if ((y == -sizeY / 2 || y == (sizeY / 2) - 1) && random.Next(0, 3) == 0) continue;

                    Global.VOXEL_MAP[posX + x, posY + y, waterLevel] = (byte)VoxelType.WATER;
                    Global.VOXEL_MAP[posX + x, posY + y, waterLevel + 1] = null;
                    Global.VOXEL_MAP[posX + x, posY + y, waterLevel + 2] = null;
                    Global.VOXEL_MAP[posX + x, posY + y, waterLevel + 3] = null;

                    Global.HEIGHT_MAP[posX + x, posY + y] = waterLevel;
                }
            }
        }

        private static (ushort, ushort) GetPondHeightDifferences(ushort x, ushort y, ushort sizeX, ushort sizeY) {
            ushort[] heights = new ushort[9];

            byte index = 0;
            for (short x2 = -1; x2 <= 1; x2++) {
                if (x + (x2 * sizeX / 2) < 0 || x + (x2 * sizeX / 2) >= Settings.WORLD_SIZE) continue;

                for (short  y2 = -1; y2 <= 1; y2++) {
                    if (y + (y2 * sizeY / 2) < 0 || y + (y2 * sizeY / 2) >= Settings.WORLD_SIZE) continue;

                    heights[index++] = Global.HEIGHT_MAP[x + (x2 * sizeX / 2), y + (y2 * sizeY / 2)];
                }
            }

            ushort waterLevel = heights.Min();
            if (waterLevel == Settings.WATER_LEVEL) goto cannotGenerate;

            for (short x2 = (short)((-sizeX / 2) - 1); x2 <= (sizeX / 2) + 1; x2++) {
                if (x + x2 < 0 || x + x2 >= Settings.WORLD_SIZE) continue;

                for (short y2 = (short)((-sizeY / 2) - 1); y2 <= (sizeY / 2) + 1; y2++) {
                    if (y + y2 < 0 || y + y2 >= Settings.WORLD_SIZE) continue;

                    if (Global.HEIGHT_MAP[x + x2, y + y2] < waterLevel) goto cannotGenerate;
                }
            }

            ushort maxDifference = (ushort)(heights.Max() - waterLevel);

            return (maxDifference, waterLevel);

            cannotGenerate: return (100, 0);
        }

        private static void Freeze(Random random) {
            for (ushort x = 0; x < Settings.WORLD_SIZE; x++) {
                for (ushort y = 0; y < Settings.WORLD_SIZE; y++) {
                    for (ushort z = 0; z < Settings.HEIGHT_LIMIT; z++) {
                        if (Global.VOXEL_MAP[x, y, z] == (byte)VoxelType.WATER) {
                            Global.VOXEL_MAP[x, y, z] = CanFreeze(x, y, (ushort)random.Next(Settings.FREEZING_DISTANCE, Settings.MIN_FREEZING_DISTANCE))
                                ? random.Next(0, 100) > Settings.ICE_HOLES - 1
                                    ? (byte)VoxelType.ICE
                                    : (byte)VoxelType.WATER
                                : (byte)VoxelType.WATER;
                        }
                    }
                }
            }
        }

        private static bool CanFreeze(ushort x, ushort y, ushort distance) {
            for (sbyte x2 = -1; x2 <= 1; x2++) {
                for (sbyte y2 = -1; y2 <= 1; y2++) {

                    short x3 = (short)(x + (x2 * distance));
                    short y3 = (short)(y + (y2 * distance));
                    if (x3 < 0 || x3 >= Settings.WORLD_SIZE || y3 < 0 || y3 >= Settings.WORLD_SIZE) continue;

                    if (Global.BIOME_MAP[x3, y3, 0] < 30) return false; // water is too close to a subtropical or tropical biome to freeze
                    if (Global.BIOME_MAP[x3, y3, 0] >= 60 && Global.BIOME_MAP[x3, y3, 0] < 70) return true; // water is close to a polar biome
                }
            }
            return false;
        }
    }
}
