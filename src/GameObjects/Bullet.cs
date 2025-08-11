using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace BulletHellGame.GameObjects
{
    public class Bullet : GameObject
    {
        public int Damage { get; set; } = 10;
        public float Speed { get; set; } = 400f;
        public bool IsPlayerBullet { get; set; } = true;
        
        public Bullet()
        {
            Scale = new Vector2(0.5f, 0.5f); // Smaller bullets
        }
        
        public Bullet(Vector2 position, Vector2 velocity, bool isPlayerBullet = true)
        {
            Position = position;
            Velocity = velocity;
            IsPlayerBullet = isPlayerBullet;
            Scale = new Vector2(0.5f, 0.5f);
        }
        
        public override void Initialize()
        {
            base.Initialize();
            
            // Set origin to center of bullet
            if (Texture != null)
            {
                Origin = new Vector2(Texture.Width / 2f, Texture.Height / 2f);
            }
        }
        
        public override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);
            
            if (IsPlayerBullet)
            {
                Texture = content.Load<Texture2D>("Textures/bullet");
            }
            else
            {
                Texture = content.Load<Texture2D>("Textures/enemy_bullet");
            }
            
            // Set origin after loading texture
            if (Texture != null)
            {
                Origin = new Vector2(Texture.Width / 2f, Texture.Height / 2f);
            }
        }
        
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            
            // Destroy bullet if it goes off screen
            if (Position.Y < -50 || Position.Y > 650 || Position.X < -50 || Position.X > 850)
            {
                Destroy();
            }
        }
        
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Texture != null && !IsDead)
            {
                var color = IsPlayerBullet ? Color.Cyan : Color.Red;
                spriteBatch.Draw(Texture, Position, null, color, Rotation, Origin, Scale, SpriteEffects.None, 0f);
            }
        }
        
        public void SetVelocity(Vector2 direction, float speed)
        {
            Velocity = direction * speed;
        }
        
        public void SetVelocity(Vector2 velocity)
        {
            Velocity = velocity;
        }
    }
}
