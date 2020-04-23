using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Engine.Componets;
using Engine.ECSCore;

namespace Engine.Systems
{
    public class PhysicsHandler : ECSCore.System
    {
        public override bool SetSystemRequirments(Entity entity)
        {
            return entity.HasComponent<Physics>() &&
                   entity.HasComponent<Transform>();
        }

        public override void Initialize() { }

        public override void InitializeEntity(Entity entity) { }

        public override void RemoveFromSystem(Entity entity) { }

        public void HandlePhysics()
        {
            foreach (Entity entity in Entities)
            {
                Transform transform = entity.GetComponent<Transform>();
                Physics physics = entity.GetComponent<Physics>();

                transform.Position += physics.Velocity;
            }
        }

    }
}
