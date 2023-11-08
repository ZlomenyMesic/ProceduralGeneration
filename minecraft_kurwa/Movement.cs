//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace minecraft_kurwa {
    static class Movement {
        private static void HandleMouse(Game1 game) {
            Vector2 difference;
            MouseState ms = Mouse.GetState();

            difference.X = (game.windowWidth / 2) - ms.X;
            difference.Y = (game.windowHeight / 2) - ms.Y;
            game.leftRightRot = game.sensibility * difference.X / 100_000;
            game.upDownRot = game.sensibility * difference.Y / 400;

            Mouse.SetPosition(game.windowWidth / 2, game.windowHeight / 2);

            game.UpdateViewMatrix();
        }

        public static void Update(Game1 game) {
            KeyboardState keyboard = Keyboard.GetState();
            float speed = 1 / game.movementSpeed * 10_000;

            HandleMouse(game);

            if (keyboard.IsKeyDown(Keys.Escape)) game.Exit();

            float xDiff = (game.camTarget.X - game.camPosition.X) / speed;
            float zDiff = (game.camTarget.Z - game.camPosition.Z) / speed;
            if (keyboard.IsKeyDown(Keys.W)) {
                game.camPosition.X += xDiff;
                game.camTarget.X += xDiff;
                game.camPosition.Z += zDiff;
                game.camTarget.Z += zDiff;
            }
            if (keyboard.IsKeyDown(Keys.S)) {
                game.camPosition.X -= xDiff;
                game.camTarget.X -= xDiff;
                game.camPosition.Z -= zDiff;
                game.camTarget.Z -= zDiff;
            }
            if (keyboard.IsKeyDown(Keys.A)) {
                game.camPosition.Z -= xDiff;
                game.camTarget.Z -= xDiff;
                game.camPosition.X += zDiff;
                game.camTarget.X += zDiff;
            }
            if (keyboard.IsKeyDown(Keys.D)) {
                game.camPosition.Z += xDiff;
                game.camTarget.Z += xDiff;
                game.camPosition.X -= zDiff;
                game.camTarget.X -= zDiff;
            }
            if (keyboard.IsKeyDown(Keys.Space)) {
                game.camPosition.Y += 200 / speed;
                game.camTarget.Y += 200 / speed;
            }
            if (keyboard.IsKeyDown(Keys.LeftShift)) {
                game.camPosition.Y -= 200 / speed;
                game.camTarget.Y -= 200 / speed;
            }
        }
    }
}
