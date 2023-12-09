//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using minecraft_kurwa.src.gui.colors;
using minecraft_kurwa.src.global;
using System;
using SharpDX.MediaFoundation;

namespace minecraft_kurwa.src.renderer.voxels {
    internal class VoxelStructure {
        private readonly Voxel[] voxels;

        private readonly IndexBuffer indexBuffer;
        private readonly VertexBuffer vertexBuffer;

        private readonly ushort[] indices;
        private readonly VertexPositionColor[] vertices;

        private ushort voxelCounter = 0, vertexCounter = 0, indexCounter = 0;

        internal static BasicEffect basicEffect;
        internal static int triangleCounter = 0;

        internal const ushort MAX_VOXEL_COUNT = 1820; // maximum possible
        internal const ushort MAX_VERTEX_COUNT = MAX_VOXEL_COUNT * 24;
        internal const ushort MAX_INDEX_COUNT = MAX_VOXEL_COUNT * 36;

        internal VoxelStructure() {
            voxels = new Voxel[MAX_VOXEL_COUNT];
            vertices = new VertexPositionColor[MAX_VERTEX_COUNT];
            indices = new ushort[MAX_INDEX_COUNT];

            vertexBuffer = new VertexBuffer(Global.GRAPHICS_DEVICE, typeof(VertexPositionColor), MAX_VERTEX_COUNT, BufferUsage.WriteOnly);
            indexBuffer = new IndexBuffer(Global.GRAPHICS_DEVICE, typeof(ushort), MAX_INDEX_COUNT, BufferUsage.WriteOnly);
        }

        internal void AddVoxel(Vector3 position, Vector3 size, Color color, byte transparency = 100) {
            voxels[voxelCounter++] = new((ushort)position.X, (ushort)position.Y, (ushort)position.Z, (ushort)size.X, (ushort)size.Y, (ushort)size.Z, null, indexCounter, transparency);

            Vector3 originalColor = !ExperimentalSettings.INVERT_COLORS
                ? color.ToVector3()
                : new(1 - color.R / 255f, 1 - color.G / 255f, 1 - color.B / 255f);
            Vector3 adjustedColor;

            bool[] visible = GetVisibleSides(position, size);

            if (visible[0]) {
                adjustedColor = originalColor * ColorManager.FRONT_SHADOW;       // front
                AddVertex(0, 0, 0, adjustedColor); AddVertex(size.X, 0, 0, adjustedColor); AddVertex(size.X, size.Y, 0, adjustedColor); AddVertex(0, size.Y, 0, adjustedColor);
                AddTriangle((ushort)(vertexCounter - 4), (ushort)(vertexCounter - 3), (ushort)(vertexCounter - 2)); AddTriangle((ushort)(vertexCounter - 2), (ushort)(vertexCounter - 1), (ushort)(vertexCounter - 4));
            }
            
            if (visible[1]) {
                adjustedColor = originalColor * ColorManager.BACK_SHADOW;        // back
                AddVertex(0, 0, size.Z, adjustedColor); AddVertex(size.X, 0, size.Z, adjustedColor); AddVertex(0, size.Y, size.Z, adjustedColor); AddVertex(size.X, size.Y, size.Z, adjustedColor);
                AddTriangle((ushort)(vertexCounter - 2), (ushort)(vertexCounter - 3), (ushort)(vertexCounter - 4)); AddTriangle((ushort)(vertexCounter - 2), (ushort)(vertexCounter - 1), (ushort)(vertexCounter - 3));
            }

            if (visible[2]) {
                adjustedColor = originalColor * ColorManager.SIDE_SHADOW;        // right
                AddVertex(0, 0, 0, adjustedColor); AddVertex(0, 0, size.Z, adjustedColor); AddVertex(0, size.Y, 0, adjustedColor); AddVertex(0, size.Y, size.Z, adjustedColor);
                AddTriangle((ushort)(vertexCounter - 1), (ushort)(vertexCounter - 3), (ushort)(vertexCounter - 4)); AddTriangle((ushort)(vertexCounter - 4), (ushort)(vertexCounter - 2), (ushort)(vertexCounter - 1));
            }

            if (visible[3]) {
                adjustedColor = originalColor * ColorManager.SIDE_SHADOW;        // left
                AddVertex(size.X, 0, 0, adjustedColor); AddVertex(size.X, size.Y, 0, adjustedColor); AddVertex(size.X, 0, size.Z, adjustedColor); AddVertex(size.X, size.Y, size.Z, adjustedColor);
                AddTriangle((ushort)(vertexCounter - 4), (ushort)(vertexCounter - 2), (ushort)(vertexCounter - 1)); AddTriangle((ushort)(vertexCounter - 1), (ushort)(vertexCounter - 3), (ushort)(vertexCounter - 4));
            }

            if (visible[4]) {
                adjustedColor = originalColor * ColorManager.TOP_SHADOW;         // top
                AddVertex(size.X, size.Y, 0, adjustedColor); AddVertex(0, size.Y, 0, adjustedColor); AddVertex(size.X, size.Y, size.Z, adjustedColor); AddVertex(0, size.Y, size.Z, adjustedColor);
                AddTriangle((ushort)(vertexCounter - 3), (ushort)(vertexCounter - 4), (ushort)(vertexCounter - 2)); AddTriangle((ushort)(vertexCounter - 2), (ushort)(vertexCounter - 1), (ushort)(vertexCounter - 3));
            }

            if (visible[5]) {
                adjustedColor = originalColor * ColorManager.BOTTOM_SHADOW;  // bottom
                AddVertex(0, 0, 0, adjustedColor); AddVertex(size.X, 0, 0, adjustedColor); AddVertex(0, 0, size.Z, adjustedColor); AddVertex(size.X, 0, size.Z, adjustedColor);
                AddTriangle((ushort)(vertexCounter - 1), (ushort)(vertexCounter - 3), (ushort)(vertexCounter - 4)); AddTriangle((ushort)(vertexCounter - 4), (ushort)(vertexCounter - 2), (ushort)(vertexCounter - 1));
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
            for (ushort i = 0; i < vertexCounter; i++) {
                newVertices[i] = vertices[i];
            }
            for (ushort i = 0; i < indexCounter; i++) {
                newIndices[i] = indices[i];
            }

            vertexBuffer.SetData(0, newVertices, 0, vertexCounter, 0);
            indexBuffer.SetData(0, newIndices, 0, indexCounter);

            Global.GRAPHICS_DEVICE.SetVertexBuffer(vertexBuffer);
            Global.GRAPHICS_DEVICE.Indices = indexBuffer;

            for (int i = 0; i < voxelCounter; i++) {
                if ((voxels[i].posX + voxels[i].sizeX < VoxelCulling.minRenderX)
                || (voxels[i].posX                    > VoxelCulling.maxRenderX)
                || (voxels[i].posZ + voxels[i].sizeZ  < VoxelCulling.minRenderY)
                || (voxels[i].posZ                    > VoxelCulling.maxRenderY)) continue;

                basicEffect.World = Matrix.CreateTranslation(new(voxels[i].posX, voxels[i].posY, voxels[i].posZ));
                basicEffect.Alpha = voxels[i].transparency / 100f;

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

        /// <summary>
        /// used to hide non visible areas
        /// </summary>
        /// <returns>
        /// front, back, right, left, top, bottom
        /// true = is visible
        /// </returns>
        private static bool[] GetVisibleSides(Vector3 position, Vector3 size) {
            bool[] output = new bool[6];

            for (ushort x = (ushort)position.X; x < position.X + size.X; x++) {
                if (position.Z == 0 || Global.VOXEL_MAP[x, (int)position.Z - 1, (int)position.Y] == null || Global.VOXEL_MAP[x, (int)position.Z - 1, (int)position.Y] == (byte)VoxelType.AIR) {
                    output[0] = true;
                }

                if (position.Z + size.Z == Settings.WORLD_SIZE || Global.VOXEL_MAP[x, (int)position.Z + (int)size.Z, (int)position.Y] == null || Global.VOXEL_MAP[x, (int)position.Z + (int)size.Z, (int)position.Y] == (byte)VoxelType.AIR) {
                    output[1] = true;
                }
            }

            for (ushort y = (ushort)position.Z; y < position.Z + size.Z; y++) {
                if (position.X == 0 || Global.VOXEL_MAP[(int)position.X - 1, y, (int)position.Y] == null || Global.VOXEL_MAP[(int)position.X - 1, y, (int)position.Y] == (byte)VoxelType.AIR) {
                    output[2] = true;
                }

                if (position.X + size.X == Settings.WORLD_SIZE || Global.VOXEL_MAP[(int)position.X + (int)size.X, y, (int)position.Y] == null || Global.VOXEL_MAP[(int)position.X + (int)size.X, y, (int)position.Y] == (byte)VoxelType.AIR) {
                    output[3] = true;
                }
            }

            if (Global.HEIGHT_MAP[(int)position.X, (int)position.Z] != position.Y) {
                output[5] = true;
            }

            output[4] = true; // maybe something in the future

            return output;
        }
    }
}
