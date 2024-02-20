using MinecraftSharp.Classes.Display;
using MinecraftSharp.Classes.Network;
using MinecraftSharp.Classes.Utils;
using System.Diagnostics;

Logging.Initialize();
Settings.Initialize("settings.txt");
IOManager.Initialize(new mAddr((string)Settings.GetValue("io.host")), (int)Settings.GetValue("io.threads"));

Process.GetCurrentProcess().WaitForExit();