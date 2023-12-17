//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

using minecraft_kurwa.src.global;
using minecraft_kurwa.src.renderer.voxels;
using System;

namespace minecraft_kurwa.src.generator.feature.tree {
    internal abstract class Tree {
        protected ushort _posX;
        protected ushort _posY;
        protected ushort _posZ;
        protected byte _height;

        protected byte _leaveType;
        protected byte _woodType;

        internal Tree(ushort posX, ushort posY, ushort posZ, byte height, VoxelType leaveType, VoxelType woodType) {
            _posX = posX; 
            _posY = posY;
            _posZ = posZ;
            _height = height;

            _leaveType = (byte)leaveType;
            _woodType = (byte)woodType;
        }

        internal virtual void Build() { }

        protected void BuildBranch(short sX, short sY, short sZ, short eX, short eY, short eZ) {
            float diffX = sX - eX;
            float diffY = sY - eY;
            float diffZ = sZ - eZ;

            float distance = (float)Math.Round(Math.Sqrt(diffX * diffX + diffY * diffY + diffZ * diffZ), 1);
            ushort strides = (ushort)(distance * 2);

            float strideX = (float)Math.Round((double)(diffX / strides), 3);
            float strideY = (float)Math.Round((double)(diffY / strides), 3);
            float strideZ = (float)Math.Round((double)(diffZ / strides), 3);

            diffX = diffY = diffZ = 0;

            for (ushort i = 0; i < strides; i++) {
                if (Math.Round(sX + diffX, 0) < 0 || Math.Round(sY + diffY, 0) < 0 || Math.Round(sZ + diffZ, 0) < 0 || Math.Round(sX + diffX, 0) >= Settings.WORLD_SIZE || Math.Round(sY + diffY, 0) >= Settings.WORLD_SIZE || Math.Round(sZ + diffZ, 0) >= Settings.HEIGHT_LIMIT) break;
                Global.VOXEL_MAP[(int)Math.Round(sX + diffX, 0), (int)Math.Round(sY + diffY, 0), (int)Math.Round(sZ + diffZ, 0)] = _woodType;

                diffX -= strideX;
                diffY -= strideY;
                diffZ -= strideZ;
            }
        }
    }
}
