using Microsoft.Xna.Framework;
using Engine.Componets;
using Engine.ECSCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Humper.Base;
using Humper.Responses;
using Humper;

namespace Engine.Systems
{
    public class CollisionHandler : ECSCore.System
    {
        #region ECS Methods

        public override bool SetSystemRequirments(Entity entity)
        {
            return entity.HasComponent<BoxCollision>() &&
                   entity.HasComponent<Componets.Transform>();
        }

        public override void Initialize()
        {
            foreach (Entity entity in Entities)
            {
                InitializeEntity(entity);
            }
        }

        public override void InitializeEntity(Entity entity) 
        {
            BoxCollision collider = entity.GetComponent<BoxCollision>();
            collider.World = SceneManager.GetCurrentScene().World;
        }

        public override void RemoveFromSystem(Entity entity)
        {
            Entities.Remove(entity);
        }

        public void CheckCollisions()
        {
            
        }

        #endregion     
    }
}
