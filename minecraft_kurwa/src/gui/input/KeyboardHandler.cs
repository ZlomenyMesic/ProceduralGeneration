//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using minecraft_kurwa.src.global;
using minecraft_kurwa.src.renderer;
using minecraft_kurwa.src.renderer.voxels;
using System;
using System.Diagnostics.Eventing.Reader;

namespace minecraft_kurwa.src.gui.input {
    internal static class KeyboardHandler {
        internal static bool debugMenuStateOpen = true; // also default debug state
        private static bool _lastDebugMenuState = false;

        private static readonly int DEFAULT_FOV = Settings.FIELD_OF_VIEW;

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

            if (keyboard.IsKeyDown(Controls.FORWARD)) UpdateCamera(differenceX, 0, differenceZ);
            if (keyboard.IsKeyDown(Controls.BACKWARD)) UpdateCamera(-differenceX, 0, -differenceZ);
            if (keyboard.IsKeyDown(Controls.LEFT)) UpdateCamera(differenceZ, 0, -differenceX);
            if (keyboard.IsKeyDown(Controls.RIGHT)) UpdateCamera(-differenceZ, 0, differenceX);
            if (keyboard.IsKeyDown(Controls.UP)) UpdateCamera(0, changeY, 0);
            if (keyboard.IsKeyDown(Controls.DOWN)) UpdateCamera(0, -changeY, 0);

            return false;
        }

        private static void UpdateCamera(float camPositionX, float camPositionY, float camPositionZ) {
            if (ExperimentalSettings.HITBOXES) {
                Vector3 pos = Global.CAM_POSITION;
                Vector3 target = Global.CAM_TARGET;
                Vector3 diff = new(camPositionX, camPositionY, camPositionZ);

                float dist = Vector3.Distance(pos, diff);
                int steps = (int)Math.Round(dist / 0.1f, 0) + 1;
                Vector3 step = diff / steps;

                if ((int)pos.X < 0 || (int)pos.X >= Settings.WORLD_SIZE || (int)pos.Y < 0 || (int)pos.Y >= Settings.HEIGHT_LIMIT || (int)pos.Z < 0 || (int)pos.Z >= Settings.WORLD_SIZE || Global.VOXEL_MAP[(int)pos.X, (int)pos.Z, (int)pos.Y] == null) {
                    for (int i = 0; i < steps; i++) {
                        pos += step;
                        target += step;

                        if ((int)pos.X >= 0 && (int)pos.X < Settings.WORLD_SIZE && (int)pos.Y >= 0 && (int)pos.Y < Settings.HEIGHT_LIMIT && (int)pos.Z >= 0 && (int)pos.Z < Settings.WORLD_SIZE && Global.VOXEL_MAP[(int)pos.X, (int)pos.Z, (int)pos.Y] != null) {
                            pos -= step;
                            target -= step;
                            break;
                        }
                    }
                }

                Global.CAM_POSITION = pos;
                Global.CAM_TARGET = target;
            }

            else {
                Vector3 diff = new(camPositionX, camPositionY, camPositionZ);
                Global.CAM_POSITION += diff;
                Global.CAM_TARGET += diff;
            }
        }
    }
}
