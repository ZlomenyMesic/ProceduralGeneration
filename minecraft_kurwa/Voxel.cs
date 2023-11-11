//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace minecraft_kurwa {
    internal class Voxel {
        internal readonly Vector3 position;
        internal readonly Color color;
        internal readonly float transparency;

        private readonly GraphicsDevice gpu;
        private readonly BasicEffect basicEffect;
        private readonly IndexBuffer indexBuffer;
        private readonly VertexBuffer vertexBuffer;
        private readonly ushort[] indices;
        private readonly VertexPositionColor[] vertices;

        private int vertexCounter = 0, indexCounter = 0;

        private const int VERTEX_COUNT = 24;
        private const int INDEX_COUNT = 36;
        private const int TRIANGLE_COUNT = 12;

        internal Voxel(GraphicsDevice gpu, Vector3 position, Color color, float transparency = 1.0f) {
            this.gpu = gpu;
            this.position = position;
            this.color = color;
            this.transparency = transparency;

            basicEffect = new(gpu) {
                Alpha = transparency,
                VertexColorEnabled = true
            };

            vertices = new VertexPositionColor[VERTEX_COUNT];
            indices = new ushort[INDEX_COUNT];
            vertexBuffer = new VertexBuffer(gpu, typeof(VertexPositionColor), VERTEX_COUNT, BufferUsage.WriteOnly);
            indexBuffer = new IndexBuffer(gpu, typeof(ushort), INDEX_COUNT, BufferUsage.WriteOnly);

            CreateTriangles();
        }

        private void CreateTriangles() {
            Vector3 adjustedColor = color.ToVector3() - new Vector3(0.1f, 0.1f, 0.1f); // front
            AddVertex(0, 0, 0, adjustedColor); AddVertex(1, 0, 0, adjustedColor); AddVertex(1, 1, 0, adjustedColor); AddVertex(0, 1, 0, adjustedColor);

            adjustedColor -= new Vector3(0.1f, 0.1f, 0.1f); // right
            AddVertex(0, 0, 0, adjustedColor); AddVertex(0, 0, 1, adjustedColor); AddVertex(0, 1, 0, adjustedColor); AddVertex(0, 1, 1, adjustedColor);

            adjustedColor -= new Vector3(0.1f, 0.1f, 0.1f); // back
            AddVertex(0, 0, 1, adjustedColor); AddVertex(1, 0, 1, adjustedColor); AddVertex(0, 1, 1, adjustedColor); AddVertex(1, 1, 1, adjustedColor);

            adjustedColor += new Vector3(0.1f, 0.1f, 0.1f); // left
            AddVertex(1, 0, 0, adjustedColor); AddVertex(1, 1, 0, adjustedColor); AddVertex(1, 0, 1, adjustedColor); AddVertex(1, 1, 1, adjustedColor);

            adjustedColor -= new Vector3(0.2f, 0.2f, 0.2f); // bottom
            AddVertex(0, 0, 0, adjustedColor); AddVertex(1, 0, 0, adjustedColor); AddVertex(0, 0, 1, adjustedColor); AddVertex(1, 0, 1, adjustedColor);

            adjustedColor += new Vector3(0.4f, 0.4f, 0.4f); // top
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
            gpu.SetVertexBuffer(vertexBuffer);
            gpu.Indices = indexBuffer;

            basicEffect.Projection = projectionMatrix;
            basicEffect.View = viewMatrix;
            basicEffect.World = Matrix.CreateTranslation(position);

            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes) {
                pass.Apply();
                gpu.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, TRIANGLE_COUNT);
            }
        }
    }
}
