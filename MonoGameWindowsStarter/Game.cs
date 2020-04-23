﻿using Engine;
using Microsoft.Xna.Framework;
using MonoGameWindowsStarter.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameWindowsStarter
{
    public class Game : GameLayout
    {
        MainScene mainScene;

        public override void AddScenes()
        {           
            SceneManager.AddScene(mainScene = new MainScene());

            mainScene.Name = "Main Scene";
        }

        public override void Initialize()
        {
            SceneManager.LoadScene("Main Scene");
        }

        public override void Update(GameTime gameTime)
        {
            
        }
    }
}