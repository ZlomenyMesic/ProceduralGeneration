//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using minecraft_kurwa.src.gui.colors;
using minecraft_kurwa.src.global;
using System.Linq;

namespace minecraft_kurwa.src.renderer.voxels {
    internal class VoxelStructure {
        private readonly Voxel[] voxels;

        private readonly IndexBuffer indexBuffer;
        private readonly VertexBuffer vertexBuffer;

        private ushort[] indices;
        private VertexPositionColor[] vertices;

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

        internal void AddVoxel(ushort posX, ushort posY, ushort posZ, ushort sizeX, ushort sizeY, ushort sizeZ, Color color, byte transparency = 100) {
            voxels[voxelCounter++] = new(posX, posY, posZ, sizeX, sizeY, sizeZ, null, indexCounter, transparency);

            Vector3 originalColor = !ExperimentalSettings.INVERT_COLORS
                ? color.ToVector3()
                : new(1 - color.R / 255f, 1 - color.G / 255f, 1 - color.B / 255f);
            Vector3 adjustedColor;

            bool[] visible = VoxelCulling.GetVisibleSides(posX, posY, posZ, sizeX, sizeY, sizeZ);

            if (visible[0]) {
                adjustedColor = originalColor * ColorManager.FRONT_SHADOW;       // front
                AddVertex(0, 0, 0, adjustedColor); AddVertex(sizeX, 0, 0, adjustedColor); AddVertex(sizeX, sizeY, 0, adjustedColor); AddVertex(0, sizeY, 0, adjustedColor);
                AddTriangle((ushort)(vertexCounter - 4), (ushort)(vertexCounter - 3), (ushort)(vertexCounter - 2)); AddTriangle((ushort)(vertexCounter - 2), (ushort)(vertexCounter - 1), (ushort)(vertexCounter - 4));
            }
            
            if (visible[1]) {
                adjustedColor = originalColor * ColorManager.BACK_SHADOW;        // back
                AddVertex(0, 0, sizeZ, adjustedColor); AddVertex(sizeX, 0, sizeZ, adjustedColor); AddVertex(0, sizeY, sizeZ, adjustedColor); AddVertex(sizeX, sizeY, sizeZ, adjustedColor);
                AddTriangle((ushort)(vertexCounter - 2), (ushort)(vertexCounter - 3), (ushort)(vertexCounter - 4)); AddTriangle((ushort)(vertexCounter - 2), (ushort)(vertexCounter - 1), (ushort)(vertexCounter - 3));
            }

            if (visible[2]) {
                adjustedColor = originalColor * ColorManager.SIDE_SHADOW;        // right
                AddVertex(0, 0, 0, adjustedColor); AddVertex(0, 0, sizeZ, adjustedColor); AddVertex(0, sizeY, 0, adjustedColor); AddVertex(0, sizeY, sizeZ, adjustedColor);
                AddTriangle((ushort)(vertexCounter - 1), (ushort)(vertexCounter - 3), (ushort)(vertexCounter - 4)); AddTriangle((ushort)(vertexCounter - 4), (ushort)(vertexCounter - 2), (ushort)(vertexCounter - 1));
            }

            if (visible[3]) {
                adjustedColor = originalColor * ColorManager.SIDE_SHADOW;        // left
                AddVertex(sizeX, 0, 0, adjustedColor); AddVertex(sizeX, sizeY, 0, adjustedColor); AddVertex(sizeX, 0, sizeZ, adjustedColor); AddVertex(sizeX, sizeY, sizeZ, adjustedColor);
                AddTriangle((ushort)(vertexCounter - 4), (ushort)(vertexCounter - 2), (ushort)(vertexCounter - 1)); AddTriangle((ushort)(vertexCounter - 1), (ushort)(vertexCounter - 3), (ushort)(vertexCounter - 4));
            }

            if (visible[4]) {
                adjustedColor = originalColor * ColorManager.TOP_SHADOW;         // top
                AddVertex(sizeX, sizeY, 0, adjustedColor); AddVertex(0, sizeY, 0, adjustedColor); AddVertex(sizeX, sizeY, sizeZ, adjustedColor); AddVertex(0, sizeY, sizeZ, adjustedColor);
                AddTriangle((ushort)(vertexCounter - 3), (ushort)(vertexCounter - 4), (ushort)(vertexCounter - 2)); AddTriangle((ushort)(vertexCounter - 2), (ushort)(vertexCounter - 1), (ushort)(vertexCounter - 3));
            }

            if (visible[5]) {
                adjustedColor = originalColor * ColorManager.BOTTOM_SHADOW;  // bottom
                AddVertex(0, 0, 0, adjustedColor); AddVertex(sizeX, 0, 0, adjustedColor); AddVertex(0, 0, sizeZ, adjustedColor); AddVertex(sizeX, 0, sizeZ, adjustedColor);
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

        internal void PrepareBuffers() {
            vertices = vertices.Take(vertexCounter).ToArray();
            indices = indices.Take(indexCounter).ToArray();

            vertexBuffer.SetData(0, vertices, 0, vertexCounter, 0);
            indexBuffer.SetData(0, indices, 0, indexCounter);
        }

        internal void Draw() {
            if (vertexCounter == 0) return;

            Global.GRAPHICS_DEVICE.SetVertexBuffer(vertexBuffer);
            Global.GRAPHICS_DEVICE.Indices = indexBuffer;

            for (int i = 0; i < voxelCounter; i++) {
                if ((voxels[i].posX + voxels[i].sizeX < VoxelCulling.minRenderX)
                || (voxels[i].posX                    > VoxelCulling.maxRenderX)
                || (voxels[i].posZ + voxels[i].sizeZ  < VoxelCulling.minRenderY)
                || (voxels[i].posZ                    > VoxelCulling.maxRenderY)) continue;

                int triangles = i != voxelCounter - 1
                    ? (voxels[i + 1].indexStart - voxels[i].indexStart) / 3
                    : (indexCounter - voxels[i].indexStart) / 3;

                if (triangles == 0) continue;

                basicEffect.World = Matrix.CreateTranslation(new(voxels[i].posX, voxels[i].posY, voxels[i].posZ));
                basicEffect.Alpha = voxels[i].transparency / 100f;

                for (ushort j = 0; j < basicEffect.CurrentTechnique.Passes.Count; j++) {
                    basicEffect.CurrentTechnique.Passes[j].Apply();
                    Global.GRAPHICS_DEVICE.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, voxels[i].indexStart, triangles);
                }
            }
        }
    }
}
