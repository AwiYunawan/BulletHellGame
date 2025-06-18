using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BulletHellGame
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Texture2D _playerTexture;
        private Vector2 _playerPosition;
        private float _speed = 200f;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _playerPosition = new Vector2(400, 500); // tengah bawah layar
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _playerTexture = Content.Load<Texture2D>("player"); // tambahkan file player.png di folder Content
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState state = Keyboard.GetState();
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (state.IsKeyDown(Keys.Left)) _playerPosition.X -= _speed * delta;
            if (state.IsKeyDown(Keys.Right)) _playerPosition.X += _speed * delta;
            if (state.IsKeyDown(Keys.Up)) _playerPosition.Y -= _speed * delta;
            if (state.IsKeyDown(Keys.Down)) _playerPosition.Y += _speed * delta;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();
            _spriteBatch.Draw(_playerTexture, _playerPosition, Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
