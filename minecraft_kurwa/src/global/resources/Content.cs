//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using minecraft_kurwa.src.renderer;
using minecraft_kurwa.src.renderer.sky;

namespace minecraft_kurwa.src.global.resources {
    internal static class Content {
        internal const string rootDirectory = @"Content";

        internal static void Load(ContentManager content) {
            content.RootDirectory = rootDirectory;

            Renderer.defaultFont = content.Load<SpriteFont>("default");

            Sky.model = content.Load<Model>(Global.SKY_DOME_MODEL_SOURCE);

            try {
                Sky.customTexture = content.Load<Texture2D>(Global.SKY_DOME_TEXTURE_SOURCE);
            } catch { }
        }
    }
}
