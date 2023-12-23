//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

using Microsoft.Xna.Framework.Input;
using minecraft_kurwa.src.global;
using minecraft_kurwa.src.renderer;
using minecraft_kurwa.src.renderer.voxels;
using System;

namespace minecraft_kurwa.src.gui.input {
    internal static class KeyboardHandler {
        internal static bool debugMenuStateOpen = true; // also default debug state
        private static bool _lastDebugMenuState = false;

        private static readonly int DEFAULT_FOV = Settings.FIELD_OF_VIEW;

        private static readonly Action<float, float, float, float> UpdatePosition = (float camPositionX, float camPositionZ, float camTargetX, float camTargetZ) => {
            Global.CAM_POSITION.X += camPositionX;
            Global.CAM_POSITION.Z += camPositionZ;
            Global.CAM_TARGET.X += camTargetX;
            Global.CAM_TARGET.Z += camTargetZ;
        };

        internal static bool HandleKeyboard() {
            KeyboardState keyboard = Keyboard.GetState();
            if (keyboard.IsKeyDown(Controls.EXIT)) return true;

            if (keyboard.IsKeyDown(Controls.DEBUG_MENU)) {
                if (!_lastDebugMenuState) {
                    debugMenuStateOpen = !debugMenuStateOpen;
                }
                _lastDebugMenuState = true;
            } else _lastDebugMenuState = false;

            if (keyboard.IsKeyDown(Controls.ZOOM_IN)) {
                if (Settings.FIELD_OF_VIEW == DEFAULT_FOV) Settings.FIELD_OF_VIEW -= ExperimentalSettings.ZOOM_LEVEL;
            } else if (Settings.FIELD_OF_VIEW < DEFAULT_FOV) Settings.FIELD_OF_VIEW += ExperimentalSettings.ZOOM_LEVEL;
            Renderer.UpdateProjectionMatrix();

            float speed = Settings.MOVEMENT_SPEED / 10_000f;

            float differenceX = (Global.CAM_TARGET.X - Global.CAM_POSITION.X) * speed;
            float differenceZ = (Global.CAM_TARGET.Z - Global.CAM_POSITION.Z) * speed;
            float changeY = VoxelCulling.MAX_DIFFERENCE_VALUE * speed;

            if (keyboard.IsKeyDown(Controls.FORWARD)) UpdatePosition(differenceX, differenceZ, differenceX, differenceZ);
            if (keyboard.IsKeyDown(Controls.BACKWARD)) UpdatePosition(-differenceX, -differenceZ, -differenceX, -differenceZ);
            if (keyboard.IsKeyDown(Controls.LEFT)) UpdatePosition(differenceZ, -differenceX, differenceZ, -differenceX);
            if (keyboard.IsKeyDown(Controls.RIGHT)) UpdatePosition(-differenceZ, differenceX, -differenceZ, differenceX);

            if (keyboard.IsKeyDown(Controls.UP)) {
                Global.CAM_POSITION.Y += changeY;
                Global.CAM_TARGET.Y += changeY;
            }
            if (keyboard.IsKeyDown(Controls.DOWN)) {
                Global.CAM_POSITION.Y -= changeY;
                Global.CAM_TARGET.Y -= changeY;
            }

            return false;
        }
    }
}
