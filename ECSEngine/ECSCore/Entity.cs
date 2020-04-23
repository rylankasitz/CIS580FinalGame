using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.ECSCore
{
    public abstract class Entity
    {
        public Dictionary<string, Component> Components { set;  get; } = new Dictionary<string, Component>();
        public string Name { get; set; } = "Unamed";

        public Entity Next { get; set; }
        public Entity Prev { get; set; }
        public int OldCellX { get; set; }
        public int OldCellY { get; set; }

        #region Component Methods

        public T AddComponent<T>() where T : Component, new()
        {
            T c = new T();
            Components[typeof(T).Name] = c;
            return c;
        }

        public T GetComponent<T>() where T : Component, new()
        {
            return (T) Components[typeof(T).Name];
        }

        public bool HasComponent<T>() where T : Component, new()
        {
            return Components.Keys.Contains(typeof(T).Name);
        }

        public void RemoveComponent<T>() where T : Component
        {
            Components.Remove(typeof(T).Name);
        }

        #endregion

        public void AddToGrid(Grid grid)
        {
            grid.Add(this);
        }

        public void RemoveFromGrid(Grid grid)
        {
            grid.Remove(this);
        }

        public abstract void Initialize();
        public abstract void Update(GameTime gameTime);
    }
}
