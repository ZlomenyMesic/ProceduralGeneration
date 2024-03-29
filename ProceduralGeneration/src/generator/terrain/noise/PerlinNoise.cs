﻿//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

using System;

namespace minecraft_kurwa.src.generator.terrain.noise;

internal class PerlinNoise {
    internal int seed;
    private long _defaultSize;
    private int[] _p;
    private int[] _permutation;

    internal PerlinNoise(int seed) {
        this.seed = seed;
        Initialize();
    }

    private void Initialize() {
        _p = new int[512];
        _permutation = new[] { 151, 160, 137, 91, 90, 15, 131, 13, 201,
                95, 96, 53, 194, 233, 7, 225, 140, 36, 103, 30, 69, 142, 8, 99,
                37, 240, 21, 10, 23, 190, 6, 148, 247, 120, 234, 75, 0, 26,
                197, 62, 94, 252, 219, 203, 117, 35, 11, 32, 57, 177, 33, 88,
                237, 149, 56, 87, 174, 20, 125, 136, 171, 168, 68, 175, 74,
                165, 71, 134, 139, 48, 27, 166, 77, 146, 158, 231, 83, 111,
                229, 122, 60, 211, 133, 230, 220, 105, 92, 41, 55, 46, 245, 40,
                244, 102, 143, 54, 65, 25, 63, 161, 1, 216, 80, 73, 209, 76,
                132, 187, 208, 89, 18, 169, 200, 196, 135, 130, 116, 188, 159,
                86, 164, 100, 109, 198, 173, 186, 3, 64, 52, 217, 226, 250,
                124, 123, 5, 202, 38, 147, 118, 126, 255, 82, 85, 212, 207,
                206, 59, 227, 47, 16, 58, 17, 182, 189, 28, 42, 223, 183, 170,
                213, 119, 248, 152, 2, 44, 154, 163, 70, 221, 153, 101, 155,
                167, 43, 172, 9, 129, 22, 39, 253, 19, 98, 108, 110, 79, 113,
                224, 232, 178, 185, 112, 104, 218, 246, 97, 228, 251, 34, 242,
                193, 238, 210, 144, 12, 191, 179, 162, 241, 81, 51, 145, 235,
                249, 14, 239, 107, 49, 192, 214, 31, 181, 199, 106, 157, 184,
                84, 204, 176, 115, 121, 50, 45, 127, 4, 150, 254, 138, 236,
                205, 93, 222, 114, 67, 29, 24, 72, 243, 141, 128, 195, 78, 66,
                215, 61, 156, 180 };
        _defaultSize = 35;

        for (int i = 0; i < 256; i++) {
            _p[256 + i] = _p[i] = _permutation[i];
        }
    }

    internal double Noise(double x, double y) {
        double value = 0, size = _defaultSize, initialSize = size;

        while (size >= 1) {
            value += SmoothNoise(x / size, y / size, 0f) * size;
            size /= 2.0f;
        }
        return value / initialSize;
    }

    internal double SmoothNoise(double x, double y, double z) {
        x += seed; y += seed; z += seed;

        int X = (int)Math.Floor(x) & 255;
        int Y = (int)Math.Floor(y) & 255;
        int Z = (int)Math.Floor(z) & 255;

        x -= Math.Floor(x);
        y -= Math.Floor(y);
        z -= Math.Floor(z);

        double u = Fade(x);
        double v = Fade(y);
        double w = Fade(z);

        int A = _p[X] + Y;
        int AA = _p[A] + Z;
        int AB = _p[A + 1] + Z;
        int B = _p[X + 1] + Y;
        int BA = _p[B] + Z;
        int BB = _p[B + 1] + Z;

        return Lerp(w, Lerp(v, Lerp(u, Grad(_p[AA], x, y, z),
                                Grad(_p[BA], x - 1, y, z)),
                        Lerp(u, Grad(_p[AB], x, y - 1, z),
                                Grad(_p[BB], x - 1, y - 1, z))),
                Lerp(v, Lerp(u, Grad(_p[AA + 1], x, y, z - 1),
                                Grad(_p[BA + 1], x - 1, y, z - 1)),
                        Lerp(u, Grad(_p[AB + 1], x, y - 1, z - 1),
                                Grad(_p[BB + 1], x - 1, y - 1, z - 1))));
    }

    private static double Fade(double t) {
        return t * t * t * (t * (t * 6 - 15) + 10);
    }

    private static double Lerp(double t, double a, double b) {
        return a + t * (b - a);
    }

    private static double Grad(int hash, double x, double y, double z) {
        int h = hash & 15;
        double u = h < 8 ? x : y,
                v = h < 4 ? y : h == 12 || h == 14 ? x : z;
        return ((h & 1) == 0 ? u : -u) + ((h & 2) == 0 ? v : -v);
    }
}
