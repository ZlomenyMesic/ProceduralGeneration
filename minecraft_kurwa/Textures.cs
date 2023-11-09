//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace minecraft_kurwa {
    public class Textures {
        public readonly Texture2D WHITE;
        public readonly Texture2D BLACK;
        public readonly Texture2D LIGHT_GREEN;

        public Textures(GraphicsDevice gpu) {
            WHITE = CreateFromColor(Color.White, gpu);
            BLACK = CreateFromColor(Color.Black, gpu);
            LIGHT_GREEN = CreateFromColor(Color.LightGreen, gpu);
        }

        public Texture2D CreateFromColor(Color color, GraphicsDevice gpu) {
            Texture2D texture = new(gpu, 1, 1);
            texture.SetData(new Color[] { color });
            return texture;
        }
    }
}
