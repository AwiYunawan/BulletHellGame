using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BulletHellGame.Scenes;

namespace BulletHellGame.Managers
{
    public class GameManager
    {
        private static GameManager _instance;
        public static GameManager Instance
        {
            get
            {
                _instance ??= new GameManager();
                return _instance;
            }
        }

        private Scene _currentScene;
        private Scene _nextScene;

        public void Initialize()
        {
            // Initialize dengan MainMenu sebagai scene pertama
            _currentScene = new MainMenu();
            _currentScene.Initialize();
        }

        public void LoadScene(Scene scene)
        {
            _nextScene = scene;
        }

        public void Update(GameTime gameTime)
        {
            if (_nextScene != null)
            {
                _currentScene?.Unload();
                _currentScene = _nextScene;
                _currentScene.Initialize();
                _nextScene = null;
            }

            _currentScene?.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _currentScene?.Draw(spriteBatch);
        }
    }
}
