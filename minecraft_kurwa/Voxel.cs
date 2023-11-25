//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace minecraft_kurwa {
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

        internal void AddVoxel(Vector3 position, Color color, float transparency = 1.0f) {
            voxels[voxelCounter++] = new(Matrix.CreateTranslation(position), indexCounter, transparency);

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

            if (position.Z + 1 == Settings.WORLD_SIZE || Global.VOXEL_MAP[(short)position.X, (short)position.Z + 1, (short)position.Y] == null) {
                adjustedColor = originalColor * ColorManager.BACK_SHADOW;    // back
                AddVertex(0, 0, 1, adjustedColor); AddVertex(1, 0, 1, adjustedColor); AddVertex(0, 1, 1, adjustedColor); AddVertex(1, 1, 1, adjustedColor);
                AddTriangle((ushort)(vertexCounter - 2), (ushort)(vertexCounter - 3), (ushort)(vertexCounter - 4)); AddTriangle((ushort)(vertexCounter - 2), (ushort)(vertexCounter - 1), (ushort)(vertexCounter - 3));
            }

            if (position.X + 1 == Settings.WORLD_SIZE || Global.VOXEL_MAP[(short)position.X + 1, (short)position.Z, (short)position.Y] == null) {
                adjustedColor = originalColor * ColorManager.SIDE_SHADOW;    // left
                AddVertex(1, 0, 0, adjustedColor); AddVertex(1, 1, 0, adjustedColor); AddVertex(1, 0, 1, adjustedColor); AddVertex(1, 1, 1, adjustedColor);
                AddTriangle((ushort)(vertexCounter - 4), (ushort)(vertexCounter - 2), (ushort)(vertexCounter - 1)); AddTriangle((ushort)(vertexCounter - 1), (ushort)(vertexCounter - 3), (ushort)(vertexCounter - 4));
            }

            if (position.Y - 1 < 0 || Global.VOXEL_MAP[(short)position.X, (short)position.Z, (short)position.Y - 1] == null) {
                adjustedColor = originalColor * ColorManager.BOTTOM_SHADOW;  // bottom
                AddVertex(0, 0, 0, adjustedColor); AddVertex(1, 0, 0, adjustedColor); AddVertex(0, 0, 1, adjustedColor); AddVertex(1, 0, 1, adjustedColor);
                AddTriangle((ushort)(vertexCounter - 1), (ushort)(vertexCounter - 3), (ushort)(vertexCounter - 4)); AddTriangle((ushort)(vertexCounter - 4), (ushort)(vertexCounter - 2), (ushort)(vertexCounter - 1));
            }

            if (position.Y + 1 == Settings.HEIGHT_LIMIT || Global.VOXEL_MAP[(short)position.X, (short)position.Z, (short)position.Y + 1] == null) {
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
                
                basicEffect.World = voxels[i].transform;
                basicEffect.Alpha = voxels[i].transparency;

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

        private struct Voxel {
            internal Matrix transform;
            internal ushort indexStart;
            internal float transparency;

            internal Voxel(Matrix transform, ushort indexStart, float transparency) {
                this.transform = transform;
                this.indexStart = indexStart;
                this.transparency = transparency;
            }
        }
    }
    
    internal enum VoxelType {
        AIR = 0,
        GRASS = 1,
        STONE = 2,
        SAND = 3,
        ICE = 4,
        TERRACOTTA = 5,
        GRAVEL = 6,
        SNOW = 7,

        OAK_LEAVES = 8,
        OAK_WOOD = 9,

        KAPOK_LEAVES = 10,
        KAPOK_WOOD = 11,

        SPRUCE_LEAVES = 12,
        SPRUCE_WOOD = 13,

        BEECH_LEAVES = 14,
        BEECH_WOOD = 15,

        MAPLE_LEAVES = 16,
        MAPLE_WOOD = 17,

        WATER = 18,

        POPLAR_LEAVES = 19,
        POPLAR_WOOD = 20,

        CHERRY_LEAVES = 21,
        CHERRY_WOOD = 22,
        
        SANDSTONE = 23,
    }
}
