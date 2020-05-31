using ECSEngine.Systems;
using Engine;
using Engine.Componets;
using Engine.ECSCore;
using Microsoft.Xna.Framework;
using MonoGameWindowsStarter.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameWindowsStarter.Scenes
{
    public class StartScene : Scene
    {
        public override void Initialize()
        {
            AddUIScreen("Start Screen", new StartScreen());

            LoadUIScreen("Start Screen");
        }

        public override void Update(GameTime gameTime)
        {

        }
    }
}
