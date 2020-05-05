using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace PlatformerContentExtension
{
    /// <summary>
    /// A class representing the details of a single 
    /// tile in a TiledSpriteSheet
    /// </summary>
    public class TileContent
    {
        public Rectangle Source { get; set; }
        public bool Solid { get; set; }
        public Rectangle BoxCollision { get; set; } = Rectangle.Empty;
        public Animation Animation { get; set; }
        public Dictionary<string, string> Properties { get; set; }

        public TileContent()
        {
            Properties = new Dictionary<string, string>();
            Animation = new Animation();
            Animation.Frames = new List<Frame>();
        }
    }

    public class Animation
    {
        public string Name { get; set; } = "None";
        public List<Frame> Frames { get; set; }
    }

    public class Frame
    {
        public Rectangle Source { get; set; }
        public double Duration { get; set; }
        public int Id { get; set; }

        public Frame(Rectangle source, double duration, int id)
        {
            Source = source;
            Duration = duration;
            Id = id;
        }
    }
}
