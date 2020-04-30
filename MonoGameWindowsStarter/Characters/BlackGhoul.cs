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
        public override int MaxHealth => 60;
        public override int Difficulty => 1;

        public override string ProjectileSprite => "Projectiles";
        public override Rectangle ProjectileSource => new Rectangle(197, 117, 8, 8);

        private float projectileSpeed = 6f;

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

        public override void Attack(Vector position, Vector direction)
        {
            Projectile projectile;
            float attackSpeed = AttackSpeed;

            if (Holder == "Player")
                attackSpeed *= PlayerStats.AttackSpeedMod;

            if (ProjectileSpawner.Spawn(position, attackSpeed, out projectile))
            {
                if (Holder == "Player")
                    projectile.Damage = AttackDamage * PlayerStats.AttackDamageMod;
                else
                    projectile.Damage = AttackDamage;

                projectile.Range = Range*100;
                projectile.Transform.Scale = new Vector(12, 12);
                projectile.Physics.Velocity = (direction * projectileSpeed);
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

            if (collider.Name != "Projectile")
                SceneManager.GetCurrentScene().RemoveEntity(projectile);
        }
    }
}
