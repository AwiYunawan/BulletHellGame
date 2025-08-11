using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace BulletHellGame.GameObjects
{
    public class EnemyBullet : Bullet
    {
        public EnemyBullet() : base()
        {
            IsPlayerBullet = false;
            Damage = 15; // Enemy bullets do more damage
            Scale = new Vector2(0.4f, 0.4f); // Slightly smaller than player bullets
        }
        
        public EnemyBullet(Vector2 position, Vector2 velocity) : base(position, velocity, false)
        {
            Damage = 15;
            Scale = new Vector2(0.4f, 0.4f);
        }
        
        public override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);
            
            // Load enemy bullet texture
            Texture = content.Load<Texture2D>("Textures/enemy_bullet");
            
            // Set origin after loading texture
            if (Texture != null)
            {
                Origin = new Vector2(Texture.Width / 2f, Texture.Height / 2f);
            }
        }
        
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            
            // Enemy bullets can have different behaviors
            // For example, they could curve or change direction
            UpdateBulletBehavior(gameTime);
        }
        
        private void UpdateBulletBehavior(GameTime gameTime)
        {
            // Add some variation to enemy bullet movement
            // This could be expanded with different bullet patterns
            var time = (float)gameTime.TotalGameTime.TotalSeconds;
            
            // Simple sine wave movement for some bullets
            if (System.Math.Abs(Velocity.X) < 0.1f) // If bullet is mostly vertical
            {
                var sineOffset = (float)System.Math.Sin(time * 3f + Position.X * 0.01f) * 20f;
                Position += new Vector2(sineOffset * (float)gameTime.ElapsedGameTime.TotalSeconds, 0);
            }
        }
        
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Texture != null && !IsDead)
            {
                // Enemy bullets are red
                var color = Color.Red;
                
                // Add a subtle glow effect
                var glowColor = Color.DarkRed;
                var glowScale = Scale * 1.2f;
                
                // Draw glow
                spriteBatch.Draw(Texture, Position, null, glowColor, Rotation, Origin, glowScale, SpriteEffects.None, 0f);
                
                // Draw main bullet
                spriteBatch.Draw(Texture, Position, null, color, Rotation, Origin, Scale, SpriteEffects.None, 0f);
            }
        }
    }
}
