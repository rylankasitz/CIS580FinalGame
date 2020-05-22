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

        public List<Key> KeysUI;

        public float TotalHealth;
        public float CurrentHealth;

        public bool Rolling;
        public bool Involnerable;

        public int KeyCount;

        private Vector rollVelocity;
        private Vector2 lastAimDirection;

        private BoxCollision boxCollision;
        private SliderBar healthBar;
        private SliderBar cooldownBar;
        private Trail jumpTrailStart;
        private Trail jumpTrailEnd;
        private MainScene scene;

        private float elapsedTintTime;
        private float elapsedRollTime;

        private float hitTime = .5f;
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
            rollVelocity = new Vector(0, 0);
            lastAimDirection = new Vector2(1, 0);
            Involnerable = false;

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
            // Set Hit tint
            if (entity.Name == "Projectile" && !Rolling)
            {
                elapsedTintTime = 0;
                Sprite.Color = Color.Red;
                healthBar.UpdateFill(CurrentHealth / TotalHealth);
                Involnerable = true;
            }

            // Set new room position
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

        #region Public Methods

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

        #endregion

        #region Private Methods

        private void move(GameTime gameTime)
        {         
            // Temporarly set box collision
            Transform.Scale = Animation.AnimationScale * MapConstants.Scale;

            Rolling = elapsedRollTime < rollTime;

            if (!Rolling) {

                Physics.Velocity = new Vector(0, 0);
                Sprite.Enabled = true;

                float speed = Character.MoveSpeed * PlayerStats.SpeedMod;

                Physics.Velocity = new Vector(InputManager.GetAxis("MoveX"), InputManager.GetAxis("MoveY")) * speed;

                if (Physics.Velocity != Vector2.Zero)
                {

                    Vector2 normalized = new Vector2(Physics.Velocity.X, Physics.Velocity.Y);
                    normalized.Normalize();
                    rollVelocity = new Vector(normalized.X, normalized.Y);
                    Physics.Velocity = new Vector(normalized.X, normalized.Y) * speed;
                }

                if (InputManager.GetAxisDown("Roll") && elapsedRollTime >= rollCooldown)
                {
                    jumpTrailStart.Transform.Position = (Transform.Position + (Transform.Scale / 2)) - jumpTrailStart.Transform.Scale / 2;
                    jumpTrailEnd.Transform.Position = jumpTrailStart.Transform.Position + rollVelocity * rollDistance;

                    jumpTrailStart.Animation.Play(jumpTrailStart.Animation.CurrentAnimation);
                    jumpTrailEnd.Animation.Play(jumpTrailEnd.Animation.CurrentAnimation);

                    jumpTrailStart.Sprite.Enabled = true;
                    jumpTrailEnd.Sprite.Enabled = true;
                    Sprite.Enabled = false;

                    elapsedRollTime = 0;
                }
            }
            else if (Rolling)
            {
                Sprite.Enabled = false;
                Physics.Velocity = rollVelocity * (float) ((rollDistance / rollTime) * gameTime.ElapsedGameTime.TotalSeconds);
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

            if (InputManager.GetAxisDown("Attack"))
            {
                Animation.Play(Character.AttackAnimation);
            }
        }

        private void attack(GameTime gameTime)
        {
            if (InputManager.GetAxisPressed("Attack"))
            {
                Vector2 direction = new Vector2(InputManager.GetAxis("AimX"), InputManager.GetAxis("AimY"));

                if (direction == Vector2.Zero)
                {
                    direction = lastAimDirection;
                }

                direction.Normalize();
                Character.Attack(this, Transform.Position, new Vector(direction.X, direction.Y));

                lastAimDirection = direction;
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
                Involnerable = false;
            }
        }

        #endregion
    }
}
