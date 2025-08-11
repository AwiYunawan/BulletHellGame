using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using BulletHellGame.Managers;
using System.Collections.Generic;

namespace BulletHellGame.GameObjects
{
    public class Enemy : GameObject
    {
        public int Health { get; private set; } = 30;
        public int MaxHealth { get; private set; } = 30;
        public float ShootCooldown { get; set; } = 2f;
        public float ShootTimer { get; set; } = 0f;
        public bool CanShoot => ShootTimer <= 0;
        
        private float _moveSpeed = 100f;
        private Vector2 _moveDirection = Vector2.UnitY; // Move downward by default
        private float _moveTimer = 0f;
        private float _moveChangeTime = 3f; // Change direction every 3 seconds
        
        public Enemy()
        {
            Scale = new Vector2(0.8f, 0.8f);
            Health = MaxHealth;
        }
        
        public Enemy(Vector2 position) : this()
        {
            Position = position;
        }
        
        public override void Initialize()
        {
            base.Initialize();
            Health = MaxHealth;
            ShootTimer = ShootCooldown;
            _moveTimer = 0f;
        }
        
        public override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);
            Texture = content.Load<Texture2D>("Textures/enemy");
            
            if (Texture != null)
            {
                Origin = new Vector2(Texture.Width / 2f, Texture.Height / 2f);
            }
        }
        
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            
            if (IsDead) return;
            
            var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            UpdateMovement(gameTime);
            UpdateShooting(gameTime);
            UpdateOffScreenCheck();
        }
        
        private void UpdateMovement(GameTime gameTime)
        {
            var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            // Update movement timer
            _moveTimer += deltaTime;
            
            // Change direction periodically
            if (_moveTimer >= _moveChangeTime)
            {
                ChangeDirection();
                _moveTimer = 0f;
            }
            
            // Move enemy
            Position += _moveDirection * _moveSpeed * deltaTime;
            
            // Keep enemy within screen bounds
            var screenBounds = new Rectangle(0, 0, 800, 600);
            if (!screenBounds.Contains(Position.ToPoint()))
            {
                // Bounce off screen edges
                if (Position.X < 0 || Position.X > 800)
                {
                    _moveDirection.X *= -1;
                }
                if (Position.Y < 0 || Position.Y > 600)
                {
                    _moveDirection.Y *= -1;
                }
                
                // Clamp position to screen
                Position = Vector2.Clamp(Position, Vector2.Zero, new Vector2(800, 600));
            }
        }
        
        private void ChangeDirection()
        {
            // Random direction change
            var random = new System.Random();
            var angle = random.Next(0, 360) * System.Math.PI / 180f;
            _moveDirection = new Vector2(
                (float)System.Math.Cos(angle),
                (float)System.Math.Sin(angle)
            );
            _moveDirection.Normalize();
        }
        
        private void UpdateShooting(GameTime gameTime)
        {
            var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            if (ShootTimer > 0)
            {
                ShootTimer -= deltaTime;
            }
            
            if (CanShoot)
            {
                Shoot();
                ShootTimer = ShootCooldown;
            }
        }
        
        private void Shoot()
        {
            // Create enemy bullet
            var bullet = new EnemyBullet();
            bullet.Position = Position + new Vector2(0, 30); // Below enemy
            bullet.Velocity = new Vector2(0, 200f); // Moving downward
            
            // Add bullet to game world (this would be handled by GameScene)
            // For now, we'll just create the bullet
        }
        
        private void UpdateOffScreenCheck()
        {
            // Destroy enemy if it goes too far off screen
            if (Position.Y > 700 || Position.Y < -100 || Position.X < -100 || Position.X > 900)
            {
                Destroy();
            }
        }
        
        public override void TakeDamage(int damage = 10)
        {
            Health -= damage;
            
            if (Health <= 0)
            {
                Health = 0;
                Destroy();
                
                // Play death sound
                AudioManager.Instance.PlaySound("enemy_death");
                
                // Drop power-up with some probability
                if (System.Math.Abs(System.Guid.NewGuid().GetHashCode()) % 10 < 3) // 30% chance
                {
                    DropPowerUp();
                }
            }
            else
            {
                // Play hit sound
                AudioManager.Instance.PlaySound("enemy_hit");
            }
        }
        
        private void DropPowerUp()
        {
            // Random power-up type
            var random = new System.Random();
            var powerUpTypes = System.Enum.GetValues(typeof(PowerUpType));
            var randomType = (PowerUpType)powerUpTypes.GetValue(random.Next(powerUpTypes.Length));
            
            var powerUp = new PowerUp(randomType, Position);
            // Add power-up to game world (this would be handled by GameScene)
        }
        
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Texture != null && !IsDead)
            {
                // Draw enemy with health bar
                spriteBatch.Draw(Texture, Position, null, Color.White, Rotation, Origin, Scale, SpriteEffects.None, 0f);
                
                // Draw health bar
                DrawHealthBar(spriteBatch);
            }
        }
        
        private void DrawHealthBar(SpriteBatch spriteBatch)
        {
            var healthBarWidth = 40f;
            var healthBarHeight = 4f;
            var healthPercentage = (float)Health / MaxHealth;
            
            var healthBarPosition = Position + new Vector2(-healthBarWidth / 2f, -30f);
            
            // Background (red)
            var backgroundRect = new Rectangle(
                (int)healthBarPosition.X,
                (int)healthBarPosition.Y,
                (int)healthBarWidth,
                (int)healthBarHeight
            );
            
            // Health (green)
            var healthRect = new Rectangle(
                (int)healthBarPosition.X,
                (int)healthBarPosition.Y,
                (int)(healthBarWidth * healthPercentage),
                (int)healthBarHeight
            );
            
            // Draw health bar (using simple colored rectangles)
            // In a real implementation, you'd use proper textures or shapes
        }
    }
}
