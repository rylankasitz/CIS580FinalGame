using Engine.Componets;
using Engine.ECSCore;
using Engine.Systems;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameWindowsStarter.UI
{
    public delegate void OnClick();

    [Transform(X: 300, Y: 300, Width: 100, Height: 100)]
    [Sprite(ContentName: "MapTileSet", Layer: 0f)]
    public class ButtonUI : Entity
    {
        public Transform Transform;
        public Sprite Sprite;
        public OnClick OnClick;

        public override void Initialize()
        {
            Transform = GetComponent<Transform>();
            Sprite = GetComponent<Sprite>();
        }

        public override void Update(GameTime gameTime)
        {
            if (InputManager.LeftMouseDown() && 
                InputManager.GetMousePosition().X < Transform.Position.X + Transform.Scale.X &&
                InputManager.GetMousePosition().X > Transform.Position.X &&
                InputManager.GetMousePosition().Y < Transform.Position.Y + Transform.Scale.Y &&
                InputManager.GetMousePosition().Y > Transform.Position.Y)
            {
                OnClick();
            }
        }
    }
}
