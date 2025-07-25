using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.IO;
using System;


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
        private Texture2D _enemyTexture;
        private List<Enemy> _enemies = new List<Enemy>();
        private Random _random = new Random();
        private double _lastSpawnTime = 0;
        private int _score = 0;
        private SpriteFont _font;

        private Texture2D _enemyBulletTexture;
        private List<EnemyBullet> _enemyBullets = new List<EnemyBullet>();
        private double _lastEnemyShotTime = 0;
        private int _playerLives = 3;
        private bool _isGameOver = false;




        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _playerPosition = new Vector2(400, 500); 
            base.Initialize();
        }

        protected override void LoadContent()
        {
            Content.RootDirectory = "Content/bin/DesktopGL/Content";
            _font = Content.Load<SpriteFont>("score");
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            using (var stream = new FileStream("Assets/player.png", FileMode.Open))
            {
                _playerTexture = Texture2D.FromStream(GraphicsDevice, stream);
            }

            using (var stream = new FileStream("Assets/bullet.png", FileMode.Open))
            {
                _bulletTexture = Texture2D.FromStream(GraphicsDevice, stream);
            }
            using (var stream = new FileStream("Assets/enemy.png", FileMode.Open))
            {
                _enemyTexture = Texture2D.FromStream(GraphicsDevice, stream);
            }
            using (var stream = new FileStream("Assets/bullet.png", FileMode.Open))
            {
                _enemyBulletTexture = Texture2D.FromStream(GraphicsDevice, stream);
            }

        }

        protected override void Update(GameTime gameTime)
        {
            if (_isGameOver)
                return;

            KeyboardState keyboard = Keyboard.GetState();
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;


            if (keyboard.IsKeyDown(Keys.A) || keyboard.IsKeyDown(Keys.Left))
                _playerPosition.X -= _playerSpeed * dt;
            if (keyboard.IsKeyDown(Keys.D) || keyboard.IsKeyDown(Keys.Right))
                _playerPosition.X += _playerSpeed * dt;
            if (keyboard.IsKeyDown(Keys.W) || keyboard.IsKeyDown(Keys.Up))
                _playerPosition.Y -= _playerSpeed * dt;
            if (keyboard.IsKeyDown(Keys.S) || keyboard.IsKeyDown(Keys.Down))
                _playerPosition.Y += _playerSpeed * dt;
            if (_isGameOver && Keyboard.GetState().IsKeyDown(Keys.R))
            {
                _playerLives = 3;
                _isGameOver = false;
                _bullets.Clear();
                _enemyBullets.Clear();
                _enemies.Clear();
                _score = 0;
                Console.WriteLine("Game Restarted!");
            }



            if (gameTime.TotalGameTime.TotalMilliseconds - _lastShotTime > 200)
            {
                float playerScale = 0.15f;
                float bulletScale = 0.01f;

                Vector2 playerCenterTop = new Vector2(
                    _playerPosition.X + (_playerTexture.Width * playerScale * 0.5f),
                    _playerPosition.Y
                );

                float bulletOffsetY = _bulletTexture.Height * bulletScale;

                Vector2 bulletStart = new Vector2(
                    playerCenterTop.X - (_bulletTexture.Width * bulletScale * 0.5f),
                    playerCenterTop.Y - bulletOffsetY
                );

                _bullets.Add(new Bullet(_bulletTexture, bulletStart));

                _lastShotTime = gameTime.TotalGameTime.TotalMilliseconds;
            }

            for (int i = _bullets.Count - 1; i >= 0; i--)
            {
                _bullets[i].Update(gameTime);
                if (_bullets[i].IsOffScreen(_graphics.PreferredBackBufferHeight))
                {
                    _bullets.RemoveAt(i);
                }
            }
            for (int i = _bullets.Count - 1; i >= 0; i--)
            {
                var bulletBounds = _bullets[i].GetBounds();

                for (int j = _enemies.Count - 1; j >= 0; j--)
                {
                    var enemyBounds = _enemies[j].GetBounds();

                    if (bulletBounds.Intersects(enemyBounds))
                    {
                        _bullets.RemoveAt(i);
                        _enemies.RemoveAt(j);
                        _score += 10;
                        break; // break inner loop
                    }
                }
            }
            base.Update(gameTime);
            if (gameTime.TotalGameTime.TotalMilliseconds - _lastSpawnTime > 1000)
            {
                float x = _random.Next(0, _graphics.PreferredBackBufferWidth - (int)(_enemyTexture.Width * 0.5f));
                Vector2 enemyStart = new Vector2(x, -_enemyTexture.Height);
                _enemies.Add(new Enemy(_enemyTexture, enemyStart));
                _lastSpawnTime = gameTime.TotalGameTime.TotalMilliseconds;
            }

            // Musuh menembak setiap 1 detik
            if (gameTime.TotalGameTime.TotalMilliseconds - _lastEnemyShotTime > 1000)
            {
                foreach (var enemy in _enemies)
                {
                    Vector2 bulletPos = new Vector2(
                        enemy.Position.X + (_enemyTexture.Width * 0.25f), // tengah enemy (karena scale 0.5)
                        enemy.Position.Y + (_enemyTexture.Height * 0.5f)
                    );

                    _enemyBullets.Add(new EnemyBullet(_enemyBulletTexture, bulletPos));
                }

                _lastEnemyShotTime = gameTime.TotalGameTime.TotalMilliseconds;
            }
            // Update enemy bullets
            for (int i = _enemyBullets.Count - 1; i >= 0; i--)
            {
                _enemyBullets[i].Update(gameTime);
                if (_enemyBullets[i].IsOffScreen(_graphics.PreferredBackBufferHeight))
                {
                    _enemyBullets.RemoveAt(i);
                }
            }
            for (int i = _enemies.Count - 1; i >= 0; i--)
            {
                _enemies[i].Update(gameTime);
                if (_enemies[i].IsOffScreen(_graphics.PreferredBackBufferHeight))
                {
                    _enemies.RemoveAt(i);
                }
            }
            if (!_isGameOver)
            {
                Rectangle playerBounds = new Rectangle(
                    (int)_playerPosition.X,
                    (int)_playerPosition.Y,
                    (int)(_playerTexture.Width * 0.15f),
                    (int)(_playerTexture.Height * 0.15f)
                );

                for (int i = _enemyBullets.Count - 1; i >= 0; i--)
                {
                    var bulletBounds = _enemyBullets[i].GetBounds();

                    if (bulletBounds.Intersects(playerBounds))
                    {
                        _enemyBullets.RemoveAt(i);
                        _playerLives--;

                        Console.WriteLine($"Player hit! Lives remaining: {_playerLives}");

                        if (_playerLives <= 0)
                        {
                            _isGameOver = true;
                            Console.WriteLine("GAME OVER!");
                        }
                    }
                }
            }


            



        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();

            
            _spriteBatch.Draw(_playerTexture, _playerPosition, null, Color.White, 0f, Vector2.Zero, 0.15f, SpriteEffects.None, 0f);

            _spriteBatch.DrawString(_font, $"Score: {_score}", new Vector2(10, 10), Color.White);

            foreach (var bullet in _bullets)
                bullet.Draw(_spriteBatch);

            
            foreach (var enemy in _enemies)
                enemy.Draw(_spriteBatch);

            // Gambar semua peluru musuh
            foreach (var ebullet in _enemyBullets)
                ebullet.Draw(_spriteBatch);

            // Tampilkan nyawa
            string livesText = $"Lives: {_playerLives}";
            _spriteBatch.DrawString(_font, livesText, new Vector2(10, 40), Color.Red);
            if (_isGameOver)
            {
                string gameOverText = "GAME OVER";
                Vector2 textSize = _font.MeasureString(gameOverText);
                Vector2 position = new Vector2(
                    (_graphics.PreferredBackBufferWidth - textSize.X) / 2,
                    (_graphics.PreferredBackBufferHeight - textSize.Y) / 2
                );

                _spriteBatch.DrawString(_font, gameOverText, position, Color.Yellow);
            }


            _spriteBatch.End(); 

            

            base.Draw(gameTime);
        }
    }
}
