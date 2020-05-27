using ECSEngine.Systems;
using Engine.Componets;
using Engine.ECSCore;
using Engine.Systems;
using Microsoft.Xna.Framework;
using MonoGameWindowsStarter.Characters;
using MonoGameWindowsStarter.Characters.Helpers;
using MonoGameWindowsStarter.Entities;
using MonoGameWindowsStarter.GlobalValues;
using MonoGameWindowsStarter.Scenes.Rooms;
using MonoGameWindowsStarter.UI;
using PlatformLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameWindowsStarter.Scenes
{
    public class MainScene : Scene
    {
        public MapGenerator MapGenerator { get; set; }
        public MiniMap MiniMap { get; set; }
        public Player Player { get; set; }

        public override void Initialize()
        {
            MapConstants.Scale = WindowManager.Width / MapConstants.Size;

            MapGenerator = new MapGenerator();
            MiniMap = new MiniMap(MapGenerator.Rooms);

            MapGenerator.LoadFloor();

            Player = CreateEntity<Player>();
            Player.Transform.Position = MapGenerator.Rooms[0].GlobalCoords;
            MiniMap.MoveTo(Player.Transform.Position);
        }

        public override void Update(GameTime gameTime)
        {
            
        }

        public void LoadRoom(string name, bool flip)
        {
            MapManager.LoadMap(name, this, MapConstants.Scale, flip);
        }
    }
}
