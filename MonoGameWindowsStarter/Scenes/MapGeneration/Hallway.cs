using Microsoft.Xna.Framework;
using MonoGameWindowsStarter.Scenes.MapGeneration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameWindowsStarter.Scenes.Rooms
{

    public class HallWay
    {
        public Rectangle VerticalConnector { get; set; }
        public Rectangle HorizonalConnector { get; set; }
        public Rectangle Corner { get; set; }
        public Room Source { get; set; }
        public Room Destination { get; set; }

        private int hallWidth = 4;

        private Random random;
        private RoomLayout vlayout;
        private RoomLayout hlayout;
        private RoomLayout clayout;
        private string cornerOrientation;

        #region Public Methods

        public HallWay(Room source, Room desination)
        {
            Source = source;
            Destination = desination;
            Corner = Rectangle.Empty;
            HorizonalConnector = Rectangle.Empty;
            VerticalConnector = Rectangle.Empty;

            random = new Random();
            cornerOrientation = "";

            // Set Dimension
            setHorizontalConnectors();
            setVerticalConnectors();

            if (VerticalConnector != Rectangle.Empty && HorizonalConnector != Rectangle.Empty)
            {
                combineToCorner();
                setCorner();
            }

            // Create layouts
            if (VerticalConnector != Rectangle.Empty)
                vlayout = new RoomLayout(VerticalConnector, new List<Rectangle>(), vhallway: true);
            if (HorizonalConnector != Rectangle.Empty)
                hlayout = new RoomLayout(HorizonalConnector, new List<Rectangle>(), hhallway: true);
            if (Corner != Rectangle.Empty)
                clayout = new RoomLayout(Corner, new List<Rectangle>(), corner: true, cornerOrientation: cornerOrientation);
        }

        public void Load()
        {
            vlayout?.CreateEntities();
            hlayout?.CreateEntities();
            clayout?.CreateEntities();
        }

        public void SetDoors()
        {
            setDoors(Source, HorizonalConnector);
            setDoors(Source, VerticalConnector);
            setDoors(Destination, HorizonalConnector);
            setDoors(Destination, VerticalConnector);
        }

        public bool Intersects(Room[] rooms)
        {
            foreach (Room room in rooms)
            {
                if (VerticalConnector.Intersects(room.Dimensions)) return true;
                if (HorizonalConnector.Intersects(room.Dimensions)) return true;
                if (Corner.Intersects(room.Dimensions)) return true;

                foreach (HallWay hallWay in room.HallWays)
                {
                    if (VerticalConnector.Intersects(hallWay.VerticalConnector)) return true;
                    if (VerticalConnector.Intersects(hallWay.HorizonalConnector)) return true;
                    if (VerticalConnector.Intersects(hallWay.Corner)) return true;

                    if (HorizonalConnector.Intersects(hallWay.VerticalConnector)) return true;
                    if (HorizonalConnector.Intersects(hallWay.HorizonalConnector)) return true;
                    if (HorizonalConnector.Intersects(hallWay.Corner)) return true;

                    if (Corner.Intersects(hallWay.VerticalConnector)) return true;
                    if (Corner.Intersects(hallWay.HorizonalConnector)) return true;
                    if (Corner.Intersects(hallWay.Corner)) return true;
                }
            }

            return false;
        }

        #endregion

        #region Private Methods

        private void setHorizontalConnectors()
        {       
            Point overlap = new Point(Math.Max(Source.Top, Destination.Top), Math.Min(Source.Bottom, Destination.Bottom) - hallWidth);

            if (overlap.X > overlap.Y)
                overlap = new Point(Source.Top, Source.Bottom - hallWidth);

            int x1 = Math.Min(Source.Right, Destination.Right);
            int x2 = Math.Max(Source.Left, Destination.Left);
            int y = random.Next(overlap.X, overlap.Y);

            if (x2 - x1 > 0)
                HorizonalConnector = new Rectangle(x1, y, x2 - x1, hallWidth + 1);
            else if (x2 - x1 > -hallWidth - 1)
                HorizonalConnector = new Rectangle(x1, y, 0, hallWidth + 1);
        }

        private void setVerticalConnectors()
        {
            Point overlap = new Point(Math.Max(Source.Left, Destination.Left), Math.Min(Source.Right, Destination.Right) - hallWidth);

            if (overlap.X > overlap.Y)
                overlap = new Point(Destination.Left, Destination.Right - hallWidth);

            int y1 = Math.Min(Source.Bottom, Destination.Bottom);
            int y2 = Math.Max(Source.Top, Destination.Top);
            int x = random.Next(overlap.X, overlap.Y);

            if (y2 - y1 > 0)
                VerticalConnector = new Rectangle(x, y1, hallWidth, y2 - y1);
            else if (y2 - y1 > -hallWidth)
                VerticalConnector = new Rectangle(x, y1, hallWidth, 0);
        }

        private void combineToCorner()
        {
            int x = Math.Min(HorizonalConnector.X, VerticalConnector.X + hallWidth);
            int width = Math.Max(Source.Left - x, VerticalConnector.X - x);

            HorizonalConnector = new Rectangle(x, HorizonalConnector.Y, Math.Abs(width), HorizonalConnector.Height);

            int y = Math.Min(VerticalConnector.Y, HorizonalConnector.Y + hallWidth);
            int height = Math.Max(Destination.Top - y, HorizonalConnector.Y - y);

            VerticalConnector = new Rectangle(VerticalConnector.X, y, VerticalConnector.Width, Math.Abs(height) + 1);
        }

        private void setCorner()
        {
            Rectangle h = new Rectangle(HorizonalConnector.X - 5, HorizonalConnector.Y, HorizonalConnector.Width + 10, HorizonalConnector.Height);
            Rectangle v = new Rectangle(VerticalConnector.X, VerticalConnector.Y - 5, VerticalConnector.Width, VerticalConnector.Height + 10);
            Corner = Rectangle.Intersect(h, v);

            if (HorizonalConnector.X <= VerticalConnector.X)
            {
                cornerOrientation += "R";
            }
            else
            {
                cornerOrientation += "L";
            }

            if (HorizonalConnector.Y <= VerticalConnector.Y)
            {
                cornerOrientation += "T";
            }
            else
            {
                cornerOrientation += "B";
            }
        }

        private void setDoors(Room room, Rectangle connector)
        {
            if (connector != Rectangle.Empty)
            {
                if (room.Right == connector.Left)
                {
                    room.Doors.Add(new Rectangle(room.Dimensions.Width - 1, connector.Y - room.Dimensions.Y, 1, 3));
                }
                else if (room.Left == connector.Right)
                {
                    room.Doors.Add(new Rectangle(0, connector.Y - room.Dimensions.Y, 1, 4));
                }
                else if (room.Bottom == connector.Top)
                {
                    room.Doors.Add(new Rectangle(connector.X - room.Dimensions.X + 1, room.Dimensions.Height - 2, 2, 2));
                }
                else if (room.Top == connector.Bottom)
                {
                    room.Doors.Add(new Rectangle(connector.X - room.Dimensions.X + 1, 0, 2, 2));
                }
            }
        }

        #endregion
    }
}
