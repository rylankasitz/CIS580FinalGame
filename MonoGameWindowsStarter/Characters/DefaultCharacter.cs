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
    /*public class DefaultCharacter : Character
    {
        public override string SpriteSheet => "MapTileSet";
        public override string IdleAnimation => "IdleDefault";
        public override string WalkAnimation  => "WalkDefault";
        public override string AttackAnimation => "AttackDefault";

        public override float MoveSpeed => 6;
        public override int AttackDamage => 10;
        public override float AttackSpeed => .4f;
        public override float Range => 3;
        public override int MaxHealth => 50;
        public override int Difficulty => 0;

        public override string ProjectileSprite => "Projectiles";
        public override Rectangle ProjectileSource => new Rectangle(197, 117, 8, 8);

        private float projectileSpeed = 5f;

        public override void OnStateSwitch(string lastState)
        {
               
        }

        public override void Attack(Player player)
        {          
            float attackSpeed = AttackSpeed;

            if (Holder == "Player") 
                attackSpeed = AttackSpeed * PlayerStats.AttackSpeedMod;

            ProjectileSpawner.CreateProjectile(attackSpeed, true);
        }

        public override void OnProjectileSpawn(Projectile projectile, )
        {
            if (Holder == "Player")
                projectile.Damage = AttackDamage * PlayerStats.AttackDamageMod;
            else
                projectile.Damage = AttackDamage;

            projectile.Range = Range * 100;
            projectile.Transform.Scale = new Vector(12, 12);
            projectile.Physics.Velocity = direction * projectileSpeed;
        }*
    }*/
}
