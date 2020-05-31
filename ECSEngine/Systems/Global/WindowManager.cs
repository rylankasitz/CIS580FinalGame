using Comora;
using Engine.Componets;
using Microsoft.Xna.Framework;
using Penumbra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECSEngine.Systems
{
    public static class WindowManager
    {
        #region Basic Properties

        public static int Width { get; set; } = 1536;
        public static int Height { get; set; } = 864;
        public static Color BackgroundColor { get; set; } = Color.White;

        #endregion

        #region Debug Properties

        public static bool ShowCollisionsDetails { get; set; } = false;
        public static bool ShowCamerDetails { get; set; } = false;
        public static bool ShowLightDetails { get; set; } = false;

        #endregion

        #region Camera Properties 

        public static Camera Camera { get; set; }
        public static Vector CameraPosition
        {
            get
            {
                return new Vector(Camera.Position.X, Camera.Position.Y);
            }
            set
            {
                Camera.Position = new Vector(value.X, value.Y).Floor();
            }
        }

        public static Vector ToScreenPosition(Vector worldPosition)
        {
            Vector2 screenPosition = Vector2.Zero;
            Vector2 pos = worldPosition;
            Camera.ToScreen(ref pos, out screenPosition);

            return new Vector(screenPosition.X, screenPosition.Y) + new Vector(Width/2, Height/2);
        }

        #endregion

        #region Lighting Properties

        public static PenumbraComponent Penumbra { get; set; }
        public static Color AmbientColor { get => Penumbra.AmbientColor; set => Penumbra.AmbientColor = value; }

        #endregion
    }
}
