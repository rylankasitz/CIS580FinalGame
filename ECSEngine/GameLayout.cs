using Engine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public abstract class GameLayout
    {
        public int WindowWidth { get; set; }
        public int WindowHeight { get; set; }
        public abstract void AddScenes();
        public abstract void Initialize();
        public abstract void Update(GameTime gameTime);
    }
}
