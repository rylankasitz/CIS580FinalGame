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

        public Vector SpriteSize = new Vector(8, 8); // Change

        public float TotalHealth;
        public float CurrentHealth;

        public bool Rolling;

        private Vector preVelocity;

        public int KeyCount;
        public List<Key> KeysUI;

        private BoxCollision boxCollision;
        private SliderBar healthBar;
        private SliderBar cooldownBar;
        private Trail jumpTrailStart;
        private Trail jumpTrailEnd;
        private MainScene scene;

        private float elapsedTintTime;
        private float elapsedRollTime;

        private float hitTime = .15f;
        private float rollTime = .1f;
        private float rollCooldown = 2f;
        private float rollDistance = 200;

        #region ESC Methods

        public override void Initialize()
        {
            Name = "Player";
            Character = new Charles();
            Character.Holder = "Player";
            TotalHealth = Character.MaxHealth;
            CurrentHealth = TotalHealth;
            elapsedTintTime = hitTime;
            elapsedRollTime = rollCooldown;

            KeyCount = 0;
            KeysUI = new List<Key>();
            Rolling = false;

            scene = (MainScene)SceneManager.GetCurrentScene();    

            Character.OnSpawn();

            Sprite = GetComponent<Sprite>();
            Animation = GetComponent<Animation>();
            Transform = GetComponent<Transform>();
            boxCollision = GetComponent<BoxCollision>();
            Physics = GetComponent<Physics>();

            boxCollision.HandleCollision = handleCollision;
            boxCollision.Layer = "Player|Character";

            Animation.CurrentAnimation = Character.IdleAnimation;

            Sprite.ContentName = Character.SpriteSheet;

            healthBar = new SliderBar("HealthBar", "HealthBarOutline",
                                       new Vector(WindowManager.Width * .03f, WindowManager.Height * .03f),
                                       new Vector(WindowManager.Width * .15f, WindowManager.Height * .035f),
                                       Rectangle.Empty,
                                       Rectangle.Empty);

            cooldownBar = new SliderBar("CoolDownBar", "CoolDownBarOutline",
                                       new Vector(WindowManager.Width * .03f, WindowManager.Height * .07f),
                                       new Vector(WindowManager.Width * .15f, WindowManager.Height * .015f),
                                       Rectangle.Empty,
                                       Rectangle.Empty);

            jumpTrailStart = scene.CreateEntity<Trail>();
            jumpTrailEnd = scene.CreateEntity<Trail>();

            preVelocity = new Vector(0, 0);
        }

        public override void Update(GameTime gameTime)
        {
            if (elapsedTintTime < hitTime)
                elapsedTintTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (elapsedRollTime < rollCooldown)
            {
                cooldownBar.UpdateFill(elapsedRollTime / rollCooldown);
                elapsedRollTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            Character.ProjectileSpawner.Update(Character.Holder, gameTime);

            if (CurrentHealth <= 0)
                onDeath();

            hitTint();
            move(gameTime);
            animate();
            attack(gameTime);
            setMinmap();
            healthBar.UpdateFill(CurrentHealth / TotalHealth);
        }

        private void handleCollision(Entity entity, string direction)
        {
            if (entity.Name == "Projectile" && !Rolling)
            {
                elapsedTintTime = 0;
                Sprite.Color = Color.Red;
                healthBar.UpdateFill(CurrentHealth / TotalHealth);
            }

            if (entity.Name.Contains("Door") && !entity.Name.Contains("Blocked"))
            {
                MainScene scene = (MainScene) SceneManager.GetCurrentScene();
                Transform.Position = scene.MapGenerator.LoadNextRoom(entity.Name) - (Transform.Scale/2);

                if (entity.Name == "DoorL")
                {
                    Transform.Position.X -= SpriteSize.X * MapConstants.Scale * 2;
                }
                else if (entity.Name == "DoorR")
                {
                    Transform.Position.X += SpriteSize.X * MapConstants.Scale * 2;
                }
                else if (entity.Name == "DoorU")
                {
                    Transform.Position.Y -= SpriteSize.Y * MapConstants.Scale * 2;
                }
                else if (entity.Name == "DoorD")
                {
                    Transform.Position.Y += SpriteSize.Y * MapConstants.Scale * 2;
                }
            }
        }

        #endregion

        public void AddKey()
        {
            Key key = SceneManager.GetCurrentScene().CreateEntity<Key>();
            key.Transform.Position = new Vector(WindowManager.Width * .2f + 
                (key.Transform.Scale.X*KeyCount), WindowManager.Height * .02f);
            KeyCount++;
            KeysUI.Add(key);
        }

        public void OnKeysCollected(Key key)
        {
            BossPortal bossPortal = SceneManager.GetCurrentScene().CreateEntity<BossPortal>();
            bossPortal.Transform.Position = key.Transform.Position;
        }

        #region Private Methods

        private void move(GameTime gameTime)
        {         
            // Temporarly set box collisionaw
            boxCollision.Scale = SpriteSize / Animation.AnimationScale;
            boxCollision.Position = new Vector(.5f, .5f) - ((SpriteSize / Animation.AnimationScale) / 2);
            Transform.Scale = Animation.AnimationScale * MapConstants.Scale;

            Vector2 normalized = Vector2.Zero;

            Rolling = elapsedRollTime < rollTime;

            if (!Rolling) {
                Physics.Velocity = new Vector(0, 0);

                float speed = Character.MoveSpeed * PlayerStats.SpeedMod;

                Sprite.Enabled = true;

                if (InputManager.KeyPressed(Keys.W))
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
                    normalized = new Vector2(Physics.Velocity.X, Physics.Velocity.Y);
                    normalized.Normalize();
                    Physics.Velocity = new Vector(normalized.X, normalized.Y) * speed;
                }

                if (InputManager.KeyDown(Keys.Space) && elapsedRollTime >= rollCooldown)
                {
                    jumpTrailStart.Transform.Position = (Transform.Position + (Transform.Scale / 2)) - jumpTrailStart.Transform.Scale / 2;
                    jumpTrailEnd.Transform.Position = jumpTrailStart.Transform.Position + new Vector(normalized.X, normalized.Y) * rollDistance;
                    jumpTrailStart.Animation.Play(jumpTrailStart.Animation.CurrentAnimation);
                    jumpTrailEnd.Animation.Play(jumpTrailEnd.Animation.CurrentAnimation);

                    preVelocity = new Vector(normalized.X, normalized.Y);

                    jumpTrailStart.Sprite.Enabled = true;
                    jumpTrailEnd.Sprite.Enabled = true;
                    Sprite.Enabled = false;
                    elapsedRollTime = 0;
                }
            }
            else if (Rolling)
            {
                Sprite.Enabled = false;
                Physics.Velocity = preVelocity * (float) ((rollDistance / rollTime) * gameTime.ElapsedGameTime.TotalSeconds);
            }

            if (!jumpTrailStart.Animation.Playing)
            {
                jumpTrailStart.Sprite.Enabled = false;
            }

            if (!jumpTrailEnd.Animation.Playing)
            {
                jumpTrailEnd.Sprite.Enabled = false;
            }
        }

        private void animate()
        {
            if (Physics.Velocity.X != 0 || Physics.Velocity.Y != 0)
            {
                Animation.CurrentAnimation = Character.WalkAnimation;

                if (Physics.Velocity.X < 0 && !Animation.Playing)
                {
                    Sprite.SpriteEffects = SpriteEffects.FlipHorizontally;
                }
                else if (Physics.Velocity.X > 0 && !Animation.Playing)
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
                Vector direction = getMouseDirection();
                Character.Attack(this, Transform.Position, direction);
            }       
        }

        private void onDeath()
        {
            SceneManager.LoadScene("Gameover");
        }

        private void setMinmap()
        {
            if (InputManager.KeyDown(Keys.LeftShift))
            {         
                scene.MapGenerator.SetMinimap(true);
            }
            else if (InputManager.KeyUp(Keys.LeftShift))
            {
                scene.MapGenerator.SetMinimap(false);
            }
        }

        private void hitTint()
        {
            if (hitTime < elapsedTintTime)
            {
                Sprite.Color = Color.White;
            }
        }

        private Vector getMouseDirection()
        {
            Vector position = InputManager.GetMousePosition() - Transform.Scale / 2;
            Vector2 direction = new Vector2(position.X - Transform.Position.X,
                                            position.Y - Transform.Position.Y);
            direction.Normalize();

            return new Vector(direction.X, direction.Y);
        }

        #endregion

    }
}
