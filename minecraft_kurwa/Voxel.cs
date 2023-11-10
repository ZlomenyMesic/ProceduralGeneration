//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace minecraft_kurwa {
    public class Voxel {
        public readonly Vector3 position;
        public readonly Color color;
        public readonly float transparency;

        private readonly GraphicsDevice gpu;
        private readonly BasicEffect basicEffect;
        private readonly IndexBuffer indexBuffer;
        private readonly VertexBuffer vertexBuffer;
        private readonly ushort[] indices;
        private readonly VertexPositionColor[] vertices;
        private int vertexCount = 0; // 24
        private int indexCount = 0;  // 36

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="gpu">graphics device</param>
        /// <param name="position">point with lowest XYZ coordinates</param>
        /// <param name="color">default color</param>
        /// <param name="transparency">transparency, values 0 to 1</param>
        public Voxel(GraphicsDevice gpu, Vector3 position, Color color, float transparency = 1.0f) {
            this.gpu = gpu;
            this.position = position;
            this.color = color;
            this.transparency = transparency;

            basicEffect = new(gpu) {
                Alpha = transparency,
                VertexColorEnabled = true
            };

            vertices = new VertexPositionColor[24];
            indices = new ushort[36];
            vertexBuffer = new VertexBuffer(gpu, typeof(VertexPositionColor), 24, BufferUsage.WriteOnly);
            indexBuffer = new IndexBuffer(gpu, typeof(ushort), 36, BufferUsage.WriteOnly);

            Create();
        }

        /// <summary>
        /// creates vertices and triangles for the cube
        /// </summary>
        private void Create() {
            Vector3 adjustedColor = color.ToVector3();

            // front side
            adjustedColor -= new Vector3(0.1f, 0.1f, 0.1f);
            AddVertex(0, 0, 0, adjustedColor); AddVertex(1, 0, 0, adjustedColor); AddVertex(1, 1, 0, adjustedColor); AddVertex(0, 1, 0, adjustedColor);

            // right side
            adjustedColor -= new Vector3(0.1f, 0.1f, 0.1f);
            AddVertex(0, 0, 0, adjustedColor); AddVertex(0, 0, 1, adjustedColor); AddVertex(0, 1, 0, adjustedColor); AddVertex(0, 1, 1, adjustedColor);

            // back side
            adjustedColor -= new Vector3(0.1f, 0.1f, 0.1f);
            AddVertex(0, 0, 1, adjustedColor); AddVertex(1, 0, 1, adjustedColor); AddVertex(0, 1, 1, adjustedColor); AddVertex(1, 1, 1, adjustedColor);

            // left side
            adjustedColor += new Vector3(0.1f, 0.1f, 0.1f);
            AddVertex(1, 0, 0, adjustedColor); AddVertex(1, 1, 0, adjustedColor); AddVertex(1, 0, 1, adjustedColor); AddVertex(1, 1, 1, adjustedColor);

            // bottom side
            adjustedColor -= new Vector3(0.2f, 0.2f, 0.2f);
            AddVertex(0, 0, 0, adjustedColor); AddVertex(1, 0, 0, adjustedColor); AddVertex(0, 0, 1, adjustedColor); AddVertex(1, 0, 1, adjustedColor);

            // top side
            adjustedColor += new Vector3(0.4f, 0.4f, 0.4f);
            AddVertex(1, 1, 0, adjustedColor); AddVertex(0, 1, 0, adjustedColor); AddVertex(1, 1, 1, adjustedColor); AddVertex(0, 1, 1, adjustedColor);

            AddTriangle(0, 1, 2); AddTriangle(2, 3, 0);
            AddTriangle(7, 5, 4); AddTriangle(4, 6, 7);
            AddTriangle(10, 9, 8); AddTriangle(10, 11, 9);
            AddTriangle(12, 14, 15); AddTriangle(15, 13, 12);
            AddTriangle(19, 17, 16); AddTriangle(16, 18, 19);
            AddTriangle(21, 20, 22); AddTriangle(22, 23, 21);

            vertexBuffer.SetData<VertexPositionColor>(0, vertices, 0, 24, 0);
            indexBuffer.SetData(0, indices, 0, indexCount);
        }

        /// <summary>
        /// adds a vertex
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <param name="z">Z coordinate</param>
        /// <param name="color">vertex's color</param>
        private void AddVertex(float x, float y, float z, Vector3 color) {
            vertices[vertexCount++] = new VertexPositionColor(new Vector3(x, y, z), new Color(color.X, color.Y, color.Z));
        }

        /// <summary>
        /// adds a triangle
        /// </summary>
        /// <param name="a">index of the first vertex</param>
        /// <param name="b">index of the second vertex</param>
        /// <param name="c">index of the third vertex</param>
        private void AddTriangle(ushort a, ushort b, ushort c) {
            indices[indexCount++] = a;
            indices[indexCount++] = b;
            indices[indexCount++] = c;
        }

        /// <summary>
        /// draws the cube on screen
        /// </summary>
        /// <param name="projectionMatrix">projection matrix</param>
        /// <param name="viewMatrix">view matrix</param>
        public void Draw(Matrix projectionMatrix, Matrix viewMatrix) {
            gpu.SetVertexBuffer(vertexBuffer);
            gpu.Indices = indexBuffer;

            basicEffect.Projection = projectionMatrix;
            basicEffect.View = viewMatrix;
            basicEffect.World = Matrix.CreateTranslation(position);

            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes) {
                pass.Apply();
                gpu.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 12); // 12 triangles
            }
        }
    }
}
