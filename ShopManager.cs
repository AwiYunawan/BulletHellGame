using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace BulletHellGame
{
    public class ShopManager
    {
        private SpriteFont font;
        private bool isOpen = false;
        private int selectedIndex = 0;
        private List<string> items = new List<string> { "Extra Life (100)", "Double Bullet (150)", "Shield (200)" };

        public bool DoubleBulletPurchased { get; private set; } = false;
        public bool ShieldPurchased { get; set; } = false;

        public ShopManager(SpriteFont font)
        {
            this.font = font;
        }

        public bool IsOpen => isOpen;

        public void ToggleShop()
        {
            isOpen = !isOpen;
        }

        public void Update(GameTime gameTime, ref int score, Player player)
        {
            if (!isOpen) return;

            KeyboardState key = Keyboard.GetState();

            if (key.IsKeyDown(Keys.Up))
                selectedIndex = (selectedIndex - 1 + items.Count) % items.Count;
            if (key.IsKeyDown(Keys.Down))
                selectedIndex = (selectedIndex + 1) % items.Count;

            if (key.IsKeyDown(Keys.Enter))
            {
                switch (selectedIndex)
                {
                    case 0:
                        if (score >= 100)
                        {
                            player.AddLife();
                            score -= 100;
                        }
                        break;
                    case 1:
                        if (!DoubleBulletPurchased && score >= 150)
                        {
                            DoubleBulletPurchased = true;
                            score -= 150;
                        }
                        break;
                    case 2:
                        if (!ShieldPurchased && score >= 200)
                        {
                            ShieldPurchased = true;
                            player.ActivateShield();
                            score -= 200;
                        }
                        break;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!isOpen) return;

            spriteBatch.DrawString(font, "SHOP - Press Enter to Buy", new Vector2(100, 100), Color.Yellow);
            spriteBatch.DrawString(font, "Use Up/Down arrows to select", new Vector2(100, 120), Color.Gray);
            for (int i = 0; i < items.Count; i++)
            {
                Color color = (i == selectedIndex) ? Color.Cyan : Color.White;
                string itemText = items[i];
                
                // Add status indicators
                if (i == 1 && DoubleBulletPurchased)
                    itemText += " (PURCHASED)";
                if (i == 2 && ShieldPurchased)
                    itemText += " (PURCHASED)";
                
                spriteBatch.DrawString(font, itemText, new Vector2(120, 140 + i * 30), color);
            }
        }
    }
} 