using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Engine.Componets;
using Engine.ECSCore;

namespace Engine.Systems
{
    public class StateHandler : ECSCore.System
    {
        #region ECS Methods

        public override bool SetSystemRequirments(Entity entity)
        {
            return entity.HasComponent<StateMachine>();
        }

        public override void Initialize() { }

        public override void InitializeEntity(Entity entity) { }

        public override void RemoveFromSystem(Entity entity) { }

        public void UpdateStateMachine(GameTime gameTime)
        {
            foreach(Entity entity in Entities)
            {
                StateMachine stateMachine = entity.GetComponent<StateMachine>();
                stateMachine.States[stateMachine.CurrentState]?.Invoke(gameTime);
            }
        }

        #endregion
    }
}
