//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using minecraft_kurwa.src.generator.terrain.biomes;
using minecraft_kurwa.src.global;
using minecraft_kurwa.src.gui;
using minecraft_kurwa.src.gui.input;
using minecraft_kurwa.src.renderer.view;
using minecraft_kurwa.src.renderer.sky;
using minecraft_kurwa.src.renderer.voxels;
using System;

namespace minecraft_kurwa.src.renderer {
    internal static class Renderer {
        internal static SpriteBatch spriteBatch;
        internal static SpriteFont defaultFont;

        internal static RenderTarget2D renderTarget;

        internal static Matrix PROJECTION_MATRIX;
        internal static Matrix VIEW_MATRIX;

        internal static void Initialize() {
            Global.GRAPHICS_DEVICE.BlendState = BlendState.AlphaBlend;
            Global.GRAPHICS_DEVICE.RasterizerState = RasterizerState.CullCounterClockwise;

            Global.CAM_POSITION = new Vector3(Settings.WORLD_SIZE / 2, 100, Settings.WORLD_SIZE / 2);
            Global.CAM_TARGET = new Vector3(Global.CAM_POSITION.X + VoxelCulling.defaultCTPosition.X, Global.CAM_POSITION.Y + VoxelCulling.defaultCTPosition.Z, Global.CAM_POSITION.Z + VoxelCulling.defaultCTPosition.Y);
            PROJECTION_MATRIX = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(Settings.FIELD_OF_VIEW), Global.GRAPHICS_DEVICE.DisplayMode.AspectRatio * ExperimentalSettings.ASPECT_RATIO, ExperimentalSettings.ANTI_RENDER_DISTANCE, Settings.RENDER_DISTANCE);
            VIEW_MATRIX = Matrix.CreateLookAt(Global.CAM_POSITION, Global.CAM_TARGET, Vector3.Up);

            spriteBatch = new SpriteBatch(Global.GRAPHICS_DEVICE);
            renderTarget = new RenderTarget2D(Global.GRAPHICS_DEVICE, ExperimentalSettings.RENDER_TARGET_WIDTH, ExperimentalSettings.RENDER_TARGET_HEIGHT, false, Global.GRAPHICS_DEVICE.PresentationParameters.BackBufferFormat, DepthFormat.Depth24);
        }

        internal static void UpdateViewMatrix() {
            Global.CAM_TARGET = Vector3.Transform(Global.CAM_TARGET - Global.CAM_POSITION, Matrix.CreateRotationY(MouseHandler.leftRightRot)) + Global.CAM_POSITION;

            Global.CAM_TARGET.Y += MouseHandler.upDownRot;
            Global.CAM_TARGET.Y = MathHelper.Min(Global.CAM_TARGET.Y, Global.CAM_POSITION.Y + 600);
            Global.CAM_TARGET.Y = MathHelper.Max(Global.CAM_TARGET.Y, Global.CAM_POSITION.Y - 600);

            VIEW_MATRIX = Matrix.CreateLookAt(Global.CAM_POSITION, Global.CAM_TARGET, Vector3.Up);
        }

        internal static void Draw() {
            // create a render target
            Global.GRAPHICS_DEVICE.SetRenderTarget(renderTarget);
            Global.GRAPHICS_DEVICE.Clear(0, Color.Black, 1.0f, 0);

            // sky is a part of the render target
            Sky.Draw();

            Global.GRAPHICS_DEVICE.DepthStencilState = DepthStencilState.Default;

            VoxelStructure.basicEffect.Projection = PROJECTION_MATRIX;
            VoxelStructure.basicEffect.View = VIEW_MATRIX;

            // add blocks to the render target
            if (Global.CAM_POSITION.X < 0 || Global.CAM_POSITION.X >= Settings.WORLD_SIZE || Global.CAM_POSITION.Z < 0 || Global.CAM_POSITION.Z >= Settings.WORLD_SIZE || Global.CAM_POSITION.Y >= Global.HEIGHT_MAP[(int)Global.CAM_POSITION.X, (int)Global.CAM_POSITION.Z] + 1) {
                for (int i = 0; i < VoxelConnector.world.Length; i++) {
                    VoxelConnector.world[i]?.Draw();
                }
            }

            Global.GRAPHICS_DEVICE.SetRenderTarget(null);

            // draw render target
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Opaque, SamplerState.LinearWrap, DepthStencilState.None, RasterizerState.CullNone);
            spriteBatch.Draw(renderTarget, new Rectangle(0, 0, Settings.WINDOW_WIDTH, Settings.WINDOW_HEIGHT), Color.White);
            spriteBatch.End();

            spriteBatch.Begin();
            if (KeyboardHandler.debugMenuStateOpen) spriteBatch.DrawString(defaultFont,
                $"Camera position:\n" +
                $"X: {Global.CAM_POSITION.X}\n" +
                $"Y: {Global.CAM_POSITION.Y}\n" +
                $"Z: {Global.CAM_POSITION.Z}\n" +
                $"Camera rotation: {Math.Round(View.VIEW_ROTATION, 0)} deg\n" +
                $"Biome: {Biome.GetBiome((ushort)Global.CAM_POSITION.X, (ushort)Global.CAM_POSITION.Z)}\n" +
                $"Subbiome: {Biome.GetSubbiome((ushort)Global.CAM_POSITION.X, (ushort)Global.CAM_POSITION.Z)}\n" +
                $"Secondary biome: {Biome.GetSecondaryBiome((ushort)Global.CAM_POSITION.X, (ushort)Global.CAM_POSITION.Z)}\n" +
                $"Tertiary biome: {Biome.GetTertiaryBiome((ushort)Global.CAM_POSITION.X, (ushort)Global.CAM_POSITION.Z)}\n" +
                $"Biomeblending: {Biome.GetBiomeBlending((ushort)Global.CAM_POSITION.X, (ushort)Global.CAM_POSITION.Z)}\n" +
                $"World size: {Settings.WORLD_SIZE}\n\n" +
                $"Generated in: {Time.LoadTime} ms\n" +
                $"Seed: {Settings.SEED}\n" +
                $"Voxels: {VoxelConnector.voxelCounter}\n" +
                $"Triangles: {VoxelStructure.triangleCounter}\n" +
                $"Frames per second: {Time.LastFPS}",
                new Vector2(30, 30), Color.White);
            spriteBatch.End();
        }
    }
}
