//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace minecraft_kurwa {
    internal static class Input {
        internal static float leftRightRot = 0;
        internal static float upDownRot = 0;

        private static bool lastDebugModeState = false;

        internal static bool Update(ref Vector3 camTarget, ref Vector3 camPosition, ref bool debugMenuOpen) {
            HandleMouse();
            return HandleKeyboard(ref camTarget, ref camPosition, ref debugMenuOpen);
        }

        private static void HandleMouse() {
            Vector2 difference;
            MouseState mouseState = Mouse.GetState();

            difference.X = (Global.WINDOW_WIDTH / 2) - mouseState.X;
            difference.Y = (Global.WINDOW_HEIGHT / 2) - mouseState.Y;
            leftRightRot = Global.SENSIBILITY * difference.X / 100_000;
            upDownRot = Global.SENSIBILITY * difference.Y / 400;

            Mouse.SetPosition(Global.WINDOW_WIDTH / 2, Global.WINDOW_HEIGHT / 2);
        }

        private static bool HandleKeyboard(ref Vector3 camTarget, ref Vector3 camPosition, ref bool debugMenuOpen) {
            KeyboardState keyboard = Keyboard.GetState();
            if (keyboard.IsKeyDown(Keys.Escape)) return true;

            if (keyboard.IsKeyDown(Keys.E)) {
                if (!lastDebugModeState) {
                    debugMenuOpen = !debugMenuOpen;
                }
                lastDebugModeState = true;
            } else lastDebugModeState = false;

            float speed = 1 / Global.MOVEMENT_SPEED * 10_000;

            float differenceX = (camTarget.X - camPosition.X) / speed;
            float differenceZ = (camTarget.Z - camPosition.Z) / speed;

            if (keyboard.IsKeyDown(Keys.W)) {
                camPosition.X += differenceX;
                camTarget.X += differenceX;
                camPosition.Z += differenceZ;
                camTarget.Z += differenceZ;
            }
            if (keyboard.IsKeyDown(Keys.S)) {
                camPosition.X -= differenceX;
                camTarget.X -= differenceX;
                camPosition.Z -= differenceZ;
                camTarget.Z -= differenceZ;
            }
            if (keyboard.IsKeyDown(Keys.A)) {
                camPosition.Z -= differenceX;
                camTarget.Z -= differenceX;
                camPosition.X += differenceZ;
                camTarget.X += differenceZ;
            }
            if (keyboard.IsKeyDown(Keys.D)) {
                camPosition.Z += differenceX;
                camTarget.Z += differenceX;
                camPosition.X -= differenceZ;
                camTarget.X -= differenceZ;
            }
            if (keyboard.IsKeyDown(Keys.Space)) {
                camPosition.Y += 200 / speed;
                camTarget.Y += 200 / speed;
            }
            if (keyboard.IsKeyDown(Keys.LeftShift)) {
                camPosition.Y -= 200 / speed;
                camTarget.Y -= 200 / speed;
            }
            return false;
        }
    }
}
