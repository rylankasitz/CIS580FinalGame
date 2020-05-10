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
        public GameManager GameManager { get; set; }
        public string Name { get; set; } = "Unamed Scene";

        private Matcher matcher;
        private List<System> systems;

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

        public void LoadScene(List<System> systems, GameManager game)
        {
            this.systems = systems;
            GameManager = game;

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
        }
    }
}
