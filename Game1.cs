using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.IO;

namespace BulletHellGame
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Texture2D _playerTexture;
        private Texture2D _bulletTexture;

        private Vector2 _playerPosition;
        private float _playerSpeed = 300f;

        private List<Bullet> _bullets = new List<Bullet>();
        private double _lastShotTime = 0;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _playerPosition = new Vector2(400, 500); // Posisi awal player
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            using (var stream = new FileStream("Assets/player.png", FileMode.Open))
            {
                _playerTexture = Texture2D.FromStream(GraphicsDevice, stream);
            }

            using (var stream = new FileStream("Assets/bullet.png", FileMode.Open))
            {
                _bulletTexture = Texture2D.FromStream(GraphicsDevice, stream);
            }
        }

        protected override void Update(GameTime gameTime)
{
    KeyboardState keyboard = Keyboard.GetState();
    float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

    // Player movement
    if (keyboard.IsKeyDown(Keys.A) || keyboard.IsKeyDown(Keys.Left))
        _playerPosition.X -= _playerSpeed * dt;
    if (keyboard.IsKeyDown(Keys.D) || keyboard.IsKeyDown(Keys.Right))
        _playerPosition.X += _playerSpeed * dt;
    if (keyboard.IsKeyDown(Keys.W) || keyboard.IsKeyDown(Keys.Up))
        _playerPosition.Y -= _playerSpeed * dt;
    if (keyboard.IsKeyDown(Keys.S) || keyboard.IsKeyDown(Keys.Down))
        _playerPosition.Y += _playerSpeed * dt;

    // Auto-fire every 333ms (3x per detik)
    if (gameTime.TotalGameTime.TotalMilliseconds - _lastShotTime > 333)
    {
        Vector2 bulletStart = new Vector2(
            _playerPosition.X + (_playerTexture.Width * 0.5f * 0.5f) - (_bulletTexture.Width * 0.5f * 0.3f),
            _playerPosition.Y - (_bulletTexture.Height * 0.3f)
        );

        _bullets.Add(new Bullet(_bulletTexture, bulletStart));
        _lastShotTime = gameTime.TotalGameTime.TotalMilliseconds;
    }

    // Update bullets
    for (int i = _bullets.Count - 1; i >= 0; i--)
    {
        _bullets[i].Update(gameTime);
        if (_bullets[i].IsOffScreen(_graphics.PreferredBackBufferHeight))
        {
            _bullets.RemoveAt(i);
        }
    }

    base.Update(gameTime);
}


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black); // Bersihkan layar dulu

            _spriteBatch.Begin();

            // Gambar player (dengan scale 0.5 untuk perkecil ukuran)
            _spriteBatch.Draw(_playerTexture, _playerPosition, null, Color.White, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0f);

            // Gambar semua bullet
            foreach (var bullet in _bullets)
                bullet.Draw(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
