using Engine.ECSCore;
using Engine.Componets;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Systems;
using Microsoft.Xna.Framework.Input;
using MonoGameWindowsStarter.Utils;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameWindowsStarter.Entities
{
    [Sprite(ContentName: "MapTileSet", Layer: 0)]
    [Animation(CurrentAnimation: "Walk")]
    [Transform(X: 0, Y: 0, Width: 32, Height: 32)]
    [Physics(VelocityX: 0, VelocityY: 0)]
    [BoxCollision(X: 0, Y: 0, Width: 1, Height: 1)]
    public class Player : Entity
    {
        public AnimationState AnimationState;
        public float Speed = 5f;

        private Sprite sprite;
        private Animation animation;
        private Transform transform;
        private BoxCollision boxCollision;
        private Physics physics;

        public override void Initialize()
        {
            sprite = GetComponent<Sprite>();
            animation = GetComponent<Animation>();
            transform = GetComponent<Transform>();
            boxCollision = GetComponent<BoxCollision>();
            physics = GetComponent<Physics>();

            AnimationState = new AnimationState("Default");

            boxCollision.HandleCollision = handleCollision;
            animation.CurrentAnimation = AnimationState.Idle;
        }

        public override void Update(GameTime gameTime)
        {
            move();
            animate();
        }

        private void move()
        {
            physics.Velocity = new Vector(0, 0);

            if(InputManager.KeyPressed(Keys.W))
            {
                physics.Velocity.Y = -Speed;
            }
            else if (InputManager.KeyPressed(Keys.S))
            {
                physics.Velocity.Y = Speed;
            }

            if (InputManager.KeyPressed(Keys.D))
            {
                physics.Velocity.X = Speed;
            }
            else if (InputManager.KeyPressed(Keys.A))
            {
                physics.Velocity.X = -Speed;
            }

            if (physics.Velocity != Vector2.Zero) 
            {
                Vector2 normalized = new Vector2(physics.Velocity.X, physics.Velocity.Y);
                normalized.Normalize();
                physics.Velocity = new Vector(normalized.X, normalized.Y) * Speed;
            }
        }

        private void animate()
        {
            if (physics.Velocity.X != 0 || physics.Velocity.Y != 0)
            {
                animation.CurrentAnimation = AnimationState.Walk;

                if (physics.Velocity.X < 0)
                {
                    sprite.SpriteEffects = SpriteEffects.FlipHorizontally;
                }
                else if (physics.Velocity.X > 0)
                {
                    sprite.SpriteEffects = SpriteEffects.None;
                }
            }
            else
            {
                animation.CurrentAnimation = AnimationState.Idle;
            }
        }

        private void handleCollision(Entity entity, string direction)
        {

        }
    }
}
