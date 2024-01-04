//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

using minecraft_kurwa.src.global;
using Microsoft.Xna.Framework;
using System;
using minecraft_kurwa.src.renderer.voxels;

namespace minecraft_kurwa.src.renderer.view;

internal static class View {
    internal static float VIEW_ROTATION = 0;

    private static Vector2 ZERO_ANGLE_VECTOR = new(0, 1);

    private static Vector2 shiftedCP;
    private static Vector2 diffShifted;

    internal static void Update() {
        Vector3 diff3D = Global.CAM_POSITION - Global.CAM_TARGET;
        Vector2 diff = new(diff3D.X, diff3D.Z);

        VIEW_ROTATION = CalculateVectorAngle(diff, ZERO_ANGLE_VECTOR);

        shiftedCP = new(Global.CAM_POSITION.X, Global.CAM_POSITION.Z);
        shiftedCP.X += diff.X * Global.CAM_POSITION.Y / VoxelCulling.MAX_DIFFERENCE_VALUE;
        shiftedCP.Y += diff.Y * Global.CAM_POSITION.Y / VoxelCulling.MAX_DIFFERENCE_VALUE;

        diffShifted = shiftedCP - new Vector2(Global.CAM_TARGET.X, Global.CAM_TARGET.Z);
    }

    /// <summary>
    /// checks if a voxel is in the camera view
    /// </summary>
    /// <returns>true = is visible</returns>
    internal static bool IsVoxelInView(ushort posX, ushort posY, ushort sizeX, ushort sizeY) {
        return IsPointInView(posX, posY) || IsPointInView((ushort)(posX + sizeX), posY) || IsPointInView(posX, (ushort)(posY + sizeY)) || IsPointInView((ushort)(posX + sizeX), (ushort)(posY + sizeY));
    }

    private static bool IsPointInView(ushort posX, ushort posY) {
        Vector2 obj = new(shiftedCP.X - posX, shiftedCP.Y - posY);
        return CalculateVectorAngle(new(-diffShifted.X, -diffShifted.Y), new(obj.X, obj.Y)) < Settings.FIELD_OF_VIEW;
    }

    private static float CalculateVectorAngle(Vector2 v1, Vector2 v2) {
        float v1Magnitude = (float)Math.Sqrt(v1.X * v1.X + v1.Y * v1.Y);
        float v2Magnitude = (float)Math.Sqrt(v2.X * v2.X + v2.Y * v2.Y);

        return (float)(180 - (Math.Acos(Vector2.Dot(v1, v2) / (v1Magnitude * v2Magnitude)) * 180 / Math.PI));
    }
}
