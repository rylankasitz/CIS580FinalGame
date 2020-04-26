using Engine.Componets;
using Engine.ECSCore;
using Microsoft.Xna.Framework;
using MonoGameWindowsStarter.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameWindowsStarter.Characters
{
    public abstract class Character
    {
        public abstract string IdleAnimation { get; }
        public abstract string WalkAnimation { get; }
        public abstract string AttackAnimation { get; }
        public abstract float MoveSpeed { get; }
        public abstract int AttackDamage { get; }
        public abstract float AttackSpeed { get; }
        public abstract float Range { get;}
        public abstract int MaxHealth { get; }
        public string Holder { get; set; }
        public abstract void Attack(GameTime gameTime, Vector position, Vector direction, bool mouseDown);
        public abstract void HandleCollision(Projectile projectile, Entity collider, string direction);
        public abstract bool AILogic(Physics physics);
    }
}
