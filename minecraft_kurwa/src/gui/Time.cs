//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

using System;
using System.Diagnostics;

namespace minecraft_kurwa.src.gui {
    internal static class Time {
        private static Stopwatch loadTime; // how much time did it take to generate the terrain and start the application
        private static Stopwatch fpsCounter; // used to count frames per second

        private static uint frames; // number of frames rendered in last second
        private static byte lastFPS; // last fps value

        internal static long LoadTime { get => loadTime.ElapsedMilliseconds; }
        internal static byte LastFPS { get => lastFPS; }

        internal static void Initialize() {
            loadTime = new();
            loadTime.Start();

            fpsCounter = new();
        }

        internal static void Update() {
            if (loadTime.IsRunning) loadTime.Stop();

            if (!fpsCounter.IsRunning) fpsCounter.Start();

            // we are over one second so we restart the timer and reset the frames
            if (fpsCounter.IsRunning && fpsCounter.ElapsedMilliseconds > 1000) {
                lastFPS = (byte)Math.Round(frames * 1000d / fpsCounter.ElapsedMilliseconds, 0);
                frames = 0;
                fpsCounter.Restart();
            } 
            else frames++;
        }
    }
}
