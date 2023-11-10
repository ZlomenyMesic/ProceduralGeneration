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

        public Vector3 camTarget;
        public Vector3 camPosition;
        public Matrix projectionMatrix;
        public Matrix viewMatrix;

        RenderTarget2D MainTarget;

        public List<Voxel> voxels;

        // CUSTOMIZABLE VARIABLES
        readonly public int windowHeight = 1400;
        readonly public int windowWidth = 2400;
        readonly public float fieldOfView = 60; // in degrees
        readonly public float renderDistance = 20_000;
        readonly public float sensibility = 200; // higher value => faster mouse
        readonly public float movementSpeed = 150; // higher value => faster movement
        readonly public float updatesPerSecond = 60; // how many times does Update() get called

        public float leftRightRot = 0f;
        public float upDownRot = 0f;

        public Game1() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferHeight = windowHeight;
            graphics.PreferredBackBufferWidth = windowWidth;
            graphics.GraphicsProfile = GraphicsProfile.HiDef;

            IsFixedTimeStep = true;
            TargetElapsedTime = TimeSpan.FromSeconds(1 / updatesPerSecond);
        }

        protected override void Initialize() {
            base.Initialize();

            camPosition = new Vector3(0f, 350f, 0f);
            camTarget = new Vector3(0f, 0f, camPosition.Z + 300f);
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(fieldOfView), GraphicsDevice.DisplayMode.AspectRatio, 1f, renderDistance);
            viewMatrix = Matrix.CreateLookAt(camPosition, camTarget, Vector3.Up);

            MainTarget = new RenderTarget2D(GraphicsDevice, windowWidth, windowHeight, false, GraphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.Depth24);

            spriteBatch = new SpriteBatch(GraphicsDevice);

            Mouse.SetPosition(windowWidth / 2, windowHeight / 2);

            voxels = new();

            for (int x = 0; x < 200; x++) {
                for (int z = 0; z < 200; z++) {
                    Voxel v = new(GraphicsDevice, new Vector3(x, new Random().Next(0, 2), z), Color.Green);
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

            foreach (var voxel in voxels) {
                voxel.Draw(projectionMatrix, viewMatrix);
            }

            GraphicsDevice.SetRenderTarget(null);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Opaque, SamplerState.LinearWrap, DepthStencilState.None, RasterizerState.CullNone);
            spriteBatch.Draw(MainTarget, new Rectangle(0, 0, windowWidth, windowHeight), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        public void UpdateViewMatrix() {
            camTarget = Vector3.Transform(camTarget - camPosition, Matrix.CreateRotationY(leftRightRot)) + camPosition;

            camTarget.Y += upDownRot;
            camTarget.Y = MathHelper.Min(camTarget.Y, camPosition.Y + 600);
            camTarget.Y = MathHelper.Max(camTarget.Y, camPosition.Y - 600);

            viewMatrix = Matrix.CreateLookAt(camPosition, camTarget, Vector3.Up);
        }
    }
}