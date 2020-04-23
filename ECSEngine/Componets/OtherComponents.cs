using Microsoft.Xna.Framework;
using Engine.ECSCore;
using System.Collections.Generic;

namespace Engine.Componets
{
    public delegate void State(GameTime gameTime);

    public class StateMachine : Component
    {
        public string CurrentState { get; set; }
        public Dictionary<string, State> States { get; set; } = new Dictionary<string, State>();
        public StateMachine() { }
    }
}
