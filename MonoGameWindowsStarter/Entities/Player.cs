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
using Microsoft.Xna.Framework.Graphics;
using MonoGameWindowsStarter.Characters;
using MonoGameWindowsStarter.GlobalValues;
using MonoGameWindowsStarter.UI;
using ECSEngine.Systems;
using Engine;
using MonoGameWindowsStarter.Scenes;

namespace MonoGameWindowsStarter.Entities
{
    [Sprite(ContentName: "MapTileSet", Layer: .75f)]
    [Animation(CurrentAnimation: "Idle")]
    [Transform(X: 300, Y: 300, Width: 40, Height: 40)]
    [Physics(VelocityX: 0, VelocityY: 0)]
    [BoxCollision(X: 0, Y: 0, Width: 1, Height: 1)]
    public class Player : Entity
    {
        public Character Character;
        public float TotalHealth;
        public float CurrentHealth;

        private Sprite sprite;
        private Animation animation;
        private Transform transform;
        private BoxCollision boxCollision;
        private Physics physics;
        private SliderBar healthBar;

        private float hitTime = .15f;

        private float elapsedTintTime;

        #region ESC Methods

        public override void Initialize()
        {
            Name = "Player";
            Character = new DefaultCharacter();
            Character.Holder = "Player";
            TotalHealth = Character.MaxHealth;
            CurrentHealth = TotalHealth;
            elapsedTintTime = hitTime;

            sprite = GetComponent<Sprite>();
            animation = GetComponent<Animation>();
            transform = GetComponent<Transform>();
            boxCollision = GetComponent<BoxCollision>();
            physics = GetComponent<Physics>();

            boxCollision.HandleCollision = handleCollision;
            boxCollision.Layer = "Player";

            animation.CurrentAnimation = Character.IdleAnimation;

            healthBar = new SliderBar("HealthBars", "HealthBars",
                                       new Vector(WindowManager.Width * .03f, WindowManager.Height * .03f),
                                       new Vector(WindowManager.Width * .15f, WindowManager.Height * .05f),
                                       new Rectangle(18, 40, 64, 9),
                                       new Rectangle(18, 29, 64, 9));
        }

        public override void Update(GameTime gameTime)
        {
            elapsedTintTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            hitTint();
            move();
            animate();
            attack(gameTime);
        }

        private void handleCollision(Entity entity, string direction)
        {
            if (entity.Name == "Projectile")
            {
                elapsedTintTime = 0;
                sprite.Color = Color.Red;
            }

            if (entity.Name.Contains("Door"))
            {
                MainScene scene = (MainScene) SceneManager.GetCurrentScene();
                transform.Position = scene.MapGenerator.LoadNextRoom(entity.Name);

                if (entity.Name == "DoorL")
                {
                    transform.Position.X -= transform.Scale.X;
                }
                else if (entity.Name == "DoorR")
                {
                    transform.Position.X += transform.Scale.X;
                }
                else if (entity.Name == "DoorU")
                {
                    transform.Position.Y -= transform.Scale.Y;
                }
                else if (entity.Name == "DoorD")
                {
                    transform.Position.Y += transform.Scale.Y;
                }
            }
        }

        #endregion

        #region Private Methods

        private void move()
        {
            physics.Velocity = new Vector(0, 0);

            float speed = Character.MoveSpeed * PlayerStats.SpeedMod;

            if(InputManager.KeyPressed(Keys.W))
            {
                physics.Velocity.Y = -speed;
            }
            else if (InputManager.KeyPressed(Keys.S))
            {
                physics.Velocity.Y = speed;
            }

            if (InputManager.KeyPressed(Keys.D))
            {
                physics.Velocity.X = speed;
            }
            else if (InputManager.KeyPressed(Keys.A))
            {
                physics.Velocity.X = -speed;
            }

            if (physics.Velocity != Vector2.Zero) 
            {
                Vector2 normalized = new Vector2(physics.Velocity.X, physics.Velocity.Y);
                normalized.Normalize();
                physics.Velocity = new Vector(normalized.X, normalized.Y) * speed;
            }
        }

        private void animate()
        {
            if (physics.Velocity.X != 0 || physics.Velocity.Y != 0)
            {
                animation.CurrentAnimation = Character.WalkAnimation;

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
                animation.CurrentAnimation = Character.IdleAnimation;
            }

            if (InputManager.LeftMouseDown())
            {
                animation.CurrentAnimation = Character.AttackAnimation;
            }
        }

        private void attack(GameTime gameTime)
        {
            if (InputManager.LeftMousePressed() || InputManager.LeftMouseDown())
            {
                Vector position = InputManager.GetMousePosition();
                Vector2 direction = new Vector2(position.X - transform.Position.X,
                                                position.Y - transform.Position.Y);
                direction.Normalize();
                Character.Attack(gameTime, transform.Position + (transform.Scale/2), new Vector(direction.X, direction.Y), 
                    InputManager.LeftMouseDown());
            }       
        }

        private void hitTint()
        {
            if (hitTime < elapsedTintTime)
            {
                sprite.Color = Color.White;
            }
        }

        #endregion

    }
}
