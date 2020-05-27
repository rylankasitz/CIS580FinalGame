using Engine;
using Engine.Componets;
using Engine.ECSCore;
using Microsoft.Xna.Framework;
using MonoGameWindowsStarter.GlobalValues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameWindowsStarter.Entities
{
    [Sprite(ContentName: "Key", Layer: .77f)]
    [Transform(X: 0, Y: 0, Width: 8, Height: 8)]
    [BoxCollision(X: 0, Y: 0, Width: 1, Height: 1, TriggerOnly: true)]
    public class Key : Entity
    {
        public Transform Transform;

        private BoxCollision boxCollision;

        public override void Initialize()
        {
            Name = "Key";
            Transform = GetComponent<Transform>();
            boxCollision = GetComponent<BoxCollision>();

            Transform.Scale = Transform.Scale * MapConstants.Scale;

            boxCollision.HandleCollision = handleCollision;
        }

        public override void Update(GameTime gameTime)
        {
            
        }

        private void handleCollision(Entity entity, string directions)
        {
            if (entity.Name == "Player")
            {
                Player player = (Player)entity;

                player.AddKey();

                if (player.KeyCount >= 3)
                {
                    player.OnKeysCollected(this);
                }

                SceneManager.GetCurrentScene().RemoveEntity(this);
            }
        }
    }
}
