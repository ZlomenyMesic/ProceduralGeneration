//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace minecraft_kurwa {
    internal class VoxelStructure {
        private readonly VoxelStruct[] voxels;

        private readonly IndexBuffer indexBuffer;
        private readonly VertexBuffer vertexBuffer;

        private readonly ushort[] indices;
        private readonly VertexPositionColor[] vertices;

        private ushort voxelCounter = 0, vertexCounter = 0, indexCounter = 0;

        internal static BasicEffect basicEffect;
        internal static int triangleCounter = 0;

        internal VoxelStructure() {
            voxels = new VoxelStruct[7];
            vertices = new VertexPositionColor[168];
            indices = new ushort[252];

            vertexBuffer = new VertexBuffer(Global.GRAPHICS_DEVICE, typeof(VertexPositionColor), 168, BufferUsage.WriteOnly);
            indexBuffer = new IndexBuffer(Global.GRAPHICS_DEVICE, typeof(ushort), 252, BufferUsage.WriteOnly);
        }

        internal void AddVoxel(Vector3 position, Color color, float transparency = 1.0f) {
            voxels[voxelCounter++] = new(Matrix.CreateTranslation(position), indexCounter);

            Vector3 originalColor = !Global.INVERT_COLORS 
                ? color.ToVector3()
                : new(1 - (color.R / 255f), 1 - (color.G / 255f), 1 - (color.B / 255f));
            Vector3 adjustedColor;

            if (position.Z - 1 < 0 || Global.VOXEL_MAP[(short)position.X, (short)position.Z - 1, (short)position.Y] == null) {
                adjustedColor = originalColor * ColorManager.FRONT_SHADOW;   // front
                AddVertex(0, 0, 0, adjustedColor); AddVertex(1, 0, 0, adjustedColor); AddVertex(1, 1, 0, adjustedColor); AddVertex(0, 1, 0, adjustedColor);
                AddTriangle((ushort)(vertexCounter - 4), (ushort)(vertexCounter - 3), (ushort)(vertexCounter - 2)); AddTriangle((ushort)(vertexCounter - 2), (ushort)(vertexCounter - 1), (ushort)(vertexCounter - 4));
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

        internal void Draw() {
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

            Global.GRAPHICS_DEVICE.SetVertexBuffer(vertexBuffer);
            Global.GRAPHICS_DEVICE.Indices = indexBuffer;

            basicEffect.Alpha = 1.0f;

            for (int i = 0; i < voxelCounter; i++) {
                basicEffect.World = voxels[i].transform;

                int triangles = i != voxelCounter - 1
                    ? (voxels[i + 1].indexStart - voxels[i].indexStart) / 3
                    : (indexCounter - voxels[i].indexStart) / 3;

                if (triangles == 0) continue;

                for (ushort j = 0; j < basicEffect.CurrentTechnique.Passes.Count; j++) {
                    basicEffect.CurrentTechnique.Passes[j].Apply();
                    Global.GRAPHICS_DEVICE.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, voxels[i].indexStart, triangles);
                }
            }
        }

        private struct VoxelStruct {
            internal Matrix transform;
            internal ushort indexStart;

            internal VoxelStruct(Matrix transform, ushort indexStart) {
                this.transform = transform;
                this.indexStart = indexStart;
            }
        }
    }
}
