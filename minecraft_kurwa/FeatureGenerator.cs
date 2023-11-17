//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

using System;

namespace minecraft_kurwa {
    internal class FeatureGenerator {
        public static class Rock {
            public static void GenerateRock(int size, int posX, int posY) {
                for (int x = posX - size / 2; x < posX + size / 2; x++) {
                    for (int y = posY - size / 2; y < posY + size / 2; y++) {
                        for (int z = Global.HEIGHT_MAP[x, y] - 2 - size / 2; z < Global.HEIGHT_MAP[x, y] - 2 + size / 2; z++) {
                            if (PseudoCircle(x, y, z)) {
                                Global.VOXEL_MAP[x, y, z] = VoxelType.STONE;
                            }
                        }
                    }
                }
            }
        }

        private static bool PseudoCircle(int x, int y, int z) {
            return Math.Round(Math.Pow(x, 2) / Math.Pow(z, 2) - Math.Pow(z, 2)) < y && y < Math.Round(Math.Pow(x, 2) / (-Math.Pow(z, 2)) + Math.Pow(z, 2));
        }
    }
}
