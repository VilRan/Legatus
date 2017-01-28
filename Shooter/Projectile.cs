using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Legatus.Mathematics;

namespace Shooter
{
    public class Projectile : GameObject
    {
        private double Timer = 1.0;

        public Projectile()
        {
            Type = GameObjectType.Projectile;
            HitPolygon = new HitPolygon(
                new Vector2(-1, -1) * 16,
                new Vector2(1, -1) * 16,
                new Vector2(1, 1) * 16,
                new Vector2(-1, 1) * 16
                );
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            Timer -= gameTime.ElapsedGameTime.TotalSeconds;
            if (Timer < 0)
                Map.RemoveObject(this);
        }

        public override void Draw(SpriteBatch spriteBatch, GameDataManager data)
        {
            spriteBatch.Draw(data.ShotTexture, Position, null, null, data.ShotTexture.Bounds.Center.ToVector2(), Heading, Vector2.One, Color.White, SpriteEffects.None, 0f);
        }
    }
}
