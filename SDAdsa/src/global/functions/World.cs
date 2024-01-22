//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

namespace minecraft_kurwa.src.global.functions {
    internal static class World {
        internal static bool IsInRange(float x)
            => x >= 0 && x < Settings.WORLD_SIZE;

        internal static bool IsInRange(float x, float y)
            => x >= 0 && y >= 0 && x < Settings.WORLD_SIZE && y < Settings.WORLD_SIZE;

        internal static bool IsInRange(float x, float y, float z)
            => x >= 0 && y >= 0 && z >= 0 && x < Settings.WORLD_SIZE && y < Settings.WORLD_SIZE && z < Settings.HEIGHT_LIMIT;
    }
}
