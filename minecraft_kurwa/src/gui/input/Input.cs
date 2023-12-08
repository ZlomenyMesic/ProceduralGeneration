//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

using Microsoft.Xna.Framework;

namespace minecraft_kurwa.src.gui.input {
    internal static class Input {
        // returns true if program should exit
        internal static bool Update() {
            MouseHandler.HandleMouse();
            return KeyboardHandler.HandleKeyboard();
        }
    }
}
