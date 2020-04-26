using Engine;
using Engine.Componets;
using Microsoft.Xna.Framework;
using MonoGameWindowsStarter.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HandleCollision = MonoGameWindowsStarter.Entities.HandleCollision;

namespace MonoGameWindowsStarter.Characters.Helpers
{
    public class ProjectileSpawner
    {
        private float elapsedTime;
        private Rectangle source;
        private string contentName;
        private HandleCollision handleCollision;
        private string layer;

        public ProjectileSpawner(HandleCollision collisionHandler, Rectangle spriteSource, string contentName, string layer)
        {
            source = spriteSource;
            this.contentName = contentName;
            this.handleCollision = collisionHandler;
            elapsedTime = 1000;
            this.layer = layer;
        }

        public bool Spawn(GameTime gameTime, Vector position, Vector direction, float spawnFrequency, float speed, out Projectile projectile)
        {
            elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (elapsedTime > spawnFrequency)
            {
                projectile = SceneManager.GetCurrentScene().CreateEntity<Projectile>();
                projectile.Transform.Position = position;
                projectile.Sprite.ContentName = contentName;
                projectile.Sprite.SpriteLocation = source;
                projectile.Physics.Velocity = direction * speed;
                projectile.HandleCollision = handleCollision;
                projectile.BoxCollision.Layer = layer;
                elapsedTime = 0;
                return true;
            }
            projectile = null;
            return false;
        }
    }
}
