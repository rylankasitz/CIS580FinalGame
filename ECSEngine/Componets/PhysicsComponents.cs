using Microsoft.Xna.Framework;
using Engine.ECSCore;
using System.Collections.Generic;

namespace Engine.Componets
{
    public delegate void HandleCollision(Entity collider, string side);
    public delegate void HandleCollisionEnter(Entity collider, string side);

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
    }

    public class Particle
    {
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public Vector2 Acceleration { get; set; }
        public float Scale { get; set; }
        public float Life { get; set; }
        public Color Color { get; set; }
    }

    public class Transform : Component
    {
        public Vector Position { get; set; }
        public Vector Scale { get; set; }
        public float Rotation { get; set; }
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
        public Vector Position { get; set; }
        public Vector Scale { get; set; }
        public HandleCollision HandleCollision { get; set; }
        public HandleCollisionEnter HandleCollisionEnter { get; set; }
        public bool TriggerOnly { get; set; }
        public string Layer { get; set; } = "All";
        public List<Entity> CollidingObjects = new List<Entity>();
        public BoxCollision() { }
        public BoxCollision(int X, int Y, float Width, float Height, bool TriggerOnly = false)
        {
            Position = new Vector(X, Y);
            Scale = new Vector(Width, Height);
            this.TriggerOnly = TriggerOnly;
        }
    }

    public class ParticleSystem : Component
    {
        public Particle[] Particles { get; set; }
        public Vector Emitter { get; set; }
        public Vector Direction { get; set; }
        public string Texture { get; set; }
        public int SpawnPerFrame { get; set; }
        public int ParticleCount { get; set; }
        public float Time { get; set; }
        public float Life { get; set; }
        public float Scale { get; set; }
        public float Velocity { get; set; }
        public float Acceleration { get; set; }
        public float Range { get; set; }
        public Color Color { get; set; }
        public int NextIndex { get; set; } = 0;
        public bool Running { get; set; } = true;
        public float ElapsedTime { get; set; }

        public ParticleSystem() { }
        public ParticleSystem(string Texture, int SpawnPerFrame, int ParticleCount, float Time,
            float EmmiterX, float EmmiterY, float DirectionX, float DirectionY, float Range,
            float Life = 3f, float Scale = 1f, float Velocity = 100f, float Acceleration = .1f)
        {
            this.Texture = Texture;
            this.SpawnPerFrame = SpawnPerFrame;
            this.ParticleCount = ParticleCount;
            this.Time = Time;
            Emitter = new Vector(EmmiterX, EmmiterY);
            Direction = new Vector(DirectionX - .5f, DirectionY - .5f);
            this.Range = Range/90;
            this.Life = Life;
            this.Scale = Scale;
            this.Velocity = Velocity;
            this.Acceleration = Acceleration;
            this.ElapsedTime = Time;
        }


        public void Play()
        {
            Running = true;
            ElapsedTime = 0;
        }
        public void Stop()
        {
            Running = false;
        }
    }
}
