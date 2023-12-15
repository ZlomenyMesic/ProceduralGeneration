//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

using Microsoft.Xna.Framework;
using minecraft_kurwa.src.global;
using System;

namespace minecraft_kurwa.src.renderer.voxels {
    internal static class VoxelCulling {
        internal static readonly Vector3 defaultCTPosition = new(0, 300, -250); // default camera target position relative to camera position

        internal static ushort minRenderX = 0;
        internal static ushort maxRenderX = (ushort)(Settings.WORLD_SIZE - 1);

        internal static ushort minRenderY = 0;
        internal static ushort maxRenderY = (ushort)(Settings.WORLD_SIZE - 1);

        internal static readonly ushort MAX_DIFFERENCE_VALUE = 300; // maximum possible xDiff and yDiff
        private static readonly ushort MIN_DIFFERENCE = (ushort)(25 + ((Settings.WORLD_SIZE - 500) / 50)); // minimum camPosition and camTarget coordinates difference to cull

        internal static void Update() {
            short xDiff = (short)(Global.CAM_POSITION.X - Global.CAM_TARGET.X);
            short yDiff = (short)(Global.CAM_POSITION.Z - Global.CAM_TARGET.Z);

            if (xDiff < -MIN_DIFFERENCE) {
                short minRenderX = (short)(Global.CAM_POSITION.X - MAX_DIFFERENCE_VALUE - xDiff - (Global.CAM_POSITION.Y / 2));
                VoxelCulling.minRenderX = (ushort)(minRenderX < 0 ? 0 : minRenderX);
            } else minRenderX = 0;

            if (xDiff > MIN_DIFFERENCE) {
                short maxRenderX = (short)(Global.CAM_POSITION.X + MAX_DIFFERENCE_VALUE - xDiff + (Global.CAM_POSITION.Y / 2));
                VoxelCulling.maxRenderX = (ushort)(maxRenderX < 0 ? 0 : maxRenderX);
            } else maxRenderX = (ushort)(Settings.WORLD_SIZE - 1);

            if (yDiff < -MIN_DIFFERENCE) {
                short minRenderY = (short)(Global.CAM_POSITION.Z - MAX_DIFFERENCE_VALUE - yDiff - (Global.CAM_POSITION.Y / 2));
                VoxelCulling.minRenderY = (ushort)(minRenderY < 0 ? 0 : minRenderY);
            } else minRenderY = 0;

            if (yDiff > MIN_DIFFERENCE) {
                short maxRenderY = (short)(Global.CAM_POSITION.Z + MAX_DIFFERENCE_VALUE - yDiff + (Global.CAM_POSITION.Y / 2));
                VoxelCulling.maxRenderY = (ushort)(maxRenderY < 0 ? 0 : maxRenderY);
            } else maxRenderY = (ushort)(Settings.WORLD_SIZE - 1);
        }

        internal static bool ShouldRender(ushort posX, ushort posY, ushort sizeX, ushort sizeY) {
            return !(posX + sizeX < minRenderX || posX > maxRenderX || posY + sizeY < minRenderY || posY > maxRenderY);
        }

        /// <summary>
        /// used to hide non visible sides of rendered voxels
        /// </summary>
        /// <returns>
        /// front, back, right, left, top, bottom
        /// true = is visible
        /// </returns>
        internal static bool[] GetVisibleSides(ushort posX, ushort posY, ushort posZ, ushort sizeX, ushort sizeY, ushort sizeZ) {
            bool[] output = new bool[6];

            for (ushort x = posX; x < posX + sizeX; x++) {
                if (posZ == 0 || Global.VOXEL_MAP[x, posZ - 1, posY] == null || Global.VOXEL_MAP[x, posZ - 1, posY] == (byte)VoxelType.AIR) {
                    output[0] = true;
                }

                if (posZ + sizeZ == Settings.WORLD_SIZE || Global.VOXEL_MAP[x, posZ + sizeZ, posY] == null || Global.VOXEL_MAP[x, posZ + sizeZ, posY] == (byte)VoxelType.AIR) {
                    output[1] = true;
                }
            }

            for (ushort y = posZ; y < posZ + sizeZ; y++) {
                if (posX == 0 || Global.VOXEL_MAP[posX - 1, y, posY] == null || Global.VOXEL_MAP[posX - 1, y, posY] == (byte)VoxelType.AIR) {
                    output[2] = true;
                }

                if (posX + sizeX == Settings.WORLD_SIZE || Global.VOXEL_MAP[posX + sizeX, y, posY] == null || Global.VOXEL_MAP[posX + sizeX, y, posY] == (byte)VoxelType.AIR) {
                    output[3] = true;
                }
            }

            for (ushort x = posX; x < posX + sizeX; x++) {
                for (ushort y = posZ; y < posZ + sizeZ; y++) {
                    if (posY + sizeY == Settings.HEIGHT_LIMIT || Global.VOXEL_MAP[x, y, posY + sizeY] == null || Global.VOXEL_MAP[x, y, posY + sizeY] == (byte)VoxelType.AIR) {
                        output[4] = true;
                    }

                    if (Global.HEIGHT_MAP[posX, posZ] < posY && (Global.VOXEL_MAP[x, y, posY - 1] == null || Global.VOXEL_MAP[x, y, posY - 1] == (byte)VoxelType.AIR)) {
                        output[5] = true;
                    }
                }
            }

            return output;
        }
    }
}