using ECSEngine.Systems;
using Engine;
using Engine.Componets;
using Engine.Systems;
using MonoGameWindowsStarter.Entities;
using PlatformLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameWindowsStarter.Characters.Helpers
{
    public class EnemySpawner
    {
        private Dictionary<string, List<Enemy>> enemies;
        private Dictionary<int, List<Type>> characters;
        private Random random;

        public EnemySpawner()
        {
            enemies = new Dictionary<string, List<Enemy>>();
            characters = getCharacterTypes();
            random = new Random();
        }

        #region Public Methods

        public void SpawnEnemiesInRoom(string roomName, bool flipped, int minDifficulty, int maxDifficulty)
        {
            if (!enemies.ContainsKey(roomName))
            {
                List<TileMapObject> spawns = MapManager.GetObjectLayer("Spawns");

                foreach (TileMapObject spawn in spawns)
                {
                    if (flipped)
                        spawnEnemy(new Vector(spawn.Position.X + (WindowManager.Width / 2 - spawn.Position.X), spawn.Position.Y),
                                   random.Next(minDifficulty, maxDifficulty + 1), roomName);
                    else
                        spawnEnemy(new Vector(spawn.Position.X, spawn.Position.Y),
                                   random.Next(minDifficulty, maxDifficulty + 1), roomName);
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
            enemy.Character = createCharacter(difficulty);
            enemy.Transform.Position = position;
            enemy.Character.OnSpawn(enemy);

            Debug.WriteLine($"Spawned enemy with difficulty {enemy.Character.Difficulty}");

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

        private Character createCharacter(int difficulty)
        {
            return (Character) Activator.CreateInstance(characters[difficulty][random.Next(0, characters[difficulty].Count)]);
        }

        private Dictionary<int, List<Type>> getCharacterTypes()
        {
            Dictionary<int, List<Type>> characters = new Dictionary<int, List<Type>>();
            foreach (Type type in Assembly.GetAssembly(typeof(Character)).GetTypes()
                .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(Character))))
            {
                Character character = (Character)Activator.CreateInstance(type);

                if (!characters.ContainsKey(character.Difficulty))
                {
                    characters.Add(character.Difficulty, new List<Type>());
                    
                }
                characters[character.Difficulty].Add(type);
            }
            return characters;
        }

        #endregion
    }
}
