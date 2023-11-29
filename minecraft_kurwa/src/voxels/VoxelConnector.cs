//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

using Microsoft.Xna.Framework;
using minecraft_kurwa.src.gui.colors;
using minecraft_kurwa.src.global;

namespace minecraft_kurwa.src.voxels {
    internal static class VoxelConnector {
        internal static VoxelStructure[] world;
        private static Voxel[,,] grid;

        internal static int voxelCounter = 0; // how many voxels is in the scene
        private static int voxelStructCount = 0;
        private static int currentVoxelCount = 0;

        // creates a grid with identical voxels connected into bigger blocks
        internal static void CreateGrid() {
            grid = new Voxel[Settings.WORLD_SIZE, Settings.WORLD_SIZE, Settings.HEIGHT_LIMIT];
            byte?[,,] voxelMap = new byte?[Settings.WORLD_SIZE, Settings.WORLD_SIZE, Settings.HEIGHT_LIMIT];

            for (ushort x = 0; x < Settings.WORLD_SIZE; x++) {
                for (ushort y = 0; y < Settings.WORLD_SIZE; y++) {
                    for (ushort z = 0; z < Settings.HEIGHT_LIMIT; z++) {
                        voxelMap[x, y, z] = Global.VOXEL_MAP[x, y, z];
                    }
                }
            }

            // x-axis connections
            for (ushort y = 0; y < Settings.WORLD_SIZE; y++) {
                for (ushort z = 0; z < Settings.HEIGHT_LIMIT; z++) {

                    ushort last = ushort.MaxValue;
                    for (ushort x = 0; x < Settings.WORLD_SIZE; x++) {
                        if (last == ushort.MaxValue && x != Settings.WORLD_SIZE - 1 && voxelMap[x, y, z] == voxelMap[x + 1, y, z] && Global.BIOME_MAP[x, y, 0] == Global.BIOME_MAP[x + 1, y, 0]) {
                            grid[x, y, z] = new(voxelMap[x, y, z], new(x, y, z), new(1, 1, 1));
                            voxelMap[x, y, z] = null;
                            last = x;
                        }
                        else if (last != ushort.MaxValue && x != Settings.WORLD_SIZE - 1 && voxelMap[x, y, z] == voxelMap[x + 1, y, z] && Global.BIOME_MAP[x, y, 0] == Global.BIOME_MAP[x + 1, y, 0]) {
                            grid[last, y, z].size.X++;
                            voxelMap[x, y, z] = null;
                        }
                        else if (last != ushort.MaxValue && x != Settings.WORLD_SIZE - 1 && (voxelMap[x, y, z] != voxelMap[x + 1, y, z] || Global.BIOME_MAP[x, y, 0] != Global.BIOME_MAP[x + 1, y, 0])) {
                            grid[last, y, z].size.X++;
                            voxelMap[x, y, z] = null;
                            last = ushort.MaxValue;
                        }
                    }
                }
            }

            // y-axis connection
            for (ushort x = 0; x < Settings.WORLD_SIZE; x++) {
                for (ushort z = 0; z < Settings.HEIGHT_LIMIT; z++) {

                    ushort last = ushort.MaxValue;
                    for (ushort y = 0; y < Settings.WORLD_SIZE; y++) {
                        if (last == ushort.MaxValue && y != Settings.WORLD_SIZE - 1 && voxelMap[x, y, z] == voxelMap[x, y + 1, z] && Global.BIOME_MAP[x, y, 0] == Global.BIOME_MAP[x, y + 1, 0]) {
                            if (voxelMap[x, y, z] != null) {
                                grid[x, y, z] = new(voxelMap[x, y, z], new(x, y, z), new(1, 1, 1));
                                voxelMap[x, y, z] = null;
                                last = y;
                            }
                        }
                        else if (last != ushort.MaxValue && y != Settings.WORLD_SIZE - 1 && voxelMap[x, y, z] == voxelMap[x, y + 1, z] && Global.BIOME_MAP[x, y, 0] == Global.BIOME_MAP[x, y + 1, 0]) {
                            if (voxelMap[x, y, z] != null) {
                                grid[x, last, z].size.Z++;
                                voxelMap[x, y, z] = null;
                            }
                        }
                        else if (last != ushort.MaxValue && y != Settings.WORLD_SIZE - 1 && (voxelMap[x, y, z] != voxelMap[x, y + 1, z] || Global.BIOME_MAP[x, y, 0] != Global.BIOME_MAP[x, y + 1, 0])) {
                            if (voxelMap[x, y, z] != null) {
                                grid[x, last, z].size.Z++;
                                voxelMap[x, y, z] = null;
                            }
                            last = ushort.MaxValue;
                        }
                    }
                }
            }

            // single voxels are placed separately
            for (ushort x = 0; x < Settings.WORLD_SIZE; x++) {
                for (ushort y = 0; y < Settings.WORLD_SIZE; y++) {
                    for (ushort z = 0; z < Settings.HEIGHT_LIMIT; z++) {
                        if (voxelMap[x, y, z] != null) {
                            grid[x, y, z] = new(voxelMap[x, y, z], new(x, y, z), new(1, 1, 1));
                            voxelMap[x, y, z] = null;
                        }
                    }
                }
            }
        }

        // transforms the grid into a VoxelStructure array
        internal static void GenerateWorld() {
            world = new VoxelStructure[Settings.WORLD_SIZE * Settings.HEIGHT_LIMIT];

            for (ushort x = 0; x < Settings.WORLD_SIZE; x++) {
                for (ushort y = 0; y < Settings.WORLD_SIZE; y++) {
                    for (ushort z = 0; z < Settings.HEIGHT_LIMIT; z++) {
                        if (grid[x, y, z].type != null) {
                            AddBlock(new(x, z, y), grid[x, y, z].size, ColorManager.GetVoxelColor(grid[x, y, z].type, Global.BIOME_MAP[x, y, 0], z, x * y * z), Settings.TRANSPARENT_TEXTURES ? ColorManager.GetVoxelTransparency(grid[x, y, z].type) : (byte)100);
                        }
                    }
                }
            }
        }

        private static void AddBlock(Vector3 position, Vector3 size, Color color, byte transparency = 100) {
            world[voxelStructCount] ??= new();
            world[voxelStructCount].AddVoxel(position, size, color, transparency);

            if (++currentVoxelCount > VoxelStructure.MAX_VOXEL_COUNT - 1) {
                currentVoxelCount = 0;
                voxelStructCount++;
            }

            voxelCounter++;
        }

        private struct Voxel {
            internal byte? type;
            internal Vector3 position;
            internal Vector3 size;

            internal Voxel(byte? type, Vector3 position, Vector3 size) {
                this.type = type;
                this.position = position;
                this.size = size;
            }
        }
    }
}
