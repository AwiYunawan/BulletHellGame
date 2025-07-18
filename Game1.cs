﻿using Microsoft.Xna.Framework;
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
        }

        protected override void Update(GameTime gameTime)
        {
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

            
            if (gameTime.TotalGameTime.TotalMilliseconds - _lastSpawnTime > 1000)
            {
                float x = _random.Next(0, _graphics.PreferredBackBufferWidth - (int)(_enemyTexture.Width * 0.5f));
                Vector2 enemyStart = new Vector2(x, -_enemyTexture.Height);
                _enemies.Add(new Enemy(_enemyTexture, enemyStart));
                _lastSpawnTime = gameTime.TotalGameTime.TotalMilliseconds;
            }

            
            for (int i = _enemies.Count - 1; i >= 0; i--)
            {
                _enemies[i].Update(gameTime);
                if (_enemies[i].IsOffScreen(_graphics.PreferredBackBufferHeight))
                {
                    _enemies.RemoveAt(i);
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

            _spriteBatch.End(); 

            base.Draw(gameTime);
        }
    }
}
