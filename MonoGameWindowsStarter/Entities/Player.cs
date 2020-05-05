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
using System.Diagnostics;

namespace MonoGameWindowsStarter.Entities
{
    [Sprite(ContentName: "MapTileSet", Layer: .75f)]
    [Animation(CurrentAnimation: "Idle")]
    [Transform(X: 300, Y: 300, Width: 100, Height: 100)]
    [Physics(VelocityX: 0, VelocityY: 0)]
    [BoxCollision(X: 0, Y: 0, Width: 1f, Height: 1f)]
    public class Player : Entity
    {
        public Character Character;
        public Transform Transform;
        public Physics Physics;
        public Sprite Sprite;
        public Animation Animation;
        public Vector SpriteSize = new Vector(8, 8);
        public float SpriteScale = 5;
        public float TotalHealth;
        public float CurrentHealth;

        private BoxCollision boxCollision;
        private SliderBar healthBar;


        private float hitTime = .15f;

        private float elapsedTintTime;

        #region ESC Methods

        public override void Initialize()
        {
            Name = "Player";
            Character = new Charles();
            Character.Holder = "Player";
            TotalHealth = Character.MaxHealth;
            CurrentHealth = TotalHealth;
            elapsedTintTime = hitTime;

            Character.OnSpawn();

            Sprite = GetComponent<Sprite>();
            Animation = GetComponent<Animation>();
            Transform = GetComponent<Transform>();
            boxCollision = GetComponent<BoxCollision>();
            Physics = GetComponent<Physics>();

            boxCollision.HandleCollision = handleCollision;
            boxCollision.Layer = "Player";

            Animation.CurrentAnimation = Character.IdleAnimation;

            Sprite.ContentName = Character.SpriteSheet;

            healthBar = new SliderBar("HealthBars", "HealthBars",
                                       new Vector(WindowManager.Width * .03f, WindowManager.Height * .03f),
                                       new Vector(WindowManager.Width * .15f, WindowManager.Height * .05f),
                                       new Rectangle(18, 40, 64, 9),
                                       new Rectangle(18, 29, 64, 9));
        }

        public override void Update(GameTime gameTime)
        {
            elapsedTintTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            Character.ProjectileSpawner.Update(Character.Holder, gameTime);

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
                Sprite.Color = Color.Red;
                healthBar.UpdateFill(CurrentHealth / TotalHealth);
            }

            if (entity.Name.Contains("Door"))
            {
                MainScene scene = (MainScene) SceneManager.GetCurrentScene();
                Transform.Position = scene.MapGenerator.LoadNextRoom(entity.Name) - (Transform.Scale/2);

                if (entity.Name == "DoorL")
                {
                    Transform.Position.X -= SpriteSize.X * SpriteScale;
                }
                else if (entity.Name == "DoorR")
                {
                    Transform.Position.X += SpriteSize.X * SpriteScale;
                }
                else if (entity.Name == "DoorU")
                {
                    Transform.Position.Y -= SpriteSize.Y * SpriteScale;
                }
                else if (entity.Name == "DoorD")
                {
                    Transform.Position.Y += SpriteSize.Y * SpriteScale;
                }
            }
        }

        #endregion

        #region Private Methods

        private void move()
        {         
            boxCollision.Scale = SpriteSize / Animation.AnimationScale;
            boxCollision.Position = (Transform.Scale - (SpriteSize * SpriteScale)) / 2;

            Physics.Velocity = new Vector(0, 0);

            float speed = Character.MoveSpeed * PlayerStats.SpeedMod;

            if(InputManager.KeyPressed(Keys.W))
            {
                Physics.Velocity.Y = -speed;
            }
            else if (InputManager.KeyPressed(Keys.S))
            {
                Physics.Velocity.Y = speed;
            }

            if (InputManager.KeyPressed(Keys.D))
            {
                Physics.Velocity.X = speed;
            }
            else if (InputManager.KeyPressed(Keys.A))
            {
                Physics.Velocity.X = -speed;
            }

            if (Physics.Velocity != Vector2.Zero) 
            {
                Vector2 normalized = new Vector2(Physics.Velocity.X, Physics.Velocity.Y);
                normalized.Normalize();
                Physics.Velocity = new Vector(normalized.X, normalized.Y) * speed;
            }
        }

        private void animate()
        {
            Transform.Scale = Animation.AnimationScale * SpriteScale;

            if (Physics.Velocity.X != 0 || Physics.Velocity.Y != 0)
            {
                Animation.CurrentAnimation = Character.WalkAnimation;

                if (Physics.Velocity.X < 0)
                {
                    Sprite.SpriteEffects = SpriteEffects.FlipHorizontally;
                }
                else if (Physics.Velocity.X > 0)
                {
                    Sprite.SpriteEffects = SpriteEffects.None;
                }
            }
            else
            {
                Animation.CurrentAnimation = Character.IdleAnimation;
            }

            if (InputManager.LeftMouseDown())
            {
                Animation.Play(Character.AttackAnimation);
            }
        }

        private void attack(GameTime gameTime)
        {
            if (InputManager.LeftMousePressed())
            {
                Vector position = InputManager.GetMousePosition() - Transform.Scale/2;
                Vector2 direction = new Vector2(position.X - Transform.Position.X,
                                                position.Y - Transform.Position.Y);
                direction.Normalize();
                Character.Attack(this, Transform.Position, new Vector(direction.X, direction.Y));
            }       
        }

        private void hitTint()
        {
            if (hitTime < elapsedTintTime)
            {
                Sprite.Color = Color.White;
            }
        }

        #endregion

    }
}
