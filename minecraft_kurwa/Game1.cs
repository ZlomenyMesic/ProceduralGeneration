//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace minecraft_kurwa {

    public class Game1 : Game {
        SpriteBatch spriteBatch;
        GraphicsDeviceManager graphics;

        internal Vector3 camTarget;
        internal Vector3 camPosition;
        internal Matrix projectionMatrix;
        internal Matrix viewMatrix;

        RenderTarget2D MainTarget;

        internal List<Voxel> world;

        public Game1() {
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

            camPosition = new Vector3(0f, 350f, 0f);
            camTarget = new Vector3(0f, 0f, camPosition.Z + 300f);
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(Global.FIELD_OF_VIEW), GraphicsDevice.DisplayMode.AspectRatio, 1f, Global.RENDER_DISTANCE);
            viewMatrix = Matrix.CreateLookAt(camPosition, camTarget, Vector3.Up);

            MainTarget = new RenderTarget2D(GraphicsDevice, Global.WINDOW_WIDTH, Global.WINDOW_HEIGHT, false, GraphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.Depth24);

            spriteBatch = new SpriteBatch(GraphicsDevice);

            Mouse.SetPosition(Global.WINDOW_WIDTH / 2, Global.WINDOW_HEIGHT / 2);

            world = new();

            GeneratorController.GenerateWorld();

            for (int x = 0; x < Global.WORLD_SIZE; x++) {
                for (int y = 0; y < Global.WORLD_SIZE; y++) {
                    for (int z = 0; z < Global.HEIGHT_LIMIT; z++) {
                        if (Global.VOXEL_MAP[x, y, z] != null) {
                            Voxel v = new(GraphicsDevice, new(x, z, y), ColorManager.GetTypeColor(Global.VOXEL_MAP[x, y, z], (BiomeType)Global.BIOME_MAP[x, y], z));
                            world.Add(v);
                        }
                    }
                }
            }
        }

        protected override void Update(GameTime gameTime) {
            Movement.Update(this);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.SetRenderTarget(MainTarget);
            GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1.0f, 0);
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.BlendState = BlendState.AlphaBlend;

            foreach (var voxel in world) {
                voxel.Draw(projectionMatrix, viewMatrix);
            }

            GraphicsDevice.SetRenderTarget(null);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Opaque, SamplerState.LinearWrap, DepthStencilState.None, RasterizerState.CullNone);
            spriteBatch.Draw(MainTarget, new Rectangle(0, 0, Global.WINDOW_WIDTH, Global.WINDOW_HEIGHT), Color.White);
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