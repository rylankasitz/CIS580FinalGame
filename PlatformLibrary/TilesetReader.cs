using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using TRead = PlatformLibrary.Tileset;

namespace PlatformLibrary
{
    /// <summary>
    /// A content reader for Tiled tilesets
    /// </summary>
    public class TilesetReader : ContentTypeReader<TRead>
    {
        /// <summary>
        /// Reads the tielset from an .xnb file
        /// </summary>
        /// <param name="input">The ContentReader</param>
        /// <param name="existingInstance"></param>
        /// <returns>A Tileset</returns>
        protected override TRead Read(ContentReader input, TRead existingInstance)
        {
            // Read in the content properties in the exact same 
            // order they were written in the corresponding writer
            // Read in the texture 
            var texture = input.ReadObject<Texture2D>();

            // Read in the tile attributes
            var tileWidth = input.ReadInt32();
            var tileHeight = input.ReadInt32();
            var margin = input.ReadInt32();
            var spacing = input.ReadInt32();
            var tileCount = input.ReadInt32();

            // Read in the tiles - the number will vary based on the tileset 
            var tiles = new Tile[tileCount];
            for(int i = 0; i < tileCount; i++)
            {
                // Get the source rectangle
                var source = new Rectangle(
                    input.ReadInt32(),
                    input.ReadInt32(),
                    tileWidth,
                    tileHeight);

                Dictionary<string, Rectangle> boxCollisions = new Dictionary<string, Rectangle>();
                int colliderCount = input.ReadInt32();
                for (int j = 0; j < colliderCount; j++)
                {
                    string type = input.ReadString();
                    Rectangle boxCollision = new Rectangle(
                        input.ReadInt32(),
                        input.ReadInt32(),
                        input.ReadInt32(),
                        input.ReadInt32());
                    boxCollisions.Add(type, boxCollision);
                }

                TilesetAnimation animation = new TilesetAnimation();
                animation.Frames = new List<TilesetFrame>();
                animation.Name = input.ReadString();
                int frameCount = input.ReadInt32();            

                for(int j = 0; j < frameCount; j++)
                {
                    animation.Frames.Add(new TilesetFrame(
                        new Rectangle(input.ReadInt32(), input.ReadInt32(), input.ReadInt32(), input.ReadInt32()), 
                                     (float)input.ReadDouble(), input.ReadInt32()));
                }

                int propLength = input.ReadInt32();
                Dictionary<string, string> properties = new Dictionary<string, string>();

                for(int j = 0; j < propLength; j++)
                {
                    properties.Add(input.ReadString(), input.ReadString());
                }

                // Create the tile
                tiles[i] = new Tile(source, boxCollisions, properties, texture, animation, spacing, margin);
            }

            // Construct and return the tileset
            return new Tileset(tiles);
        }
    }
}
