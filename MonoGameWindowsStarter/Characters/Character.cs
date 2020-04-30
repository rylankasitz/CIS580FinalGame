using Engine;
using Engine.Componets;
using Engine.ECSCore;
using Microsoft.Xna.Framework;
using MonoGameWindowsStarter.Characters.Helpers;
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
        public abstract string SpriteSheet { get; }
        public abstract string IdleAnimation { get; }
        public abstract string WalkAnimation { get; }
        public abstract string AttackAnimation { get; }
        public abstract float MoveSpeed { get; }
        public abstract int AttackDamage { get; }
        public abstract float AttackSpeed { get; }
        public abstract float Range { get;}
        public abstract int MaxHealth { get; }
        public abstract int Difficulty { get; }
        public abstract string ProjectileSprite { get; }
        public abstract Rectangle ProjectileSource { get; }

        public string Holder { get; set; }
        public AILogic AILogicCMD { get; set; }
        public ProjectileSpawner ProjectileSpawner { get; set; }
        public List<Enemy> Enemies { get; set; } = new List<Enemy>();

        public abstract void OnStateSwitch(string lastState);
        public abstract void Attack(Vector position, Vector direction);
        public abstract void HandleCollision(Projectile projectile, Entity collider, string direction);

        public void OnSpawn(Enemy enemy)
        {
            Enemies = SceneManager.GetCurrentScene().GetEntities<Enemy>();
            AILogicCMD = new AILogic(enemy);
            ProjectileSpawner = new ProjectileSpawner(HandleCollision, ProjectileSource, ProjectileSprite);
        }

        public void OnSpawn()
        {
            Enemies = SceneManager.GetCurrentScene().GetEntities<Enemy>();
            ProjectileSpawner = new ProjectileSpawner(HandleCollision, ProjectileSource, ProjectileSprite);
        }
    }
}
