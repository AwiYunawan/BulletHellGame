using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BulletHellGame
{
    public class Bullet
    {
        public Vector2 Position;
        private float Speed = 400f;
        private Texture2D _texture;

        public Bullet(Texture2D texture, Vector2 position)
        {
            _texture = texture;
            Position = position;
        }

        public void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Position.Y -= Speed * dt;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Scale bullet to 50%
            spriteBatch.Draw(_texture, Position, null, Color.White, 0f, Vector2.Zero, 0.3f, SpriteEffects.None, 0f);
        }

        public bool IsOffScreen(int screenHeight)
        {
            return Position.Y + _texture.Height * 0.5f < 0;
        }
    }
}
