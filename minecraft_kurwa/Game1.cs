using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace minecraft_kurwa {

    public class Game1 : Game {
        GraphicsDeviceManager graphics;

        Vector3 camTarget;
        Vector3 camPosition;
        Matrix projectionMatrix;
        Matrix viewMatrix;
        Matrix worldMatrix;

        BasicEffect basicEffect;

        VertexPositionColor[] triangleVertices;
        VertexBuffer vertexBuffer;

        // TADY JSOU NEJAKY HODNOTY CO BY SE ASI MELY DAT NASTAVIT PRIMO Z TY APLIKACE
        float fieldOfView = 60f;
        float renderDistance = 1000f;
        float sensibility = 2f;
        // KONEC NASTAVITELNYCH HODNOT

        float leftRightRot = 0f;
        float upDownRot = 0f;

        public Game1() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferHeight = 1400;
            graphics.PreferredBackBufferWidth = 2400;
        }

        protected override void Initialize() {
            base.Initialize();

            camTarget = new Vector3(0f, 0f, 0f);
            camPosition = new Vector3(0f, 0f, -200f);
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(fieldOfView), GraphicsDevice.DisplayMode.AspectRatio, 1f, renderDistance);
            viewMatrix = Matrix.CreateLookAt(camPosition, camTarget, new Vector3(0f, 1f, 0f));
            worldMatrix = Matrix.CreateWorld(camTarget, Vector3.Forward, Vector3.Up);


            // BORDEL TYKAJICI SE BAREVNEHO TROJUHELNIKU (VSECHNO DLE TUTORIALU)
            basicEffect = new BasicEffect(GraphicsDevice);
            basicEffect.Alpha = 1f;
            basicEffect.VertexColorEnabled = true;
            basicEffect.LightingEnabled = false;

            triangleVertices = new VertexPositionColor[3];
            triangleVertices[0] = new VertexPositionColor(new Vector3(0, 20, 0), Color.Red);
            triangleVertices[1] = new VertexPositionColor(new Vector3(-20, -20, 0), Color.Green);
            triangleVertices[2] = new VertexPositionColor(new Vector3(20, -20, 0), Color.Blue);

            vertexBuffer = new VertexBuffer(GraphicsDevice, typeof(VertexPositionColor), 3, BufferUsage.WriteOnly);
            vertexBuffer.SetData<VertexPositionColor>(triangleVertices);
            // KONEC TROJUHELNIKOVEHO BORDELU


            Mouse.SetPosition(1200, 700);
        }

        protected override void LoadContent() {

        }

        protected override void UnloadContent() {

        }

        protected override void Update(GameTime gameTime) {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TADY BUDE NEJAKY POHYB POMOCI TLACITEK
            if (Keyboard.GetState().IsKeyDown(Keys.D)) {
                
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A)) {
                
            }
            if (Keyboard.GetState().IsKeyDown(Keys.W)) {
                
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S)) {
                
            }
            if (Keyboard.GetState().IsKeyDown(Keys.LeftShift)) {
                
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Space)) {
                
            }

            HandleMouse();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            basicEffect.Projection = projectionMatrix;
            basicEffect.View = viewMatrix;
            basicEffect.World = worldMatrix;

            GraphicsDevice.Clear(Color.CornflowerBlue);
            GraphicsDevice.SetVertexBuffer(vertexBuffer);


            // ZACINA BORDEL TYKAJICI SE TROJUHELNIKU
            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            GraphicsDevice.RasterizerState = rasterizerState;

            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes) {
                pass.Apply();
                GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 3);
            }
            // KONCI BORDEL TYKAJICI SE TROJUHELNIKU


            base.Draw(gameTime);
        }

        private void HandleMouse() {
            // TADY TO ZJISTI JAK MOC HEJBU MYSI

            Vector2 difference;
            MouseState ms = Mouse.GetState();

            difference.X = 1200 - ms.X;
            difference.Y = 700 - ms.Y;
            leftRightRot = sensibility * difference.X / 1000;
            upDownRot = sensibility * difference.Y / 4;

            Mouse.SetPosition(1200, 700);

            UpdateViewMatrix();
        }

        private void UpdateViewMatrix() {
            // TADY TO UPDATUJE KAM MIRI KAMERA PODLE TOHO JAK MOC JSEM HEJBAL MYSI

            camTarget = Vector3.Transform(camTarget - camPosition, Matrix.CreateRotationY(leftRightRot)) + camPosition;

            camTarget.Y += upDownRot;
            camTarget.Y = MathHelper.Min(camTarget.Y, 135);
            camTarget.Y = MathHelper.Max(camTarget.Y, -135);

            viewMatrix = Matrix.CreateLookAt(camPosition, camTarget, Vector3.Up);
        }
    }
}