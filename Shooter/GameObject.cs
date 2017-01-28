using Legatus.Mathematics;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Shooter
{
    public abstract class GameObject
    {
        public GameMap Map;
        public GameObjectType Type;
        public int ID;
        public Vector2 Position;
        public Vector2 Velocity;
        public Vector2 Acceleration;
        public float Heading;
        public float AngularSpeed;

        protected HitPolygon HitPolygon;

        //public Matrix HeadingMatrix { get { return Matrix.CreateRotationZ(Heading); } }
        public Vector2 Front { get { return new Vector2((float)Math.Cos(Heading), (float)Math.Sin(Heading)); } }
        public Vector2 Left { get { return new Vector2(Front.Y, -Front.X); } }
        public Vector2 Right { get { return new Vector2(-Front.Y, Front.X); } }
        public Vector2 Back { get { return new Vector2(-Front.X, -Front.Y); } }
        public Vector2 HalfAcceleration { get { return Acceleration / 2; } }
        public float Speed { set { Velocity = Vector2.Normalize(Velocity) * value; } get { return Velocity.Length(); } }

        public GameObject()
        {
        }

        public virtual void Update(GameTime gameTime)
        {
            float time = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Vector2 delta = Velocity * time + HalfAcceleration * time * time;
            HitRay ray = new HitRay(Position, Position + delta);

            foreach (GameObject obj in Map.Objects.Values)
            {
                Vector2? intersection = obj.HitPolygon.FindIntersection(ray);
                if (intersection.HasValue)
                    OnCollision(obj);
            }

            Position += delta;
            Velocity += Acceleration * time;
            Heading += AngularSpeed * time;
            HitPolygon.MoveBy(delta);
        }

        protected virtual void OnCollision(GameObject other)
        {

        }

        public virtual void WriteTo(NetOutgoingMessage msg)
        {
            msg.Write(Position.X);
            msg.Write(Position.Y);
            msg.Write(Velocity.X);
            msg.Write(Velocity.Y);
            //msg.Write(Acceleration.X);
            //msg.Write(Acceleration.Y);
            msg.Write(Heading);
            //msg.Write(AngularSpeed);
        }

        public virtual void ReadFrom(NetIncomingMessage msg)
        {
            Position.X = msg.ReadFloat();
            Position.Y = msg.ReadFloat();
            Velocity.X = msg.ReadFloat();
            Velocity.Y = msg.ReadFloat();
            //Acceleration.X = msg.ReadFloat();
            //Acceleration.Y = msg.ReadFloat();
            Heading = msg.ReadFloat();
            //AngularSpeed = msg.ReadFloat();
        }

        /// <summary>
        /// Advances the message without actually taking any values from it.
        /// </summary>
        /// <param name="msg"></param>
        public virtual void FakeReadFrom(NetIncomingMessage msg)
        {
            msg.Position += 20 * 8;
        }

        public abstract void Draw(SpriteBatch spriteBatch, GameDataManager data);
    }

    public enum GameObjectType : byte
    {
        Player,
        Projectile
    }
}
