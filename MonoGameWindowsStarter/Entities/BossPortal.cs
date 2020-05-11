using Engine;
using Engine.Componets;
using Engine.ECSCore;
using Engine.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGameWindowsStarter.GlobalValues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameWindowsStarter.Entities
{
    [Sprite(ContentName: "BossPortal", Layer: .81f)]
    [Transform(X: 0, Y: 0, Width: 8, Height: 8)]
    [BoxCollision(X: 0, Y: 0, Width: 1, Height: 1, TriggerOnly: true)]
    public class BossPortal : Entity
    {
        public Transform Transform;

        private BoxCollision boxCollision;

        public override void Initialize()
        {
            Name = "BossPortal";
            Transform = GetComponent<Transform>();
            boxCollision = GetComponent<BoxCollision>();

            Transform.Scale = Transform.Scale * MapConstants.Scale;

            boxCollision.HandleCollision = handleCollision;

            MapConstants.BossPortalRoom = true;
        }

        public override void Update(GameTime gameTime)
        {

        }

        private void handleCollision(Entity entity, string directions)
        {
            if (InputManager.KeyDown(Keys.E))
            {
                SceneManager.LoadScene("End Screen");
            }
        }
    }
}
