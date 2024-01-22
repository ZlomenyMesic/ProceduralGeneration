//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using minecraft_kurwa.src.global;
using System;

namespace minecraft_kurwa.src.gui.input;

internal static class MouseHandler {
    internal static float leftRightRot = 0;
    internal static float upDownRot = 0;

    internal static void HandleMouse() {
        Vector2 difference;
        MouseState mouseState = Mouse.GetState();

        difference.X = Settings.WINDOW_WIDTH / 2 - mouseState.X;
        difference.Y = Settings.WINDOW_HEIGHT / 2 - mouseState.Y;
        leftRightRot = Settings.SENSIBILITY * difference.X / 100_000;
        upDownRot = Settings.SENSIBILITY * difference.Y / 400;

        Mouse.SetPosition(Settings.WINDOW_WIDTH / 2, Settings.WINDOW_HEIGHT / 2);

        //int scroll = mouseState.ScrollWheelValue / 100;
        //Settings.MOVEMENT_SPEED = Math.Max(10, Math.Min(75, scroll));
    }
}