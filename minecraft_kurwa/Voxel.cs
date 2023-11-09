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
        public readonly Texture2D texture;

        private readonly GraphicsDevice gpu;
        private readonly BasicEffect basicEffect;
        private readonly IndexBuffer indexBuffer;
        private readonly VertexBuffer vertexBuffer;
        private readonly ushort[] indices;
        private readonly VertexPositionNormalTexture[] vertices;

        private int vertexCount = 0; // 16
        private int indexCount = 0;  // 36

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="position">point with lowest X, Y, Z coordinates</param>
        /// <param name="texture">texture file name (no extension)</param>
        /// <param name="gpu">graphics device</param>
        /// <param name="content">content manager</param>
        public Voxel(Vector3 position, Texture2D texture, GraphicsDevice gpu) {
            this.position = position;
            this.texture = texture;
            this.gpu = gpu;

            // simple lighting, should be improved in the future
            basicEffect = new(gpu) {
                Alpha = 1.0f, // TRANSPARENCY
                AmbientLightColor = new Vector3(0.3f, 0.3f, 0.3f),
                DiffuseColor = new Vector3(0.7f, 0.7f, 0.7f),
                TextureEnabled = true
            };
            basicEffect.EnableDefaultLighting();

            vertices = new VertexPositionNormalTexture[16];
            indices = new ushort[36];
            vertexBuffer = new VertexBuffer(gpu, typeof(VertexPositionNormalTexture), 16, BufferUsage.WriteOnly);
            indexBuffer = new IndexBuffer(gpu, typeof(ushort), 36, BufferUsage.WriteOnly);

            Create();
        }

        /// <summary>
        /// creates vertices and triangles for the cube
        /// </summary>
        private void Create() {
            float x1 = position.X, x2 = x1 + 2;
            float y1 = position.Y, y2 = y1 + 2;
            float z1 = position.Z, z2 = z1 + 2;

            // 16 vertices
            Vector3 normal = Vector3.Up; AddVertex(x1, y1, z2, normal, 0, 0); AddVertex(x2, y1, z2, normal, 1, 0); AddVertex(x2, y1, z1, normal, 1, 1); AddVertex(x1, y1, z1, normal, 0, 1);
            normal = Vector3.Right;      AddVertex(x2, y2, z2, normal, 0, 0); AddVertex(x2, y2, z1, normal, 0, 1);
            normal = Vector3.Down;       AddVertex(x1, y2, z2, normal, 1, 0); AddVertex(x1, y2, z1, normal, 1, 1);
            normal = Vector3.Backward;   AddVertex(x1, y1, z1, normal, 0, 0); AddVertex(x2, y1, z1, normal, 1, 0); AddVertex(x2, y2, z1, normal, 1, 1); AddVertex(x1, y2, z1, normal, 0, 1);
            normal = Vector3.Forward;    AddVertex(x2, y1, z2, normal, 0, 0); AddVertex(x1, y1, z2, normal, 1, 0); AddVertex(x1, y2, z2, normal, 1, 1); AddVertex(x2, y2, z2, normal, 0, 1);

            // 12 triangles
            AddTriangle(0, 1, 2); AddTriangle(2, 3, 0);
            AddTriangle(2, 1, 4); AddTriangle(4, 5, 2);
            AddTriangle(5, 4, 6); AddTriangle(6, 7, 5);
            AddTriangle(7, 6, 0); AddTriangle(0, 3, 7);
            AddTriangle(8, 9, 10); AddTriangle(10, 11, 8);
            AddTriangle(12, 13, 14); AddTriangle(14, 15, 12);

            vertexBuffer.SetData<VertexPositionNormalTexture>(0, vertices, 0, vertexCount, 32);
            indexBuffer.SetData<ushort>(0, indices, 0, indexCount);
        }

        /// <summary>
        /// adds a vertex
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <param name="z">Z coordinate</param>
        /// <param name="normal">direction of triangle's front side</param>
        /// <param name="u">U coordinate</param>
        /// <param name="v">V coordinate</param>
        private void AddVertex(float x, float y, float z, Vector3 normal, float u, float v) {
            vertices[vertexCount++] = new VertexPositionNormalTexture(new Vector3(x, y, z), normal, new Vector2(u, v));
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

            basicEffect.Texture = texture;
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
