using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Engine.Componets;
using Engine.ECSCore;
using TiledSharp;
using PlatformLibrary;

namespace Engine.Systems
{
    public static class MapManager
    {
        private static Dictionary<string, List<TileMapObject>> ObjectLayers { get; set; }
        private static List<Entity> mapObjects = new List<Entity>();
        private static Tilemap tilemap;
        public static ContentManager Content;

        public static void LoadMap (string name, Scene scene, float scale, 
            bool flipVertically = false, bool flipHorizontally = false)
        {
            tilemap = Content.Load<Tilemap>("Maps\\" + name);
            ObjectLayers = tilemap.ObjectLayers;

            removeMapObjects();
            createMapObjects(scene, scale, flipVertically, flipHorizontally);
            Debug.WriteLine($"Loaded Map: {name}\nFlip Vertically: {flipVertically}\nFlip Horizontally: {flipHorizontally}");
        }

        public static List<TileMapObject> GetObjectLayer(string name)
        {
            if (ObjectLayers.ContainsKey(name))
            {
                return ObjectLayers["Spawns"];
            }

            Debug.WriteLine($"No object layer found for '{name}'");

            return new List<TileMapObject>();
            
        }

        #region Private Methods

        private static void createMapObjects(Scene scene, float mapScale, bool flipVertically, bool flipHorizontally)
        {
            float layernum = 1;
            float mapWidth = tilemap.MapWidth * tilemap.TileWidth * mapScale;
            float mapHeight = tilemap.MapHeight * tilemap.TileHeight * mapScale;
            foreach (var layer in tilemap.Layers)
            {
                for (uint y = 0; y < tilemap.MapHeight; y++)
                {
                    for (uint x = 0; x < tilemap.MapWidth; x++)
                    {
                        uint dataIndex = y * tilemap.MapWidth + x;
                        uint tileIndex = layer.Data[dataIndex];
                        if (tileIndex != 0 && tileIndex < tilemap.Tiles.Length)
                        {
                            Vector position = new Vector(x * (tilemap.TileWidth), y * (tilemap.TileHeight)) * mapScale;
                            Vector scale = new Vector(tilemap.Tiles[tileIndex].Width, tilemap.Tiles[tileIndex].Height) * mapScale;

                            if (flipVertically)
                            {
                                position.X += ((mapWidth / 2) - position.X)*2;
                            }

                            if (flipHorizontally)
                            {
                                position.Y += ((mapWidth / 2) - position.Y)*2;
                            }

                            Tile tile = tilemap.Tiles[tileIndex];
                            if (tile.BoxCollider == Rectangle.Empty)
                            {
                                MapObject mapObject = scene.CreateEntity<MapObject>();
                                setObjectPosition(mapObject, position, scale, tile, layernum, "MapTileSet"); // Change to not manual string
                                mapObjects.Add(mapObject);
                            }
                            else
                            {
                                MapObjectCollision mapObject = scene.CreateEntity<MapObjectCollision>();
                                setObjectPosition(mapObject, position, scale, tile, layernum, "MapTileSet"); // Change to not manual string
                                setCollision(mapObject, tile.BoxCollider, tile, mapScale);
                                mapObjects.Add(mapObject);
                            }
                        }
                    }
                }
                layernum -= 1f / (float)(tilemap.Layers.Length);
            }
        }

        private static void setObjectPosition(Entity obj, Vector position, Vector scale, Tile tile, float layernum, string contentName)
        {
            Transform transform = obj.GetComponent<Transform>();
            Sprite sprite = obj.GetComponent<Sprite>();

            obj.Name = tile.Properties.ContainsKey("Name") ? tile.Properties["Name"] : "Unnamed";

            transform.Position = position;
            transform.Scale = scale;

            sprite.ContentName = contentName;
            sprite.SpriteLocation = tile.Source;
            sprite.Layer = layernum;
        }

        private static void setCollision(Entity obj, Rectangle collider, Tile tile, float mapScale)
        {
            BoxCollision col = obj.GetComponent<BoxCollision>();
            col.Position = new Vector(collider.X * mapScale, collider.Y * mapScale);
            col.Scale = new Vector(collider.Width / (float)tilemap.TileWidth, collider.Height / (float)tilemap.TileHeight);
            col.TriggerOnly = tile.Properties.ContainsKey("Trigger") ? (tile.Properties["Trigger"] == "true") : false;
        }

        private static void removeMapObjects()
        {
            foreach(Entity mapObject in mapObjects)
            {
                SceneManager.GetCurrentScene().RemoveEntity(mapObject);
            }
        }

        #endregion
    }

    #region Map Objects

    [Transform(X: 100, Y: 100, Width: 100, Height: 100)]
    [Sprite(ContentName: "spritesheet")]
    public class MapObject : Entity
    {
        public override void Initialize()
        {

        }

        public override void Update(GameTime gameTime)
        {
            
        }
    }

    [Transform(X: 100, Y: 100, Width: 100, Height: 100)]
    [Sprite(ContentName: "spritesheet")]
    [BoxCollision()]
    public class MapObjectCollision : Entity
    {
        public override void Initialize()
        {

        }

        public override void Update(GameTime gameTime)
        {

        }
    }

    #endregion
}
