//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

namespace minecraft_kurwa.src.renderer.voxels;

internal struct Voxel1 {
    internal ushort posX;
    internal ushort posY;
    internal ushort posZ;

    internal byte sizeX;
    internal byte sizeY;
    internal byte sizeZ;

    internal ushort indexStart;
    internal byte triangles;
    internal byte transparency;

    internal Voxel1(ushort posX, ushort posY, ushort posZ, byte sizeX, byte sizeY, byte sizeZ, ushort indexStart, byte triangles, byte transparency) {
        this.posX = posX;
        this.posY = posY;
        this.posZ = posZ;

        this.sizeX = sizeX;
        this.sizeY = sizeY;
        this.sizeZ = sizeZ;

        this.indexStart = indexStart;
        this.triangles = triangles;
        this.transparency = transparency;
    }
}

internal struct Voxel2 {
    internal byte sizeX;
    internal byte sizeY;
    internal byte sizeZ;

    internal byte? type;

    internal Voxel2(byte sizeX, byte sizeY, byte sizeZ, byte? type) {
        this.sizeX = sizeX;
        this.sizeY = sizeY;
        this.sizeZ = sizeZ;

        this.type = type;
    }
}
