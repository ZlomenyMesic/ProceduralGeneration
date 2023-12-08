﻿//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

using Microsoft.Xna.Framework.Input;
using minecraft_kurwa.src.global;

namespace minecraft_kurwa.src.gui.input {
    internal static class KeyboardHandler {
        internal static bool debugMenuStateOpen = true; // also default debug state
        private static bool lastDebugMenuState = false;

        internal static bool HandleKeyboard() {
            KeyboardState keyboard = Keyboard.GetState();
            if (keyboard.IsKeyDown(Controls.EXIT)) return true;

            if (keyboard.IsKeyDown(Controls.DEBUG_MENU)) {
                if (!lastDebugMenuState) {
                    debugMenuStateOpen = !debugMenuStateOpen;
                }
                lastDebugMenuState = true;
            } else lastDebugMenuState = false;

            float speed = 1f / Settings.MOVEMENT_SPEED * 10_000f;

            float differenceX = (Global.CAM_TARGET.X - Global.CAM_POSITION.X) / (float)speed;
            float differenceZ = (Global.CAM_TARGET.Z - Global.CAM_POSITION.Z) / (float)speed;

            if (keyboard.IsKeyDown(Controls.FORWARD)) {
                Global.CAM_POSITION.X += differenceX;
                Global.CAM_TARGET.X += differenceX;
                Global.CAM_POSITION.Z += differenceZ;
                Global.CAM_TARGET.Z += differenceZ;
            }
            if (keyboard.IsKeyDown(Controls.BACKWARD)) {
                Global.CAM_POSITION.X -= differenceX;
                Global.CAM_TARGET.X -= differenceX;
                Global.CAM_POSITION.Z -= differenceZ;
                Global.CAM_TARGET.Z -= differenceZ;
            }
            if (keyboard.IsKeyDown(Controls.LEFT)) {
                Global.CAM_POSITION.Z -= differenceX;
                Global.CAM_TARGET.Z -= differenceX;
                Global.CAM_POSITION.X += differenceZ;
                Global.CAM_TARGET.X += differenceZ;
            }
            if (keyboard.IsKeyDown(Controls.RIGHT)) {
                Global.CAM_POSITION.Z += differenceX;
                Global.CAM_TARGET.Z += differenceX;
                Global.CAM_POSITION.X -= differenceZ;
                Global.CAM_TARGET.X -= differenceZ;
            }
            if (keyboard.IsKeyDown(Controls.UP)) {
                Global.CAM_POSITION.Y += 200 / speed;
                Global.CAM_TARGET.Y += 200 / speed;
            }
            if (keyboard.IsKeyDown(Controls.DOWN)) {
                Global.CAM_POSITION.Y -= 200 / speed;
                Global.CAM_TARGET.Y -= 200 / speed;
            }
            return false;
        }
    }
}
