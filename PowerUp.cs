using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BulletHellGame
{
    enum PowerUpType
    {
        ExtraLife,
        DoubleBullet,
        Shield
    }

    class PowerUp
    {
        public Vector2 Position;
        public Texture2D Texture;
        public PowerUpType Type;
        public Rectangle Bounds => new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
        public bool IsCollected = false;
        public float Speed = 100f;

        public PowerUp(Texture2D texture, Vector2 pos, PowerUpType type)
        {
            Texture = texture;
            Position = pos;
            Type = type;
        }

        public void Update(GameTime gameTime)
        {
            Position.Y += Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, Color.White);
        }
    }
} 