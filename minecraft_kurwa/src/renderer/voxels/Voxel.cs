//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

namespace minecraft_kurwa.src.renderer.voxels {
    internal struct Voxel1 {
        internal ushort posX;
        internal ushort posY;
        internal ushort posZ;

        internal ushort sizeX;
        internal ushort sizeY;
        internal ushort sizeZ;

        internal ushort indexStart;
        internal byte transparency;

        internal Voxel1(ushort posX, ushort posY, ushort posZ, ushort sizeX, ushort sizeY, ushort sizeZ, ushort indexStart, byte transparency) {
            this.posX = posX;
            this.posY = posY;
            this.posZ = posZ;

            this.sizeX = sizeX;
            this.sizeY = sizeY;
            this.sizeZ = sizeZ;

            this.indexStart = indexStart;
            this.transparency = transparency;
        }
    }

    internal struct Voxel2 {
        internal ushort sizeX;
        internal ushort sizeY;
        internal ushort sizeZ;

        internal byte? type;

        internal Voxel2(ushort sizeX, ushort sizeY, ushort sizeZ, byte? type) {
            this.sizeX = sizeX;
            this.sizeY = sizeY;
            this.sizeZ = sizeZ;

            this.type = type;
        }
    }
}
