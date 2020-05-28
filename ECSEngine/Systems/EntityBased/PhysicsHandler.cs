using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Engine.Componets;
using Engine.ECSCore;
using Humper;
using Humper.Responses;
using System.Diagnostics;

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

                // Check Collision
                if (entity.HasComponent<BoxCollision>())
                {
                    BoxCollision boxCollision = entity.GetComponent<BoxCollision>();

                    IBox collidedBox = null;

                    foreach(IBox box in boxCollision.Boxes)
                    {
                        BoxData data = (BoxData)box.Data;
                        var move = box.Move(transform.Position.X + data.Position.X * transform.Scale.X, 
                                            transform.Position.Y + data.Position.Y * transform.Scale.Y, 
                                            (collision) =>
                        {
                            if (!data.TriggerOnly && !collision.Other.HasTag(data.Layer))
                            {
                                collidedBox = box;
                                return CollisionResponses.Slide;
                            }

                            return CollisionResponses.None;
                        });
                    }

                    if (collidedBox != null)
                    {
                        BoxData boxData = (BoxData) collidedBox.Data;
                        transform.Position = new Vector(collidedBox.X - transform.Scale.X * boxData.Position.X, collidedBox.Y - transform.Scale.Y * boxData.Position.Y);
                    }
                }
            }
        }

        public void SetPreTransform()
        {
            foreach (Entity entity in Entities)
            {
                Transform transform = entity.GetComponent<Transform>();
                Physics physics = entity.GetComponent<Physics>();

                if (physics.Velocity.X != 0 || physics.Velocity.Y != 0)
                    physics.PreVelocity = new Vector(physics.Velocity.X, physics.Velocity.Y);

                transform.PrePosition = new Vector(transform.Position.X, transform.Position.Y);
            }
        }
    }
}
