using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Engine.Componets;
using Engine.ECSCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Systems
{
    public class ParallaxHandler : ECSCore.System
    {
        private Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();

        #region ECS Methods

        public override void Initialize()
        {
            foreach(Entity entity in Entities)
            {
                InitializeEntity(entity);
            }
        }

        public override void InitializeEntity(Entity entity)
        {
            entity.GetComponent<Parallax>().LayerNum = 1 - entity.GetComponent<Parallax>().Layer / (float) 10;
        }

        public override bool SetSystemRequirments(Entity entity)
        {
            return entity.HasComponent<Sprite>() && entity.HasComponent<Transform>() && entity.HasComponent<Parallax>();
        }

        public override void RemoveFromSystem(Entity entity) { }

        public void LoadContent(Dictionary<string, Texture2D> textures)
        {
            this.textures = textures;
        }

        public void UpdateParallax(GameTime gameTime)
        {
            foreach (Entity entity in Entities)
            {
                Parallax parallax = entity.GetComponent<Parallax>();
                parallax.ElapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
        }

        public void Draw(SpriteBatch spriteBatch, GameManager gameManager)
        {
            foreach (Entity entity in Entities)
            {
                Sprite sprite = entity.GetComponent<Sprite>();
                Transform transform = entity.GetComponent<Transform>();
                Parallax parallax = entity.GetComponent<Parallax>();

                spriteBatch.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, parallax.Transform + Camera.GetTransformation());

                if (sprite.SpriteLocation == Rectangle.Empty)
                    sprite.SpriteLocation = new Rectangle(0, 0, textures[sprite.ContentName].Width, textures[sprite.ContentName].Height);

                // Cleanup Repeated Textures
                if ((transform.Position.X + transform.Scale.X * parallax.RepeatCount.Y + parallax.Transform.M11) < (Camera.Position.X + gameManager.WindowWidth))
                    parallax.RepeatCount.Y++;
                
                for(int i = (int)parallax.RepeatCount.X; i < parallax.RepeatCount.Y; i++)
                { 
                    spriteBatch.Draw(textures[sprite.ContentName],
                        new Rectangle((int)transform.Position.X + (int)transform.Scale.X*i, (int)transform.Position.Y, 
                                      (int)transform.Scale.X, (int)transform.Scale.Y),
                        sprite.SpriteLocation, sprite.Color, transform.Rotation, new Vector2(0, 0),
                        sprite.SpriteEffects, parallax.LayerNum);
                }

                spriteBatch.End();
            }
        }

        #endregion
    }
}
