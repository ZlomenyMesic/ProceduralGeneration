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

        private readonly Matrix transform;

        private readonly GraphicsDevice graphicsDevice;
        private readonly IndexBuffer indexBuffer;
        private readonly VertexBuffer vertexBuffer;
        private readonly ushort[] indices;
        private readonly VertexPositionColor[] vertices;

        private ushort vertexCounter = 0, indexCounter = 0;

        private const byte VERTEX_COUNT = 24;
        private const byte INDEX_COUNT = 36;

        internal static BasicEffect basicEffect;
        internal static int triangleCounter = 0;

        internal Voxel(GraphicsDevice graphicsDevice, Vector3 position, Color color, float transparency = 1.0f) {
            this.graphicsDevice = graphicsDevice;
            this.position = position;
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
            Vector3 adjustedColor;
         
            if (position.Z - 1 < 0 || Global.VOXEL_MAP[(short)position.X, (short)position.Z - 1, (short)position.Y] == null ) {
                adjustedColor = originalColor * ColorManager.FRONT_SHADOW;   // front
                AddVertex(0, 0, 0, adjustedColor); AddVertex(1, 0, 0, adjustedColor); AddVertex(1, 1, 0, adjustedColor); AddVertex(0, 1, 0, adjustedColor);
                AddTriangle(0, 1, 2); AddTriangle(2, 3, 0);
            }

            if (position.X - 1 < 0 || Global.VOXEL_MAP[(short)position.X - 1, (short)position.Z, (short)position.Y] == null) {
                adjustedColor = originalColor * ColorManager.SIDE_SHADOW;    // right
                AddVertex(0, 0, 0, adjustedColor); AddVertex(0, 0, 1, adjustedColor); AddVertex(0, 1, 0, adjustedColor); AddVertex(0, 1, 1, adjustedColor);
                AddTriangle((ushort)(vertexCounter - 1), (ushort)(vertexCounter - 3), (ushort)(vertexCounter - 4)); AddTriangle((ushort)(vertexCounter - 4), (ushort)(vertexCounter - 2), (ushort)(vertexCounter - 1));
            }

            if (position.Z + 1 == Global.WORLD_SIZE || Global.VOXEL_MAP[(short)position.X, (short)position.Z + 1, (short)position.Y] == null) {
                adjustedColor = originalColor * ColorManager.BACK_SHADOW;    // back
                AddVertex(0, 0, 1, adjustedColor); AddVertex(1, 0, 1, adjustedColor); AddVertex(0, 1, 1, adjustedColor); AddVertex(1, 1, 1, adjustedColor);
                AddTriangle((ushort)(vertexCounter - 2), (ushort)(vertexCounter - 3), (ushort)(vertexCounter - 4)); AddTriangle((ushort)(vertexCounter - 2), (ushort)(vertexCounter - 1), (ushort)(vertexCounter - 3));
            }

            if (position.X + 1 == Global.WORLD_SIZE || Global.VOXEL_MAP[(short)position.X + 1, (short)position.Z, (short)position.Y] == null) {
                adjustedColor = originalColor * ColorManager.SIDE_SHADOW;    // left
                AddVertex(1, 0, 0, adjustedColor); AddVertex(1, 1, 0, adjustedColor); AddVertex(1, 0, 1, adjustedColor); AddVertex(1, 1, 1, adjustedColor);
                AddTriangle((ushort)(vertexCounter - 4), (ushort)(vertexCounter - 2), (ushort)(vertexCounter - 1)); AddTriangle((ushort)(vertexCounter - 1), (ushort)(vertexCounter - 3), (ushort)(vertexCounter - 4));
            }

            if (position.Y - 1 < 0 || Global.VOXEL_MAP[(short)position.X, (short)position.Z, (short)position.Y - 1] == null) {
                adjustedColor = originalColor * ColorManager.BOTTOM_SHADOW;  // bottom
                AddVertex(0, 0, 0, adjustedColor); AddVertex(1, 0, 0, adjustedColor); AddVertex(0, 0, 1, adjustedColor); AddVertex(1, 0, 1, adjustedColor);
                AddTriangle((ushort)(vertexCounter - 1), (ushort)(vertexCounter - 3), (ushort)(vertexCounter - 4)); AddTriangle((ushort)(vertexCounter - 4), (ushort)(vertexCounter - 2), (ushort)(vertexCounter - 1));
            }

            if (position.Y + 1 == Global.HEIGHT_LIMIT || Global.VOXEL_MAP[(short)position.X, (short)position.Z, (short)position.Y + 1] == null) {
                adjustedColor = originalColor * ColorManager.TOP_SHADOW;     // top
                AddVertex(1, 1, 0, adjustedColor); AddVertex(0, 1, 0, adjustedColor); AddVertex(1, 1, 1, adjustedColor); AddVertex(0, 1, 1, adjustedColor);
                AddTriangle((ushort)(vertexCounter - 3), (ushort)(vertexCounter - 4), (ushort)(vertexCounter - 2)); AddTriangle((ushort)(vertexCounter - 2), (ushort)(vertexCounter - 1), (ushort)(vertexCounter - 3));
            }

            if (vertexCounter == 0) return;

            VertexPositionColor[] newVertices = new VertexPositionColor[vertexCounter];
            ushort[] newIndices = new ushort[indexCounter];
            for (byte i = 0; i < vertexCounter; i++) {
                newVertices[i] = vertices[i];
            }
            for (byte i = 0; i < indexCounter; i++) {
                newIndices[i] = indices[i];
            }
            
            vertexBuffer.SetData(0, newVertices, 0, vertexCounter, 0);
            indexBuffer.SetData(0, newIndices, 0, indexCounter);
        }

        private void AddVertex(float x, float y, float z, Vector3 color) {
            vertices[vertexCounter++] = new VertexPositionColor(new Vector3(x, y, z), new Color(color.X, color.Y, color.Z));
        }

        private void AddTriangle(ushort a, ushort b, ushort c) {
            triangleCounter++;
            indices[indexCounter++] = a;
            indices[indexCounter++] = b;
            indices[indexCounter++] = c;
        }

        internal void Draw(Matrix projectionMatrix, Matrix viewMatrix) {
            if (vertexCounter == 0) return;

            graphicsDevice.SetVertexBuffer(vertexBuffer);
            graphicsDevice.Indices = indexBuffer;

            basicEffect.Alpha = transparency;

            basicEffect.Projection = projectionMatrix;
            basicEffect.View = viewMatrix;
            basicEffect.World = transform;

            for (ushort i = 0; i < basicEffect.CurrentTechnique.Passes.Count; i++) {
                basicEffect.CurrentTechnique.Passes[i].Apply();
                graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, indexCounter / 3);
            }
        }
    }
}
