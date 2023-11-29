//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using minecraft_kurwa.src.global;
using System;

namespace minecraft_kurwa.src.gui.sky {
    internal static class Sky {
        private static Model model; // hemisphere model (stolen)
        private static float rotation = 0; // dome is slowly rotating
        private static Matrix transform;  // used to adjust starting position of dome
        private static Texture2D customTexture; // custom sky texture

        internal static void Initialize(ContentManager content) {
            transform = Matrix.CreateTranslation(0, -30, 0);
            model = content.Load<Model>(Global.SKY_DOME_MODEL_SOURCE);

            try {
                customTexture = content.Load<Texture2D>(Global.SKY_DOME_TEXTURE_SOURCE);
            }
            catch { }
        }

        internal static void Draw(Matrix projectionMatrix, Matrix viewMatrix) {
            Global.GRAPHICS_DEVICE.RasterizerState = RasterizerState.CullNone;
            Global.GRAPHICS_DEVICE.DepthStencilState = DepthStencilState.None;

            viewMatrix.Translation = Vector3.Zero;

            BasicEffect basicEffect = (BasicEffect)model.Meshes[0].Effects[0];

            if (customTexture != null) basicEffect.Texture = customTexture;
            basicEffect.World = Matrix.CreateFromYawPitchRoll(rotation, (float)-Math.PI / 2, 0) * transform;
            basicEffect.View = viewMatrix;
            basicEffect.Projection = projectionMatrix;

            model.Meshes[0].Draw();

            rotation += Global.SKY_ROTATION_SPEED / 100;
        }
    }
}
