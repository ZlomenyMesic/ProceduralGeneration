//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using minecraft_kurwa.src.global;

namespace minecraft_kurwa.src.gui.input {
    internal static class KeyboardHandler {
        internal static bool debugMenuStateOpen = true; // also default debug state
        private static bool lastDebugMenuState = false;

        internal static bool HandleKeyboard(ref Vector3 camTarget, ref Vector3 camPosition) {
            KeyboardState keyboard = Keyboard.GetState();
            if (keyboard.IsKeyDown(Controls.EXIT)) return true;

            if (keyboard.IsKeyDown(Controls.DEBUG_MENU)) {
                if (!lastDebugMenuState) {
                    debugMenuStateOpen = !debugMenuStateOpen;
                }
                lastDebugMenuState = true;
            } else lastDebugMenuState = false;

            float speed = 1 / Settings.MOVEMENT_SPEED * 10_000;

            float differenceX = (camTarget.X - camPosition.X) / speed;
            float differenceZ = (camTarget.Z - camPosition.Z) / speed;

            if (keyboard.IsKeyDown(Controls.FORWARD)) {
                camPosition.X += differenceX;
                camTarget.X += differenceX;
                camPosition.Z += differenceZ;
                camTarget.Z += differenceZ;
            }
            if (keyboard.IsKeyDown(Controls.BACKWARD)) {
                camPosition.X -= differenceX;
                camTarget.X -= differenceX;
                camPosition.Z -= differenceZ;
                camTarget.Z -= differenceZ;
            }
            if (keyboard.IsKeyDown(Controls.LEFT)) {
                camPosition.Z -= differenceX;
                camTarget.Z -= differenceX;
                camPosition.X += differenceZ;
                camTarget.X += differenceZ;
            }
            if (keyboard.IsKeyDown(Controls.RIGHT)) {
                camPosition.Z += differenceX;
                camTarget.Z += differenceX;
                camPosition.X -= differenceZ;
                camTarget.X -= differenceZ;
            }
            if (keyboard.IsKeyDown(Controls.UP)) {
                camPosition.Y += 200 / speed;
                camTarget.Y += 200 / speed;
            }
            if (keyboard.IsKeyDown(Controls.DOWN)) {
                camPosition.Y -= 200 / speed;
                camTarget.Y -= 200 / speed;
            }
            return false;
        }
    }
}
