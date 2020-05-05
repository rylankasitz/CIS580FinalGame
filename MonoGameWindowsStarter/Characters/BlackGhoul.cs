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
        public override string SpriteSheet => "MapTileSet";
        public override string IdleAnimation => "IdleBlackGhoul";
        public override string WalkAnimation => "WalkBlackGhoul";
        public override string AttackAnimation => "AttackBlackGhoul";

        public override float MoveSpeed => 6;
        public override int AttackDamage => 8;
        public override float AttackSpeed => .4f;
        public override float Range => 2;
        public override int MaxHealth => 10;
        public override int Difficulty => 1;

        public override string ProjectileSprite => "Projectiles";
        public override Rectangle ProjectileSource => new Rectangle(197, 117, 8, 8);

        private float projectileSpeed = 6f;
        private float aiAttackSpeedMod = 3f;

        public override void OnStateSwitch(string lastState)
        {
            switch (lastState)
            {
                case "Start":
                    AILogicCMD.Wait(.5f);
                    AILogicCMD.CurrentState = "Waiting";
                    break;

                case "Waiting":
                    AILogicCMD.Wait(.2f);
                    if (AILogicCMD.InRangeOfPlayer(100))
                        AILogicCMD.CurrentState = "Attacking";
                    else
                        AILogicCMD.CurrentState = "Moving";
                    break;

                case "Moving":
                    AILogicCMD.MoveToPlayer(.3f);
                    if (AILogicCMD.InRangeOfPlayer((int)(Range * 100)))
                        AILogicCMD.CurrentState = "Attacking";
                    else
                        AILogicCMD.CurrentState = "Waiting";
                    break;

                case "Attacking":
                    AILogicCMD.AttackPlayer(.5f, 0);
                    if (AILogicCMD.InRangeOfPlayer((int)(Range * 100)))
                        AILogicCMD.CurrentState = "Attacking";
                    else
                        AILogicCMD.CurrentState = "Moving";
                    break;

                default:
                    break;
            }       
        }

        public override void Attack(Entity holder, Vector position, Vector direction)
        {
            float attackSpeed = AttackSpeed * aiAttackSpeedMod;

            if (Holder == "Player")
                attackSpeed = AttackSpeed * PlayerStats.AttackSpeedMod;

            Projectile projectile;
            if (ProjectileSpawner.CreateProjectile(position, attackSpeed, out projectile))
            {
                if (Holder == "Player")
                    projectile.Damage = AttackDamage * PlayerStats.AttackDamageMod;
                else
                    projectile.Damage = AttackDamage;

                projectile.Range = Range * 100;
                projectile.Transform.Scale = new Vector(12, 12);
                projectile.Physics.Velocity = (direction * projectileSpeed);
            }           
        }
    }
}
