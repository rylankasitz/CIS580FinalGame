using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace PlatformLibrary
{
    /// <summary>
    /// A class representing a tilemap created with the Tiled editor
    /// </summary>
    public class Tilemap
    {
        #region private fields

        // An array of all tiles in the tilemap
        public Tile[] Tiles { get; set; }
        
        // An array of all tile layers in the tilemap
        public TilemapLayer[] Layers { get; set; }

        public Dictionary<string, List<TileMapObject>> ObjectLayers;

        // The width of the map, measured in tiles
        public uint MapWidth { get; set; }

        // The height of the map, measured in tiles
        public uint MapHeight { get; set; }

        // The width of the tiles in the map
        public uint TileWidth { get; set; }

        // The height of the tiles in the map
        public uint TileHeight { get; set; }

        #endregion

        #region initialization

        /// <summary>
        /// Constructs a new instance of a Tilemap
        /// </summary>
        /// <param name="mapWidth">The width of the map, in tiles</param>
        /// <param name="mapHeight">The height of the map, in tiles</param>
        /// <param name="tileWidth">The width of the tiles, in pixels</param>
        /// <param name="tileHeight">The heigh of the tiles, in pixels</param>
        /// <param name="layers">The layers of the map</param>
        /// <param name="tiles">The tiles of the map</param>
        public Tilemap(uint mapWidth, uint mapHeight, uint tileWidth, uint tileHeight, TilemapLayer[] layers, Tile[] tiles,
            Dictionary<string, List<TileMapObject>> objectlayers)
        {
            this.MapWidth = mapWidth;
            this.MapHeight = mapHeight;
            this.TileWidth = tileWidth;
            this.TileHeight = tileHeight;
            this.Layers = layers;
            this.Tiles = tiles;

            ObjectLayers = objectlayers;
        }

        #endregion    
    }

    public class TileMapObject
    {
        public Vector2 Position { get; set; }
        public Vector2 Scale { get; set; }

        public TileMapObject(Vector2 position, Vector2 scale) 
        {
            Position = position;
            Scale = scale;
        }
    }
}
