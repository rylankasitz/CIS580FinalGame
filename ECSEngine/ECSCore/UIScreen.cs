using Engine.Systems;
using GeonBit.UI;
using GeonBit.UI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECSEngine.ECSCore
{
    public abstract class UIScreen
    {
        public string Name { get; set; } = "Unamed";
        public List<Entity> Entities { get; set; } = new List<Entity>();
        public Dictionary<string, Entity> EntityBindings = new Dictionary<string, Entity>();
        public Dictionary<string, Entity> EntitySelectedBindings = new Dictionary<string, Entity>();

        public abstract void Initialize();
        public abstract void Update();

        public void UpdateBinds()
        {
            foreach (KeyValuePair<string, Entity> binding in EntityBindings) 
            {
                if (InputManager.GetAxisDown(binding.Key))
                {
                    binding.Value.OnClick(binding.Value);
                }
            }

            foreach (KeyValuePair<string, Entity> binding in EntitySelectedBindings)
            {
                if (InputManager.GetAxisDown(binding.Key) && binding.Value.State == EntityState.MouseHover)
                {
                    binding.Value.OnClick(binding.Value);
                }
            }
        }

        public void Bind(string inputAxis, Entity entity, bool OnlyWhenSelected = false)
        {
            if (OnlyWhenSelected)
                EntitySelectedBindings[inputAxis] = entity;
            else
                EntityBindings[inputAxis] = entity;
        }

        public void Select(Entity entity)
        {
            entity.State = EntityState.MouseHover;
        }

        public void AddEntity(Entity entity)
        {
            UserInterface.Active.AddEntity(entity);
            Entities.Add(entity);
        }

        public void RemoveEntity(Entity entity)
        {
            UserInterface.Active.RemoveEntity(entity);
            Entities.Remove(entity);
        }

        public void RemoveAllEntities()
        {
            foreach(Entity entity in Entities)
            {
                UserInterface.Active.RemoveEntity(entity);
            }
        }
    }
}
