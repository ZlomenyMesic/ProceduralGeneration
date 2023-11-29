//
// minecraft_kurwa
// by KryKom and ZlomenyMesic
//

using minecraft_kurwa.src.generator.terrain.biomes;
using minecraft_kurwa.src.voxels;

namespace minecraft_kurwa.src.generator.feature {

    internal class FeatureGenerator {
        
    }
    
    internal class Feature {

        internal static (FeatureType featureType, int desity)[] GetFeatures(BiomeType biome) {
            switch (biome) {
                case BiomeType.POLAR_TAIGA: return new[] { (FeatureType.BUSH_POLAR_SMALL, 10), (FeatureType.BUSH_POLAR_LARGE, 5), (FeatureType.TREE_PINE_LARGE, 50), (FeatureType.TREE_PINE_SMALL, 70) };
                default: return new[] { (FeatureType.GRASS_SHORT, 10) };
            }
        }
        
        internal static (byte wood, byte leaves) GetTreeBlocks(FeatureType feature) {
            switch (feature) {
                case FeatureType.TREE_PINE_LARGE: return (0, 0);
                case FeatureType.BUSH: return (0, 0);
                default: return ((byte) VoxelType.OAK_WOOD, (byte) VoxelType.OAK_LEAVES);
            }
        }
    }

    internal enum FeatureType {
        TREE,
        TREE_OAK_SMALL,
        TREE_OAK_LARGE,
        TREE_PINE_SMALL,
        TREE_PINE_LARGE,
        TREE_KAPOK_SMALL,
        TREE_KAPOK_LARGE,
        TREE_SPRUCE_SMALL,
        TREE_SPRUCE_LARGE,
        TREE_ACACIA_SMALL,
        TREE_ACACIA_LARGE,
        TREE_JUNGLE_SMALL,
        TREE_JUNGLE_LARGE,
        TREE_POPLAR_SMALL,
        TREE_POPLAR_LARGE,
        
        GRASS,
        GRASS_SHORT,
        GRASS_TALL,
        
        ROCK,
        ROCK_SMALL,
        ROCK_LARGE,
        
        BUSH,
        BUSH_DRY_SMALL,
        BUSH_DRY_LARGE,
        BUSH_SMALL,
        BUSH_LARGE,
        BUSH_POLAR_SMALL,
        BUSH_POLAR_LARGE,
        
        POND
    }
}