using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace BulletHellGame
{
    public class Enemy
    {
        public Vector2 Position;
        private float Speed = 100f;
        private Texture2D _texture;
        public enum EnemyFireType
        {
            Straight,
            Aimed,
            Spread,
            Burst,
            Circular
        }
        public EnemyFireType FireType;

        public Enemy(Texture2D texture, Vector2 position, EnemyFireType fireType)
        {
            _texture = texture;
            Position = position;
            this.FireType = fireType;
        }

        public Rectangle GetBounds(float scale = 0.2f)
        {
            return new Rectangle(
                (int)Position.X,
                (int)Position.Y,
                (int)(_texture.Width * scale),
                (int)(_texture.Height * scale)
            );
        }

        public void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Position.Y += Speed * dt;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position, null, Color.White, 0f, Vector2.Zero, 0.2f, SpriteEffects.None, 0f);
        }

        public bool IsOffScreen(int screenHeight)
        {
            return Position.Y > screenHeight;
        }

        public List<EnemyBullet> Fire(Vector2 playerPosition, Texture2D bulletTexture)
        {
            List<EnemyBullet> bullets = new();

            switch (FireType)
            {
                case EnemyFireType.Straight:
                    bullets.Add(new EnemyBullet(bulletTexture, Position, Vector2.UnitY));
                    break;

                case EnemyFireType.Aimed:
                    Vector2 direction = playerPosition - Position;
                    bullets.Add(new EnemyBullet(bulletTexture, Position, direction));
                    break;

                case EnemyFireType.Spread:
                    float[] angles = { -0.3f, 0f, 0.3f };
                    foreach (float angle in angles)
                    {
                        Vector2 spreadDir = new Vector2((float)Math.Sin(angle), 1f);
                        bullets.Add(new EnemyBullet(bulletTexture, Position, spreadDir));
                    }
                    break;

                case EnemyFireType.Burst:
                    for (int i = -1; i <= 1; i++)
                    {
                        bullets.Add(new EnemyBullet(bulletTexture, Position, new Vector2(i * 0.3f, 1f)));
                    }
                    break;

                case EnemyFireType.Circular:
                    int bulletCount = 8;
                    for (int i = 0; i < bulletCount; i++)
                    {
                        float angle = MathHelper.TwoPi * i / bulletCount;
                        Vector2 dir = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
                        bullets.Add(new EnemyBullet(bulletTexture, Position, dir));
                    }
                    break;
            }

            return bullets;
        }
    }
}
