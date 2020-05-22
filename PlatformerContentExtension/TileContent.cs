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
        public Dictionary<string, List<Box>> BoxCollisions { get; set; } = new Dictionary<string, List<Box>>();
        public Animation Animation { get; set; }
        public Dictionary<string, string> Properties { get; set; }

        public TileContent()
        {
            Properties = new Dictionary<string, string>();
            Animation = new Animation();
            Animation.Frames = new List<Frame>();
        }
    }

    public class Box
    {
        public Rectangle Col { get; set; } = Rectangle.Empty;
        public bool TriggerOnly { get; set; } = false;
        public string Name { get; set; }
        public Box(Rectangle col, bool trigger, string name = "Unamed")
        {
            Col = col;
            TriggerOnly = trigger;
            Name = name;
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
