using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace BulletHellGame.GameObjects
{
    public abstract class GameObject
    {
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public float Rotation { get; set; }
        public Vector2 Scale { get; set; }
        public Vector2 Origin { get; set; }
        public Texture2D Texture { get; protected set; }
        public bool IsDead { get; set; } = false;
        
        public Rectangle Bounds
        {
            get
            {
                if (Texture == null) return Rectangle.Empty;
                return new Rectangle(
                    (int)Position.X - (int)Origin.X,
                    (int)Position.Y - (int)Origin.Y,
                    (int)(Texture.Width * Scale.X),
                    (int)(Texture.Height * Scale.Y)
                );
            }
        }
        
        protected GameObject()
        {
            Position = Vector2.Zero;
            Velocity = Vector2.Zero;
            Rotation = 0f;
            Scale = Vector2.One;
            Origin = Vector2.Zero;
        }
        
        public virtual void Initialize()
        {
            // Override untuk initialization yang diperlukan
        }
        
        public virtual void LoadContent(ContentManager content)
        {
            // Override untuk load content yang diperlukan
        }
        
        public virtual void Update(GameTime gameTime)
        {
            if (IsDead) return;
            
            // Update position based on velocity
            Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
        
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (Texture != null && !IsDead)
            {
                spriteBatch.Draw(Texture, Position, null, Color.White, Rotation, Origin, Scale, SpriteEffects.None, 0f);
            }
        }
        
        public virtual void Destroy()
        {
            IsDead = true;
        }
        
        public virtual void TakeDamage(int damage = 1)
        {
            // Override untuk implementasi damage yang spesifik
        }
    }
}
