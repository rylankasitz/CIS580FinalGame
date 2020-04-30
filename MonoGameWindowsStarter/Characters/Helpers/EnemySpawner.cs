using Engine;
using Engine.Componets;
using Engine.Systems;
using MonoGameWindowsStarter.Entities;
using PlatformLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameWindowsStarter.Characters.Helpers
{
    public class EnemySpawner
    {
        private Dictionary<string, List<Enemy>> enemies;
        private Random random;

        public EnemySpawner()
        {
            enemies = new Dictionary<string, List<Enemy>>();
            random = new Random();
        }

        #region Public Methods

        public void SpawnEnemiesInRoom(string roomName, int minDifficulty, int maxDifficulty)
        {
            if (!enemies.ContainsKey(roomName))
            {
                List<TileMapObject> spawns = MapManager.GetObjectLayer("Spawns");

                foreach (TileMapObject spawn in spawns)
                {
                    spawnEnemy(new Vector(spawn.Position.X, spawn.Position.Y),
                               random.Next(minDifficulty, maxDifficulty + 1), roomName);
                    break;
                }
            }
            else
            {
                respawnEnemies(roomName);
            }
        }

        public void RemoveAllEnemies(string room)
        {
            if (!enemies.ContainsKey(room)) return;

            foreach(Enemy enemy in enemies[room])
            {
                SceneManager.GetCurrentScene().RemoveEntity(enemy);

                if (enemy.CharacterPickup != null)
                {
                    SceneManager.GetCurrentScene().RemoveEntity(enemy.CharacterPickup);
                }
            }
        }

        #endregion

        #region Private Methods

        private void spawnEnemy(Vector position, int difficulty, string roomName)
        {
            Enemy enemy = SceneManager.GetCurrentScene().CreateEntity<Enemy>();
            enemy.Character = createCharacter<BlackGhoul>(); // Change to random difficulty
            enemy.Transform.Position = position;
            enemy.Character.OnSpawn(enemy);

            if (!enemies.ContainsKey(roomName))
                enemies.Add(roomName, new List<Enemy>());

            enemies[roomName].Add(enemy);
        }

        private void respawnEnemies(string roomName)
        {
            for (int i = 0; i < enemies[roomName].Count; i++)
            {
                if (enemies[roomName][i].CharacterPickup == null)
                {
                    Enemy enemy = SceneManager.GetCurrentScene().CreateEntity<Enemy>();
                    enemy.Character = enemies[roomName][i].Character;
                    enemy.Transform.Position = enemies[roomName][i].Transform.Position;
                    enemy.Character.OnSpawn(enemy);
                    enemies[roomName][i] = enemy;
                }
            }
        }

        private T createCharacter<T>() where T : Character, new()
        {
            return new T();
        }

        #endregion
    }
}
