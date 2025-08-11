using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using BulletHellGame.Managers;
using System.Collections.Generic;

namespace BulletHellGame.GameObjects
{
    public class Boss : GameObject
    {
        public int Health { get; private set; } = 200;
        public int MaxHealth { get; private set; } = 200;
        public float ShootCooldown { get; set; } = 1f;
        public float ShootTimer { get; set; } = 0f;
        public bool CanShoot => ShootTimer <= 0;
        
        private float _moveSpeed = 50f;
        private Vector2 _moveDirection = Vector2.UnitX; // Move horizontally by default
        private float _moveTimer = 0f;
        private float _moveChangeTime = 2f; // Change direction every 2 seconds
        private float _phaseTimer = 0f;
        private float _phaseChangeTime = 10f; // Change phase every 10 seconds
        private int _currentPhase = 1;
        private BossPhase _currentBossPhase;
        
        public enum BossPhase
        {
            Phase1, // Basic movement and shooting
            Phase2, // More aggressive, faster shooting
            Phase3, // Final phase, very aggressive
            Phase4  // Desperate phase, maximum aggression
        }
        
        public Boss()
        {
            Scale = new Vector2(1.5f, 1.5f); // Boss is larger than regular enemies
            Health = MaxHealth;
            _currentBossPhase = BossPhase.Phase1;
        }
        
        public Boss(Vector2 position) : this()
        {
            Position = position;
        }
        
        public override void Initialize()
        {
            base.Initialize();
            Health = MaxHealth;
            ShootTimer = ShootCooldown;
            _moveTimer = 0f;
            _phaseTimer = 0f;
            _currentPhase = 1;
            _currentBossPhase = BossPhase.Phase1;
        }
        
        public override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);
            Texture = content.Load<Texture2D>("Textures/boss");
            
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
            
            UpdatePhase(gameTime);
            UpdateMovement(gameTime);
            UpdateShooting(gameTime);
            UpdateOffScreenCheck();
        }
        
        private void UpdatePhase(GameTime gameTime)
        {
            var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _phaseTimer += deltaTime;
            
            if (_phaseTimer >= _phaseChangeTime)
            {
                AdvancePhase();
                _phaseTimer = 0f;
            }
        }
        
        private void AdvancePhase()
        {
            _currentPhase++;
            
            switch (_currentPhase)
            {
                case 2:
                    _currentBossPhase = BossPhase.Phase2;
                    ShootCooldown = 0.7f;
                    _moveSpeed = 70f;
                    break;
                case 3:
                    _currentBossPhase = BossPhase.Phase3;
                    ShootCooldown = 0.5f;
                    _moveSpeed = 90f;
                    break;
                case 4:
                    _currentBossPhase = BossPhase.Phase4;
                    ShootCooldown = 0.3f;
                    _moveSpeed = 120f;
                    break;
            }
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
            
            // Move boss
            Position += _moveDirection * _moveSpeed * deltaTime;
            
            // Keep boss within screen bounds
            var screenBounds = new Rectangle(0, 0, 800, 600);
            if (!screenBounds.Contains(Position.ToPoint()))
            {
                // Bounce off screen edges
                if (Position.X < 100 || Position.X > 700)
                {
                    _moveDirection.X *= -1;
                }
                if (Position.Y < 50 || Position.Y > 300)
                {
                    _moveDirection.Y *= -1;
                }
                
                // Clamp position to screen
                Position = Vector2.Clamp(Position, new Vector2(100, 50), new Vector2(700, 300));
            }
        }
        
        private void ChangeDirection()
        {
            // More complex movement patterns for boss
            var random = new System.Random();
            
            switch (_currentBossPhase)
            {
                case BossPhase.Phase1:
                    // Simple horizontal movement
                    _moveDirection = new Vector2(random.Next(2) == 0 ? -1 : 1, 0);
                    break;
                case BossPhase.Phase2:
                    // Diagonal movement
                    var angle = random.Next(0, 360) * System.Math.PI / 180f;
                    _moveDirection = new Vector2(
                        (float)System.Math.Cos(angle),
                        (float)System.Math.Sin(angle)
                    );
                    break;
                case BossPhase.Phase3:
                    // More aggressive movement
                    var playerDirection = Vector2.UnitY; // This would be towards player
                    _moveDirection = Vector2.Lerp(_moveDirection, playerDirection, 0.3f);
                    break;
                case BossPhase.Phase4:
                    // Erratic movement
                    _moveDirection = new Vector2(
                        (float)(random.NextDouble() * 2 - 1),
                        (float)(random.NextDouble() * 2 - 1)
                    );
                    break;
            }
            
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
            // Create multiple bullets based on phase
            var bulletCount = _currentPhase;
            
            for (int i = 0; i < bulletCount; i++)
            {
                var bullet = new EnemyBullet();
                bullet.Position = Position + new Vector2(0, 40);
                
                // Different bullet patterns based on phase
                switch (_currentBossPhase)
                {
                    case BossPhase.Phase1:
                        bullet.Velocity = new Vector2(0, 150f);
                        break;
                    case BossPhase.Phase2:
                        var spreadAngle = (i - bulletCount / 2f) * 0.3f;
                        bullet.Velocity = new Vector2(
                            (float)System.Math.Sin(spreadAngle) * 100f,
                            200f
                        );
                        break;
                    case BossPhase.Phase3:
                        var circularAngle = System.Math.PI * 2 * i / bulletCount;
                        bullet.Velocity = new Vector2(
                            (float)System.Math.Cos(circularAngle) * 150f,
                            (float)System.Math.Sin(circularAngle) * 150f
                        );
                        break;
                    case BossPhase.Phase4:
                        // Maximum chaos
                        var random = new System.Random();
                        var randomAngle = random.Next(0, 360) * System.Math.PI / 180f;
                        bullet.Velocity = new Vector2(
                            (float)System.Math.Cos(randomAngle) * 200f,
                            (float)System.Math.Sin(randomAngle) * 200f
                        );
                        break;
                }
                
                // Add bullet to game world (this would be handled by GameScene)
            }
        }
        
        private void UpdateOffScreenCheck()
        {
            // Boss doesn't get destroyed when going off screen
            // It just bounces back
        }
        
        public override void TakeDamage(int damage = 20)
        {
            Health -= damage;
            
            if (Health <= 0)
            {
                Health = 0;
                Destroy();
                
                // Play boss death sound
                AudioManager.Instance.PlaySound("boss_death");
                
                // Drop multiple power-ups
                for (int i = 0; i < 3; i++)
                {
                    DropPowerUp();
                }
            }
            else
            {
                // Play boss hit sound
                AudioManager.Instance.PlaySound("boss_hit");
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
                // Draw boss with health bar
                spriteBatch.Draw(Texture, Position, null, Color.White, Rotation, Origin, Scale, SpriteEffects.None, 0f);
                
                // Draw health bar
                DrawHealthBar(spriteBatch);
                
                // Draw phase indicator
                DrawPhaseIndicator(spriteBatch);
            }
        }
        
        private void DrawHealthBar(SpriteBatch spriteBatch)
        {
            var healthBarWidth = 80f;
            var healthBarHeight = 8f;
            var healthPercentage = (float)Health / MaxHealth;
            
            var healthBarPosition = Position + new Vector2(-healthBarWidth / 2f, -50f);
            
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
        
        private void DrawPhaseIndicator(SpriteBatch spriteBatch)
        {
            var phaseText = $"Phase {_currentPhase}";
            var phasePosition = Position + new Vector2(0, -70f);
            
            // Draw phase text (this would require a font)
            // In a real implementation, you'd use SpriteBatch.DrawString
        }
    }
} 