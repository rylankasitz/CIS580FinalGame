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
                foreach (MapLocationObject spawn in MapManager.GetObjectLayer("Spawns"))
                {
                    if (flipped)
                        spawnEnemy(new Vector(spawn.Position.X + ((WindowManager.Width / 2) - spawn.Position.X)*2, spawn.Position.Y),
                                   random.Next(minDifficulty, maxDifficulty + 1), roomName);
                    else
                        spawnEnemy(new Vector(spawn.Position.X, spawn.Position.Y),
                                   random.Next(minDifficulty, maxDifficulty + 1), roomName);
                }
            }
            else
            {
                // explored
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
            enemy.Transform.Position = new Vector(position.X, position.Y);
            enemy.Character.OnSpawn(enemy);

            Debug.WriteLine($"Spawned '{enemy.Character.GetType().Name}' with difficulty {enemy.Character.Difficulty} at {position.X}, {position.Y}");

            if (!enemies.ContainsKey(roomName))
                enemies.Add(roomName, new List<Enemy>());

            enemies[roomName].Add(enemy);
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
