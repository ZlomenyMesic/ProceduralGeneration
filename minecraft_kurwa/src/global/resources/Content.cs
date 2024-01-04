//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace minecraft_kurwa.src.global.resources;

internal static class Content {
    internal const string rootDirectory = @"Content";

    internal static SpriteFont defaultFont;

    internal static Model skyModel; // hemisphere model (stolen)

    internal static Texture2D customSkyTexture;
    internal static Texture2D josh; // josh


    internal static void Load(ContentManager content) {
        content.RootDirectory = rootDirectory;

        defaultFont = content.Load<SpriteFont>("default");

        josh = content.Load<Texture2D>("josh");

        skyModel = content.Load<Model>(Global.SKY_DOME_MODEL_SOURCE);

        try {
            customSkyTexture = content.Load<Texture2D>(Global.SKY_DOME_TEXTURE_SOURCE);
        } catch { }
    }
}
