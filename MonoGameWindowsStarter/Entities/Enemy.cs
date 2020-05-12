using Engine;
using Engine.Componets;
using Engine.ECSCore;
using Engine.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameWindowsStarter.Characters;
using MonoGameWindowsStarter.GlobalValues;
using MonoGameWindowsStarter.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameWindowsStarter.Entities
{
    [Sprite(ContentName: "MapTileSet", Layer: .76f)]
    [Animation(CurrentAnimation: "Idle")]
    [Transform(X: 100, Y: 100, Width: 8, Height: 8)]
    [Physics(VelocityX: 0, VelocityY: 0)]
    [BoxCollision(X: 0, Y: 0, Width: 1, Height: 1)]
    public class Enemy : Entity
    {
        public Character Character;
        public Transform Transform;
        public Physics Physics;
        public Animation Animation;
        public BoxCollision BoxCollision;
        public Sprite Sprite;
        public float TotalHealth;
        public float CurrentHealth;
        public CharacterPickup CharacterPickup;

        private float hitTime = .15f;

        private float elapsedTintTime;
        private bool attack;

        #region ECS Methods

        public override void Initialize()
        {
            Name = "Enemy";
            Character = new Charles();
            TotalHealth = Character.MaxHealth * Character.AIHealthMod;
            CurrentHealth = TotalHealth;
            attack = false;
            elapsedTintTime = hitTime;

            Sprite = GetComponent<Sprite>();
            Animation = GetComponent<Animation>();
            Transform = GetComponent<Transform>();
            BoxCollision = GetComponent<BoxCollision>();
            Physics = GetComponent<Physics>();

            BoxCollision.Layer = "Enemy|Character";
            BoxCollision.HandleCollision = handleCollision;

            Transform.Scale = Transform.Scale * MapConstants.Scale;

            Animation.CurrentAnimation = Character.IdleAnimation;
        }

        public override void Update(GameTime gameTime)
        {
            elapsedTintTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            Character.Holder = Name;

            Character.AILogicCMD.Update(gameTime);
            Character.ProjectileSpawner.Update(Character.Holder, gameTime);

            // Temporary set collisions and scale
            Transform.Scale = Animation.AnimationScale * MapConstants.Scale;
            BoxCollision.Scale = new Vector(8, 8) / Animation.AnimationScale;
            BoxCollision.Position = new Vector(.5f, .5f) - ((new Vector(8, 8) / Animation.AnimationScale) / 2);

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
        }

        private void onDeath()
        {
            CharacterPickup = SceneManager.GetCurrentScene().CreateEntity<CharacterPickup>();
            CharacterPickup.Transform.Position = new Vector(Transform.Position.X, Transform.Position.Y) + 
                                                 new Vector(Transform.Scale.X, Transform.Scale.Y)/2 - 
                                                 CharacterPickup.Transform.Scale/2;
            CharacterPickup.Character = Character;
            CharacterPickup.NewScale = Transform.Scale;

            SceneManager.GetCurrentScene().RemoveEntity(this);

            if (Character.ProjectileSpawner.projectile != null)
            {
                SceneManager.GetCurrentScene().RemoveEntity(Character.ProjectileSpawner.projectile);
            }

            // Remove doors
            if (SceneManager.GetCurrentScene().GetEntities<Enemy>().Count == 0)
            {
                foreach (MapObjectCollision obj in SceneManager.GetCurrentScene().GetEntities<MapObjectCollision>())
                {
                    if (obj.Name == "BlockedDoor")
                    {
                        SceneManager.GetCurrentScene().RemoveEntity(obj);
                    }
                }

                if (MapConstants.KeyRoom) 
                {
                    Key key = SceneManager.GetCurrentScene().CreateEntity<Key>();
                    MapObject keySpawn = SceneManager.GetCurrentScene().GetEntity<MapObject>("KeySpawn");
                    key.Transform.Position = keySpawn.GetComponent<Transform>().Position;
                }
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
