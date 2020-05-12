using ECSEngine.Systems;
using Engine;
using Engine.Systems;
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
        GameoverScene gameoverScene;
        PlayScene playScene;
        EndScreen endScreen;

        public override void AddScenes()
        {
            SceneManager.AddScene(mainScene = new MainScene());
            SceneManager.AddScene(gameoverScene = new GameoverScene());
            SceneManager.AddScene(playScene = new PlayScene());
            SceneManager.AddScene(endScreen = new EndScreen());

            mainScene.Name = "Main";
            gameoverScene.Name = "Gameover";
            playScene.Name = "Play";
            endScreen.Name = "End Screen";
        }

        public override void Initialize()
        {
            SceneManager.LoadScene("Play");

            Camera.Position.X = WindowManager.Width / 2;
            Camera.Position.Y = WindowManager.Height / 2;

            WindowManager.BackgroundColor = new Color(25, 23, 22);
            WindowManager.MouseTexture = "MouseIcon";

            //WindowManager.Debug = true;
        }

        public override void Update(GameTime gameTime)
        {
            
        }
    }
}
