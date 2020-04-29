using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline;

namespace PlatformerContentExtension
{
    public class TilemapTileset
    {
        public int FirstGID;

        public string Source;

        public ExternalReference<TilesetContent> Reference;
    }

    public class TilemapContent
    {
        public uint MapWidth { get; set; }

        public uint MapHeight { get; set; }

        public uint TileWidth { get; set; }

        public uint TileHeight { get; set; }

        public List<TilemapTileset> Tilesets = new List<TilemapTileset>();

        public List<TilemapLayerContent> Layers = new List<TilemapLayerContent>();

        public Dictionary<string, List<Object>> Objects = new Dictionary<string, List<Object>>();

    }

    public class Object
    {
        public Vector2 Position { get; set; }
        public Vector2 Scale { get; set; }

        public Object(Vector2 position, Vector2 scale)
        {
            Position = position;
            Scale = scale;
        }
    }
}
