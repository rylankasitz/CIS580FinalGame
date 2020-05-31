using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Engine.Componets;
using Engine.ECSCore;
using System.IO;
using System.Diagnostics;
using ECSEngine.Systems;
using Humper;
using Comora;
using Penumbra;

namespace Engine.Systems
{
    public class Renderer : ECSCore.System 
    {
        private ContentManager contentManager;

        private Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();

        public override bool SetSystemRequirments(Entity entity)
        {
            return entity.HasComponent<Sprite>() && entity.HasComponent<Componets.Transform>();
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

        }

        public override void RemoveFromSystem(Entity entity) { }

        public void LoadContent(ContentManager content, Dictionary<string, Texture2D> textures)
        {
            contentManager = content;
            this.textures = textures;
        }

        public void Draw(SpriteBatch spriteBatch, PenumbraComponent penumbra)
        {     
            foreach (Entity entity in Entities)
            {
                Componets.Transform transform = entity.GetComponent<Componets.Transform>();

                // Sprite Draw
                if (entity.HasComponent<Sprite>() && entity.GetComponent<Sprite>().Enabled)
                {
                    Sprite sprite = entity.GetComponent<Sprite>();

                    if (sprite.SpriteLocation == Rectangle.Empty)
                        sprite.SpriteLocation = new Rectangle(0, 0, textures[sprite.ContentName].Width, textures[sprite.ContentName].Height);

                    Rectangle spriteLocation = new Rectangle(sprite.SpriteLocation.X, sprite.SpriteLocation.Y,
                        (int)(sprite.SpriteLocation.Width * sprite.Fill), sprite.SpriteLocation.Height);

                    if (textures.ContainsKey(sprite.ContentName))
                        spriteBatch.Draw(textures[sprite.ContentName],
                            new Rectangle((int)Math.Floor(transform.Position.X), (int)Math.Floor(transform.Position.Y), (int)Math.Floor(transform.Scale.X * sprite.Fill), (int)Math.Floor(transform.Scale.Y)),
                            spriteLocation, sprite.Color, transform.Rotation, new Vector2(0, 0),
                            sprite.SpriteEffects, sprite.Layer);
                    else
                        Debug.WriteLine($"Content '{sprite.ContentName}' does not exist");
                }

                // Debug
                if (WindowManager.ShowCollisionsDetails)
                {
                    if (entity.HasComponent<BoxCollision>())
                    {
                        BoxCollision boxCollision = entity.GetComponent<BoxCollision>();
                        foreach (IBox box in boxCollision.Boxes)
                        {
                            Color color = new Color(Color.White, .5f);

                            spriteBatch.Draw(textures["Pixel"], new Rectangle((int)box.X, (int)box.Y, (int)box.Width, (int)box.Height), color);
                        }
                    }
                }
            }
        }
    }
}
