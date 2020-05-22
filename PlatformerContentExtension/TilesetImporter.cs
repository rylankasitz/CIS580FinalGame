using System;
using System.Xml;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content.Pipeline;

using TInput = PlatformerContentExtension.TilesetContent;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace PlatformerContentExtension
{
    /// <summary>
    /// This class will be instantiated by the XNA Framework Content Pipeline
    /// to import a file from disk into the specified type, TImport.
    ///
    /// This should be part of a Content Pipeline Extension Library project.
    ///
    /// TODO: change the ContentImporter attribute to specify the correct file
    /// extension, display name, and default processor for this importer.
    /// </summary>

    [ContentImporter(".tsx", DisplayName = "TSX Importer - Tiled", DefaultProcessor = "TilesetProcessor - Tiled")]
    public class TilesetImporter : ContentImporter<TInput>
    {

        public override TInput Import(string filename, ContentImporterContext context)
        {
            XmlDocument document = new XmlDocument();
            document.Load(filename);

            // The tileset should be the tileset tag
            XmlNode tileset = document.SelectSingleNode("//tileset");

            // The attributes on the tileset are the properties of our spritesheet
            string name = tileset.Attributes["name"].Value;
            int tileWidth = int.Parse(tileset.Attributes["tilewidth"].Value);
            int tileHeight = int.Parse(tileset.Attributes["tileheight"].Value);
            int spacing = tileset.Attributes["spacing"] == null ? 0 : int.Parse(tileset.Attributes["spacing"].Value);
            int margin = tileset.Attributes["margin"] == null ? 0 : int.Parse(tileset.Attributes["margin"].Value);
            int tileCount = int.Parse(tileset.Attributes["tilecount"].Value);
            int columns = int.Parse(tileset.Attributes["columns"].Value);

            // A tileset will contain an image element that serves as the source of the tiles
            XmlNodeList images = tileset.SelectNodes("//image");
            var imageFilename = images[0].Attributes["source"].Value;
            var imageColorKey = images[0].Attributes["trans"] != null ? images[0].Attributes["trans"].Value : null;

            TileContent[] tileContent = new TileContent[tileCount];
            XmlNodeList tiles = tileset.SelectNodes("//tile");

            foreach(XmlNode node in tiles) 
            {
                int id = int.Parse(node.Attributes["id"].Value);
       
                tileContent[id] = new TileContent();

                bool animation = false;
                
                // Get properties
                XmlNodeList children = node.ChildNodes;
                foreach(XmlNode child in children)
                {
                    if (child.Name == "properties")
                    {
                        XmlNodeList properity = child.ChildNodes;
                        foreach (XmlNode p in properity)
                        {
                            if(p.Name == "property")
                                tileContent[id].Properties[p.Attributes["name"].Value] = p.Attributes["value"].Value;
                        }
                    }       
                    if (child.Name == "objectgroup")
                    {
                        foreach (XmlNode collision in child.ChildNodes)
                        {
                            float x = float.Parse(collision.Attributes["x"].Value);
                            float y = float.Parse(collision.Attributes["y"].Value);
                            float width = float.Parse(collision.Attributes["width"].Value);
                            float height = float.Parse(collision.Attributes["height"].Value);
                            string pname = collision.Attributes["type"] != null ? collision.Attributes["type"].Value : "unamed";
                            bool trigger = false;
                            string bname = "Unamed";

                            foreach (XmlNode colProps in collision.ChildNodes)
                            {
                                if (colProps.Name == "properties") 
                                { 
                                    foreach (XmlNode colProp in colProps.ChildNodes)
                                    {

                                        if (colProp.Name == "property") 
                                        {
                                            if (colProp.Attributes["name"].Value == "Trigger" )
                                                trigger = colProp.Attributes["value"].Value == "true";
                                            if (colProp.Attributes["name"].Value == "Name")
                                                bname = colProp.Attributes["value"].Value;
                                        }
                                    }
                                }
                            }

                            if (tileContent[id].BoxCollisions.ContainsKey(pname))
                            {
                                tileContent[id].BoxCollisions[pname].Add(new Box(new Rectangle((int)x, (int)y, (int)width, (int)height), trigger, name));
                            }
                            else
                            {
                                tileContent[id].BoxCollisions.Add(pname, new List<Box>() { new Box(new Rectangle((int)x, (int)y, (int)width, (int)height), trigger, bname) });
                            }
                            
                        }
                    }
                    if (child.Name == "animation")
                    {
                        foreach (XmlNode a in child.ChildNodes)
                        {
                            int tileid = int.Parse(a.Attributes["tileid"].Value);
                            int duration = int.Parse(a.Attributes["duration"].Value);
                            Rectangle source = new Rectangle((int)(tileid % columns), 
                                (int)Math.Floor(tileid / (float) columns), tileWidth, tileHeight);        
                            tileContent[id].Animation.Frames.Add(new Frame(source, duration / (double)1000, tileid));
                            animation = true;
                        }
                    }
                }
                if (animation) 
                    tileContent[id].Animation.Name = tileContent[id].Properties["Animation"] == null
                                                    ? "Unamed" : tileContent[id].Properties["Animation"];
            }

            // Create and return the TileContent
            return new TilesetContent()
            {
                Name = name,
                TileWidth = tileWidth,
                TileHeight = tileHeight,
                Spacing = spacing,
                Margin = margin,
                TileCount = tileCount,
                Columns = columns,
                ImageFilename = imageFilename,
                ImageColorKey = imageColorKey,
                Tiles = tileContent
            };
        }

    }

}
