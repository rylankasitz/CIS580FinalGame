using Engine;
using Engine.Componets;
using Engine.Systems;
using Microsoft.Xna.Framework;
using MonoGameWindowsStarter.Characters.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameWindowsStarter.Scenes.Rooms
{
    public class MapGenerator
    {
        private int mapWidth = 6;
        private int mapHeight = 6;
        private int currentX, currentY = 0;

        private float roomCount = 14;

        private Room[,] roomLayout;
        private Random rand;
        private MainScene scene;
        private EnemySpawner enemySpawner;

        private string[] roomNames;

        public MapGenerator(string[] roomNames, MainScene scene)
        {
            rand = new Random();
            roomLayout = new Room[mapWidth, mapHeight];
            enemySpawner = new EnemySpawner();
            this.roomNames = roomNames;
            this.scene = scene;

            generateMap();
            Vector2 spawn = assignMap();
            printMap();

            currentX = (int)spawn.X;
            currentY = (int)spawn.Y;

            scene.LoadRoom(roomLayout[currentX, currentY].MapName, 
                           roomLayout[currentX, currentY].Flip);
        }

        #region Public Methods

        public Vector LoadNextRoom(string name)
        {
            string newDoor = "";
            enemySpawner.RemoveAllEnemies(roomLayout[currentX, currentY].DungeonName);

            if (name == "DoorL")
            {
                currentX--;
                newDoor = "DoorR";
            }
            else if (name == "DoorR")
            {
                currentX++;
                newDoor = "DoorL";
            }
            else if (name == "DoorU")
            {
                currentY--;
                newDoor = "DoorD";
            }
            else if (name == "DoorD")
            {
                currentY++;
                newDoor = "DoorU";
            }

            bool flip = roomLayout[currentX, currentY].Flip;

            scene.LoadRoom(roomLayout[currentX, currentY].MapName, flip);

            if (flip)
            {
                if (scene.GetEntity<MapObjectCollision>("DoorL") != null) 
                    scene.GetEntity<MapObjectCollision>("DoorL").Name = "DoorR";
                else if (scene.GetEntity<MapObjectCollision>("DoorR") != null)
                    scene.GetEntity<MapObjectCollision>("DoorR").Name = "DoorL";
            }

            Transform door = scene.GetEntity<MapObjectCollision>(newDoor).GetComponent<Transform>();

            enemySpawner.SpawnEnemiesInRoom(roomLayout[currentX, currentY].DungeonName, 1, 1); // change manual values

            return door.Position + (door.Scale/2);
        }

        #endregion

        #region Private Methods

        private Vector2 assignMap()
        {
            Vector2 spawn = Vector2.Zero;
            string type = "Room";
            for(int i = 0; i < mapWidth; i++)
            {
                for (int j = 0; j < mapHeight; j++)
                {
                    if (roomLayout[i,j] != null)
                    {
                        Dictionary<string, int> possibleRooms = getPossibleRooms(type, i, j);

                        string roomName = possibleRooms.Keys.ToArray<string>()[rand.Next(0, possibleRooms.Keys.Count)];
                        string[] roomNameParts = roomName.Split('_');

                        roomLayout[i, j].Name = roomNameParts[1];
                        roomLayout[i, j].Type = roomNameParts[0];
                        roomLayout[i, j].Directions = roomNameParts[2];
                        roomLayout[i, j].Flip = possibleRooms[roomName] == 1;
                        roomLayout[i, j].DungeonName = roomNameParts[1] + "-" + i + "," + j;

                        spawn = new Vector2(i, j);
                    }
                }
            }

            return spawn;
        }

        private void generateMap()
        {
            Room spawn = new Room();

            int prevX = rand.Next(0, mapWidth);
            int prevY = rand.Next(0, mapHeight);

            roomLayout[prevX, prevY] = spawn;
            roomCount--;

            while (roomCount > 0)
            {
                List<Vector2> nodes = getAvalibility(prevX, prevY);
  
                Vector2 node = nodes.Count != 0 ? nodes[rand.Next(0, nodes.Count)] : Vector2.Zero;

                if (nodes.Count == 0 || !isTouching((int)node.X, (int)node.Y))
                {
                    prevX = rand.Next(0, mapWidth);
                    prevY = rand.Next(0, mapHeight);
                }
                else
                {             
                    roomLayout[(int)node.X, (int)node.Y] = new Room();

                    prevX = (int)node.X;
                    prevY = (int)node.Y;
                    roomCount--;
                }
            }
        }

        private List<Vector2> getAvalibility(int x, int y)
        {
            List<Vector2> nodes = new List<Vector2>();

            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    int newX = i + x;
                    int newY = j + y;

                    if ((i != 0 && j != 0) || (i == 0 && j == 0) || 
                        newX < 0 || newY < 0 || newX >= mapWidth || newY >= mapHeight) continue;
                    
                    if (roomLayout[newX, newY] == null)
                    {
                        nodes.Add(new Vector2(newX, newY));
                    }
                }
            }

            return nodes;
        }

        private bool isTouching(int x, int y)
        {
            int sides = 0;
            if (x == (mapWidth-1) || x == 0) sides++;
            if (y == (mapHeight-1) || y == 0) sides++;

            sides += getAvalibility(x, y).Count;

            return sides != 4;
        }

        private Dictionary<string, int> getPossibleRooms(string type, int x, int y)
        {
            Dictionary<string, int> filtered = new Dictionary<string, int>();
            List<string> directions = getDirections(x, y);
            foreach (string roomName in roomNames)
            {             
                string[] parts = roomName.Split('_');

                for (int i = 0; i < directions.Count; i++)
                {
                    if (type == parts[0] && parts[2] == directions[i])
                    {
                        filtered.Add(roomName, i);
                        break;  
                    }
                }
            }

            return filtered;
        }

        private List<string> getDirections(int x, int y)
        {
            string l = x > 0 && roomLayout[x - 1, y] != null ? "+" : "-";
            string r = x < mapWidth-1 && roomLayout[x + 1, y] != null ? "+" : "-";
            string u = y > 0 && roomLayout[x, y - 1] != null ? "+" : "-";
            string d = y < mapHeight-1 && roomLayout[x, y + 1] != null ? "+" : "-";
            return new List<string>() { l + r + u + d, r + l + u + d};
        }

        private void printMap()
        {
            for(int i = 0; i < mapHeight; i++)
            {
                for(int j = 0; j < mapWidth; j++)
                {
                    if (roomLayout[j, i] != null)
                    {
                        Debug.Write($" X ");
                    }
                    else
                    {
                        Debug.Write($" * ");
                    }             
                }
                Debug.Write($"\n");
            }
        }

        #endregion
    }
}
