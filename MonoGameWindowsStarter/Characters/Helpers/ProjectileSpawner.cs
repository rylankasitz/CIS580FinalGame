using Engine;
using Engine.Componets;
using Engine.ECSCore;
using Engine.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameWindowsStarter.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameWindowsStarter.Characters.Helpers
{
    public class ProjectileSpawner
    {
        private float elapsedTime;
        private Rectangle source;
        private string contentName;
        private string layer;
        private Animation animation;
        private bool animationBased;
        private Projectile projectile;
        private Transform transform;
        private Sprite sprite;

        public ProjectileSpawner(Rectangle spriteSource, string contentName)
        {
            source = spriteSource;
            this.contentName = contentName;
            elapsedTime = 1000;
            animationBased = false;
        }

        public void Update(string holder, GameTime gameTime)
        {
            this.layer = holder;
            elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (animationBased && animation.CurrentCollisions.ContainsKey("Attack"))
            {
                projectile.Transform.Position = transform.Position;
                if (sprite.SpriteEffects == SpriteEffects.FlipHorizontally)
                {
                    float diffX = animation.CurrentCollisions["Attack"].Position.X - (transform.Scale.X/2);
                    float sub = diffX * 2 + transform.Scale.X * animation.CurrentCollisions["Attack"].Scale.X;
                    projectile.BoxCollision.Position = animation.CurrentCollisions["Attack"].Position - new Vector(sub, 0);
                    projectile.BoxCollision.Scale = animation.CurrentCollisions["Attack"].Scale;
                }
                else
                {
                    projectile.BoxCollision.Position = animation.CurrentCollisions["Attack"].Position;
                    projectile.BoxCollision.Scale = animation.CurrentCollisions["Attack"].Scale;
                }
            }
            else if (projectile != null)
            {
                projectile.BoxCollision.Scale = new Vector(0, 0);
            }
        }

        public bool CreateProjectile(Vector position, float spawnFrequency, out Projectile projectile)
        {
            if (elapsedTime > spawnFrequency)
            {
                projectile = SceneManager.GetCurrentScene().CreateEntity<Projectile>();
                projectile.Sprite.ContentName = contentName;
                projectile.Sprite.SpriteLocation = source;
                projectile.BoxCollision.Layer = layer;
                projectile.Transform.Position = position;

                if (projectile.Sprite.ContentName == "")
                    projectile.Sprite.Enabled = false;

                elapsedTime = 0;

                return true;
            }

            projectile = null;
            return false;
        }

        public bool AnimationBasedProjectile(Entity entity, string holder, out Projectile projectile)
        {
            if (!animationBased)
            {
                projectile = SceneManager.GetCurrentScene().CreateEntity<Projectile>();
                projectile.Sprite.ContentName = contentName;
                projectile.Sprite.SpriteLocation = source;
                projectile.BoxCollision.Layer = layer;

                if (projectile.Sprite.ContentName == "")
                    projectile.Sprite.Enabled = false;

                if (holder == "Player")
                {
                    Player player = (Player)entity;
                    this.transform = player.Transform;
                    this.animation = player.Animation;
                    this.sprite = player.Sprite;
                }
                else
                {
                    Enemy enemy = (Enemy)entity;
                    this.transform = enemy.Transform;
                    this.animation = enemy.Animation;
                    this.sprite = enemy.Sprite;
                }

                projectile.Transform.Scale = this.transform.Scale;

                this.projectile = projectile;

                animationBased = true;

                return true;
            }
            projectile = null;
            return false;
        }
    }
}
