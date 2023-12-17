//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

using System;
using System.Diagnostics;

namespace minecraft_kurwa.src.gui {
    internal static class Time {
        private static Stopwatch _loadTime; // how much time did it take to generate the terrain and start the application
        private static Stopwatch _fpsCounter; // used to count frames per second

        private static uint _frames; // number of frames rendered in last second
        private static byte _lastFPS; // last fps value

        internal static long LoadTime { get => _loadTime.ElapsedMilliseconds; }
        internal static byte LastFPS { get => _lastFPS; }

        internal static void UpdateLoadTime() {
            if (_loadTime == null) {
                _loadTime = new();
                _loadTime.Start();
            }
            else _loadTime.Stop();
        }

        internal static void UpdateFPS() {
            _fpsCounter ??= new();

            if (!_fpsCounter.IsRunning)
                _fpsCounter.Start();

            // we are over one second so we restart the timer and reset the frames
            if (_fpsCounter.IsRunning && _fpsCounter.ElapsedMilliseconds > 1000) {
                _lastFPS = (byte)Math.Round(_frames * 1000d / _fpsCounter.ElapsedMilliseconds, 0);
                _frames = 0;
                _fpsCounter.Restart();
            } 
            else _frames++;
        }
    }
}
