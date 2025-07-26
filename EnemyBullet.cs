using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BulletHellGame
{
    public class EnemyBullet
    {
        public Vector2 Position;
        public Vector2 Velocity;
        private Texture2D _texture;
        private float _speed = 200f;

        public EnemyBullet(Texture2D texture, Vector2 position, Vector2 direction)
        {
            _texture = texture;
            Position = position;
            Velocity = Vector2.Normalize(direction) * _speed;
        }

        public void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Position += Velocity * dt;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position, null, Color.Red, 0f, Vector2.Zero, 0.1f, SpriteEffects.None, 0f);
        }

        public bool IsOffScreen(int screenWidth, int screenHeight)
        {
            return Position.Y > screenHeight || Position.Y < 0 || Position.X < 0 || Position.X > screenWidth;
        }
        public Rectangle GetBounds(float scale = 0.1f)
        {
            return new Rectangle(
                (int)Position.X,
                (int)Position.Y,
                (int)(_texture.Width * scale),
                (int)(_texture.Height * scale)
            );
        }
    }
}
