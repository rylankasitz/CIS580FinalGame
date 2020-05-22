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

namespace Engine.Systems
{
    public class Renderer : ECSCore.System 
    {
        private int pixelUnit = 1920;

        private ContentManager contentManager;
        private SpriteFont font;

        private Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();

        public override bool SetSystemRequirments(Entity entity)
        {
            return (entity.HasComponent<Sprite>() || entity.HasComponent<TextDraw>()) &&
                   entity.HasComponent<Transform>() && !entity.HasComponent<Parallax>();
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
            font = contentManager.Load<SpriteFont>("Fonts/PixelFont");
            this.textures = textures;
        }

        public void Draw(SpriteBatch spriteBatch)
        {     
            foreach (Entity entity in Entities)
            {
                Transform transform = entity.GetComponent<Transform>();

                if (entity.HasComponent<Sprite>() && entity.GetComponent<Sprite>().Enabled)
                {
                    Sprite sprite = entity.GetComponent<Sprite>();

                    if (sprite.SpriteLocation == Rectangle.Empty)
                        sprite.SpriteLocation = new Rectangle(0, 0, textures[sprite.ContentName].Width, textures[sprite.ContentName].Height);

                    Rectangle spriteLocation = new Rectangle(sprite.SpriteLocation.X, sprite.SpriteLocation.Y,
                        (int)(sprite.SpriteLocation.Width * sprite.Fill), sprite.SpriteLocation.Height);

                    if (textures.ContainsKey(sprite.ContentName))
                        spriteBatch.Draw(textures[sprite.ContentName],
                            new Rectangle((int)transform.Position.X, (int)transform.Position.Y, (int)(transform.Scale.X * sprite.Fill), (int)transform.Scale.Y),
                            spriteLocation, sprite.Color, transform.Rotation, new Vector2(0, 0),
                            sprite.SpriteEffects, sprite.Layer);
                    else
                        Debug.WriteLine($"Content '{sprite.ContentName}' does not exist");
                }

                if (entity.HasComponent<TextDraw>())
                {
                    TextDraw text = entity.GetComponent<TextDraw>();

                    if (!text.Center)
                        spriteBatch.DrawString(font, text.Text, transform.Position, text.Color, transform.Rotation, new Vector2(0, 0), transform.Scale, SpriteEffects.None, 0f);
                    else
                        spriteBatch.DrawString(font, text.Text, transform.Position - ((font.MeasureString(text.Text)/2)*transform.Scale), 
                            text.Color, transform.Rotation, new Vector2(0, 0), transform.Scale, SpriteEffects.None, 0f); ;
                }

                if (WindowManager.Debug && entity.HasComponent<BoxCollision>())
                {
                    BoxCollision boxCollision = entity.GetComponent<BoxCollision>();
                    foreach (Box box in boxCollision.Boxes)
                    {
                        Color color = new Color(Color.White, .5f);

                        if (box.TriggerOnly)
                            color = new Color(Color.Blue, .5f);

                        spriteBatch.Draw(textures[WindowManager.MouseTexture],
                            new Rectangle((int)((box.Position.X * transform.Scale.X) + transform.Position.X),
                                          (int)((box.Position.Y * transform.Scale.Y) + transform.Position.Y),
                                          (int)(box.Scale.X * transform.Scale.X),
                                          (int)(box.Scale.Y * transform.Scale.Y)), color);
                    }
                }
            }

            if (WindowManager.MouseTexture != "")
                spriteBatch.Draw(textures[WindowManager.MouseTexture], 
                    new Rectangle((int)InputManager.GetMousePosition().X, (int)InputManager.GetMousePosition().Y, 8, 8), 
                    Color.White);
        }
    }
}
