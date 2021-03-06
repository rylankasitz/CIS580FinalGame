﻿using Engine;
using Engine.Componets;
using Engine.ECSCore;
using Engine.Systems;
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
        public abstract float AIMoveSpeedMod { get; }
        public abstract float AIAttackDamageMod { get; }
        public abstract float AIAttackSpeedMod { get; }
        public abstract float AIAttackRangeMod { get; }
        public abstract float AIHealthMod { get; }

        public abstract string ProjectileSprite { get; }
        public abstract string ProjectileAnimation { get; }
        public abstract Vector ProjectileScale { get; }
        public abstract float ProjectileSpeed { get; }

        public string Holder { get; set; }
        public AILogic AILogicCMD { get; set; }
        public ProjectileSpawner ProjectileSpawner { get; set; }
        public List<Enemy> Enemies { get; set; } = new List<Enemy>();

        public abstract void OnStateSwitch(string lastState);
        public abstract void Attack(Entity holder, Vector position, Vector direction);

        public void OnSpawn(Enemy enemy)
        {
            Enemies = SceneManager.GetCurrentScene().GetEntities<Enemy>();
            Player player = SceneManager.GetCurrentScene().GetEntity<Player>("Player");
            AILogicCMD = new AILogic(enemy);
            ProjectileSpawner = new ProjectileSpawner(ProjectileSprite, this);
        }

        public void OnSpawn()
        {
            Enemies = SceneManager.GetCurrentScene().GetEntities<Enemy>();
            Player player = SceneManager.GetCurrentScene().GetEntity<Player>("Player");
            ProjectileSpawner = new ProjectileSpawner(ProjectileSprite, this);
        }
    }
}
