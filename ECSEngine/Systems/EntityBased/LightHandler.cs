using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.ECSCore;
using Engine.Componets;
using Engine;
using Penumbra;

namespace ECSEngine.Systems.EntityBased
{
    public class LightHandler : Engine.ECSCore.System
    {
        PenumbraComponent penumbra;

        #region ESC Methods

        public LightHandler(PenumbraComponent penumbra)
        {
            this.penumbra = penumbra;
        }

        public override bool SetSystemRequirments(Entity entity)
        {
            return entity.HasComponent<Engine.Componets.Light>();
        }

        public override void Initialize()
        {
            foreach(Entity entity in Entities)
            {
                InitializeEntity(entity);
            }
        }

        public override void InitializeEntity(Entity entity)
        {
            Engine.Componets.Light light = entity.GetComponent<Engine.Componets.Light>();

            light.PointLight.ShadowType = light.ShadowType;
            penumbra.Lights.Add(light.PointLight);
        }

        public override void RemoveFromSystem(Entity entity)
        {
            Engine.Componets.Light light = entity.GetComponent<Engine.Componets.Light>();

            penumbra.Lights.Remove(light.PointLight);
        }

        public void UpdateLights()
        {
            foreach(Entity entity in Entities)
            {
                Engine.Componets.Light light = entity.GetComponent<Engine.Componets.Light>();

                light.PointLight.Position = WindowManager.ToScreenPosition(light.Position);
            }
        }

        #endregion
    }
}
