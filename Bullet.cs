using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BulletHellGame
{
    public class Bullet
    {
        public Vector2 Position;
        public float Speed = 500f;
        private Texture2D _texture;

        public Bullet(Texture2D texture, Vector2 startPos)
        {
            _texture = texture;
            Position = startPos;
        }

        public void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Position.Y -= Speed * dt;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position, Color.White);
        }

        public bool IsOffScreen(int screenHeight)
        {
            return Position.Y + _texture.Height < 0;
        }
    }
}
