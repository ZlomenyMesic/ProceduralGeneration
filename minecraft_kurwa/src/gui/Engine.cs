//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using minecraft_kurwa.src.generator;
using minecraft_kurwa.src.global;
using minecraft_kurwa.src.gui.input;
using minecraft_kurwa.src.renderer.sky;
using minecraft_kurwa.src.renderer.voxels;
using System;
using minecraft_kurwa.src.renderer;

namespace minecraft_kurwa.src.gui {

    public class Engine : Game {
        internal GraphicsDeviceManager graphics;

        public Engine() {
            Time.Initialize();

            graphics = new GraphicsDeviceManager(this);
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

            global.resources.Content.Load(Content);

            Renderer.Initialize();

            VoxelStructure.basicEffect = new(Global.GRAPHICS_DEVICE) {
                VertexColorEnabled = true
            };

            WorldGenerator.GenerateWorld();

            VoxelConnector.CreateGrid();
            VoxelConnector.GenerateWorld();

            Mouse.SetPosition(Settings.WINDOW_WIDTH / 2, Settings.WINDOW_HEIGHT / 2);
        }

        protected override void Update(GameTime gameTime) {
            if (Input.Update()) Exit(); // exit if Controls.EXIT is pressed

            Renderer.UpdateViewMatrix();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            VoxelCulling.UpdateRenderCoordinates();

            Renderer.Draw();

            Time.Update();

            base.Draw(gameTime);
        }
    }
}