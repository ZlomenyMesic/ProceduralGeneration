//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//


[System.Runtime.InteropServices.DllImport("nvapi64.dll", EntryPoint = "fake")]
static extern int LoadNvApi64();

[System.Runtime.InteropServices.DllImport("nvapi.dll", EntryPoint = "fake")]
static extern int LoadNvApi32();

try {
    if (System.Environment.Is64BitProcess) _ = LoadNvApi64();
    else _ = LoadNvApi32();
} catch { }

var engine = new minecraft_kurwa.src.gui.Engine();
engine.Run();