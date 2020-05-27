using Engine.Componets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameWindowsStarter.GlobalValues
{
    public static class MapConstants
    {
        public static float Scale { get; set; }
        public static int Size { get; } = 256;
        public static Vector TileSize { get => new Vector(8, 8) * Scale; }
    }
}
