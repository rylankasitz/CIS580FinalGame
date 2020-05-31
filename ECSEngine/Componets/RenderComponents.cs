using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Engine.ECSCore;
using Engine.Systems;
using System.Collections.Generic;
using System.Diagnostics;
using Penumbra;

namespace Engine.Componets
{
    public class Sprite : Component
    {
        public string ContentName { get; set; } = "test";
        public Rectangle SpriteLocation { get; set; }
        public Color Color { get; set; } = Color.White;
        public float Layer { get; set; }
        public SpriteEffects SpriteEffects { get; set; }
        public float Fill { get; set; }
        public Sprite() { }
        public Sprite(string ContentName, int SpriteX = 0, int SpriteY = 0, int SpriteWidth = 0, int SpriteHeight = 0, float Layer = 1f,
            SpriteEffects SpriteEffects = SpriteEffects.None, float Fill = 1)
        {
            this.ContentName = ContentName;
            this.Layer = Layer;
            this.SpriteEffects = SpriteEffects;
            this.Fill = Fill;

            SpriteLocation = new Rectangle(SpriteX, SpriteY, SpriteWidth, SpriteHeight);
        }
    }

    public class Light : Component
    {
        public Vector Position { get; set; }
        public PointLight PointLight { get; set; }
        public ShadowType ShadowType { get; set; }
        public Light() 
        {
            PointLight = new PointLight();
            Position = new Vector(0, 0);
        }
    }

    public class Animation : Component 
    {
        public string CurrentAnimation { get; set; } = string.Empty;
        public Dictionary<string, AnimationTracker> AnimationTracker { get; set; } = new Dictionary<string, AnimationTracker>();
        public Vector AnimationScale { get; set; } = new Vector(0, 0);
        public bool Playing { get; set; } = false;
        public string PlayeringAnimation { get; set; } = "";
        public Dictionary<string, BoxCollision> CurrentCollisions = new Dictionary<string, BoxCollision>();
        public Animation() { }
        public Animation(string CurrentAnimation)
        {
            this.CurrentAnimation = CurrentAnimation;
        }

        public void Play(string animation)
        {
            Playing = true;
            PlayeringAnimation = animation;

            if (AnimationTracker.ContainsKey(animation))
            {
                AnimationTracker[animation].FrameNumber = 0;
            }
        }
    }
}
