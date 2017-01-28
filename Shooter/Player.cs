using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Legatus.Mathematics;

namespace Shooter
{
    public class Player : GameObject
    {
        public bool IsMovingFront, IsMovingLeft, IsMovingRight, IsMovingBack;
        public double ReloadTimer = 0.0;
        private float MaxSpeed = 500f;
        private Color Color;
        public float HP;

        public bool CanShoot { get { return ReloadTimer <= 0; } }

        public Player()
            : base ()
        {
            Type = GameObjectType.Player;
            HitPolygon = new HitPolygon(
                new Vector2(-1, -1) * 16,
                new Vector2(1, -1) * 16,
                new Vector2(1, 1) * 16,
                new Vector2(-1, 1) * 16
                );
            Random rng = new Random();
            Color = new Color((float)rng.NextDouble(), (float)rng.NextDouble(), (float)rng.NextDouble());
            HP = 100f;
        }

        public override void Update(GameTime gameTime)
        {
            Velocity = Vector2.Zero;

            if (IsMovingFront)
                Velocity += MaxSpeed * new Vector2(0f, -1f);
            if (IsMovingLeft)
                Velocity += MaxSpeed * new Vector2(-1f, 0f);
            if (IsMovingRight)
                Velocity += MaxSpeed * new Vector2(1f, 0f);
            if (IsMovingBack)
                Velocity += MaxSpeed * new Vector2(0f, 1f);

            if (Speed > MaxSpeed)
                Speed = MaxSpeed;

            if (ReloadTimer > 0)
            {
                ReloadTimer -= gameTime.ElapsedGameTime.TotalSeconds;
                if (ReloadTimer < 0)
                    ReloadTimer = 0;
            }

            base.Update(gameTime);
        }

        protected override void OnCollision(GameObject other)
        {
            base.OnCollision(other);

            if (other.Type == GameObjectType.Projectile)
            {
                HP -= 1f;
                Color = new Color(HP / 100f, HP / 100f, HP / 100f);
            }
        }

        public override void Draw(SpriteBatch spriteBatch, GameDataManager data)
        {
            spriteBatch.Draw(data.PlayerTexture, Position, null, null, data.PlayerTexture.Bounds.Center.ToVector2(), Heading, Vector2.One, Color, SpriteEffects.None, 0f);
        }

        public override void WriteTo(NetOutgoingMessage msg)
        {
            base.WriteTo(msg);
            msg.Write(HP);
        }

        public override void ReadFrom(NetIncomingMessage msg)
        {
            base.ReadFrom(msg);
            HP = msg.ReadFloat();
        }

        public override void FakeReadFrom(NetIncomingMessage msg)
        {
            base.FakeReadFrom(msg);
            msg.Position += 4 * 8;
        }
    }
}
