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
        // Game State enum
        enum GameState
        {
            Playing,
            GameOver
        }

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
        public List<Enemy> Enemies => _enemies;
        private Random _random = new Random();
        private double _lastSpawnTime = 0;
        private int _score = 0;
        private SpriteFont _font;

        private Texture2D _enemyBulletTexture;
        private List<EnemyBullet> _enemyBullets = new List<EnemyBullet>();
        private double _lastEnemyShotTime = 0;
        private int _playerLives = 3;
        
        // Game State
        private GameState _gameState = GameState.Playing;
        
        // Boss properties
        private Texture2D _bossTexture;
        private Boss _boss;
        public Boss Boss => _boss;
        private Texture2D _hpBarTexture;
        private List<EnemyBullet> _bossBullets = new List<EnemyBullet>();
        private bool _bossSpawned = false;
        
        // Wave Manager
        private WaveManager _waveManager;
        
        // Wave delay system
        private bool _waveCleared = false;
        private float _waveTimer = 0f;
        private float _waveDelay = 3f; // 3 seconds delay between waves
        
        // Boss visual effects
        private bool _bossAppearing = false;
        private float _bossFlashTimer = 0f;
        private float _bossFlashDuration = 1.5f;
        private Texture2D _pixelTexture;




        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _playerPosition = new Vector2(400, 500); 
            _waveManager = new WaveManager(this);
            base.Initialize();
        }

        private Enemy.EnemyFireType GetRandomFireType()
        {
            var values = Enum.GetValues(typeof(Enemy.EnemyFireType));
            return (Enemy.EnemyFireType)values.GetValue(_random.Next(values.Length));
        }
        
        public void SpawnEnemy(int wave)
        {
            var rand = new Random();
            var position = new Vector2(rand.Next(0, _graphics.PreferredBackBufferWidth - 40), -40);
            _enemies.Add(new Enemy(_enemyTexture, position, GetRandomFireType()));
        }

        public void SpawnBoss()
        {
            var bossPos = new Vector2(_graphics.PreferredBackBufferWidth / 2 - _bossTexture.Width / 2, -_bossTexture.Height);
            _boss = new Boss(_bossTexture, bossPos);
            _bossSpawned = true;
            
            // Trigger boss visual effect
            _bossAppearing = true;
            _bossFlashTimer = 0f;
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
            
            // Load boss texture (using enemy texture for now)
            using (var stream = new FileStream("Assets/enemy.png", FileMode.Open))
            {
                _bossTexture = Texture2D.FromStream(GraphicsDevice, stream);
            }
            
            // Create HP bar texture
            _hpBarTexture = new Texture2D(GraphicsDevice, 1, 1);
            _hpBarTexture.SetData(new[] { Color.White });
            
            // Create pixel texture for boss visual effects
            _pixelTexture = new Texture2D(GraphicsDevice, 1, 1);
            _pixelTexture.SetData(new[] { Color.White });

        }

        protected override void Update(GameTime gameTime)
        {
            if (_gameState == GameState.GameOver)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.R))
                {
                    RestartGame();
                }
                return;
            }

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
            
            // Update Wave Manager
            _waveManager.Update(gameTime);
            
            // Wave delay system
            if (_enemies.Count == 0 && _boss == null && !_waveCleared && _waveManager.IsWaveInProgress)
            {
                _waveCleared = true;
                _waveTimer = 0f;
            }
            
            if (_waveCleared)
            {
                _waveTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (_waveTimer >= _waveDelay)
                {
                    _waveManager.StartNextWave();
                    _waveCleared = false;
                }
            }
            
            // Boss visual effect update
            if (_bossAppearing)
            {
                _bossFlashTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (_bossFlashTimer >= _bossFlashDuration)
                {
                    _bossAppearing = false;
                }
            }

            // Update enemies and handle firing
            foreach (var enemy in _enemies)
            {
                enemy.Update(gameTime);

                // Handle enemy firing
                if (gameTime.TotalGameTime.TotalSeconds - _lastEnemyShotTime > 1.5)
                {
                    var bullets = enemy.Fire(_playerPosition, _enemyBulletTexture);
                    _enemyBullets.AddRange(bullets);
                }
            }
            
            // Reset fire timer
            if (gameTime.TotalGameTime.TotalSeconds - _lastEnemyShotTime > 1.5)
            {
                _lastEnemyShotTime = gameTime.TotalGameTime.TotalSeconds;
            }
            
            // Update boss
            if (_boss != null && !_boss.IsDead)
            {
                _boss.Update(gameTime, pos => _bossBullets.Add(new EnemyBullet(_enemyBulletTexture, pos, Vector2.UnitY)));
                
                // Check bullet collision with boss
                for (int i = _bullets.Count - 1; i >= 0; i--)
                {
                    var bulletBounds = _bullets[i].GetBounds();
                    if (bulletBounds.Intersects(_boss.GetBounds()))
                    {
                        _boss.TakeDamage();
                        _bullets.RemoveAt(i);
                        _score += 5;
                        
                        if (_boss.IsDead)
                        {
                            _score += 50; // Bonus for defeating boss
                        }
                    }
                }
            }
            
            // Update enemy bullets
            for (int i = _enemyBullets.Count - 1; i >= 0; i--)
            {
                _enemyBullets[i].Update(gameTime);
                if (_enemyBullets[i].IsOffScreen(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight))
                {
                    _enemyBullets.RemoveAt(i);
                }
            }
            
            // Update boss bullets
            for (int i = _bossBullets.Count - 1; i >= 0; i--)
            {
                _bossBullets[i].Update(gameTime);
                if (_bossBullets[i].IsOffScreen(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight))
                {
                    _bossBullets.RemoveAt(i);
                }
            }
            // Remove enemies that are off screen
            for (int i = _enemies.Count - 1; i >= 0; i--)
            {
                if (_enemies[i].IsOffScreen(_graphics.PreferredBackBufferHeight))
                {
                    _enemies.RemoveAt(i);
                }
            }
            if (_gameState == GameState.Playing)
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
                            _gameState = GameState.GameOver;
                            Console.WriteLine("GAME OVER!");
                        }
                    }
                }
                
                // Check boss bullet collision with player
                for (int i = _bossBullets.Count - 1; i >= 0; i--)
                {
                    var bulletBounds = _bossBullets[i].GetBounds();

                    if (bulletBounds.Intersects(playerBounds))
                    {
                        _bossBullets.RemoveAt(i);
                        _playerLives--;

                        Console.WriteLine($"Player hit by boss! Lives remaining: {_playerLives}");

                        if (_playerLives <= 0)
                        {
                            _gameState = GameState.GameOver;
                            Console.WriteLine("GAME OVER!");
                        }
                    }
                }
            }


            



        }


        private void RestartGame()
        {
            // Reset all game entities
            _playerPosition = new Vector2(400, 500);
            _enemies.Clear();
            _bullets.Clear();
            _enemyBullets.Clear();
            _bossBullets.Clear();
            _boss = null;
            _bossSpawned = false;
            _score = 0;
            _playerLives = 3;
            _waveManager.Reset();
            
            // Reset wave delay system
            _waveCleared = false;
            _waveTimer = 0f;
            
            // Reset boss visual effects
            _bossAppearing = false;
            _bossFlashTimer = 0f;
            
            // Reset timers
            _lastShotTime = 0;
            _lastSpawnTime = 0;
            _lastEnemyShotTime = 0;
            
            _gameState = GameState.Playing;
            
            Console.WriteLine("Game Restarted!");
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();
            
            // Boss visual effect - red flash overlay
            if (_bossAppearing)
            {
                var flashColor = new Color(Color.Red, 0.5f); // semi-transparent red
                _spriteBatch.Draw(_pixelTexture, new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight), flashColor);
                
                // Show "BOSS APPEARING!" message
                string bossText = "BOSS APPEARING!";
                Vector2 textSize = _font.MeasureString(bossText);
                Vector2 position = new Vector2(
                    (_graphics.PreferredBackBufferWidth - textSize.X) / 2,
                    (_graphics.PreferredBackBufferHeight - textSize.Y) / 2
                );
                _spriteBatch.DrawString(_font, bossText, position, Color.Red);
            }

            
            _spriteBatch.Draw(_playerTexture, _playerPosition, null, Color.White, 0f, Vector2.Zero, 0.15f, SpriteEffects.None, 0f);

            _spriteBatch.DrawString(_font, $"Score: {_score}", new Vector2(10, 10), Color.White);
            _spriteBatch.DrawString(_font, $"Wave: {_waveManager.CurrentWave}", new Vector2(10, 70), Color.Yellow);
            
            // Show wave countdown when wave is cleared
            if (_waveCleared)
            {
                float remainingTime = _waveDelay - _waveTimer;
                _spriteBatch.DrawString(_font, $"Next Wave in: {remainingTime:F1}s", new Vector2(10, 100), Color.Orange);
                
                // Show "WAVE CLEARED!" message
                string waveClearedText = "WAVE CLEARED!";
                Vector2 textSize = _font.MeasureString(waveClearedText);
                Vector2 position = new Vector2(
                    (_graphics.PreferredBackBufferWidth - textSize.X) / 2,
                    (_graphics.PreferredBackBufferHeight - textSize.Y) / 2 - 50
                );
                _spriteBatch.DrawString(_font, waveClearedText, position, Color.Green);
            }

            foreach (var bullet in _bullets)
                bullet.Draw(_spriteBatch);

            
            foreach (var enemy in _enemies)
                enemy.Draw(_spriteBatch);

            // Gambar semua peluru musuh
            foreach (var ebullet in _enemyBullets)
                ebullet.Draw(_spriteBatch);
                
            // Draw boss and boss bullets
            if (_boss != null && !_boss.IsDead)
            {
                _boss.Draw(_spriteBatch);
                _boss.DrawHPBar(_spriteBatch, _hpBarTexture, _font, _graphics.PreferredBackBufferWidth);
            }
            
            foreach (var bbullet in _bossBullets)
                bbullet.Draw(_spriteBatch);

            // Tampilkan nyawa
            string livesText = $"Lives: {_playerLives}";
            _spriteBatch.DrawString(_font, livesText, new Vector2(10, 40), Color.Red);
            if (_gameState == GameState.GameOver)
            {
                string gameOverText = "GAME OVER";
                Vector2 textSize = _font.MeasureString(gameOverText);
                Vector2 position = new Vector2(
                    (_graphics.PreferredBackBufferWidth - textSize.X) / 2,
                    (_graphics.PreferredBackBufferHeight - textSize.Y) / 2
                );

                _spriteBatch.DrawString(_font, gameOverText, position, Color.Red);
                
                string restartText = "Press R to Restart";
                Vector2 restartTextSize = _font.MeasureString(restartText);
                Vector2 restartPosition = new Vector2(
                    (_graphics.PreferredBackBufferWidth - restartTextSize.X) / 2,
                    position.Y + 50
                );

                _spriteBatch.DrawString(_font, restartText, restartPosition, Color.White);
                return; // Don't render game objects
            }


            _spriteBatch.End(); 

            

            base.Draw(gameTime);
        }
    }
}
