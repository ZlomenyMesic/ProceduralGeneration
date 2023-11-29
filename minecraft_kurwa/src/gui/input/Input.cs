//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

using Microsoft.Xna.Framework;

namespace minecraft_kurwa.src.gui.input {
    internal static class Input {
        // returns true if program should exit
        internal static bool Update(ref Vector3 camTarget, ref Vector3 camPosition) {
            MouseHandler.HandleMouse();
            return KeyboardHandler.HandleKeyboard(ref camTarget, ref camPosition);
        }
    }
}
