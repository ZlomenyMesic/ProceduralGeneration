//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

using SharpDX;
using System;

namespace minecraft_kurwa {
    internal class FeatureGenerator {
        internal static class Tree {
            internal static readonly int[,] probability = {
                {0 , 20, 50 , 20, 0 },
                {20, 65, 90 , 65, 20},
                {50, 90, 100, 90, 50},
                {20, 65, 90 , 65, 20},
                {0 , 20, 50 , 20, 0} };

            internal static void BuildDeciduousTree(int size, int treetopCount, int treetopSize, int featureX, int featureY) {
                int[,] output = new int[5, 5];
                Random r = new(Global.SEED);

                for (int i = 0; i < size; i++) {
                    for (int x = 0; x < 5; x++) {
                        for (int y = 0; y < 5; y++) {
                            if (r.Next(100) <= probability[x, y]) {
                                output[x, y]++;
                            }
                        }
                    }
                }

                int[,] treetopPos = new int[treetopCount, 3];

                for (int i = 0; i < treetopCount; i++) {
                    treetopPos[i, 0] = r.Next(5, 20) * (r.Next(0, 2) * 2 - 1);
                    treetopPos[i, 1] = r.Next(5, 20) * (r.Next(0, 2) * 2 - 1);
                    treetopPos[i, 2] = r.Next() + (size / 3 * 2) + Global.HEIGHT_MAP[featureX, featureY] - 2;
                }

                for (int i = 0; i < treetopCount; i++) {
                    for (int x = treetopPos[i, 0] - treetopSize / 2; x < treetopPos[i, 0] + treetopSize / 2; x++) {
                        for (int y = treetopPos[i, 1] - treetopSize / 2; y < treetopPos[i, 1] + treetopSize / 2; y++) {
                            for (int z = treetopPos[i, 2] - 2 - treetopSize / 2; z < treetopPos[i, 2] - 2 + treetopSize / 2; z++) {
                                if (PseudoCircle(x, y, z)) {
                                    Global.VOXEL_MAP[x, y, z] = VoxelType.OAK_LEAVES;
                                }
                            }
                        }
                    }
                }

                for (int i = 0; i < treetopCount; i++) {
                    // TODO branches
                }
            }

            internal static void BuildConiferousTree(int size, int treetopCount, int featureX, int featureY) {

            }
        }

        internal static class Rock {
            public static void GenerateRock(int size, int featureX, int featureY) {
                for (int x = featureX - size / 2; x < featureX + size / 2; x++) {
                    for (int y = featureY - size / 2; y < featureY + size / 2; y++) {
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
