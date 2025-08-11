using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace BulletHellGame.GameObjects
{
    public enum PowerUpType
    {
        Health,
        Speed,
        Weapon,
        Shield
    }
    
    public class PowerUp : GameObject
    {
        public PowerUpType Type { get; private set; }
        private float _lifetime = 10f; // Power-up disappears after 10 seconds
        private float _timer = 0f;
        
        public PowerUp(PowerUpType type, Vector2 position)
        {
            Type = type;
            Position = position;
            Velocity = new Vector2(0, 50f); // Slowly falls down
        }
        
        public override void Initialize()
        {
            base.Initialize();
            _timer = 0f;
        }
        
        public override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);
            
            // Load different textures based on power-up type
            switch (Type)
            {
                case PowerUpType.Health:
                    Texture = content.Load<Texture2D>("Textures/powerup_health");
                    break;
                case PowerUpType.Speed:
                    Texture = content.Load<Texture2D>("Textures/powerup_speed");
                    break;
                case PowerUpType.Weapon:
                    Texture = content.Load<Texture2D>("Textures/powerup_weapon");
                    break;
                case PowerUpType.Shield:
                    Texture = content.Load<Texture2D>("Textures/powerup_shield");
                    break;
                default:
                    Texture = content.Load<Texture2D>("Textures/powerup_default");
                    break;
            }
        }
        
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            
            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            // Destroy power-up after lifetime expires
            if (_timer >= _lifetime)
            {
                Destroy();
            }
            
            // Destroy if it goes off screen
            if (Position.Y > 650)
            {
                Destroy();
            }
        }
        
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Texture != null && !IsDead)
            {
                // Add a pulsing effect
                var pulse = 1f + 0.1f * (float)System.Math.Sin(_timer * 5f);
                var pulseScale = Scale * pulse;
                
                spriteBatch.Draw(Texture, Position, null, Color.White, Rotation, Origin, pulseScale, SpriteEffects.None, 0f);
            }
        }
    }
} 