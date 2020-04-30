using Engine;
using Engine.Componets;
using Engine.ECSCore;
using Engine.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGameWindowsStarter.Characters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameWindowsStarter.Entities
{
    [Sprite(ContentName: "MapTileSet", Layer: .8f)]
    [Animation(CurrentAnimation: "CharacterPickup")]
    [Transform(X: 0, Y: 0, Width: 32, Height: 32)]
    [BoxCollision(X: 0, Y: 0, Width: 1, Height: 1, TriggerOnly: true)]
    public class CharacterPickup : Entity
    {
        public Transform Transform;
        public Character Character;

        private BoxCollision boxCollision;

        public override void Initialize()
        {
            Transform = GetComponent<Transform>();
            boxCollision = GetComponent<BoxCollision>();

            boxCollision.HandleCollision = handleCollision;
        }

        public override void Update(GameTime gameTime)
        {
            
        }

        private void handleCollision(Entity entity, string direction)
        {
            if (entity.Name == "Player")
            {
                Player player = (Player)entity;

                if (InputManager.KeyDown(Keys.Q))
                {
                    player.Character = Character;
                    player.Character.Holder = "Player";
                    player.Sprite.ContentName = player.Character.SpriteSheet;

                    SceneManager.GetCurrentScene().RemoveEntity(this);
                }
            }
        }
    }
}
