//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

using System;

namespace minecraft_kurwa.src.global.geometry;

internal static class Geometry {
    internal static bool Circle(float x, float y, float radius) 
        => Math.Sqrt(x * x + y * y) <= radius;

    internal static bool Sphere(float x, float y, float z, float radius)
        => Math.Sqrt(x * x + y * y + z * z) <= radius;

    internal static bool Ellipse(float x, float y, float a, float b)
        => (x * x / (a * a)) + (y * y / (b * b)) <= 1;

    internal static bool Ellipsoid(float x, float y, float z, float a, float b, float c)
        => (x * x / (a * a)) + (y * y / (b * b)) + (z * z / (c * c)) <= 1;

    internal static float Sigmoid(float x)
        => x / (1f + x);
}
