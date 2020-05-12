using Engine.Componets;
using Engine.ECSCore;
using Engine.Systems;
using Microsoft.Xna.Framework;
using MonoGameWindowsStarter.GlobalValues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameWindowsStarter.Entities
{
    [Sprite(ContentName: "CharacterTrail", Layer: .7f)]
    [Animation(CurrentAnimation: "CharacterTrail")]
    [Transform(X: 0, Y: 0, Width: 24, Height: 24)]
    public class Trail : Entity
    {
        public Transform Transform;
        public Sprite Sprite;
        public Animation Animation;

        public override void Initialize()
        {
            Transform = GetComponent<Transform>();
            Sprite = GetComponent<Sprite>();
            Animation = GetComponent<Animation>();

            Sprite.Enabled = false;

            Transform.Scale = Transform.Scale * MapConstants.Scale * 1f;
        }

        public override void Update(GameTime gameTime)
        {

        }
    }
}
