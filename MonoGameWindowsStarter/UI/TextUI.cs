using Engine.Componets;
using Engine.ECSCore;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameWindowsStarter.UI
{
    [TextDraw()] [Transform()]
    public class TextUI : Entity
    {
        public Transform Transform;
        public TextDraw Text;

        public override void Initialize()
        {
            Transform = GetComponent<Transform>();
            Text = GetComponent<TextDraw>();
        }

        public override void Update(GameTime gameTime)
        {
            
        }
    }
}
