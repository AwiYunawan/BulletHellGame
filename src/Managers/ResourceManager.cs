using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace BulletHellGame.Managers
{
    public class ResourceManager
    {
        private static ResourceManager _instance;
        public static ResourceManager Instance
        {
            get
            {
                _instance ??= new ResourceManager();
                return _instance;
            }
        }

        private ContentManager _content;
        private Dictionary<string, Texture2D> _textures;
        private Dictionary<string, SpriteFont> _fonts;

        private ResourceManager()
        {
            _textures = new Dictionary<string, Texture2D>();
            _fonts = new Dictionary<string, SpriteFont>();
        }

        public void Initialize(ContentManager content)
        {
            _content = content;
            LoadResources();
        }

        private void LoadResources()
        {
            // Load textures
            _textures["player"] = _content.Load<Texture2D>("Textures/player");
            _textures["enemy"] = _content.Load<Texture2D>("Textures/enemy");
            _textures["bullet"] = _content.Load<Texture2D>("Textures/bullet");
            
            // Load fonts
            _fonts["title"] = _content.Load<SpriteFont>("Fonts/title");
            _fonts["score"] = _content.Load<SpriteFont>("Fonts/score");
        }

        public Texture2D GetTexture(string textureName)
        {
            return _textures.ContainsKey(textureName) ? _textures[textureName] : null;
        }

        public SpriteFont GetFont(string fontName)
        {
            return _fonts.ContainsKey(fontName) ? _fonts[fontName] : null;
        }

        public void UnloadResources()
        {
            _textures.Clear();
            _fonts.Clear();
        }
    }
}
