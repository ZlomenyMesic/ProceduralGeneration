//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace minecraft_kurwa {
    internal class Voxel {
        internal readonly Color color;
        internal readonly float transparency;

        private readonly Matrix transform;

        private readonly GraphicsDevice graphicsDevice;
        private readonly IndexBuffer indexBuffer;
        private readonly VertexBuffer vertexBuffer;
        private readonly ushort[] indices;
        private readonly VertexPositionColor[] vertices;

        private int vertexCounter = 0, indexCounter = 0;

        private const int VERTEX_COUNT = 24;
        private const int INDEX_COUNT = 36;
        private const int TRIANGLE_COUNT = 12;

        internal static BasicEffect basicEffect;

        internal Voxel(GraphicsDevice graphicsDevice, Vector3 position, Color color, float transparency = 1.0f) {
            this.graphicsDevice = graphicsDevice;
            this.color = color;
            this.transparency = transparency;

            transform = Matrix.CreateTranslation(position);

            vertices = new VertexPositionColor[VERTEX_COUNT];
            indices = new ushort[INDEX_COUNT];
            vertexBuffer = new VertexBuffer(graphicsDevice, typeof(VertexPositionColor), VERTEX_COUNT, BufferUsage.None);
            indexBuffer = new IndexBuffer(graphicsDevice, typeof(ushort), INDEX_COUNT, BufferUsage.None);

            CreateTriangles();
        }

        private void CreateTriangles() {
            Vector3 originalColor = color.ToVector3();
            Vector3 adjustedColor = originalColor * ColorManager.FRONT_SHADOW; // front
            AddVertex(0, 0, 0, adjustedColor); AddVertex(1, 0, 0, adjustedColor); AddVertex(1, 1, 0, adjustedColor); AddVertex(0, 1, 0, adjustedColor);

            adjustedColor = originalColor * ColorManager.SIDE_SHADOW; // right
            AddVertex(0, 0, 0, adjustedColor); AddVertex(0, 0, 1, adjustedColor); AddVertex(0, 1, 0, adjustedColor); AddVertex(0, 1, 1, adjustedColor);

            adjustedColor = originalColor * ColorManager.BACK_SHADOW; // back
            AddVertex(0, 0, 1, adjustedColor); AddVertex(1, 0, 1, adjustedColor); AddVertex(0, 1, 1, adjustedColor); AddVertex(1, 1, 1, adjustedColor);

            adjustedColor = originalColor * ColorManager.SIDE_SHADOW; // left
            AddVertex(1, 0, 0, adjustedColor); AddVertex(1, 1, 0, adjustedColor); AddVertex(1, 0, 1, adjustedColor); AddVertex(1, 1, 1, adjustedColor);

            adjustedColor = originalColor * ColorManager.BOTTOM_SHADOW; // bottom
            AddVertex(0, 0, 0, adjustedColor); AddVertex(1, 0, 0, adjustedColor); AddVertex(0, 0, 1, adjustedColor); AddVertex(1, 0, 1, adjustedColor);

            adjustedColor = originalColor * ColorManager.TOP_SHADOW; // top
            AddVertex(1, 1, 0, adjustedColor); AddVertex(0, 1, 0, adjustedColor); AddVertex(1, 1, 1, adjustedColor); AddVertex(0, 1, 1, adjustedColor);

            AddTriangle(0, 1, 2); AddTriangle(2, 3, 0);
            AddTriangle(7, 5, 4); AddTriangle(4, 6, 7);
            AddTriangle(10, 9, 8); AddTriangle(10, 11, 9);
            AddTriangle(12, 14, 15); AddTriangle(15, 13, 12);
            AddTriangle(19, 17, 16); AddTriangle(16, 18, 19);
            AddTriangle(21, 20, 22); AddTriangle(22, 23, 21);

            vertexBuffer.SetData<VertexPositionColor>(0, vertices, 0, VERTEX_COUNT, 0);
            indexBuffer.SetData(0, indices, 0, INDEX_COUNT);
        }

        private void AddVertex(float x, float y, float z, Vector3 color) {
            vertices[vertexCounter++] = new VertexPositionColor(new Vector3(x, y, z), new Color(color.X, color.Y, color.Z));
        }

        private void AddTriangle(ushort a, ushort b, ushort c) {
            indices[indexCounter++] = a;
            indices[indexCounter++] = b;
            indices[indexCounter++] = c;
        }

        internal void Draw(Matrix projectionMatrix, Matrix viewMatrix) {
            graphicsDevice.SetVertexBuffer(vertexBuffer);
            graphicsDevice.Indices = indexBuffer;

            basicEffect.Alpha = transparency;

            basicEffect.Projection = projectionMatrix;
            basicEffect.View = viewMatrix;
            basicEffect.World = transform;

            for (int i = 0; i < basicEffect.CurrentTechnique.Passes.Count; i++) {
                basicEffect.CurrentTechnique.Passes[i].Apply();
                graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, TRIANGLE_COUNT);
            }
        }
    }
}
