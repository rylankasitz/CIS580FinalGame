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

namespace MonoGameWindowsStarter.Characters.Templates
{
    public class CharacterMeleeTemplate : Character
    {
        #region Animation Variables

        public override string SpriteSheet => "SpriteSheet";
        public override string IdleAnimation => "Idle";
        public override string WalkAnimation => "Walk";
        public override string AttackAnimation => "Attack";

        #endregion

        #region Character Stats

        public override float MoveSpeed => 0;
        public override int AttackDamage => 0;
        public override float AttackSpeed => 0;
        public override float Range => 0;
        public override int MaxHealth => 0;

        #endregion

        #region AI Modifiers

        public override int Difficulty => -1;
        public override float AIMoveSpeedMod => 1;
        public override float AIAttackDamageMod => 1;
        public override float AIAttackSpeedMod => 1;
        public override float AIAttackRangeMod => 1;
        public override float AIHealthMod => 1;

        #endregion

        #region Projectile Variables

        public override string ProjectileSprite => "";
        public override string ProjectileAnimation => "";
        public override Vector ProjectileScale => new Vector(0, 0);
        public override float ProjectileSpeed => 0f;

        #endregion

        #region Character Methods

        public override void OnStateSwitch(string lastState)
        {

        }

        public override void Attack(Entity holder, Vector position, Vector direction)
        {
            Projectile projectile;
            if (ProjectileSpawner.AnimationBasedProjectile(holder, Holder, out projectile))
            {
                
            }
        }

        #endregion
    }
}
