using ECSEngine.Systems;
using Engine.Componets;
using Engine.ECSCore;
using Engine.Systems;
using Microsoft.Xna.Framework;
using MonoGameWindowsStarter.Characters;
using MonoGameWindowsStarter.Characters.Helpers;
using MonoGameWindowsStarter.Entities;
using MonoGameWindowsStarter.Scenes.Rooms;
using MonoGameWindowsStarter.UI;
using PlatformLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameWindowsStarter.Scenes
{
    public class MainScene : Scene
    {
        public MapGenerator MapGenerator { get; set; }
        public Player Player { get; set; }

        private int mapScale;

        public override void Initialize()
        {
            mapScale = WindowManager.Width / 256;

            // Temporarly add map names
            MapGenerator = new MapGenerator(new string[] { 
                "Room_1_+---",
                "Room_2_--+-",
                "Room_3_--++",
                "Room_4_++--",
                "Room_5_-+-+",
                "Room_6_+++-",
                "Room_7_-+++",
                "Room_8_++++",
                "Room_9_+--+",
                "Room_10_---+",
                "Room_11_+-+-",
                "Room_12_++-+"
            }, this);

            Player = CreateEntity<Player>();
        }

        public override void Update(GameTime gameTime)
        {
            
        }

        public void LoadRoom(string name, bool flip)
        {
            MapManager.LoadMap(name, this, mapScale, flip);
        }
    }
}
