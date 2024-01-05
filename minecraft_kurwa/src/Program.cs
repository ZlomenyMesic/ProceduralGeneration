//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using minecraft_kurwa.src.launcher;
using minecraft_kurwa.src.gui;

namespace minecraft_kurwa.src;

static class Program {
    [DllImport("nvapi64.dll", EntryPoint = "fake")]
    static extern int LoadNvApi64();

    [DllImport("nvapi.dll", EntryPoint = "fake")]
    static extern int LoadNvApi32();

    static void Main() {
        var launcher = new MCKurwaLauncher();

        if (launcher.ShowDialog() == DialogResult.OK) {
            RunEngine();
        }
    }

    static void RunEngine() {
        try {
            if (Environment.Is64BitProcess) _ = LoadNvApi64();
            else _ = LoadNvApi32();
        } catch { }

        var engine = new Engine();
        engine.Run();
    }
}