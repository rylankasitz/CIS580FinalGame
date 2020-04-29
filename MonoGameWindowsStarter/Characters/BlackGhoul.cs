using Engine.Componets;
using Engine.ECSCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using MonoGameWindowsStarter.Entities;
using MonoGameWindowsStarter.Characters.Helpers;
using MonoGameWindowsStarter.GlobalValues;
using Engine;

namespace MonoGameWindowsStarter.Characters
{
    public class BlackGhoul : Character
    {
        public override string IdleAnimation => "IdleBlackGhoul";

        public override string WalkAnimation => "WalkBlackGhoul";

        public override string AttackAnimation => "AttackBlackGhoul";

        public override float MoveSpeed => 6;

        public override int AttackDamage => 8;

        public override float AttackSpeed => .4f;

        public override float Range => 2;

        public override int MaxHealth => 60;

        public override int Difficulty => 1;

        private ProjectileSpawner projectileSpawner;
        private float projectileSpeed = 6f;
        private string projectileSprite = "Projectiles";
        private Rectangle projectileSource = new Rectangle(197, 117, 8, 8);

        public override bool AILogic(Physics physics)
        {
            return false;
        }

        public override void Attack(GameTime gameTime, Vector position, Vector direction, bool mouseDown)
        {
            if (mouseDown)
            {
                projectileSpawner = new ProjectileSpawner(HandleCollision, projectileSource, projectileSprite, Holder);
            }

            Projectile projectile;
            float attackSpeed = AttackSpeed;
            if (Holder == "Player") attackSpeed = AttackSpeed * PlayerStats.AttackSpeedMod;

            if (projectileSpawner.Spawn(gameTime, position, direction, attackSpeed, projectileSpeed, out projectile))
            {
                if (Holder == "Player")
                    projectile.Damage = AttackDamage * PlayerStats.AttackDamageMod;
                else
                    projectile.Damage = AttackDamage;

                projectile.Range = Range*100;
                projectile.Transform.Scale = new Vector(12, 12);
            }
        }

        public override void HandleCollision(Projectile projectile, Entity collider, string direction)
        {
            if (collider.Name == "Enemy")
            {
                Enemy enemy = (Enemy)collider;
                enemy.CurrentHealth -= projectile.Damage;
            }
            else if (collider.Name == "Player")
            {
                Player player = (Player)collider;
                player.CurrentHealth -= projectile.Damage;
            }

            SceneManager.GetCurrentScene().RemoveEntity(projectile);
        }
    }
}
