using Microsoft.Xna.Framework;
using Engine.ECSCore;
using System.Collections.Generic;
using Humper.Responses;
using Humper;
using System.Runtime.Remoting.Messaging;
using System;

namespace Engine.Componets
{
    public delegate void HandleCollision(Entity collider, string side);
    public delegate void HandleCollisionEnter(Entity collider, string side);

    public enum CollisionLayers
    {
        Player = 1 << 0,
        Enemy = 1 << 1,
        All = 1 << 2,
    }

    public class Vector
    {
        public Vector(float x, float y) { X = x; Y = y; }
        public float X { get; set; } = 0;
        public float Y { get; set; } = 0;

        public static implicit operator Vector2(Vector v) => new Vector2(v.X, v.Y);

        public static Vector operator +(Vector a, Vector b) => new Vector(a.X + b.X, a.Y + b.Y);
        public static Vector operator -(Vector a, Vector b) => new Vector(a.X - b.X, a.Y - b.Y);
        public static Vector operator *(Vector a, Vector b) => new Vector(a.X * b.X, a.Y * b.Y);
        public static Vector operator /(Vector a, Vector b) => new Vector(a.X / b.X, a.Y / b.Y);

        public static Vector operator *(Vector v, float n) => new Vector(v.X * n, v.Y * n);
        public static Vector operator /(Vector v, float n) => new Vector(v.X/n, v.Y/n);

        public Vector Floor()
        {
            return new Vector((int) Math.Floor(X), (int) Math.Floor(Y));
        }
    }

    public class Transform : Component
    {
        public Vector Position { get; set; }
        public Vector Scale { get; set; }
        public float Rotation { get; set; }
        public bool IsHull { get; set; }
        public Transform() { }
        public Transform(int X, int Y, int Width, int Height)
        {
            Position = new Vector(X, Y);
            Scale = new Vector(Width, Height);
        }
    }

    public class Physics : Component
    {
        public Vector Velocity { get; set; }
        public Physics() { }
        public Physics(float VelocityX, float VelocityY)
        {
            Velocity = new Vector(VelocityX, VelocityY);
        }
    }

    public class BoxCollision : Component
    {
        public World World { get; set; }
        public List<IBox> Boxes { get; set; }
        public CollisionLayers Layer { set
            {
                foreach (IBox box in Boxes)
                {
                    BoxData data = (BoxData) box.Data;
                    data.Layer = value;
                }
            } 
        }
        public HandleCollision HandleCollision { get; set; }
        public HandleCollisionEnter HandleCollisionEnter { get; set; }
        public BoxCollision() 
        { 
            Boxes = new List<IBox>();  
        }
        public BoxCollision(int X, int Y, float Width, float Height, bool TriggerOnly = false)
        {
            Boxes = new List<IBox>();
        }
    }

    public class BoxData
    {
        public Vector Position { get; set; }
        public Vector Scale { get; set; }
        public bool TriggerOnly { get; set; }
        public CollisionLayers Layer { get; set; }
        public string Name { get; set; }
        public BoxData() 
        {
            Position = new Vector(0, 0);
            Scale = new Vector(0, 0);
            TriggerOnly = false;
            Layer = CollisionLayers.All;
        }

        public BoxData(Vector position, Vector scale, bool trigger, CollisionLayers layer, string name = "Unamed")
        {
            Position = position;
            Scale = scale;
            TriggerOnly = trigger;
            Layer = layer;
            Name = name;
        }
    }
}
