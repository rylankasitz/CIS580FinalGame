using Engine;
using Engine.Componets;
using Engine.ECSCore;
using Engine.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameWindowsStarter.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameWindowsStarter.Entities
{
    [Sprite(ContentName: "MapTileSet", Layer: 0)]
    [Animation(CurrentAnimation: "Idle")]
    [Transform(X: 100, Y: 100, Width: 40, Height: 40)]
    [Physics(VelocityX: 0, VelocityY: 0)]
    [BoxCollision(X: 0, Y: 0, Width: 1, Height: 1)]
    public class Enemy : Entity
    {
        public Character Character;
        public Transform Transform;
        public Physics Physics;
        public Animation Animation;
        public Sprite Sprite;
        public float TotalHealth;
        public float CurrentHealth;
        public CharacterPickup CharacterPickup;

        private BoxCollision boxCollision;
  

        private float hitTime = .15f;

        private float elapsedTintTime;
        private bool attack;

        #region ECS Methods

        public override void Initialize()
        {
            Name = "Enemy";
            Character = new BlackGhoul();
            TotalHealth = Character.MaxHealth * Character.AIHealthMod;
            CurrentHealth = TotalHealth;
            attack = false;
            elapsedTintTime = hitTime;

            Sprite = GetComponent<Sprite>();
            Animation = GetComponent<Animation>();
            Transform = GetComponent<Transform>();
            boxCollision = GetComponent<BoxCollision>();
            Physics = GetComponent<Physics>();

            boxCollision.Layer = "Enemy|Character";
            boxCollision.HandleCollision = handleCollision;

            Animation.CurrentAnimation = Character.IdleAnimation;
        }

        public override void Update(GameTime gameTime)
        {
            elapsedTintTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            Character.Holder = Name;

            Character.AILogicCMD.Update(gameTime);
            Character.ProjectileSpawner.Update(Character.Holder, gameTime);

            animate();
            hitTint();

            if (CurrentHealth <= 0)
                onDeath();
        }

        private void handleCollision(Entity entity, string direction)
        {
            if (entity.Name == "Projectile")
            {
                elapsedTintTime = 0;
                Sprite.Color = Color.Red;
            }
        }

        #endregion

        #region Private Methods

        private void animate()
        {
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

            if (attack)
            {
                Animation.CurrentAnimation = Character.AttackAnimation;
            }
        }

        private void onDeath()
        {
            CharacterPickup = SceneManager.GetCurrentScene().CreateEntity<CharacterPickup>();
            CharacterPickup.Transform.Position = new Vector(Transform.Position.X, Transform.Position.Y);
            CharacterPickup.Character = Character;

            SceneManager.GetCurrentScene().RemoveEntity(this);
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
