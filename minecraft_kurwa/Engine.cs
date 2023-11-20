//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace minecraft_kurwa {

    public class Engine : Game {
        private readonly Stopwatch stopWatch;

        internal SpriteBatch spriteBatch;
        internal GraphicsDeviceManager graphics;

        internal Vector3 camTarget;
        internal Vector3 camPosition;

        internal Matrix projectionMatrix;
        internal Matrix viewMatrix;

        internal RenderTarget2D MainTarget;

        internal SpriteFont defaultFont;

        internal Voxel[] world;
        internal int voxelCount = 0;

        public Engine() {
            stopWatch = new();
            stopWatch.Start();

            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Window.Title = "minecraft?";

            graphics.PreferredBackBufferHeight = Global.WINDOW_HEIGHT;
            graphics.PreferredBackBufferWidth = Global.WINDOW_WIDTH;
            graphics.GraphicsProfile = GraphicsProfile.HiDef;

            IsFixedTimeStep = true;
            TargetElapsedTime = TimeSpan.FromSeconds(1 / Global.UPDATES_PER_SECOND);
        }

        protected override void Initialize() {
            base.Initialize();

            camPosition = new Vector3(Global.START_POS_X, Global.START_POS_Y, Global.START_POS_Z);
            camTarget = new Vector3(camPosition.X, camPosition.Y - 350f, camPosition.Z + 300f);
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(Global.FIELD_OF_VIEW), GraphicsDevice.DisplayMode.AspectRatio, 1f, Global.RENDER_DISTANCE);
            viewMatrix = Matrix.CreateLookAt(camPosition, camTarget, Vector3.Up);

            MainTarget = new RenderTarget2D(GraphicsDevice, Global.WINDOW_WIDTH, Global.WINDOW_HEIGHT, false, GraphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.Depth24);

            spriteBatch = new SpriteBatch(GraphicsDevice);

            defaultFont = Content.Load<SpriteFont>("default");

            Mouse.SetPosition(Global.WINDOW_WIDTH / 2, Global.WINDOW_HEIGHT / 2);

            Voxel.basicEffect = new(GraphicsDevice) {
                VertexColorEnabled = true
            };

            world = new Voxel[Global.WORLD_SIZE * Global.WORLD_SIZE * Global.HEIGHT_LIMIT];

            GeneratorController.GenerateWorld();

            for (int x = 0; x < Global.WORLD_SIZE; x++) {
                for (int y = 0; y < Global.WORLD_SIZE; y++) {
                    for (int z = 0; z < Global.HEIGHT_LIMIT; z++) {
                        if (Global.VOXEL_MAP[x, y, z] != null) {
                            Voxel v = new(GraphicsDevice, new(x, z, y), ColorManager.GetVoxelColor(Global.VOXEL_MAP[x, y, z], (BiomeType)Global.BIOME_MAP[x, y], z, x * y * z), ColorManager.GetVoxelTransparency(Global.VOXEL_MAP[x, y, z]));
                            world[voxelCount++] = v;
                        }
                    }
                }
            }
        }

        protected override void Update(GameTime gameTime) {
            if (Movement.Update(ref camTarget, ref camPosition)) Exit();
            UpdateViewMatrix();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.SetRenderTarget(MainTarget);
            GraphicsDevice.Clear(0, Color.Black, 1.0f, 0);
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.BlendState = BlendState.AlphaBlend;

            for (int i = 0; i < voxelCount; i++) {
                world[i].Draw(projectionMatrix, viewMatrix);
            }

            GraphicsDevice.SetRenderTarget(null);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Opaque, SamplerState.LinearWrap, DepthStencilState.None, RasterizerState.CullNone);
            spriteBatch.Draw(MainTarget, new Rectangle(0, 0, Global.WINDOW_WIDTH, Global.WINDOW_HEIGHT), Color.White);
            spriteBatch.End();

            if (stopWatch.IsRunning) stopWatch.Stop();

            spriteBatch.Begin();
            spriteBatch.DrawString(defaultFont,
                $"Camera position:\n" +
                $"X: {camPosition.X}\n" +
                $"Y: {camPosition.Y}\n" +
                $"Z: {camPosition.Z}\n" +
                $"Biome: {Biome.GetBiome((int)camPosition.X, (int)camPosition.Z)}\n" +
                $"Subbiome: {Biome.GetSubbiome((int)camPosition.X, (int)camPosition.Z)}\n\n" +
                $"Generated in: {stopWatch.ElapsedMilliseconds} ms\n" +
                $"Seed: {Global.SEED}\n" +
                $"Triangles: {Voxel.triangleCounter}",
                new(30, 30), Color.White); ;
            spriteBatch.End();

            base.Draw(gameTime);
        }

        internal void UpdateViewMatrix() {
            camTarget = Vector3.Transform(camTarget - camPosition, Matrix.CreateRotationY(Movement.leftRightRot)) + camPosition;

            camTarget.Y += Movement.upDownRot;
            camTarget.Y = MathHelper.Min(camTarget.Y, camPosition.Y + 600);
            camTarget.Y = MathHelper.Max(camTarget.Y, camPosition.Y - 600);

            viewMatrix = Matrix.CreateLookAt(camPosition, camTarget, Vector3.Up);
        }
    }
}