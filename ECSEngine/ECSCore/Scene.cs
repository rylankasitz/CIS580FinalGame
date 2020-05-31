using ECSEngine.ECSCore;
using Engine.Systems;
using GeonBit.UI;
using Humper;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Engine.ECSCore
{
    public abstract class Scene
    {
        public List<Entity> Entities { get; set; } = new List<Entity>();
        public Dictionary<string, UIScreen> UIScreens { get; set; } = new Dictionary<string, UIScreen>();
        public GameManager GameManager { get; set; }
        public string Name { get; set; } = "Unamed Scene";

        public World World { get; set; }
        public Vector2 WorldDimensions { get; set; } = new Vector2(100000, 100000);

        private Matcher matcher;
        private List<System> systems;
        private UIScreen currentScreen;

        public abstract void Initialize();
        public abstract void Update(GameTime gameTime);

        public T CreateEntity<T>() where T : Entity, new()
        {
            T entity = GameManager.Template.CreateTemplateObj<T>();

            entity.Initialize();

            foreach (System system in systems)
            {
                if (system.Loaded)
                    system.AddEntity(entity);
            }

            Entities.Add(entity);

            return entity;
        }

        public void RemoveEntity(Entity entity)
        {
            foreach (System system in systems)
            {
                if (system.Entities.Contains(entity))
                    system.RemoveEntity(entity);
            }

            Entities.Remove(entity);
        }

        public void RemoveAllEntities()
        {
            while(Entities.Count > 0)
            {
                RemoveEntity(Entities[0]);
            }
        }

        public T GetEntity<T>(string name) where T : Entity, new()
        {
            foreach(Entity entity in Entities)
            {
                if (entity.Name == name)
                {
                    return (T) entity;
                }
            }

            return null;
        }

        public List<T> GetEntities<T>() where T : Entity, new()
        {
            List<T> entities = new List<T>();
            foreach (Entity entity in Entities)
            {
                // Make more efficient
                if (entity.GetType().Name == typeof(T).Name)
                {
                    entities.Add((T)entity);
                }
            }

            return entities;
        }

        public void AddUIScreen(string name, UIScreen uiScreen)
        {
            uiScreen.Initialize();
            UIScreens.Add(name, uiScreen);
        }

        public void LoadUIScreen(string name)
        {
            currentScreen = UIScreens[name];
        }

        public void RemoveAllUIScreeens()
        {
            foreach (UIScreen uIScreen in UIScreens.Values)
            {
                uIScreen.RemoveAllEntities();
            }
        }

        public void LoadScene(List<System> systems, GameManager game)
        {
            this.systems = systems;
            GameManager = game;
            World = new World(WorldDimensions.X, WorldDimensions.Y);

            Initialize();

            matcher = new Matcher(Entities);

            foreach (System system in systems)
            {
                system.LoadEntities(matcher);
                system.Initialize();
            }
        }

        public void UpdateScene(GameTime gameTime)
        {
            for (int i = 0;  i < Entities.Count; i++)
            {
                Entities[i].Update(gameTime);
            }

            currentScreen?.Update();
            currentScreen?.UpdateBinds();
        }
    }
}
