using System;
using System.IO;
using Engine;

namespace MonoGameWindowsStarter
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (var game = new GameManager(new Game(), Directory.GetCurrentDirectory() + "/Content"))
                game.Run();
        }
    }
#endif
}
