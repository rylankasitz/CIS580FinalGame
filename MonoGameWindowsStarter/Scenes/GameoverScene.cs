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
    public class GameoverScene : Scene
    {
        public override void Initialize()
        {
            TextUI gameoverText = CreateEntity<TextUI>();
            gameoverText.Transform.Position = new Vector(WindowManager.Width, WindowManager.Height - 200)/2;
            gameoverText.Transform.Scale = new Vector(2, 2);
            gameoverText.Text.Text = "GAMEOVER";
            gameoverText.Text.Color = Color.White;
            gameoverText.Text.Center = true;

            TextUI restartText = CreateEntity<TextUI>();
            restartText.Transform.Position = new Vector(WindowManager.Width, WindowManager.Height + 200) / 2;
            restartText.Transform.Scale = new Vector(1, 1);
            restartText.Text.Text = "RESTART";
            restartText.Text.Color = Color.White;
            restartText.Text.Center = true;

            ButtonUI restartButton = CreateEntity<ButtonUI>();
            restartButton.Transform.Scale = new Vector(400, 140);
            restartButton.Transform.Position = (new Vector(WindowManager.Width, WindowManager.Height + 200) / 2) -
                                               (restartButton.Transform.Scale/2);
            restartButton.Sprite.ContentName = "Button";
            restartButton.Sprite.Color = Color.White;
            restartButton.OnClick = restartOnclick;
        }

        public override void Update(GameTime gameTime)
        {
            
        }

        private void restartOnclick()
        {
            SceneManager.LoadScene("Main");
        }
    }
}
