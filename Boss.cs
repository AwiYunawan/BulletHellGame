using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace BulletHellGame
{
    public class Boss
    {
        public Vector2 Position;
        public int HP = 100;
        private Texture2D _texture;
        private float _shootTimer = 0f;
        private float _shootInterval = 1f; // Shoot every 1 second
        private Random _random = new Random();

        public bool IsDead => HP <= 0;

        public Boss(Texture2D texture, Vector2 startPosition)
        {
            _texture = texture;
            Position = startPosition;
        }

        public void Update(GameTime gameTime, Action<Vector2> shootAction)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Basic horizontal movement (zigzag)
            Position.X += (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds * 2) * 50 * dt;

            // Shoot bullets
            _shootTimer += dt;
            if (_shootTimer >= _shootInterval)
            {
                _shootTimer = 0;
                Vector2 bulletPos = new Vector2(Position.X + _texture.Width / 2, Position.Y + _texture.Height);
                shootAction?.Invoke(bulletPos);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position, Color.White);
        }

        public void TakeDamage()
        {
            HP -= 1;
        }

        public Rectangle GetBounds(float scale = 0.3f)
        {
            return new Rectangle(
                (int)Position.X,
                (int)Position.Y,
                (int)(_texture.Width * scale),
                (int)(_texture.Height * scale)
            );
        }

        public void DrawHPBar(SpriteBatch spriteBatch, Texture2D barTexture, SpriteFont font, int screenWidth)
        {
            float barWidth = screenWidth * 0.6f;
            float barHeight = 20f;
            float hpPercent = HP / 100f;

            Vector2 barPosition = new Vector2((screenWidth - barWidth) / 2, 10);

            // Background bar
            spriteBatch.Draw(barTexture, new Rectangle((int)barPosition.X, (int)barPosition.Y, (int)barWidth, (int)barHeight), Color.DarkRed);
            // HP bar
            spriteBatch.Draw(barTexture, new Rectangle((int)barPosition.X, (int)barPosition.Y, (int)(barWidth * hpPercent), (int)barHeight), Color.Red);
            // Text
            spriteBatch.DrawString(font, $"Boss HP: {HP}", new Vector2(barPosition.X, barPosition.Y + 22), Color.White);
        }
    }
} 