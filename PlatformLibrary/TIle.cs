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
        public TilesetAnimation Animation { get; set; }
        public Dictionary<string, string> Properties { get; set; }

        public int Width => Source.Width;
        public int Height => Source.Height;

        #endregion

        #region Initialization

        public Tile(Rectangle source, Rectangle boxCollider, Dictionary<string, string> properties, Texture2D texture, TilesetAnimation animation)
        {
            Texture = texture;
            Source = source;
            BoxCollider = boxCollider;
            Properties = properties;
            Animation = animation;
        }

        #endregion
    
    }
    
    public class TilesetAnimation
    {
        public string Name { get; set; }
        public List<TilesetFrame> Frames { get; set; }     
    }

    public class TilesetFrame
    {
        public float Duration { get; set; }
        public Rectangle Source { get; set; }

        public TilesetFrame(Rectangle source, float duration)
        {
            Source = source;
            Duration = duration;
        }
    }
}