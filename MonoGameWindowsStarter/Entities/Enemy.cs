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
    [Transform(X: 100, Y: 100, Width: 32, Height: 32)]
    [Physics(VelocityX: 0, VelocityY: 0)]
    [BoxCollision(X: 0, Y: 0, Width: 1, Height: 1)]
    public class Enemy : Entity
    {
        public Character Character;
        public float TotalHealth;
        public float CurrentHealth;

        private Sprite sprite;
        private Animation animation;
        private Transform transform;
        private BoxCollision boxCollision;
        private Physics physics;

        private float hitTime = .15f;

        private float elapsedTintTime;
        private bool attack;

        public override void Initialize()
        {
            Name = "Enemy";
            Character = new DefaultCharacter();
            TotalHealth = Character.MaxHealth;
            CurrentHealth = TotalHealth;
            attack = false;
            elapsedTintTime = hitTime;

            sprite = GetComponent<Sprite>();
            animation = GetComponent<Animation>();
            transform = GetComponent<Transform>();
            boxCollision = GetComponent<BoxCollision>();
            physics = GetComponent<Physics>();

            boxCollision.Layer = "Enemy";
            boxCollision.HandleCollision = handleCollision;

            animation.CurrentAnimation = Character.IdleAnimation;
        }

        public override void Update(GameTime gameTime)
        {
            elapsedTintTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            attack = Character.AILogic(physics);
            animate();
            hitTint();

            if (CurrentHealth <= 0)
                onDeath();
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

            if (attack)
            {
                animation.CurrentAnimation = Character.AttackAnimation;
            }
        }

        private void onDeath()
        {
            CharacterPickup characterPickup = SceneManager.GetCurrentScene().CreateEntity<CharacterPickup>();
            characterPickup.Transform.Position = new Vector(transform.Position.X, transform.Position.Y);
            characterPickup.Character = Character;

            SceneManager.GetCurrentScene().RemoveEntity(this);
        }

        private void hitTint()
        {
            if (hitTime < elapsedTintTime)
            {
                sprite.Color = Color.White;
            }
        }

        private void handleCollision(Entity entity, string direction)
        {
            if (entity.Name == "Projectile")
            {
                elapsedTintTime = 0;
                sprite.Color = Color.Red;
            }
        }
    }
}
