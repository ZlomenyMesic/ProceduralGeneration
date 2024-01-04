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
using minecraft_kurwa.src.renderer.voxels;
using minecraft_kurwa.src.renderer;
using minecraft_kurwa.src.renderer.view;
using System;

namespace minecraft_kurwa.src.gui;

public class Engine : Game {
    private readonly GraphicsDeviceManager _graphics;

    public Engine() {
        Time.UpdateLoadTime();
        _graphics = new GraphicsDeviceManager(this);
        Window.Title = "minecraft?";

        _graphics.PreferredBackBufferHeight = Settings.WINDOW_HEIGHT;
        _graphics.PreferredBackBufferWidth = Settings.WINDOW_WIDTH;
        _graphics.GraphicsProfile = GraphicsProfile.HiDef;

        IsFixedTimeStep = true;
        TargetElapsedTime = TimeSpan.FromSeconds(1 / Global.UPDATES_PER_SECOND);

        if (ExperimentalSettings.RANDOM_SEED) Settings.SEED = new Random().Next(int.MinValue, int.MaxValue);
        Global.RANDOM = new(Settings.SEED);
    }

    protected override void Initialize() {
        base.Initialize();

        Global.GRAPHICS_DEVICE = GraphicsDevice;

        global.resources.Content.Load(Content);

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

        Time.UpdateLoadTime();

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime) {
        VoxelCulling.Update();
        View.Update();

        Renderer.Draw();

        Time.UpdateFPS();

        base.Draw(gameTime);
    }
}