using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECSEngine.Systems
{
    public static class WindowManager
    {
        public static int Width { get; set; } = 1280;
        public static int Height { get; set; } = 720;
        public static Color BackgroundColor { get; set; } = Color.White;
    }
}
