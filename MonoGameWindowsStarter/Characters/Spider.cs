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
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameWindowsStarter.Characters
{
    public class Spider : Character
    {
        #region Animation Variables

        public override string SpriteSheet => "SpiderCharacter";
        public override string IdleAnimation => "IdleSpider";
        public override string WalkAnimation => "WalkSpider";
        public override string AttackAnimation => "AttackSpider";

        #endregion

        #region Character Stats

        public override float MoveSpeed => 7;
        public override int AttackDamage => 15;
        public override float AttackSpeed => .5f;
        public override float Range => 1f;
        public override int MaxHealth => 120;

        #endregion

        #region AI Modifiers

        public override int Difficulty => 4;
        public override float AIMoveSpeedMod => 1f;
        public override float AIAttackDamageMod => .5f;
        public override float AIAttackSpeedMod => 1f;
        public override float AIAttackRangeMod => 1f;
        public override float AIHealthMod => .5f;

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
            int range = (int)(Range * 100 * AIAttackRangeMod);
            AILogicCMD.BasicMovementTemplate(lastState, new Vector(range, 8 * MapConstants.Scale), 1f, 
                attackTime: .01f,
                waitTime: 1f,
                moveTime: .5f);
        }

        public override void Attack(Entity holder, Vector position, Vector direction)
        {
            Projectile projectile;
            if (ProjectileSpawner.AnimationBasedProjectile(holder, Holder, out projectile))
            {
                if (holder.Name == "Enemy")
                {
                    if (direction.X > 0)
                    {
                        holder.GetComponent<Sprite>().SpriteEffects = SpriteEffects.None;
                    }
                    else
                    {
                        holder.GetComponent<Sprite>().SpriteEffects = SpriteEffects.FlipHorizontally;
                    }
                }
            }
        }

        #endregion
    }
}
