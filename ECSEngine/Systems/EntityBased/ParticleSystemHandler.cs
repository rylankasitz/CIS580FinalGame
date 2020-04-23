using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Engine.Componets;
using Engine.ECSCore;

namespace Engine.Systems
{
    public class ParticleSystemHandler : ECSCore.System
    {
        private Random random = new Random();
        private Dictionary<string, Texture2D> textures;

        public override void Initialize()
        {
            foreach(Entity entity in Entities)
            {
                InitializeEntity(entity);
            }
        }

        public override void InitializeEntity(Entity entity)
        {
            ParticleSystem particleSystem = entity.GetComponent<ParticleSystem>();
            particleSystem.Particles = new Particle[particleSystem.ParticleCount];

            for(int i = 0; i < particleSystem.Particles.Length; i++)
                particleSystem.Particles[i] = new Particle();

        }

        public void LoadContent(ContentManager content)
        {
            textures = new Dictionary<string, Texture2D>();
            foreach(Entity entity in Entities)
            {
                ParticleSystem particleSystem = entity.GetComponent<ParticleSystem>();
                textures[particleSystem.Texture] = content.Load<Texture2D>(particleSystem.Texture);
            }
        }

        public override void RemoveFromSystem(Entity entity)
        {
            
        }

        public override bool SetSystemRequirments(Entity entity)
        {
            return entity.HasComponent<ParticleSystem>() && entity.HasComponent<Transform>();
        }

        public void UpdateParticleSystems(GameTime gameTime)
        {
            foreach(Entity entity in Entities)
            {
                ParticleSystem system = entity.GetComponent<ParticleSystem>();
                system.ElapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (system.Running && (system.ElapsedTime < system.Time || system.Time == -1)) 
                { 
                    SpawnParticles(entity);
                }
                UpdateParticles(entity, gameTime);
            }
        }

        public void DrawParticleSystems(SpriteBatch spriteBatch)
        {
            foreach(Entity entity in Entities)
            {
                DrawParticles(entity, spriteBatch);
            }
        }

        #region Private Methods

        private void SpawnParticles(Entity entity)
        {
            ParticleSystem particleSystem = entity.GetComponent<ParticleSystem>();
            Transform transform = entity.GetComponent<Transform>();
            for (int i = 0; i < particleSystem.SpawnPerFrame; i++)
            {
                particleSystem.Particles[particleSystem.NextIndex].Position = new Vector2(particleSystem.Emitter.X * transform.Scale.X + transform.Position.X, 
                                                                                          particleSystem.Emitter.Y * transform.Scale.Y + transform.Position.Y);
                particleSystem.Particles[particleSystem.NextIndex].Velocity = particleSystem.Velocity * 
                                                                              new Vector2((float)random.NextDouble() * particleSystem.Range + particleSystem.Direction.X, 
                                                                                          (float)random.NextDouble() * particleSystem.Range + particleSystem.Direction.Y);
                particleSystem.Particles[particleSystem.NextIndex].Acceleration = particleSystem.Acceleration * 
                                                                                  new Vector2((float)random.NextDouble() * particleSystem.Range + particleSystem.Direction.X,
                                                                                              (float)random.NextDouble() * particleSystem.Range + particleSystem.Direction.Y);
                particleSystem.Particles[particleSystem.NextIndex].Color = particleSystem.Color;
                particleSystem.Particles[particleSystem.NextIndex].Scale = particleSystem.Scale;
                particleSystem.Particles[particleSystem.NextIndex].Life = particleSystem.Life;

                particleSystem.NextIndex++;
                if (particleSystem.NextIndex > particleSystem.Particles.Length - 1) particleSystem.NextIndex = 0;
            }
        }

        private void UpdateParticles(Entity entity, GameTime gameTime)
        {
            ParticleSystem particleSystem = entity.GetComponent<ParticleSystem>();
            float deltaT = (float)gameTime.ElapsedGameTime.TotalSeconds;

            for (int i = 0; i < particleSystem.Particles.Length; i++)
            {
                if (particleSystem.Particles[i].Life <= 0) continue;

                particleSystem.Particles[i].Velocity += deltaT * particleSystem.Particles[i].Acceleration;
                particleSystem.Particles[i].Position += deltaT * particleSystem.Particles[i].Velocity;
                particleSystem.Particles[i].Life -= deltaT;
            }
        }

        private void DrawParticles(Entity entity, SpriteBatch spriteBatch)
        {
            ParticleSystem particleSystem = entity.GetComponent<ParticleSystem>();
            for (int i = 0; i < particleSystem.Particles.Length; i++)
            {
                if (particleSystem.Particles[i].Life <= 0) continue;

                spriteBatch.Draw(textures[particleSystem.Texture], particleSystem.Particles[i].Position, null, 
                    particleSystem.Particles[i].Color, 0f, Vector2.Zero, particleSystem.Particles[i].Scale, SpriteEffects.None, 0);
            }
        }

        #endregion
    }
}
