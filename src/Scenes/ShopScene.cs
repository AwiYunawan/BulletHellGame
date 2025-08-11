using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using BulletHellGame.UI;
using BulletHellGame.Managers;
using System.Collections.Generic;

namespace BulletHellGame.Scenes
{
    public class ShopScene : Scene
    {
        private List<Button> _buttons;
        private SpriteFont _titleFont;
        private SpriteFont _buttonFont;
        private SpriteFont _itemFont;
        private string _titleText;
        private ShopManager _shopManager;
        
        public override void Initialize()
        {
            base.Initialize();
            
            _buttons = new List<Button>();
            _titleText = "SHOP";
            _shopManager = new ShopManager();
        }
        
        public override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);
            
            _titleFont = content.Load<SpriteFont>("Fonts/title");
            _buttonFont = content.Load<SpriteFont>("Fonts/score");
            _itemFont = content.Load<SpriteFont>("Fonts/score");
            
            CreateButtons();
        }
        
        private void CreateButtons()
        {
            // Back to menu button
            var backButton = new Button(new Rectangle(50, 500, 150, 40), "Back to Menu", _buttonFont);
            backButton.OnClick += () => GameManager.Instance.LoadScene(new MainMenu());
            _buttons.Add(backButton);
            
            // Buy weapon upgrade button
            var weaponButton = new Button(new Rectangle(300, 200, 200, 50), "Buy Weapon Upgrade", _buttonFont);
            weaponButton.OnClick += () => _shopManager.BuyWeaponUpgrade();
            _buttons.Add(weaponButton);
            
            // Buy shield upgrade button
            var shieldButton = new Button(new Rectangle(300, 270, 200, 50), "Buy Shield Upgrade", _buttonFont);
            shieldButton.OnClick += () => _shopManager.BuyShieldUpgrade();
            _buttons.Add(shieldButton);
            
            // Buy speed upgrade button
            var speedButton = new Button(new Rectangle(300, 340, 200, 50), "Buy Speed Upgrade", _buttonFont);
            speedButton.OnClick += () => _shopManager.BuySpeedUpgrade();
            _buttons.Add(speedButton);
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
                (800 - titleSize.X) / 2,
                50
            );
            spriteBatch.DrawString(_titleFont, _titleText, titlePosition, Color.White);
            
            // Draw shop items info
            DrawShopItems(spriteBatch);
            
            // Draw buttons
            foreach (var button in _buttons)
            {
                button.Draw(spriteBatch);
            }
        }
        
        private void DrawShopItems(SpriteBatch spriteBatch)
        {
            // Draw weapon upgrade info
            spriteBatch.DrawString(_itemFont, "Weapon Upgrade - Cost: 1000", new Vector2(100, 200), Color.White);
            spriteBatch.DrawString(_itemFont, "Increases bullet damage", new Vector2(100, 220), Color.Gray);
            
            // Draw shield upgrade info
            spriteBatch.DrawString(_itemFont, "Shield Upgrade - Cost: 800", new Vector2(100, 270), Color.White);
            spriteBatch.DrawString(_itemFont, "Increases player health", new Vector2(100, 290), Color.Gray);
            
            // Draw speed upgrade info
            spriteBatch.DrawString(_itemFont, "Speed Upgrade - Cost: 600", new Vector2(100, 340), Color.White);
            spriteBatch.DrawString(_itemFont, "Increases player movement speed", new Vector2(100, 360), Color.Gray);
            
            // Draw current money
            spriteBatch.DrawString(_itemFont, $"Money: {_shopManager.Money}", new Vector2(100, 400), Color.Gold);
        }
    }
}
