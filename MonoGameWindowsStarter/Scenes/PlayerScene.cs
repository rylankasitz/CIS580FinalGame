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
    public class PlayScene : Scene
    {
        public override void Initialize()
        {
            TextUI title = CreateEntity<TextUI>();
            title.Transform.Position = new Vector(WindowManager.Width, WindowManager.Height - 200) / 2;
            title.Transform.Scale = new Vector(2, 2);
            title.Text.Text = "DUNGEON CRALW";
            title.Text.Color = Color.White;
            title.Text.Center = true;

            TextUI playerText = CreateEntity<TextUI>();
            playerText.Transform.Position = new Vector(WindowManager.Width, WindowManager.Height + 200) / 2;
            playerText.Transform.Scale = new Vector(1, 1);
            playerText.Text.Text = "START";
            playerText.Text.Color = Color.White;
            playerText.Text.Center = true;

            ButtonUI playerButton = CreateEntity<ButtonUI>();
            playerButton.Transform.Scale = new Vector(400, 140);
            playerButton.Transform.Position = (new Vector(WindowManager.Width, WindowManager.Height + 200) / 2) -
                                               (playerButton.Transform.Scale / 2);
            playerButton.Sprite.ContentName = "Button";
            playerButton.Sprite.Color = Color.White;
            playerButton.OnClick = playOnclick;
        }

        public override void Update(GameTime gameTime)
        {

        }

        private void playOnclick()
        {
            SceneManager.LoadScene("Main");
        }
    }
}
