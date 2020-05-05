using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Engine.ECSCore;
using Engine.Systems;
using System.Collections.Generic;
using System.Diagnostics;

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
            SpriteLocation = new Rectangle(SpriteX, SpriteY, SpriteWidth, SpriteHeight);
            this.Fill = Fill;
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
        }
    }

    public class TextDraw : Component
    {
        public string Text { get; set; } = "";
        public Color Color { get; set; } = Color.Black;

        public TextDraw() { }
        public TextDraw(string Text)
        {
            this.Text = Text;
        }
    }

    public class Parallax : Component
    {
        public int Layer { get; set; }
        public float LayerNum { get; set; }
        public bool Repeat { get; set; }
        public float Speed { get; set; }
        public float ElapsedTime { get; set; }
        public Vector RepeatCount { get; set; } = new Vector(0, 1);
        public Matrix Transform
        {
            get
            {
                return Matrix.CreateTranslation(-ElapsedTime * Speed, 0, 0);
            }
        }
        public Parallax() { }
        public Parallax(int Layer = 0, bool Repeat = true, float Speed = 100f)
        {
            this.Layer = Layer;
            this.Repeat = Repeat;
            this.Speed = Speed;
        }
    }
}
