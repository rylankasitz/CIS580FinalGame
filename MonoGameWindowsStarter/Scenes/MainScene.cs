using ECSEngine.Systems;
using Engine.Componets;
using Engine.ECSCore;
using Engine.Systems;
using Microsoft.Xna.Framework;
using MonoGameWindowsStarter.Characters;
using MonoGameWindowsStarter.Entities;
using MonoGameWindowsStarter.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameWindowsStarter.Scenes
{
    public class MainScene : Scene
    {
        public override void Initialize()
        {
            MapManager.LoadMap("Room1", this, 4);
            
            Player player = CreateEntity<Player>();

            Enemy enemy1 = CreateEntity<Enemy>();
            enemy1.Character = new BlackGhoul();
            enemy1.GetComponent<Transform>().Position = new Vector(300, 300);
        }

        public override void Update(GameTime gameTime)
        {
            
        }
    }
}
