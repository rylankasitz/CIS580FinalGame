using Engine.Componets;
using Microsoft.Xna.Framework;
using MonoGameWindowsStarter.Entities;
using MonoGameWindowsStarter.GlobalValues;
using MonoGameWindowsStarter.Scenes.MapGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameWindowsStarter.Scenes.Rooms
{
    public class Room
    {
        public Rectangle Dimensions { get => dimensions; }
        public Vector2 Center { get => dimensions.Center.ToVector2(); }
        public Vector GlobalCoords { get => new Vector(Center.X, Center.Y) * MapConstants.TileSize; }
        public int Right { get => dimensions.Right; }
        public int Left { get => dimensions.Left; }
        public int Top { get => dimensions.Top; }
        public int Bottom { get => dimensions.Bottom;  }

        public List<Rectangle> Doors { get; set; }
        public List<HallWay> HallWays { get; set; }
        public int Number { get; }

        private RoomLayout layout { get; set; }
        private Rectangle dimensions;

        public Room(int x, int y, int w, int h, int number)
        {
            dimensions = new Rectangle(x, y, w, h);
            HallWays = new List<HallWay>();
            Doors = new List<Rectangle>();
            Number = number;
            layout = new RoomLayout(Dimensions, Doors);
        }

        #region Public Methods

        public void Load()
        {
            layout.CreateEntities();
        }

        public void Unload()
        {
            foreach (HallWay hallWay in HallWays)
            {
                hallWay.Unload();
            }

            layout.RemoveEntites();
        }

        public Vector2 GetOverlapV(Room room, int hallHeight)
        {
            Vector2 overlap = new Vector2(Math.Max(Left, room.Left), Math.Min(Right, room.Right) - hallHeight);

            if (overlap.X >= overlap.Y)
                return new Vector2(Left, Right - hallHeight);

            return overlap;
        }

        public bool Intersects(Room room, int minDistance)
        {
            Rectangle r1 = new Rectangle(dimensions.X - minDistance, dimensions.Y - minDistance, 
                                         dimensions.Width + minDistance, dimensions.Height + minDistance);
            Rectangle r2 = new Rectangle(room.dimensions.X - minDistance, room.dimensions.Y - minDistance,
                                         room.dimensions.Width + minDistance, room.dimensions.Height + minDistance);
            return r1.Intersects(r2);
        }

        public bool Intersects(Player player)
        {
            Point pos = new Vector2(player.Transform.Position.X + player.Transform.Scale.X / 2,
                        player.Transform.Position.Y + player.Transform.Scale.Y / 2).ToPoint();
            Point scale = new Vector2(MapConstants.TileSize.X, MapConstants.TileSize.Y).ToPoint();

            Rectangle worldDimentions = new Rectangle(dimensions.X * (int) MapConstants.TileSize.X,
                                                      dimensions.Y * (int) MapConstants.TileSize.Y,
                                                      dimensions.Width * (int) MapConstants.TileSize.X,
                                                      dimensions.Height * (int) MapConstants.TileSize.Y);

            return worldDimentions.Contains(pos);
        }

        public bool HasConnection(Room room)
        {
            foreach(HallWay hallWay in HallWays)
            {
                if (hallWay.Destination == room)
                {
                    return true;
                }
            }

            return false;
        }

        public Room FindClosestRoom(Room[] rooms)
        {
            float minDistance = float.MaxValue;

            Room closestRoom = null;

            for (int i = 0; i < rooms.Length; i++)
            {
                float newDistance = getDistance(rooms[i]);

                if (newDistance < minDistance)
                {
                    minDistance = newDistance;
                    closestRoom = rooms[i];
                }
            }

            return closestRoom;
        }

        #endregion

        #region Private Methods

        private float getDistance(Room room)
        {
            Vector2 diff = Center - room.Center;
            return diff.Length();
        }

        #endregion
    }

}
