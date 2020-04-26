using Engine;
using Engine.Componets;
using Engine.ECSCore;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameWindowsStarter.UI
{
    public class SliderBar
    {
        private SliderBarBG healthBarBG;
        private SliderBarFG healthBarFG;

        public SliderBar(string foreground, string background, Vector position, Vector scale, 
            Rectangle sourceBG, Rectangle sourceFG)
        {
            healthBarBG = SceneManager.GetCurrentScene().CreateEntity<SliderBarBG>();
            healthBarFG = SceneManager.GetCurrentScene().CreateEntity<SliderBarFG>();

            healthBarBG.GetComponent<Transform>().Position = new Vector(position.X, position.Y);
            healthBarFG.GetComponent<Transform>().Position = new Vector(position.X, position.Y);

            healthBarBG.GetComponent<Transform>().Scale = new Vector(scale.X, scale.Y);
            healthBarFG.GetComponent<Transform>().Scale = new Vector(scale.X, scale.Y);

            healthBarBG.GetComponent<Sprite>().ContentName = background;
            healthBarFG.GetComponent<Sprite>().ContentName = foreground;

            healthBarBG.GetComponent<Sprite>().SpriteLocation = sourceBG;
            healthBarFG.GetComponent<Sprite>().SpriteLocation = sourceFG;
        }

        public void UpdateFill(float fill)
        {
            healthBarFG.GetComponent<Sprite>().Fill = fill;
        }
    }

    [Sprite(ContentName: "MapTileSet", Layer: 0)]
    [Transform(X: 0, Y: 0, Width: 32, Height: 32)]
    public class SliderBarBG : Entity
    {
        public override void Initialize()
        {
            
        }

        public override void Update(GameTime gameTime)
        {
            
        }
    }

    [Sprite(ContentName: "MapTileSet", Layer: 0, Fill: 1)]
    [Transform(X: 0, Y: 0, Width: 32, Height: 32)]
    public class SliderBarFG : Entity
    {
        public override void Initialize()
        {

        }

        public override void Update(GameTime gameTime)
        {

        }
    }
}
