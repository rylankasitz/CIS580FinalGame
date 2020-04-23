using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Engine.Componets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Systems
{
    public static class Camera
    {
        private static float zoomUpperLimit = 4f;
        private static float zoomLowerLimit = .24f;

        private static float zoom;
        private static Vector pos;
        private static int viewportWidth;
        private static int viewportHeight;
        private static int worldWidth;
        private static int worldHeight;

        public static float Zoom
        {
            get { return zoom; }
            set
            {
                zoom = value;
                if (zoom < zoomLowerLimit)
                    zoom = zoomLowerLimit;
                if (zoom > zoomUpperLimit)
                    zoom = zoomUpperLimit;
            }
        }

        public static float Rotation { get; set; } = 0f;

        public static Vector Position
        {
            get { return pos; }
            set
            {
                float leftBarrier = (float)viewportWidth * .5f / zoom;
                float rightBarrier = worldWidth - (float)viewportWidth * .5f / zoom;
                float topBarrier = worldHeight - (float)viewportHeight * .5f / zoom;
                float bottomBarrier = (float)viewportHeight * .5f / zoom;
                pos = value;
                if (pos.X < leftBarrier)
                    pos.X = leftBarrier;
                if (pos.X > rightBarrier)
                    pos.X = rightBarrier;
                if (pos.Y > topBarrier)
                    pos.Y = topBarrier;
                if (pos.Y < bottomBarrier)
                    pos.Y = bottomBarrier;
            }
        }

        public static void Move(Vector amount)
        {
            pos += amount;
        }

        public static void Intitialize(Viewport viewport, int wWidth, int wHeight)
        {
            zoom = 1;
            pos = new Vector(0,0);
            viewportWidth = viewport.Width;
            viewportHeight = viewport.Height;
            worldWidth = wWidth;
            worldHeight = wHeight;
        }

        public static Matrix GetTransformation()
        {
            return Matrix.CreateTranslation(new Vector3(-pos.X, -pos.Y, 0)) *
                   Matrix.CreateRotationZ(Rotation) *
                   Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
                   Matrix.CreateTranslation(new Vector3(viewportWidth * 0.5f, viewportHeight * 0.5f, 0));
        }
    }
}
