using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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



        public Enemy(Texture2D texture, Vector2 position)
        {
            _texture = texture;
            Position = position;
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
    }
}
