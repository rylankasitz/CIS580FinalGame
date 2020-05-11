using ECSEngine.Systems;
using Engine;
using Engine.Componets;
using Engine.ECSCore;
using Engine.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameWindowsStarter.Scenes;
using MonoGameWindowsStarter.Scenes.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameWindowsStarter.UI
{
    public class MiniMap
    {
        private Vector roomScale = new Vector(8, 8);

        private MinimapRoom[,] rooms;
        private MiniMapObj background;
        private MiniMapObj playerLocation;
        private Vector mapScale;
        private int width;
        private int height;

        public MiniMap(Room[,] roomLayout, int width, int height)
        {
            this.width = width;
            this.height = height;
            playerLocation = SceneManager.GetCurrentScene().CreateEntity<MiniMapObj>();
            playerLocation.Sprite.ContentName = "MinimapPlayer";
            playerLocation.Transform.Scale = roomScale * MapManager.Scale;
            playerLocation.Sprite.Layer = .1f;
            rooms = new MinimapRoom[width, height];

            generateMapUI(roomLayout);
        }

        public void Enable()
        {
            background.Sprite.Enabled = true;
            playerLocation.Sprite.Enabled = true;
            foreach(MinimapRoom room in rooms)
            {
                room.SetEnabled(true);
            }
        }

        public void Disable()
        {
            background.Sprite.Enabled = false;
            playerLocation.Sprite.Enabled = false;
            foreach (MinimapRoom room in rooms)
            {
                room.SetEnabled(false);
            }
        }

        public bool isDiscoverd(int x, int y)
        {
            return rooms[x, y].Discovered;
        }

        public void Discover(int x, int y)
        {
            rooms[x, y].Discovered = true;
            rooms[x, y].SetEnabled(true);
            SetCurrentRoom(x, y);
        }

        private void SetCurrentRoom(int x, int y)
        {
            playerLocation.Transform.Position = new Vector(x, y) * MapManager.Scale * roomScale + 
                new Vector(WindowManager.Width, WindowManager.Height) / 2 - (mapScale / 2);
        }

        private void generateMapUI(Room[,] roomLayout)
        {
            mapScale = new Vector(width, height) * roomScale * MapManager.Scale;
            background = SceneManager.GetCurrentScene().CreateEntity<MiniMapObj>();
            background.Transform.Scale = mapScale;
            background.Transform.Position = new Vector(WindowManager.Width, WindowManager.Height) / 2 - mapScale / 2;
            background.Sprite.Layer = .13f;
            background.Sprite.ContentName = "MinimapBackground";
            background.Sprite.Color = new Color(Color.White, .7f);

            for (int i = 0; i < width; i++)
            {
                for(int j = 0; j < height; j++)
                {
                    if (roomLayout[i, j] != null)
                    {
                        rooms[i, j] = new MinimapRoom(i, j, roomLayout[i, j].Directions, roomLayout[i, j].Flip, mapScale, roomScale);
                    }
                    else
                    {
                        rooms[i, j] = new MinimapRoom();
                    }
                }
            }
        }
    }

    public class MinimapRoom
    {
        public bool Discovered { get; set; } = false;

        private MiniMapObj room;
        private MiniMapObj[] doors;

        public MinimapRoom()
        {

        }

        public MinimapRoom(int x, int y, string directions, bool flipped, Vector mapScale, Vector roomScale)
        {
            Vector offset = new Vector(WindowManager.Width, WindowManager.Height) / 2 - (mapScale / 2);

            room = SceneManager.GetCurrentScene().CreateEntity<MiniMapObj>();
            room.Transform.Scale = roomScale * MapManager.Scale;
            room.Transform.Position = new Vector(x, y) * MapManager.Scale * roomScale + offset;
            doors = new MiniMapObj[4];

            for(int i = 0; i < doors.Length; i++)
            {
                if (directions[i] == '+')
                {
                    doors[i] = SceneManager.GetCurrentScene().CreateEntity<MiniMapObj>();
                    doors[i].Transform.Position = new Vector(x, y) * MapManager.Scale * roomScale + offset;
                    doors[i].Transform.Scale = roomScale * MapManager.Scale;
                    doors[i].Sprite.Layer = .11f;

                    if (i == 0 || i == 1)
                    {
                        doors[i].Sprite.ContentName = "MiniMapDoorLeft";

                        if ((i == 1 && !flipped) || (i == 0 && flipped))
                            doors[i].Sprite.SpriteEffects = SpriteEffects.FlipHorizontally;
                    }
                    else
                    {
                        doors[i].Sprite.ContentName = "MinimapDoorUp";

                        if (i == 3)
                            doors[i].Sprite.SpriteEffects = SpriteEffects.FlipVertically;
                    }
                }
            }
        }

        public void SetEnabled(bool enabled)
        {
            if (room != null)
            {
                room.Sprite.Enabled = enabled && Discovered;
                foreach (MiniMapObj door in doors)
                {
                    if (door != null)
                        door.Sprite.Enabled = enabled && Discovered;
                }
            }
        }
    }

    [Sprite(ContentName: "MiniMapRoom", Layer: .12f)]
    [Transform(X: 0, Y: 0, Width: 6, Height: 6)]
    public class MiniMapObj : Entity
    {
        public Transform Transform;
        public Sprite Sprite;

        public override void Initialize()
        {
            Transform = GetComponent<Transform>();
            Sprite = GetComponent<Sprite>();
        }

        public override void Update(GameTime gameTime)
        {
           
        }
    }
}
