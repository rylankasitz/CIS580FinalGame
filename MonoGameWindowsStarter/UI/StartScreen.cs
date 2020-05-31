using ECSEngine.ECSCore;
using Engine;
using Engine.Systems;
using GeonBit.UI;
using GeonBit.UI.Entities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameWindowsStarter.UI
{
    public class StartScreen : UIScreen
    {
        private Button playButton;

        public override void Initialize()
        {
            Panel panel = new Panel(new Vector2(400, 400), PanelSkin.Default, Anchor.Center);
            playButton = new Button("PLAY", ButtonSkin.Default, Anchor.BottomCenter);

            playButton.OnClick = onPlay;

            Bind("Start", playButton);

            panel.AddChild(new Header("SOUL CRAWL"));
            panel.AddChild(new HorizontalLine());           
            panel.AddChild(playButton);

            AddEntity(panel);
        }

        public override void Update()
        {

        }

        private void onPlay(Entity uiEntity)
        {
            SceneManager.LoadScene("Dugneon");
        }
    }
}
