using Microsoft.Xna.Framework;
using Engine.ECSCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public static class SceneManager
    {
        public static List<ECSCore.System> systems { get; set; }

        private static Scene currentScene;
        private static List<Scene> scenes = new List<Scene>();
        private static GameManager gameManager;

        public static void AddScene(Scene scene)
        {
            scenes.Add(scene);
        }

        public static void Initialize(GameManager game)
        {
            gameManager = game;     
        }

        public static Scene GetCurrentScene()
        {
            return currentScene;
        }

        public static void LoadScene(string name)
        {
            if (currentScene != null)
            {
                currentScene.RemoveAllEntities();
                currentScene.RemoveAllUIScreeens();
            }

            findScene(name);

            currentScene.LoadScene(systems, gameManager);
        }

        public static void UpdateScene(GameTime gameTime)
        {
            currentScene.UpdateScene(gameTime);
            currentScene.Update(gameTime);
        }

        private static void findScene(string name)
        {
            foreach (Scene scene in scenes)
            {
                if (scene.Name == name)
                    currentScene = scene;
            }
        }
    }
}
