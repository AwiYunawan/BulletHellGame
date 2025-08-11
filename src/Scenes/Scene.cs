using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BulletHellGame.Scenes
{
    public abstract class Scene
    {
        protected bool _isInitialized;

        public virtual void Initialize()
        {
            _isInitialized = true;
        }

        public virtual void LoadContent(ContentManager content)
        {
            // Override untuk load content yang diperlukan
        }

        public virtual void Unload()
        {
            _isInitialized = false;
        }

        public virtual void Update(GameTime gameTime)
        {
            if (!_isInitialized) return;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (!_isInitialized) return;
        }

        public virtual void HandleInput()
        {
            if (!_isInitialized) return;
        }
    }
}
