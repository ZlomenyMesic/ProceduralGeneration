//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

using minecraft_kurwa.src.global;
using minecraft_kurwa.src.renderer.voxels;
using minecraft_kurwa.src.generator.feature.tree.types;
using System.Numerics;
using System;

namespace minecraft_kurwa.src.generator.feature.tree;

internal static class TreeGenerator {
    private static Tree[] _trees;
    private static int _treeCount = 0;

    internal static void Generate() {
        ushort maxCount = (ushort)(Settings.WORLD_SIZE * Settings.WORLD_SIZE * Settings.TREE_DENSITY / 10_000);
        _trees = new Tree[maxCount];
        Vector2[] existing = new Vector2[maxCount];

        for (ushort i = 0; i < maxCount; i++) {
            ushort x = (ushort)Global.RANDOM.Next(Settings.WOODY_PLANTS_EDGE_OFFSET, Settings.WORLD_SIZE - Settings.WOODY_PLANTS_EDGE_OFFSET - 1);
            ushort y = (ushort)Global.RANDOM.Next(Settings.WOODY_PLANTS_EDGE_OFFSET, Settings.WORLD_SIZE - Settings.WOODY_PLANTS_EDGE_OFFSET - 1);
            byte biome = Global.BIOME_MAP[x, y, 0];

            // trees can't generate under the water level, on water, or on ice
            if (Global.HEIGHT_MAP[x, y] <= Settings.WATER_LEVEL) continue;
            if (Global.VOXEL_MAP[x, y, Global.HEIGHT_MAP[x, y]] == (byte)VoxelType.WATER || Global.VOXEL_MAP[x, y, Global.HEIGHT_MAP[x, y]] == (byte)VoxelType.ICE) continue;

            // prevent trees from generating too close to each other (at least 1 block gap)
            for (ushort j = 0; j < _treeCount; j++) {
                if (Math.Abs(existing[j].X - x) <= 1 && Math.Abs(existing[j].Y - y) <= 1) goto @continue;
            }
            existing[_treeCount] = new(x, y);

            if (biome == 32 || biome == 42) {
                _trees[_treeCount++] = Global.RANDOM.Next(0, 50) switch {
                    0 => new BasicDeciduousTree(x, y, (ushort)(Global.HEIGHT_MAP[x, y] + 1), (byte)Global.RANDOM.Next(Dimensions.CHERRY_MIN_HEIGHT, Dimensions.CHERRY_MAX_HEIGHT), VoxelType.CHERRY_LEAVES, VoxelType.CHERRY_WOOD),
                    1 or 2 or 3 => new SpruceTree(x, y, (ushort)(Global.HEIGHT_MAP[x, y] + 1), (byte)Global.RANDOM.Next(Dimensions.SPRUCE_MIN_HEIGHT, Dimensions.SPRUCE_MAX_HEIGHT)),
                    4 or 5 or 6 or 7 => new BasicDeciduousTree(x, y, (ushort)(Global.HEIGHT_MAP[x, y] + 1), (byte)Global.RANDOM.Next(Dimensions.BEECH_MIN_HEIGHT, Dimensions.BEECH_MAX_HEIGHT), VoxelType.BEECH_LEAVES, VoxelType.BEECH_WOOD),
                    8 or 9 or 10 or 11 => new PoplarTree(x, y, (ushort)(Global.HEIGHT_MAP[x, y] + 1), (byte)Global.RANDOM.Next(Dimensions.POPLAR_MIN_HEIGHT, Dimensions.POPLAR_MAX_HEIGHT)),
                    12 or 13 or 14 or 15 or 16 or 17 => new BasicDeciduousTree(x, y, (ushort)(Global.HEIGHT_MAP[x, y] + 1), (byte)Global.RANDOM.Next(Dimensions.MAPLE_MIN_HEIGHT, Dimensions.MAPLE_MAX_HEIGHT), VoxelType.MAPLE_LEAVES, VoxelType.MAPLE_WOOD),
                    _ => new BasicDeciduousTree(x, y, (ushort)(Global.HEIGHT_MAP[x, y] + 1), (byte)Global.RANDOM.Next(Dimensions.OAK_MIN_HEIGHT, Dimensions.OAK_MAX_HEIGHT), VoxelType.OAK_LEAVES, VoxelType.OAK_WOOD),
                };
            } else if (biome == 11) {
                _trees[_treeCount++] = Global.RANDOM.Next(0, 7) switch {
                    1 or 2 => new BasicDeciduousTree(x, y, (ushort)(Global.HEIGHT_MAP[x, y] + 1), (byte)Global.RANDOM.Next(Dimensions.MAHOGANY_MIN_HEIGHT, Dimensions.MAHOGANY_MAX_HEIGHT), VoxelType.MAHOGANY_LEAVES, VoxelType.MAHOGANY_WOOD),
                    _ => new KapokTree(x, y, (ushort)(Global.HEIGHT_MAP[x, y] + 1), (byte)Global.RANDOM.Next(Dimensions.KAPOK_MIN_HEIGHT, Dimensions.KAPOK_MAX_HEIGHT))
                };
            } else if (biome == 31 || biome == 41) {
                _trees[_treeCount++] = Global.RANDOM.Next(0, 15) switch {
                    0 => new PineTree(x, y, (ushort)(Global.HEIGHT_MAP[x, y] + 1), (byte)Global.RANDOM.Next(Dimensions.PINE_MIN_HEIGHT, Dimensions.PINE_MAX_HEIGHT)),
                    _ => new SpruceTree(x, y, (ushort)(Global.HEIGHT_MAP[x, y] + 1), (byte)Global.RANDOM.Next(Dimensions.SPRUCE_MIN_HEIGHT, Dimensions.SPRUCE_MAX_HEIGHT))
                };
            } else if (biome == 51 || biome == 61 || biome == 63) {
                _trees[_treeCount++] = new SpruceTree(x, y, (ushort)(Global.HEIGHT_MAP[x, y] + 1), (byte)Global.RANDOM.Next(Dimensions.SPRUCE_MIN_HEIGHT, Dimensions.SPRUCE_MAX_HEIGHT));
            } else if (biome == 5 || biome == 23) {
                _trees[_treeCount++] = Global.RANDOM.Next(0, 20) switch {
                    0 => new BasicDeciduousTree(x, y, (ushort)(Global.HEIGHT_MAP[x, y] + 1), (byte)Global.RANDOM.Next(Dimensions.JACKALBERRY_MIN_HEIGHT, Dimensions.JACKALBERRY_MAX_HEIGHT), VoxelType.JACKALBERRY_LEAVES, VoxelType.JACKALBERRY_WOOD),
                    1 or 2 or 3 or 4 or 5 => new AcaciaTree(x, y, (ushort)(Global.HEIGHT_MAP[x, y] + 1), (byte)Global.RANDOM.Next(Dimensions.ACACIA_MIN_HEIGHT, Dimensions.ACACIA_MAX_HEIGHT)),
                    _ => null
                };
            } else if (biome == 21) {
                _trees[_treeCount++] = new PineTree(x, y, (ushort)(Global.HEIGHT_MAP[x, y] + 1), (byte)Global.RANDOM.Next(Dimensions.PINE_MIN_HEIGHT, Dimensions.PINE_MAX_HEIGHT));
            }

            @continue: continue;
        }

        for (int i = 0; i < _treeCount; i++) {
            _trees[i]?.Build();
        }

        _trees = null;
    }
}
