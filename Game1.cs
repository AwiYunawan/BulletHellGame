using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;


namespace BulletHellGame
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Texture2D _playerTexture;
        private Vector2 _playerPosition = new Vector2(400, 500);
        private float _playerSpeed = 300f;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            IsMouseVisible = true;
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load gambar dari Assets langsung tanpa pipeline
            using (FileStream stream = new FileStream("Assets/player.png", FileMode.Open))
            {
                _playerTexture = Texture2D.FromStream(GraphicsDevice, stream);
            }
        }

        protected override void Update(GameTime gameTime)
        {
            var keyboard = Keyboard.GetState();
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (keyboard.IsKeyDown(Keys.A)) _playerPosition.X -= _playerSpeed * dt;
            if (keyboard.IsKeyDown(Keys.D)) _playerPosition.X += _playerSpeed * dt;
            if (keyboard.IsKeyDown(Keys.W)) _playerPosition.Y -= _playerSpeed * dt;
            if (keyboard.IsKeyDown(Keys.S)) _playerPosition.Y += _playerSpeed * dt;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            _spriteBatch.Draw(_playerTexture, _playerPosition, Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
