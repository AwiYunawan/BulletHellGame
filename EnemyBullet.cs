using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BulletHellGame
{
    public class EnemyBullet
    {
        public Vector2 Position;
        private float Speed = 200f;
        private Texture2D _texture;

        public EnemyBullet(Texture2D texture, Vector2 position)
        {
            _texture = texture;
            Position = position;
        }

        public void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Position.Y += Speed * dt;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position, null, Color.Red, 0f, Vector2.Zero, 0.05f, SpriteEffects.None, 0f);
        }

        public bool IsOffScreen(int screenHeight)
        {
            return Position.Y > screenHeight;
        }

        public Rectangle GetBounds(float scale = 0.05f)
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
