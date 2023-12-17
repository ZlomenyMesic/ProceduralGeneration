//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using minecraft_kurwa.src.gui.colors;
using minecraft_kurwa.src.global;
using System.Linq;
using System;

namespace minecraft_kurwa.src.renderer.voxels {
    internal class VoxelStructure {
        private readonly Voxel1[] _voxels;

        private readonly IndexBuffer _indexBuffer;
        private readonly VertexBuffer _vertexBuffer;

        private ushort[] _indices;
        private VertexPositionColor[] _vertices;

        private ushort _voxelCounter = 0, _vertexCounter = 0, _indexCounter = 0;

        internal static BasicEffect basicEffect;
        internal static int triangleCounter = 0;

        internal const ushort MAX_VOXEL_COUNT = 1820; // maximum possible 1820
        internal const ushort MAX_VERTEX_COUNT = MAX_VOXEL_COUNT * 24;
        internal const ushort MAX_INDEX_COUNT = MAX_VOXEL_COUNT * 36;

        internal VoxelStructure() {
            _voxels = new Voxel1[MAX_VOXEL_COUNT];
            _vertices = new VertexPositionColor[MAX_VERTEX_COUNT];
            _indices = new ushort[MAX_INDEX_COUNT];

            _vertexBuffer = new VertexBuffer(Global.GRAPHICS_DEVICE, typeof(VertexPositionColor), MAX_VERTEX_COUNT, BufferUsage.WriteOnly);
            _indexBuffer = new IndexBuffer(Global.GRAPHICS_DEVICE, typeof(ushort), MAX_INDEX_COUNT, BufferUsage.WriteOnly);
        }

        internal void AddVoxel(ushort posX, ushort posY, ushort posZ, byte sizeX, byte sizeY, byte sizeZ, Color color, byte transparency = 100) {
            bool[] visible = VoxelCulling.GetVisibleSides(posX, posY, posZ, sizeX, sizeY, sizeZ);

            if (visible[0] || visible[1] || visible[2] || visible[3] || visible[4] || visible[5]) {
                _voxels[_voxelCounter] = new(posX, posY, posZ, sizeX, sizeY, sizeZ, _indexCounter, 0, transparency);
            } else return;

            Vector3 originalColor = color.ToVector3();
            Vector3 adjustedColor;

            if (visible[0]) {
                adjustedColor = originalColor * ColorManager.FRONT_SHADOW;       // front
                AddVertex(0, 0, 0, adjustedColor); AddVertex(sizeX, 0, 0, adjustedColor); AddVertex(sizeX, sizeY, 0, adjustedColor); AddVertex(0, sizeY, 0, adjustedColor);
                AddTriangle((ushort)(_vertexCounter - 4), (ushort)(_vertexCounter - 3), (ushort)(_vertexCounter - 2)); AddTriangle((ushort)(_vertexCounter - 2), (ushort)(_vertexCounter - 1), (ushort)(_vertexCounter - 4));
            }
            
            if (visible[1]) {
                adjustedColor = originalColor * ColorManager.BACK_SHADOW;        // back
                AddVertex(0, 0, sizeZ, adjustedColor); AddVertex(sizeX, 0, sizeZ, adjustedColor); AddVertex(0, sizeY, sizeZ, adjustedColor); AddVertex(sizeX, sizeY, sizeZ, adjustedColor);
                AddTriangle((ushort)(_vertexCounter - 2), (ushort)(_vertexCounter - 3), (ushort)(_vertexCounter - 4)); AddTriangle((ushort)(_vertexCounter - 2), (ushort)(_vertexCounter - 1), (ushort)(_vertexCounter - 3));
            }

            if (visible[2]) {
                adjustedColor = originalColor * ColorManager.SIDE_SHADOW;        // right
                AddVertex(0, 0, 0, adjustedColor); AddVertex(0, 0, sizeZ, adjustedColor); AddVertex(0, sizeY, 0, adjustedColor); AddVertex(0, sizeY, sizeZ, adjustedColor);
                AddTriangle((ushort)(_vertexCounter - 1), (ushort)(_vertexCounter - 3), (ushort)(_vertexCounter - 4)); AddTriangle((ushort)(_vertexCounter - 4), (ushort)(_vertexCounter - 2), (ushort)(_vertexCounter - 1));
            }

            if (visible[3]) {
                adjustedColor = originalColor * ColorManager.SIDE_SHADOW;        // left
                AddVertex(sizeX, 0, 0, adjustedColor); AddVertex(sizeX, sizeY, 0, adjustedColor); AddVertex(sizeX, 0, sizeZ, adjustedColor); AddVertex(sizeX, sizeY, sizeZ, adjustedColor);
                AddTriangle((ushort)(_vertexCounter - 4), (ushort)(_vertexCounter - 2), (ushort)(_vertexCounter - 1)); AddTriangle((ushort)(_vertexCounter - 1), (ushort)(_vertexCounter - 3), (ushort)(_vertexCounter - 4));
            }

            if (visible[4]) {
                adjustedColor = originalColor * ColorManager.TOP_SHADOW;         // top
                AddVertex(sizeX, sizeY, 0, adjustedColor); AddVertex(0, sizeY, 0, adjustedColor); AddVertex(sizeX, sizeY, sizeZ, adjustedColor); AddVertex(0, sizeY, sizeZ, adjustedColor);
                AddTriangle((ushort)(_vertexCounter - 3), (ushort)(_vertexCounter - 4), (ushort)(_vertexCounter - 2)); AddTriangle((ushort)(_vertexCounter - 2), (ushort)(_vertexCounter - 1), (ushort)(_vertexCounter - 3));
            }

            if (visible[5]) {
                adjustedColor = originalColor * ColorManager.BOTTOM_SHADOW;      // bottom
                AddVertex(0, 0, 0, adjustedColor); AddVertex(sizeX, 0, 0, adjustedColor); AddVertex(0, 0, sizeZ, adjustedColor); AddVertex(sizeX, 0, sizeZ, adjustedColor);
                AddTriangle((ushort)(_vertexCounter - 1), (ushort)(_vertexCounter - 3), (ushort)(_vertexCounter - 4)); AddTriangle((ushort)(_vertexCounter - 4), (ushort)(_vertexCounter - 2), (ushort)(_vertexCounter - 1));
            }

            _voxelCounter++;
        }

        private void AddVertex(float x, float y, float z, Vector3 color) {
            _vertices[_vertexCounter++] = new VertexPositionColor(new Vector3(x, y, z), new Color(color.X, color.Y, color.Z));
        }

        private void AddTriangle(ushort a, ushort b, ushort c) {
            triangleCounter++;
            _voxels[_voxelCounter].triangles++;

            _indices[_indexCounter++] = a;
            _indices[_indexCounter++] = b;
            _indices[_indexCounter++] = c;
        }

        internal void PrepareBuffers() {
            _vertices = _vertices.Take(_vertexCounter).ToArray();
            _indices = _indices.Take(_indexCounter).ToArray();

            _vertexBuffer.SetData(0, _vertices, 0, _vertexCounter, 0);
            _indexBuffer.SetData(0, _indices, 0, _indexCounter);

            _vertices = null;
            _indices = null;
        }

        internal void Draw() {
            Global.GRAPHICS_DEVICE.SetVertexBuffer(_vertexBuffer);
            Global.GRAPHICS_DEVICE.Indices = _indexBuffer;

            for (ushort i = 0; i < _voxelCounter; i++) {
                Voxel1 cache = _voxels[i];
                if (VoxelCulling.ShouldNOTRender(cache.posX, cache.posZ, cache.sizeX, cache.sizeZ)) continue;

                /* TESTY
                
                //if (!Rays.IsVoxelInView(cache.posX, cache.posZ, cache.sizeX, cache.sizeZ)) continue;

                */

                basicEffect.World = Matrix.CreateTranslation(new(cache.posX, cache.posY, cache.posZ));
                basicEffect.Alpha = cache.transparency / 100f;

                basicEffect.CurrentTechnique.Passes[0].Apply();
                Global.GRAPHICS_DEVICE.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, cache.indexStart, cache.triangles);
            }
        }
    }
}
