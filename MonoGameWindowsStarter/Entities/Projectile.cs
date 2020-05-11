using ECSEngine.Systems;
using Engine;
using Engine.Componets;
using Engine.ECSCore;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameWindowsStarter.Entities
{

    [Sprite(ContentName: "Projectiles", Layer: 0.74f)]
    [Transform(X: 0, Y: 0, Width: 12, Height: 12)]
    [Physics(VelocityX: 0, VelocityY: 0)]
    [BoxCollision(X: 0, Y: 0, Width: 1, Height: 1, TriggerOnly: true)]
    [Animation()]
    public class Projectile : Entity
    {
        public Sprite Sprite;
        public Transform Transform;
        public Physics Physics;
        public BoxCollision BoxCollision;
        public Animation Animation;

        public float Damage;
        public float Range;
        public bool DeleteOnHit;

        private float distanceTraveled;

        public override void Initialize()
        {
            Name = "Projectile";

            Sprite = GetComponent<Sprite>();
            Transform = GetComponent<Transform>();
            Physics = GetComponent<Physics>();
            BoxCollision = GetComponent<BoxCollision>();
            Animation = GetComponent<Animation>();

            BoxCollision.HandleCollisionEnter = handleCollisionEnter;

            DeleteOnHit = true;
            Damage = 0;
            Range = -1;
            distanceTraveled = 0;
        }

        public override void Update(GameTime gameTime)
        {
            if (Transform.Position.X + Transform.Scale.X < 0 || 
                Transform.Position.X > WindowManager.Width ||
                Transform.Position.Y + Transform.Scale.Y < 0 ||
                Transform.Position.Y > WindowManager.Height)
            {
                SceneManager.GetCurrentScene().RemoveEntity(this);
            }

            if (Range != -1 && distanceTraveled > Range)
            {
                SceneManager.GetCurrentScene().RemoveEntity(this);
            }

            Vector2 vel = new Vector2(Physics.Velocity.X, Physics.Velocity.Y);
            distanceTraveled += vel.Length();
        }

        private void handleCollisionEnter(Entity entity, string direction)
        {
            if (entity.Name == "Enemy")
            {
                Enemy enemy = (Enemy)entity;
                enemy.CurrentHealth -= Damage;
            }
            else if (entity.Name == "Player")
            {
                Player player = (Player)entity;

                if (!player.Rolling)
                    player.CurrentHealth -= Damage;
            }

            if (entity.Name != "Projectile" && Range != -1 && DeleteOnHit)
                SceneManager.GetCurrentScene().RemoveEntity(this);
        }
    }
}
