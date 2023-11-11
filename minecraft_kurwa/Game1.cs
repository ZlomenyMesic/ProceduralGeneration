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

        // temporary
        internal List<Voxel> voxels;

        internal List<Chunk> world;

        public Game1() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Window.Title = "minecraft?";

            graphics.PreferredBackBufferHeight = Options.WINDOW_HEIGHT;
            graphics.PreferredBackBufferWidth = Options.WINDOW_WIDTH;
            graphics.GraphicsProfile = GraphicsProfile.HiDef;

            IsFixedTimeStep = true;
            TargetElapsedTime = TimeSpan.FromSeconds(1 / Options.UPDATES_PER_SECOND);
        }

        protected override void Initialize() {
            base.Initialize();

            camPosition = new Vector3(0f, 350f, 0f);
            camTarget = new Vector3(0f, 0f, camPosition.Z + 300f);
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(Options.FIELD_OF_VIEW), GraphicsDevice.DisplayMode.AspectRatio, 1f, Options.RENDER_DISTANCE);
            viewMatrix = Matrix.CreateLookAt(camPosition, camTarget, Vector3.Up);

            MainTarget = new RenderTarget2D(GraphicsDevice, Options.WINDOW_WIDTH, Options.WINDOW_HEIGHT, false, GraphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.Depth24);

            spriteBatch = new SpriteBatch(GraphicsDevice);

            Mouse.SetPosition(Options.WINDOW_WIDTH / 2, Options.WINDOW_HEIGHT / 2);

            voxels = new();

            for (int x = 0; x < 100; x++) {
                for (int z = 0; z < 100; z++) {
                    Voxel v = new(GraphicsDevice, new Vector3(x, 0, z), new Color(0, 255, 0));
                    voxels.Add(v);
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

            foreach (var voxel in voxels) {
                voxel.Draw(projectionMatrix, viewMatrix);
            }

            GraphicsDevice.SetRenderTarget(null);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Opaque, SamplerState.LinearWrap, DepthStencilState.None, RasterizerState.CullNone);
            spriteBatch.Draw(MainTarget, new Rectangle(0, 0, Options.WINDOW_WIDTH, Options.WINDOW_HEIGHT), Color.White);
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