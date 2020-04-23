using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace PlatformLibrary
{
    /// <summary>
    /// A class representing a single tile from a tilesheet
    /// </summary>
    public struct Tile 
    {
        #region Properties

        public Rectangle Source { get; set; }
        public Rectangle BoxCollider { get; set; }
        public Texture2D Texture { get; set; }
        public Dictionary<string, string> Properties { get; set; }

        public int Width => Source.Width;
        public int Height => Source.Height;

        #endregion

        #region Initialization

        public Tile(Rectangle source, Rectangle boxCollider, Dictionary<string, string> properties, Texture2D texture)
        {
            Texture = texture;
            Source = source;
            BoxCollider = boxCollider;
            Properties = properties;
        }

        #endregion

        /*#region Drawing

        /// <summary>
        /// Draws the tile using the provided SpriteBatch 
        /// This method should should be called between 
        /// SpriteBatch.Begin() and SpriteBatch.End()
        /// </summary>
        /// <param name="SpriteBatch">The SpriteBatch</param>
        /// <param name="destinationRectangle">The rectangle to draw the tile into</param>
        /// <param name="color">The color</param>
        /// <param name="rotation">Rotation about the origin (in radians)</param>
        /// <param name="origin">A vector2 to the origin</param>
        /// <param name="effects">The SpriteEffects</param>
        /// <param name="layerDepth">The sorting layer of the tile</param>
        public void Draw(SpriteBatch SpriteBatch, Rectangle destinationRectangle, Color color, float rotation, Vector2 origin, SpriteEffects effects, float layerDepth)
        {
            if(texture != null)
            {
                SpriteBatch.Draw(texture, destinationRectangle, source, color, rotation, origin, effects, layerDepth);
            }
            
        }

        /// <summary>
        /// Draws the tile using the provided SpriteBatch 
        /// This method should should be called between 
        /// SpriteBatch.Begin() and SpriteBatch.End()
        /// </summary>
        /// <param name="SpriteBatch">The SpriteBatch</param>
        /// <param name="destinationRectangle">The rectangle to draw the tile into</param>
        /// <param name="color">The color</param>
        public void Draw(SpriteBatch SpriteBatch, Rectangle destinationRectangle, Color color)
        {
            if (texture != null)
            {
                SpriteBatch.Draw(texture, destinationRectangle, source, color);
            }
        }

        /// <summary>
        /// Draws the tile using the provided SpriteBatch 
        /// This method should should be called between 
        /// SpriteBatch.Begin() and SpriteBatch.End()
        /// </summary>
        /// <param name="SpriteBatch">The SpriteBatch</param>
        /// <param name="position">A Vector2 for position</param>
        /// <param name="color">The color</param>
        public void Draw(SpriteBatch SpriteBatch, Vector2 position, Color color)
        {
            if (texture != null)
            {
                SpriteBatch.Draw(texture, position, source, color);
            }
        }

        /// <summary>
        /// Draws the tile using the provided SpriteBatch 
        /// This method should should be called between 
        /// SpriteBatch.Begin() and SpriteBatch.End()
        /// </summary>
        /// <param name="SpriteBatch">The SpriteBatch</param>
        /// <param name="position">A Vector2 for position</param>
        /// <param name="color">The color</param>
        /// <param name="rotation">Rotation about the origin (in radians)</param>
        /// <param name="origin">A vector2 to the origin</param>
        /// <param name="scale">The scale of the tile centered on the origin</param>
        /// <param name="effects">The SpriteEffects</param>
        /// <param name="layerDepth">The sorting layer of the tile</param>
        public void Draw(SpriteBatch SpriteBatch, Vector2 position, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
        {
            if (texture != null)
            {
                SpriteBatch.Draw(texture, position, source, color, rotation, origin, scale, effects, layerDepth);
            }
        }

        #endregion*/
    }
}