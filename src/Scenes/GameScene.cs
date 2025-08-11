using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using BulletHellGame.GameObjects;
using BulletHellGame.Managers;
using BulletHellGame.Systems;
using System.Collections.Generic;

namespace BulletHellGame.Scenes
{
    public class GameScene : Scene
    {
        private Player _player;
        private List<Enemy> _enemies;
        private List<Bullet> _playerBullets;
        private List<EnemyBullet> _enemyBullets;
        private List<PowerUp> _powerUps;
        private List<Boss> _bosses;
        
        private CollisionSystem _collisionSystem;
        private WaveManager _waveManager;
        private ShopManager _shopManager;
        
        private SpriteFont _scoreFont;
        private int _score;
        private int _lives;

        public override void Initialize()
        {
            base.Initialize();
            
            _enemies = new List<Enemy>();
            _playerBullets = new List<Bullet>();
            _enemyBullets = new List<EnemyBullet>();
            _powerUps = new List<PowerUp>();
            _bosses = new List<Boss>();
            
            _collisionSystem = new CollisionSystem();
            _waveManager = new WaveManager();
            _shopManager = new ShopManager();
            
            _score = 0;
            _lives = 3;
            
            // Initialize player
            _player = new Player();
            _player.Initialize();
        }

        public override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);
            
            _scoreFont = content.Load<SpriteFont>("Fonts/score");
            _player.LoadContent(content);
            _waveManager.LoadContent(content);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            
            HandleInput();
            
            _player.Update(gameTime);
            _waveManager.Update(gameTime);
            
            // Update enemies
            for (int i = _enemies.Count - 1; i >= 0; i--)
            {
                _enemies[i].Update(gameTime);
                if (_enemies[i].IsDead)
                {
                    _enemies.RemoveAt(i);
                    _score += 100;
                }
            }
            
            // Update bullets
            UpdateBullets(gameTime);
            
            // Update power-ups
            UpdatePowerUps(gameTime);
            
            // Update bosses
            UpdateBosses(gameTime);
            
            // Check collisions
            _collisionSystem.CheckCollisions(_player, _enemies, _playerBullets, _enemyBullets, _powerUps);
            
            // Check game over
            if (_lives <= 0)
            {
                // Game over logic
            }
        }

        private void UpdateBullets(GameTime gameTime)
        {
            // Update player bullets
            for (int i = _playerBullets.Count - 1; i >= 0; i--)
            {
                _playerBullets[i].Update(gameTime);
                if (_playerBullets[i].IsDead)
                {
                    _playerBullets.RemoveAt(i);
                }
            }
            
            // Update enemy bullets
            for (int i = _enemyBullets.Count - 1; i >= 0; i--)
            {
                _enemyBullets[i].Update(gameTime);
                if (_enemyBullets[i].IsDead)
                {
                    _enemyBullets.RemoveAt(i);
                }
            }
        }

        private void UpdatePowerUps(GameTime gameTime)
        {
            for (int i = _powerUps.Count - 1; i >= 0; i--)
            {
                _powerUps[i].Update(gameTime);
                if (_powerUps[i].IsDead)
                {
                    _powerUps.RemoveAt(i);
                }
            }
        }

        private void UpdateBosses(GameTime gameTime)
        {
            for (int i = _bosses.Count - 1; i >= 0; i--)
            {
                _bosses[i].Update(gameTime);
                if (_bosses[i].IsDead)
                {
                    _bosses.RemoveAt(i);
                    _score += 1000;
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            
            // Draw game objects
            _player.Draw(spriteBatch);
            
            foreach (var enemy in _enemies)
            {
                enemy.Draw(spriteBatch);
            }
            
            foreach (var bullet in _playerBullets)
            {
                bullet.Draw(spriteBatch);
            }
            
            foreach (var bullet in _enemyBullets)
            {
                bullet.Draw(spriteBatch);
            }
            
            foreach (var powerUp in _powerUps)
            {
                powerUp.Draw(spriteBatch);
            }
            
            foreach (var boss in _bosses)
            {
                boss.Draw(spriteBatch);
            }
            
            // Draw UI
            DrawUI(spriteBatch);
        }

        private void DrawUI(SpriteBatch spriteBatch)
        {
            string scoreText = $"Score: {_score}";
            string livesText = $"Lives: {_lives}";
            
            spriteBatch.DrawString(_scoreFont, scoreText, new Vector2(10, 10), Color.White);
            spriteBatch.DrawString(_scoreFont, livesText, new Vector2(10, 40), Color.White);
        }
    }
}
