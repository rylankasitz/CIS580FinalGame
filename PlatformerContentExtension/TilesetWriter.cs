﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;

using TWrite = PlatformerContentExtension.TilesetContent;

namespace PlatformerContentExtension
{
    /// <summary>
    /// A ContentTypeWriter for the TiledSpriteSheetContent type
    /// </summary>
    [ContentTypeWriter]
    public class TilesetWriter : ContentTypeWriter<TWrite>
    {
        
        /// <summary>
        /// Write the binary (xnb) file corresponding to the supplied 
        /// TilesetContent that will be imported into our game
        /// as a Tileset
        /// </summary>
        /// <param name="output">The ContentWriter that writes the binary output</param>
        /// <param name="value">The TilesetContent we are writing</param>
        protected override void Write(ContentWriter output, TWrite value)
        {
            // We only need to write the data that is needed in-game.  
            // For the TiledSpriteSheet, this is the texture and the tiles.
            // Everything else can be thrown away at this point.

            // Write the texture 
            output.WriteObject<TextureContent>(value.Texture);

            // Write the tile width & height 
            output.Write(value.TileWidth);
            output.Write(value.TileHeight);
            output.Write(value.Margin);
            output.Write(value.Spacing);

            // Write the number of tiles - this will be used to 
            // specify the expected number of tiles in the reader
            output.Write(value.TileCount);

            // Write the individual tile data
            for(int i = 0; i < value.TileCount; i++)
            {
                // We only need to specify the X and Y of the source 
                // rectangles, as they are all the same width and height
                var tile = value.Tiles[i];
                output.Write(tile.Source.X);
                output.Write(tile.Source.Y);
                output.Write(tile.BoxCollisions.Count);
                foreach(KeyValuePair<string, List<Box>> pair in tile.BoxCollisions)
                {
                    output.Write(pair.Key);
                    output.Write(pair.Value.Count);
                    foreach (Box box in pair.Value)
                    {
                        output.Write(box.Col.X);
                        output.Write(box.Col.Y);
                        output.Write(box.Col.Width);
                        output.Write(box.Col.Height);
                        output.Write(box.TriggerOnly);
                        output.Write(box.Name);
                    }
                }
                output.Write(tile.Animation.Name);
                output.Write(tile.Animation.Frames.Count);
                foreach(Frame frame in tile.Animation.Frames)
                {
                    output.Write(frame.Source.X);
                    output.Write(frame.Source.Y);
                    output.Write(frame.Source.Width);
                    output.Write(frame.Source.Height);
                    output.Write(frame.Duration);
                    output.Write(frame.Id);
                }
                output.Write(tile.Properties.Count);
                foreach(KeyValuePair<string, string> prop in tile.Properties)
                {
                    output.Write(prop.Key);
                    output.Write(prop.Value);
                }
            }

        }

        /// <summary>
        /// Gets the reader needed to read the binary content written by this writer
        /// </summary>
        /// <param name="targetPlatform"></param>
        /// <returns>The name of the reader</returns>
        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return "PlatformLibrary.TilesetReader, PlatformLibrary";
        }
    }
}
