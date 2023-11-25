//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;

namespace minecraft_kurwa {

    public class Engine : Game {
        private readonly Stopwatch loadTime; // how much time did it take to generate the terrain and startup the application
        private readonly Stopwatch totalTime; // time since the application was started
        private uint frames; // frames rendered since the application was started

        internal SpriteBatch spriteBatch; // TODO
        internal GraphicsDeviceManager graphics; // TODO

        internal Vector3 camTarget; // position the camera is pointed to
        internal Vector3 camPosition; // camera position

        internal Matrix projectionMatrix; // TODO
        internal Matrix viewMatrix; // TODO

        internal RenderTarget2D renderTarget; // TODO

        internal SpriteFont defaultFont; // font

        private bool debugMenuOpen = true;

        public Engine() {
            loadTime = new();
            totalTime = new();
            loadTime.Start();

            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Window.Title = "minecraft?";

            graphics.PreferredBackBufferHeight = Settings.WINDOW_HEIGHT;
            graphics.PreferredBackBufferWidth = Settings.WINDOW_WIDTH;
            graphics.GraphicsProfile = GraphicsProfile.HiDef;

            IsFixedTimeStep = true;
            TargetElapsedTime = TimeSpan.FromSeconds(1 / Global.UPDATES_PER_SECOND);
        }

        protected override void Initialize() {
            base.Initialize();

            Global.GRAPHICS_DEVICE = GraphicsDevice;

            camPosition = new Vector3(Global.START_POS_X, Global.START_POS_Y, Global.START_POS_Z);
            camTarget = new Vector3(camPosition.X, camPosition.Y - 350, camPosition.Z + 300f);
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(Settings.FIELD_OF_VIEW), Global.GRAPHICS_DEVICE.DisplayMode.AspectRatio * ExperimentalSettings.ASPECT_RATIO, ExperimentalSettings.ANTI_RENDER_DISTANCE, Settings.RENDER_DISTANCE);
            viewMatrix = Matrix.CreateLookAt(camPosition, camTarget, Vector3.Up);

            renderTarget = new RenderTarget2D(Global.GRAPHICS_DEVICE, ExperimentalSettings.RENDER_TARGET_WIDTH, ExperimentalSettings.RENDER_TARGET_HEIGHT, false, Global.GRAPHICS_DEVICE.PresentationParameters.BackBufferFormat, DepthFormat.Depth24);

            spriteBatch = new SpriteBatch(Global.GRAPHICS_DEVICE);

            defaultFont = Content.Load<SpriteFont>("default");

            Mouse.SetPosition(Settings.WINDOW_WIDTH / 2, Settings.WINDOW_HEIGHT / 2);

            VoxelStructure.basicEffect = new(Global.GRAPHICS_DEVICE) {
                VertexColorEnabled = true
            };

            GeneratorController.GenerateWorld();

            VoxelConnector.CreateGrid();
            VoxelConnector.GenerateWorld();

            totalTime.Start();
        }

        protected override void Update(GameTime gameTime) {
            if (Input.Update(ref camTarget, ref camPosition, ref debugMenuOpen)) Exit();
            UpdateViewMatrix();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            Global.GRAPHICS_DEVICE.SetRenderTarget(renderTarget);
            Global.GRAPHICS_DEVICE.Clear(0, Color.Black, 1.0f, 0);
            Global.GRAPHICS_DEVICE.DepthStencilState = DepthStencilState.Default;

            VoxelStructure.basicEffect.Projection = projectionMatrix;
            VoxelStructure.basicEffect.View = viewMatrix;

            for (int i = 0; i < VoxelConnector.world.Length; i++) {
                VoxelConnector.world[i]?.Draw();
            }

            Global.GRAPHICS_DEVICE.SetRenderTarget(null);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Opaque, SamplerState.LinearWrap, DepthStencilState.None, RasterizerState.CullNone);
            spriteBatch.Draw(renderTarget, new Rectangle(0, 0, Settings.WINDOW_WIDTH, Settings.WINDOW_HEIGHT), Color.White);
            spriteBatch.End();

            if (loadTime.IsRunning) loadTime.Stop();

            spriteBatch.Begin();
            if (debugMenuOpen) spriteBatch.DrawString(defaultFont,
                $"Camera position:\n" +
                $"X: {camPosition.X}\n" +
                $"Y: {camPosition.Y}\n" +
                $"Z: {camPosition.Z}\n" +
                $"Biome: {Biome.GetBiome((ushort)camPosition.X, (ushort)camPosition.Z)}\n" +
                $"Subbiome: {Biome.GetSubbiome((ushort)camPosition.X, (ushort)camPosition.Z)}\n" +
                $"Secondary biome: {Biome.GetSecondaryBiome((ushort)camPosition.X, (ushort)camPosition.Z)}\n" +
                $"Biomeblending: {Biome.GetBiomeBlending((ushort)camPosition.X, (ushort)camPosition.Z)}\n" +
                $"World size: {Settings.WORLD_SIZE}\n\n" +
                $"Generated in: {loadTime.ElapsedMilliseconds} ms\n" +
                $"Seed: {Settings.SEED}\n" +
                $"Voxels: {VoxelConnector.voxelCounter}\n" +
                $"Triangles: {VoxelStructure.triangleCounter}\n" +
                $"Frames per second: {Math.Round((double)(++frames * 1000) / totalTime.ElapsedMilliseconds, 0)}",
                new(30, 30), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        internal void UpdateViewMatrix() {
            camTarget = Vector3.Transform(camTarget - camPosition, Matrix.CreateRotationY(Input.leftRightRot)) + camPosition;

            camTarget.Y += Input.upDownRot;
            camTarget.Y = MathHelper.Min(camTarget.Y, camPosition.Y + 600);
            camTarget.Y = MathHelper.Max(camTarget.Y, camPosition.Y - 600);

            viewMatrix = Matrix.CreateLookAt(camPosition, camTarget, Vector3.Up);
        }
    }
}