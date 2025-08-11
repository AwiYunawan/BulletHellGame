using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using BulletHellGame.Managers;

namespace BulletHellGame.GameObjects
{
    public class Player : GameObject
    {
        private float _speed = 200f;
        private int _health = 100;
        private int _maxHealth = 100;
        private float _shootCooldown = 0.1f;
        private float _shootTimer = 0f;
        
        public int Health => _health;
        public int MaxHealth => _maxHealth;
        public bool IsDead => _health <= 0;
        
        public Player()
        {
            Position = new Vector2(400, 500); // Center bottom of screen
            Scale = Vector2.One;
        }
        
        public override void Initialize()
        {
            base.Initialize();
            _health = _maxHealth;
        }
        
        public override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);
            Texture = content.Load<Texture2D>("Textures/player");
        }
        
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            
            if (IsDead) return;
            
            HandleInput(gameTime);
            UpdateShootTimer(gameTime);
        }
        
        private void HandleInput(GameTime gameTime)
        {
            var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            var inputManager = InputManager.Instance;
            
            // Movement
            Vector2 movement = Vector2.Zero;
            
            if (inputManager.IsKeyHeld(Keys.W) || inputManager.IsKeyHeld(Keys.Up))
                movement.Y -= 1;
            if (inputManager.IsKeyHeld(Keys.S) || inputManager.IsKeyHeld(Keys.Down))
                movement.Y += 1;
            if (inputManager.IsKeyHeld(Keys.A) || inputManager.IsKeyHeld(Keys.Left))
                movement.X -= 1;
            if (inputManager.IsKeyHeld(Keys.D) || inputManager.IsKeyHeld(Keys.Right))
                movement.X += 1;
            
            if (movement != Vector2.Zero)
            {
                movement.Normalize();
                Position += movement * _speed * deltaTime;
                
                // Keep player within screen bounds
                Position = Vector2.Clamp(Position, Vector2.Zero, new Vector2(800, 600));
            }
            
            // Shooting
            if (inputManager.IsKeyHeld(Keys.Space) && _shootTimer <= 0)
            {
                Shoot();
                _shootTimer = _shootCooldown;
            }
        }
        
        private void UpdateShootTimer(GameTime gameTime)
        {
            if (_shootTimer > 0)
            {
                _shootTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
        }
        
        private void Shoot()
        {
            // Create bullet at player position
            var bullet = new Bullet();
            bullet.Position = Position + new Vector2(0, -20); // Slightly above player
            bullet.Velocity = new Vector2(0, -400); // Moving upward
            
            // Add bullet to game world (this would be handled by GameScene)
        }
        
        public void TakeDamage(int damage = 20)
        {
            _health -= damage;
            if (_health < 0) _health = 0;
            
            // Play damage sound
            AudioManager.Instance.PlaySound("damage");
        }
        
        public void Heal(int amount)
        {
            _health += amount;
            if (_health > _maxHealth) _health = _maxHealth;
        }
        
        public void CollectPowerUp(PowerUp powerUp)
        {
            // Apply power-up effects
            switch (powerUp.Type)
            {
                case PowerUpType.Health:
                    Heal(50);
                    break;
                case PowerUpType.Speed:
                    _speed += 50f;
                    break;
                case PowerUpType.Weapon:
                    _shootCooldown *= 0.8f; // Faster shooting
                    break;
            }
        }
        
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Texture != null)
            {
                spriteBatch.Draw(Texture, Position, null, Color.White, Rotation, Origin, Scale, SpriteEffects.None, 0f);
            }
        }
    }
} 