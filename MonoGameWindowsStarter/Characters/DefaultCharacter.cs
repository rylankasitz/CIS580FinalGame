using Engine.Componets;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.ECSCore;
using Engine;
using MonoGameWindowsStarter.Entities;
using MonoGameWindowsStarter.Characters.Helpers;
using MonoGameWindowsStarter.GlobalValues;

namespace MonoGameWindowsStarter.Characters
{
    public class DefaultCharacter : Character
    {
        public override string IdleAnimation => "IdleDefault";

        public override string WalkAnimation  => "WalkDefault";

        public override string AttackAnimation => "AttackDefault";

        public override float MoveSpeed => 6;

        public override int AttackDamage => 10;

        public override float AttackSpeed => .4f;

        public override float Range => 3;

        public override int MaxHealth => 50;

        public override int Difficulty => 0;

        private ProjectileSpawner projectileSpawner;
        private float projectileSpeed = 5f;
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

            if (projectileSpawner.Spawn(gameTime, position, direction, AttackSpeed, projectileSpeed, out projectile)) 
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
