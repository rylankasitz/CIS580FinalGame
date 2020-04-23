using ECSEngine.Systems;
using Engine.ECSCore;
using Engine.Systems;
using Microsoft.Xna.Framework;
using MonoGameWindowsStarter.Entities;
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
            CreateEntity<Player>();
        }

        public override void Update(GameTime gameTime)
        {
            
        }
    }
}
