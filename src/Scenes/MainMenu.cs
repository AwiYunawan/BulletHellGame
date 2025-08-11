using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using BulletHellGame.UI;
using BulletHellGame.Managers;
using System.Collections.Generic;

namespace BulletHellGame.Scenes
{
    public class MainMenu : Scene
    {
        private List<Button> _buttons;
        private SpriteFont _titleFont;
        private SpriteFont _buttonFont;
        private string _titleText;
        
        public override void Initialize()
        {
            base.Initialize();
            
            _buttons = new List<Button>();
            _titleText = "BULLET HELL GAME";
        }
        
        public override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);
            
            _titleFont = content.Load<SpriteFont>("Fonts/title");
            _buttonFont = content.Load<SpriteFont>("Fonts/score");
            
            CreateButtons();
        }
        
        private void CreateButtons()
        {
            // Start Game button
            var startButton = new Button(new Rectangle(300, 250, 200, 50), "Start Game", _buttonFont);
            startButton.OnClick += () => GameManager.Instance.LoadScene(new GameScene());
            _buttons.Add(startButton);
            
            // Shop button
            var shopButton = new Button(new Rectangle(300, 320, 200, 50), "Shop", _buttonFont);
            shopButton.OnClick += () => GameManager.Instance.LoadScene(new ShopScene());
            _buttons.Add(shopButton);
            
            // Exit button
            var exitButton = new Button(new Rectangle(300, 390, 200, 50), "Exit", _buttonFont);
            exitButton.OnClick += () => System.Environment.Exit(0);
            _buttons.Add(exitButton);
        }
        
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            
            foreach (var button in _buttons)
            {
                button.Update();
            }
        }
        
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            
            // Draw title
            var titleSize = _titleFont.MeasureString(_titleText);
            var titlePosition = new Vector2(
                (800 - titleSize.X) / 2, // Assuming 800x600 resolution
                100
            );
            spriteBatch.DrawString(_titleFont, _titleText, titlePosition, Color.White);
            
            // Draw buttons
            foreach (var button in _buttons)
            {
                button.Draw(spriteBatch);
            }
        }
    }
}
