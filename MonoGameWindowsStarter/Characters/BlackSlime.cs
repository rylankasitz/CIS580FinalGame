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
    public class BlackSlime : Character
    {
        #region Animation Variables

        public override string SpriteSheet => "MapTileSet";
        public override string IdleAnimation => "IdleBlackSlime";
        public override string WalkAnimation => "WalkBlackSlime";
        public override string AttackAnimation => "AttackBlackSlime";

        #endregion

        #region Character Stats

        public override float MoveSpeed => 3;
        public override int AttackDamage => 30;
        public override float AttackSpeed => 1f;
        public override float Range => 2;
        public override int MaxHealth => 300;

        #endregion

        #region AI Modifiers

        public override int Difficulty => 3;
        public override float AIMoveSpeedMod => 1f;
        public override float AIAttackDamageMod => 1f;
        public override float AIAttackSpeedMod => 1f;
        public override float AIAttackRangeMod => 1f;
        public override float AIHealthMod => .3f;

        #endregion

        #region Projectile Variables

        public override string ProjectileSprite => "ProjectileCircle";
        public override string ProjectileAnimation => "ProjectileCircle";
        public override Vector ProjectileScale => new Vector(16, 16)*1.5f;
        public override float ProjectileSpeed => 3f;

        #endregion

        #region Character Methods

        public override void OnStateSwitch(string lastState)
        {
            int range = (int)(Range * 100 * AIAttackRangeMod);
            AILogicCMD.BasicMovementTemplate(lastState, new Vector(range, range), 1f,
                attackTime: 1f,
                waitTime: .8f,
                moveTime: .4f);
        }

        public override void Attack(Entity holder, Vector position, Vector direction)
        {
            float attackSpeed = AttackSpeed * (1 / AIAttackSpeedMod);

            if (Holder == "Player")
                attackSpeed = AttackSpeed * PlayerStats.AttackSpeedMod;

            Projectile projectile;
            if (ProjectileSpawner.CreateProjectile(position, attackSpeed, out projectile))
            {
                projectile.Physics.Velocity = direction * ProjectileSpeed;
            }
        }

        #endregion
    }
}
