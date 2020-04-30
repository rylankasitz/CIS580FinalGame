using Engine.Componets;
using Engine.ECSCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using MonoGameWindowsStarter.Entities;

namespace MonoGameWindowsStarter.Characters
{
    public class Charles : Character
    {
        public override string SpriteSheet => "Charles_Spear";
        public override string IdleAnimation => "IdleCharles";
        public override string WalkAnimation => "WalkCharles";
        public override string AttackAnimation => "AttackCharles";

        public override float MoveSpeed => 6;
        public override int AttackDamage => 8;
        public override float AttackSpeed => .4f;
        public override float Range => 2;
        public override int MaxHealth => 60;
        public override int Difficulty => 1;

        public override string ProjectileSprite => "";
        public override Rectangle ProjectileSource => Rectangle.Empty;

        public override void OnStateSwitch(string lastState)
        {

        }

        public override void Attack(Vector position, Vector direction)
        {
            
        }

        public override void HandleCollision(Projectile projectile, Entity collider, string direction)
        {
            
        }
    }
}
