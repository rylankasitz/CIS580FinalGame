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
using PlatformLibrary;
using Humper;

namespace Engine.Systems
{
    public static class MapManager
    {
        private static Dictionary<string, List<MapLocationObject>> ObjectLayers { get; set; }
        private static List<Entity> mapObjects = new List<Entity>();
        private static Tilemap tilemap;
        public static ContentManager Content;
        public static float Scale;

        public static void LoadMap (string name, Scene scene, float scale, 
            bool flipVertically = false, bool flipHorizontally = false)
        {
            tilemap = Content.Load<Tilemap>("Maps\\" + name);
            ObjectLayers = new Dictionary<string, List<MapLocationObject>>();
            Scale = scale;

            removeMapObjects();
            createMapObjects(scene, scale, flipVertically, flipHorizontally);
            setObjectLayers(scale);

            Debug.WriteLine($"Loaded Map: {name}\nFlip Vertically: {flipVertically}\nFlip Horizontally: {flipHorizontally}");
        }

        public static List<MapLocationObject> GetObjectLayer(string name)
        {
            if (ObjectLayers.ContainsKey(name))
            {
                return ObjectLayers["Spawns"];
            }

            Debug.WriteLine($"No object layer found for '{name}'");

            return new List<MapLocationObject>();
            
        }

        public static void CreateObjectFromTileSet(string name, string tileset, Vector position, Vector scale, float layer, out MapObject mapObject, out MapObjectCollision mapObjectCol)
        {
            mapObject = null;
            mapObjectCol = null;
            Tileset tilesetContent = Content.Load<Tileset>("Tilesets\\" + tileset);
            foreach (Tile tile in tilesetContent.tiles)
            {
                if (tile.Properties.ContainsKey("Name") && tile.Properties["Name"] == name)
                {
                    if (tile.BoxColliders.Count > 0)
                    {
                        mapObjectCol = SceneManager.GetCurrentScene().CreateEntity<MapObjectCollision>();
                        mapObjectCol.GetComponent<Transform>().Position = position;
                        mapObjectCol.GetComponent<Transform>().Scale = scale;
                        mapObjectCol.GetComponent<Sprite>().ContentName = tileset;
                        mapObjectCol.GetComponent<Sprite>().SpriteLocation = tile.Source;
                        mapObjectCol.GetComponent<Sprite>().Layer = layer;

                        BoxCol collider = tile.BoxColliders.First().Value[0];
                        BoxCollision col = mapObjectCol.GetComponent<BoxCollision>();

                        BoxData box = new BoxData();
                        box.Position = new Vector(collider.Rectangle.X / (float)tile.Width, collider.Rectangle.Y / (float)tile.Height);
                        box.Scale = new Vector(collider.Rectangle.Width / (float)tile.Width, collider.Rectangle.Height / (float)tile.Height);
                        box.TriggerOnly = collider.TriggerOnly;

                        IBox ibox = col.World.Create(position.X + box.Position.X * scale.X, 
                                                     position.Y + box.Position.Y * scale.Y, 
                                                     box.Scale.X * scale.X, 
                                                     box.Scale.Y * scale.Y);

                        ibox.Data = box;
                        col.Boxes.Add(ibox);
                    }
                    else
                    {
                        mapObject = SceneManager.GetCurrentScene().CreateEntity<MapObject>();
                        mapObject.GetComponent<Transform>().Position = position;
                        mapObject.GetComponent<Transform>().Scale = scale;
                        mapObject.GetComponent<Sprite>().ContentName = tileset;
                        mapObject.GetComponent<Sprite>().SpriteLocation = tile.Source;
                        mapObject.GetComponent<Sprite>().Layer = layer;
                    }
                }
            }
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
                            if (tile.BoxColliders.Count == 0)
                            {
                                MapObject mapObject = scene.CreateEntity<MapObject>();
                                setObjectPosition(mapObject, position, scale, tile, layernum, "MapTileSet"); // Change to not manual string
                                mapObjects.Add(mapObject);
                            }
                            else
                            {
                                KeyValuePair<string, List<BoxCol>> boxCollider = tile.BoxColliders.First();
                                MapObjectCollision mapObject = scene.CreateEntity<MapObjectCollision>();
                                setObjectPosition(mapObject, position, scale, tile, layernum, "MapTileSet"); // Change to not manual string
                                setCollision(mapObject, boxCollider.Value[0]);
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

        private static void setCollision(Entity obj, BoxCol collider)
        {
            BoxCollision col = obj.GetComponent<BoxCollision>();
            //col.Position = new Vector(collider.Rectangle.X / (float)tilemap.TileWidth, collider.Rectangle.Y / (float)tilemap.TileHeight);
            //col.Scale = new Vector(collider.Rectangle.Width / (float)tilemap.TileWidth, collider.Rectangle.Height / (float)tilemap.TileHeight);
            //col.TriggerOnly = collider.TriggerOnly;
        }

        private static void removeMapObjects()
        {
            foreach(Entity mapObject in mapObjects)
            {
                SceneManager.GetCurrentScene().RemoveEntity(mapObject);
            }
        }

        private static void setObjectLayers(float mapScale)
        {
            foreach(KeyValuePair <string, List<TileMapObject>> mapObjects in tilemap.ObjectLayers)
            {
                ObjectLayers.Add(mapObjects.Key, new List<MapLocationObject>());
                foreach (TileMapObject mapObject in mapObjects.Value)
                {
                    MapLocationObject mapLocationObject = new MapLocationObject();
                    mapLocationObject.Position = new Vector(mapObject.Position.X, mapObject.Position.Y) * mapScale;
                    ObjectLayers[mapObjects.Key].Add(mapLocationObject);
                }
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

    public class MapLocationObject
    {
        public Vector Position { get; set; }
        public Vector Scale { get; set; }
    }

    #endregion
}
