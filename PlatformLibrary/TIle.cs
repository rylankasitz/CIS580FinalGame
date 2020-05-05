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
        public Dictionary<string, Rectangle> BoxColliders { get; set; }
        public Texture2D Texture { get; set; }
        public TilesetAnimation Animation { get; set; }
        public Dictionary<string, string> Properties { get; set; }

        public int Width => Source.Width;
        public int Height => Source.Height;

        #endregion

        #region Initialization

        public Tile(Rectangle source, Dictionary<string, Rectangle> boxColliders, Dictionary<string, string> properties, Texture2D texture, TilesetAnimation animation)
        {
            Texture = texture;
            Source = source;
            BoxColliders = boxColliders;
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
        public int Id { get; set; }

        public TilesetFrame(Rectangle source, float duration, int id)
        {
            Source = source;
            Duration = duration;
            Id = id;
        }
    }
}