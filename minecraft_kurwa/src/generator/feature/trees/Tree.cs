//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

using minecraft_kurwa.src.renderer.voxels;

namespace minecraft_kurwa.src.generator.feature.trees {
    internal abstract class Tree {
        internal ushort posX;
        internal ushort posY;
        internal ushort posZ;
        internal byte height;

        internal byte leaveType;
        internal byte woodType;

        internal Tree(ushort posX, ushort posY, ushort posZ, byte height, VoxelType leaveType, VoxelType woodType) {
            this.posX = posX; 
            this.posY = posY; 
            this.posZ = posZ;
            this.height = height;

            this.leaveType = (byte)leaveType;
            this.woodType = (byte)woodType;
        }

        internal virtual void Build() { }
    }
}
