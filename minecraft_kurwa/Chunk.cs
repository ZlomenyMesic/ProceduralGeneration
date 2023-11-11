//
// minecraft_kurwa
// ZlomenyMesic
//

using Microsoft.Xna.Framework;

namespace minecraft_kurwa {
    internal class Chunk {
        internal Vector3 position;
        internal Voxel[] blocks;
        private int blockCounter = 0;

        internal Chunk(Vector3 position) {
            this.position = position;
            blocks = new Voxel[Options.CHUNK_SIZE * Options.HEIGHT_LIMIT];
        }
    }
}
