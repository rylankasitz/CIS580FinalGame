using Engine;
using Engine.Componets;
using Engine.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameWindowsStarter.Characters;
using MonoGameWindowsStarter.Characters.Helpers;
using MonoGameWindowsStarter.Entities;
using MonoGameWindowsStarter.GlobalValues;
using MonoGameWindowsStarter.UI;
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
        private int mapWidth = 8;
        private int mapHeight = 8;
        private int currentX, currentY = 0;
        private int keycount = 3;

        private int roomCount = 12;

        private Room[,] roomLayout;
        private Random rand;
        private MainScene scene;
        private EnemySpawner enemySpawner;
        private MiniMap miniMap;

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

            miniMap = new MiniMap(roomLayout, mapWidth, mapHeight);
            removeBlockedDoors();
            miniMap.Disable();
            miniMap.Discover(currentX, currentY);
        }

        #region Public Methods

        public void SetMinimap(bool state)
        {
            if (state)
                miniMap.Enable();
            else
                miniMap.Disable();
        }

        public Vector LoadNextRoom(string name)
        {
            string newDoor = "";

            enemySpawner.RemoveAllEnemies(roomLayout[currentX, currentY].DungeonName);
            removeKey();
            removeBossPortal();
            removeProjectiles();

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

            // Load new room
            scene.LoadRoom(roomLayout[currentX, currentY].MapName, flip);

            if (miniMap.isDiscoverd(currentX, currentY))
            {
                removeBlockedDoors();
            }

            if (flip)
            {
                if (scene.GetEntity<MapObjectCollision>("DoorL") != null)
                {
                    MapObjectCollision doorObj = scene.GetEntity<MapObjectCollision>("DoorL");
                    doorObj.Name = "DoorR";
                    doorObj.GetComponent<Sprite>().SpriteEffects = SpriteEffects.FlipHorizontally;
                }
                    
                else if (scene.GetEntity<MapObjectCollision>("DoorR") != null)
                {
                    MapObjectCollision doorObj = scene.GetEntity<MapObjectCollision>("DoorR");
                    doorObj.Name = "DoorL";
                    doorObj.GetComponent<Sprite>().SpriteEffects = SpriteEffects.FlipHorizontally;
                }              
            }

            Transform door = scene.GetEntity<MapObjectCollision>(newDoor).GetComponent<Transform>();

            enemySpawner.SpawnEnemiesInRoom(roomLayout[currentX, currentY].DungeonName, roomLayout[currentX, currentY].Flip, 1, 4); // change manual values

            MapConstants.KeyRoom = roomLayout[currentX, currentY].HasKey;
            MapConstants.BossPortalRoom = roomLayout[currentX, currentY].HasBossKey;

            if (MapConstants.KeyRoom && miniMap.isDiscoverd(currentX, currentY))
            {
                Key key = SceneManager.GetCurrentScene().CreateEntity<Key>();
                key.Transform.Position = SceneManager.GetCurrentScene().GetEntity<MapObject>("KeySpawn").GetComponent<Transform>().Position;
            }

            if (MapConstants.BossPortalRoom)
            {
                BossPortal boss = SceneManager.GetCurrentScene().CreateEntity<BossPortal>();
                boss.Transform.Position = SceneManager.GetCurrentScene().GetEntity<MapObject>("KeySpawn").GetComponent<Transform>().Position;
            }

            miniMap.Discover(currentX, currentY);

            return door.Position + (door.Scale/2);
        }

        #endregion

        #region Private Methods

        private void removeKey()
        {
            roomLayout[currentX, currentY].HasKey = MapConstants.KeyRoom;
            if (MapConstants.KeyRoom)
            {
                SceneManager.GetCurrentScene().RemoveEntity(SceneManager.GetCurrentScene().GetEntity<Key>("Key"));
            }
        }

        private void removeBossPortal()
        {
            roomLayout[currentX, currentY].HasBossKey = MapConstants.BossPortalRoom;
            if (MapConstants.BossPortalRoom)
            {
                SceneManager.GetCurrentScene().RemoveEntity(SceneManager.GetCurrentScene().GetEntity<BossPortal>("BossPortal"));
            }
        }

        private void removeProjectiles()
        {
            foreach(Projectile projectile in SceneManager.GetCurrentScene().GetEntities<Projectile>())
            {
                if (projectile.Range != -1)
                    SceneManager.GetCurrentScene().RemoveEntity(projectile);
            }
        }

        private void removeBlockedDoors()
        {
            foreach (MapObjectCollision obj in SceneManager.GetCurrentScene().GetEntities<MapObjectCollision>())
            {
                if (obj.Name == "BlockedDoor")
                {
                    SceneManager.GetCurrentScene().RemoveEntity(obj);
                }
            }
        }

        private Vector2 assignMap()
        {
            Vector2 spawn = Vector2.Zero;
            string type = "Room";
            float keyperRoom = roomCount / (float) keycount;
            int assignedRooms = 0;
            int assignedKeys = 0;
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

                        assignedRooms++;

                        if (Math.Floor(keyperRoom * assignedKeys) < assignedRooms && assignedKeys < keycount) 
                        {
                            roomLayout[i, j].HasKey = true;
                            assignedKeys++;
                        }
                    }
                }
            }

            Debug.WriteLine($"Create {assignedKeys} keys");

            return spawn;
        }

        private void generateMap()
        {
            Room spawn = new Room();
            int rC = roomCount;

            int prevX = rand.Next(0, mapWidth);
            int prevY = rand.Next(0, mapHeight);

            roomLayout[prevX, prevY] = spawn;
            rC--;

            while (rC > 0)
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
                    rC--;
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
