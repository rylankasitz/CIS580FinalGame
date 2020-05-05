using Microsoft.Xna.Framework;
using Engine.Componets;
using Engine.ECSCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Systems
{
    public class CollisionHandler : ECSCore.System
    {
        private Vector p1, p2, s1, s2;
        private Grid grid;

        #region ECS Methods

        public override bool SetSystemRequirments(Entity entity)
        {
            return entity.HasComponent<BoxCollision>() &&
                   entity.HasComponent<Transform>();
        }

        public override void Initialize()
        {
            grid = new Grid();

            foreach (Entity entity in Entities)
            {
                InitializeEntity(entity);
            }
        }

        public override void InitializeEntity(Entity entity) 
        {
            entity.AddToGrid(grid);
        }

        public override void RemoveFromSystem(Entity entity)
        {
            entity.RemoveFromGrid(grid);
        }

        public void CheckCollisions()
        {
            grid.Handle(handleCollisions);

            foreach (Entity entity in Entities)
            {
                grid.Move(entity);
            }
        }

        #endregion

        #region Private Methods

        private bool checkCollision(BoxCollision collider1, BoxCollision collider2, 
            Transform transform1, Transform transform2)
        {
            p1 = collider1.Position + transform1.Position;
            p2 = collider2.Position + transform2.Position;
            s1 = collider1.Scale * transform1.Scale;
            s2 = collider2.Scale * transform2.Scale;

            return (p1.X < p2.X + s2.X && p1.X + s1.X > p2.X && p1.Y < p2.Y + s2.Y && p1.Y + s1.Y > p2.Y);
        }

        private string handlePhysics(Entity entity, Transform transform, Vector p1, Vector s1, Vector p2, Vector s2, bool isTrigger)
        {
            string side = "";
            if (entity.HasComponent<Physics>())
            {
                Physics physics = entity.GetComponent<Physics>();

                if (p2.Y + s2.Y - (physics.Velocity.Y*2) > p1.Y && 
                    p2.Y - (physics.Velocity.Y*2) < p1.Y + s1.Y) {

                    if (physics.Velocity.X > 0)
                    {
                        if (!isTrigger)
                            transform.Position.X = p1.X - transform.Scale.X - (transform.Position.X - p2.X);
                        side = "Left";
                    }
                    else if (physics.Velocity.X < 0)
                    {
                        if (!isTrigger)
                            transform.Position.X = p1.X + s1.X - (p2.X - transform.Position.X);
                        side = "Right";
                    }
                }

                if (p2.X + s2.X - (physics.Velocity.X*2) > p1.X && 
                    p2.X - (physics.Velocity.X*2) < p1.X + s1.X)
                {

                    if (physics.Velocity.Y > 0)
                    {
                        if (!isTrigger)
                            transform.Position.Y = p1.Y - transform.Scale.Y - (transform.Position.Y - p2.Y);
                        side = "Top";
                    }
                    else if (physics.Velocity.Y < 0)
                    {
                        if (!isTrigger)
                            transform.Position.Y = p1.Y + s1.Y - (p2.Y - transform.Position.Y);
                        side = "Bottom";
                    }
                }
            }

            return side;
        }

        private void handleCollisions(Entity entity1, Entity entity2)
        {
            BoxCollision collider1 = entity1.GetComponent<BoxCollision>();
            BoxCollision collider2 = entity2.GetComponent<BoxCollision>();

            if (collider1.Enabled && collider2.Enabled)
            {
                if (collider1.Layer != collider2.Layer || (collider1.Layer == "All" && collider2.Layer == "All"))
                {
                    Transform transform1 = entity1.GetComponent<Transform>();
                    Transform transform2 = entity2.GetComponent<Transform>();

                    if (checkCollision(collider1, collider2, transform1, transform2))
                    {
                        string side1 = handlePhysics(entity1, transform1, p2, s2, p1, s1, collider1.TriggerOnly || collider2.TriggerOnly);
                        string side2 = handlePhysics(entity2, transform2, p1, s1, p2, s2, collider1.TriggerOnly || collider2.TriggerOnly);

                        if (!collider1.CollidingObjects.Contains(entity2))
                        {
                            collider1.HandleCollisionEnter?.Invoke(entity2, side1);
                            collider2.HandleCollisionEnter?.Invoke(entity1, side2);
                        }

                        collider1.HandleCollision?.Invoke(entity2, side1);
                        collider2.HandleCollision?.Invoke(entity1, side2);

                        collider1.CollidingObjects.Add(entity2);
                        collider2.CollidingObjects.Add(entity1);
                    }
                    else
                    {
                        if (collider1.CollidingObjects.Contains(entity2))
                            collider1.CollidingObjects.Remove(entity2);

                        if (collider2.CollidingObjects.Contains(entity1))
                            collider2.CollidingObjects.Remove(entity1);
                    }
                }
            }
        }

        #endregion
    }
}
