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
            Entities.Remove(entity);
        }

        public void CheckCollisions()
        {
            grid.Handle(handleCollisions);

            for(int i = 0; i < Entities.Count; i++)
            {
                grid.Move(Entities[i]);
                //for(int j = i; j < Entities.Count; j++)
                //{
                //    handleCollisions(Entities[i], Entities[j]);
                //}
            }
        }

        #endregion

        #region Private Methods

        private bool checkCollision(Box collider1, Box collider2, Transform transform1, Transform transform2)
        {
            p1 = transform1.Position + (collider1.Position * transform1.Scale);
            p2 = transform2.Position + (collider2.Position * transform2.Scale);
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

                if (p2.Y + s2.Y - (physics.PreVelocity.Y*2) > p1.Y && 
                    p2.Y - (physics.PreVelocity.Y*2) < p1.Y + s1.Y) {

                    if (physics.PreVelocity.X > 0)
                    {
                        if (!isTrigger)
                            transform.Position.X = p1.X - (p2.X - transform.Position.X) - s2.X;
                        side = "Left";
                    }
                    else if (physics.PreVelocity.X < 0)
                    {
                        if (!isTrigger)
                            transform.Position.X = p1.X + s1.X - (p2.X - transform.Position.X);
                        side = "Right";
                    }
                }

                if (p2.X + s2.X - (physics.PreVelocity.X*2) > p1.X && 
                    p2.X - (physics.PreVelocity.X*2) < p1.X + s1.X)
                {

                    if (physics.PreVelocity.Y > 0)
                    {
                        if (!isTrigger)
                            transform.Position.Y = p1.Y - (p2.Y - transform.Position.Y) - s2.Y;
                        side = "Top";
                    }
                    else if (physics.PreVelocity.Y < 0)
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
                bool differentLayers = true;
                foreach (string layer1 in collider1.Layers)
                {
                    foreach(string layer2 in collider2.Layers)
                    {
                        if (layer1 == layer2)
                        {
                            differentLayers = false;
                            break;
                        }                     
                    }
                }

                if (differentLayers || (collider1.Layer == "All" && collider2.Layer == "All"))
                {
                    Transform transform1 = entity1.GetComponent<Transform>();
                    Transform transform2 = entity2.GetComponent<Transform>();

                    foreach (Box box1 in collider1.Boxes)
                    {
                        foreach (Box box2 in collider2.Boxes)
                        {
                            if (checkCollision(box1, box2, transform1, transform2))
                            {
                                string side1 = handlePhysics(entity1, transform1, p2, s2, p1, s1, box1.TriggerOnly || box2.TriggerOnly);
                                string side2 = handlePhysics(entity2, transform2, p1, s1, p2, s2, box1.TriggerOnly || box2.TriggerOnly);

                                if (collider1.HandleCollisionEnter != null && !box1.CollidingObjects.Contains(box2))
                                {
                                    collider1.HandleCollisionEnter?.Invoke(entity2, side1);
                                    box1.CollidingObjects.Add(box2); 
                                }

                                if (collider2.HandleCollisionEnter != null && !box2.CollidingObjects.Contains(box1))
                                {  
                                    collider2.HandleCollisionEnter?.Invoke(entity1, side2);
                                    box2.CollidingObjects.Add(box1);
                                }
                   
                                collider1.HandleCollision?.Invoke(entity2, side1);
                                collider2.HandleCollision?.Invoke(entity1, side2);      
                            }
                            else
                            {
                                if (collider1.HandleCollisionEnter != null)
                                    box1.CollidingObjects.Remove(box2);

                                if (collider2.HandleCollisionEnter != null)
                                    box2.CollidingObjects.Remove(box1);
                            }
                        }
                    }
                }
            }
        }

        #endregion
    }
}
