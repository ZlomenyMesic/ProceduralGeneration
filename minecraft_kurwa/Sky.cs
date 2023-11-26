//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace minecraft_kurwa {
    internal static class Sky {
        private static Model model;
        private static float rotation = 0;
        private static Matrix transform;  // used to adjust starting position of dome
        private static Texture2D texture;

        internal static void Initialize(ContentManager content) {
            transform = Matrix.CreateTranslation(0, -20, 0);
            model = content.Load<Model>(Global.SKY_DOME_MODEL_SOURCE);
            texture = content.Load<Texture2D>(Global.SKY_DOME_TEXTURE_SOURCE);
        }

        internal static void Draw(Matrix projectionMatrix, Matrix viewMatrix) {
            Global.GRAPHICS_DEVICE.RasterizerState = RasterizerState.CullNone;
            Global.GRAPHICS_DEVICE.DepthStencilState = DepthStencilState.None;
            
            viewMatrix.Translation = Vector3.Zero;

            for (int i = 0; i < model.Meshes.Count; i++) {
                for (int j = 0; j < model.Meshes[i].Effects.Count; j++) {
                    BasicEffect effect = (BasicEffect)model.Meshes[i].Effects[j];

                    if (i == 0) {
                        effect.Texture = texture;
                        Global.GRAPHICS_DEVICE.BlendState = BlendState.Additive;
                        effect.World = Matrix.CreateFromYawPitchRoll(rotation, (float)-Math.PI / 2, 0) * transform;
                        rotation += 0.002f;
                    } 
                    else effect.World = Matrix.CreateFromYawPitchRoll(0, (float)-Math.PI / 2, 0) * transform;

                    effect.LightingEnabled = false;
                    effect.View = viewMatrix;
                    effect.Projection = projectionMatrix;                  
                    effect.TextureEnabled = true;                   
                }
                model.Meshes[i].Draw();
            }
        }
    }
}
