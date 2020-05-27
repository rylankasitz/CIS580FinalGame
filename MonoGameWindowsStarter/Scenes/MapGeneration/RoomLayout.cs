using Engine.Componets;
using Engine.Systems;
using Microsoft.Xna.Framework;
using MonoGameWindowsStarter.GlobalValues;
using MonoGameWindowsStarter.Scenes.Rooms;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameWindowsStarter.Scenes.MapGeneration
{
    public class RoomLayout
    {
        private float backgroundLayer = 1f;
        private float forgroundLayer = .7f;

        private string[,] layout;
        private float[,] layers;

        private int width;
        private int height;

        private bool vhallway;
        private bool hhallway;
        private bool corner;

        private string cornerOrientation;

        private Rectangle dimensions;
        private List<Rectangle> doors;

        public RoomLayout(Rectangle dimensions, List<Rectangle> doors, bool vhallway = false, bool hhallway = false, bool corner = false, string cornerOrientation = "")
        {
            width = dimensions.Width;
            height = dimensions.Height;

            this.dimensions = dimensions;
            this.doors = doors;

            layout = new string[width, height];
            layers = new float[width, height];

            this.vhallway = vhallway;
            this.hhallway = hhallway;
            this.corner = corner;
            this.cornerOrientation = cornerOrientation;
        }

        public void CreateEntities()
        {
            if (vhallway)
            {
                generateVHallway();
            }
            else if (hhallway)
            {
                generateHHallway();
            }
            else if (corner)
            {
                generateCorner();
            }
            else
            {
                generateRoom();
            }

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    MapManager.CreateObjectFromTileSet(layout[x, y], "DungeonTileSet", 
                        new Vector(dimensions.X + x, dimensions.Y + y) * MapConstants.TileSize, 
                        MapConstants.TileSize, layers[x, y]);
                }
            }
        }

        #region Generation Methods

        private void generateRoom()
        {
            addHorizontalWall(1, width - 2, 0, 1f);
            addHorizontalWall(0, width - 1, height - 2, .7f);

            addVerticalWall(0, height - 3, 0, .7f);
            addVerticalWall(0, height - 3, width - 1, .7f);

            addFloor(new Rectangle(1, 2, width - 2, height - 4), backgroundLayer);

            foreach (Rectangle door in doors) 
            {
                addDoors(door);
            }
        }

        private void generateVHallway()
        {
            addVerticalWall(0, height, 0, .7f);
            addVerticalWall(0, height, width - 1, .7f);
            addFloor(new Rectangle(1, 0, width - 2, height), backgroundLayer);
        }

        private void generateHHallway()
        {
            addHorizontalWall(0, width - 1, 0, 1f);
            addHorizontalWall(0, width - 1, height - 2, .7f);
            addFloor(new Rectangle(0, 2, width, height - 4), backgroundLayer);
        }

        private void generateCorner()
        {
            addCorner(new Rectangle(0, 0, width, height), cornerOrientation);
        }

        #endregion

        #region Generating Map Parts

        private void addDoors(Rectangle door)
        {
            if (door.Width == 1)
            {
                addHorizontalWall(door.X, door.X, door.Y, backgroundLayer);
                addDoor(door.X, door.Y + 2);
            }
            else if (door.Width == 2)
            {
                addFloor(new Rectangle(door.Location, new Point(2, 1)), backgroundLayer);
                addDoor(door.X, door.Y + 1);
                addDoor(door.X + 1, door.Y + 1);

                if (door.Y == height - 2)
                {
                    addVerticalWall(door.Y, door.Y + 1, door.X - 1, forgroundLayer);
                    addVerticalWall(door.Y, door.Y + 1, door.X + door.Width, forgroundLayer);
                }

            }
        }

        private void addDoor(int x, int y)
        {
            if (x >= 0 && y >= 0 && x < width && y < height)
            {
                layout[x, y] = "Door";
                layers[x, y] = backgroundLayer;
            }
        }

        private void addCorner(Rectangle area, string orientation)
        {
            addFloor(area, backgroundLayer);

            if (orientation.Contains("T"))
            {
                addHorizontalWall(area.X, area.X + area.Width, area.Y, backgroundLayer);

                if (orientation.Contains("L"))
                {
                    addVerticalWall(area.Y + area.Height - 1, area.Y + area.Height - 1, area.X + area.Width - 1, forgroundLayer);
                }
                else if (orientation.Contains("R"))
                {
                    addVerticalWall(area.Y + area.Height - 1, area.Y + area.Height - 1, area.X, forgroundLayer);
                }
            }

            if (orientation.Contains("R"))
            {
                addVerticalWall(area.Y, area.Y + area.Height, area.X + area.Width - 1, forgroundLayer);
            }
            if (orientation.Contains("L"))
            {
                addVerticalWall(area.Y, area.Y + area.Height, area.X, forgroundLayer);                
            }

            if (orientation.Contains("B"))
            {
                addHorizontalWall(area.X, area.X + area.Width, area.Y + area.Height - 2, forgroundLayer);

                if (orientation.Contains("L"))
                {
                    addHorizontalWall(area.X + area.Width - 1, area.X + area.Width - 1, area.Y, backgroundLayer);
                }
                else if (orientation.Contains("R"))
                {
                    addHorizontalWall(area.X, area.X, area.Y, backgroundLayer);
                }
            }
        }

        private void addHorizontalWall(int start, int end, int y, float layer)
        {
            for (int i = start; i <= end; i++)
            {
                if (i < width && y < height - 1 && i >= 0 && y >= 0)
                {
                    layout[i, y] = "Ceiling-H";
                    layers[i, y] = layer;
                    layout[i, y + 1] = "Wall";
                    layers[i, y + 1] = layer;
                }
            }
        }

        private void addVerticalWall(int start, int end, int x, float layer)
        {
            for(int i = start; i <= end; i++)
            {
                if (x < width && i < height && i >= 0 && x >= 0)
                {
                    layout[x, i] = "Ceiling-V";
                    layers[x, i] = layer;
                }
            }
        }

        private void addFloor(Rectangle area, float layer)
        {
            for(int i = area.X; i < area.X + area.Width; i++)
            {
                for(int j =  area.Y; j < area.Y + area.Height; j++)
                {
                    if (i >= 0 && j >= 0 && i < width && j < height)
                    {
                        layout[i, j] = "Floor";
                        layers[i, j] = layer;
                    }
                }
            }
        }

        #endregion
    }
}
