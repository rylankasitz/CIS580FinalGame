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
    public class FireSummoner : Character
    {
        #region Animation Variables

        public override string SpriteSheet => "MapTileSet";
        public override string IdleAnimation => "IdleFireSummoner";
        public override string WalkAnimation => "WalkFireSummoner";
        public override string AttackAnimation => "AttackFireSummoner";

        #endregion

        #region Character Stats

        public override float MoveSpeed => 4;
        public override int AttackDamage => 40;
        public override float AttackSpeed => .4f;
        public override float Range => 5f;
        public override int MaxHealth => 100;

        #endregion

        #region AI Modifiers

        public override int Difficulty => 3;
        public override float AIMoveSpeedMod => 1;
        public override float AIAttackDamageMod => 1;
        public override float AIAttackSpeedMod => 1;
        public override float AIAttackRangeMod => 1;
        public override float AIHealthMod => 1;

        #endregion

        #region Projectile Variables

        public override string ProjectileSprite => "ProjectileFire";
        public override string ProjectileAnimation => "ProjectileFire";
        public override Vector ProjectileScale => new Vector(8, 31)*3;
        public override float ProjectileSpeed => 5f;

        #endregion

        #region Character Methods

        public override void OnStateSwitch(string lastState)
        {
            int range = (int)(Range * 100 * AIAttackRangeMod);
            AILogicCMD.BasicMovementTemplate(lastState, new Vector(range, ProjectileScale.Y/1.5f), 1f);
        }

        public override void Attack(Entity holder, Vector position, Vector direction)
        {
            float attackSpeed = AttackSpeed * (1 / AIAttackSpeedMod);

            if (Holder == "Player")
                attackSpeed = AttackSpeed * PlayerStats.AttackSpeedMod;

            Projectile projectile;
            if (ProjectileSpawner.CreateProjectile(position, attackSpeed, out projectile))
            {
                if (direction.X > 0)
                {
                    projectile.Physics.Velocity = new Vector(ProjectileSpeed, 0);
                    projectile.Transform.Position = position + holder.GetComponent<Transform>().Scale - ProjectileScale +
                                                    new Vector(holder.GetComponent<Transform>().Scale.X / 1.5f, 0);
                }
                else
                {
                    projectile.Physics.Velocity = new Vector(-ProjectileSpeed, 0);
                    projectile.Transform.Position = position + new Vector(0, holder.GetComponent<Transform>().Scale.Y) - ProjectileScale -
                                                    new Vector(holder.GetComponent<Transform>().Scale.X / 1.5f, 0) +
                                                    new Vector(ProjectileScale.X, 0);
                }
            }
        }

        #endregion
    }
}
