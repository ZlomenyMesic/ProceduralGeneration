//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

namespace minecraft_kurwa {
    internal static class Options {
        internal const int WINDOW_HEIGHT = 1400;
        internal const int WINDOW_WIDTH = 2400;

        internal const float FIELD_OF_VIEW = 60; // in degrees
        internal const float RENDER_DISTANCE = 2000;

        internal const float SENSIBILITY = 200; // higher value => faster mouse
        internal const float MOVEMENT_SPEED = 100; // higher value => faster movement

        internal const float UPDATES_PER_SECOND = 60; // how many times does Update() get called

        internal const int CHUNK_SIZE = 16;
        internal const int HEIGHT_LIMIT = 128;
    }
}
