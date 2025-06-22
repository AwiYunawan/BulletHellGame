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

        public Rectangle GetBounds(float scale = 0.1f)
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
            Position.Y -= Speed * dt;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Scale bullet to 50%
float bulletScale = 0.01f;

spriteBatch.Draw(
    _texture,
    Position,
    null,
    Color.White,
    0f,
    Vector2.Zero,
    bulletScale,
    SpriteEffects.None,
    0f
);
        }

        public bool IsOffScreen(int screenHeight)
        {
            return Position.Y + _texture.Height * 0.5f < 0;
        }
    }
}
