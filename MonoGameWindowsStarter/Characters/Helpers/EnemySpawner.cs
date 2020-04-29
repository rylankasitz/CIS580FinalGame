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
        private Dictionary<int, List<Character>> characters;
        private Random random;

        public EnemySpawner()
        {
            characters = new Dictionary<int, List<Character>>();
            random = new Random();

            CreateCharacters<Character>();
        }

        #region Public Methods

        public void SpawnEnemiesInCurrentRoom(int minDifficulty, int maxDifficulty)
        {
            List<TileMapObject> spawns = MapManager.GetObjectLayer("Spawns");

            foreach(TileMapObject spawn in spawns)
            {
                spawnEnemy(new Vector(spawn.Position.X, spawn.Position.Y), 
                           random.Next(minDifficulty, maxDifficulty + 1));
            }
        }

        #endregion

        #region Private Methods

        private void spawnEnemy(Vector position, int difficulty)
        {
            Enemy enemy = SceneManager.GetCurrentScene().CreateEntity<Enemy>();
            enemy.Character = characters[difficulty][random.Next(0, characters[difficulty].Count)];
            enemy.Transform.Position = position;
        }

        private void CreateCharacters<T>() where T : Character
        {
            foreach (Type type in Assembly.GetAssembly(typeof(T)).GetTypes()
                .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(T))))
            {
                T obj = (T)Activator.CreateInstance(type, new object[0]);

                if (!characters.ContainsKey(obj.Difficulty))
                    characters[obj.Difficulty] = new List<Character>();

                characters[obj.Difficulty].Add(obj);
            }
        }

        #endregion
    }
}
