//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using minecraft_kurwa.src.global;
using minecraft_kurwa.src.global.resources;
using System;

namespace minecraft_kurwa.src.renderer.sky;

internal static class Sky {
    private static Matrix transform = Matrix.CreateTranslation(0, -30, 0);  // used to adjust starting position of dome

    private static float rotation = 0; // slow rotation

    internal static void Draw() {
        Global.GRAPHICS_DEVICE.RasterizerState = RasterizerState.CullNone;
        Global.GRAPHICS_DEVICE.DepthStencilState = DepthStencilState.None;

        Matrix viewMatrix = Renderer.VIEW_MATRIX;
        viewMatrix.Translation = Vector3.Zero;

        BasicEffect basicEffect = (BasicEffect)Content.skyModel.Meshes[0].Effects[0];

        if (Content.customSkyTexture != null) basicEffect.Texture = Content.customSkyTexture;
        basicEffect.World = Matrix.CreateFromYawPitchRoll(rotation, (float)-Math.PI / 2, 0) * transform;
        basicEffect.Projection = Renderer.PROJECTION_MATRIX;
        basicEffect.View = viewMatrix;

        Content.skyModel.Meshes[0].Draw();

        rotation += Global.SKY_ROTATION_SPEED / 100;
    }
}
