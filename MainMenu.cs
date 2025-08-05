using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace BulletHellGame
{
    public class MainMenu
    {
        private SpriteFont _font;
        private SpriteFont _titleFont;
        private List<MenuItem> _menuItems;
        private int _selectedIndex = 0;
        private KeyboardState _previousKeyboardState;
        private KeyboardState _currentKeyboardState;
        private float _selectionTimer = 0f;
        private const float SELECTION_DELAY = 0.15f; // Delay between menu selections
        
        // Menu states
        public enum MenuState
        {
            Main,
            Options,
            Credits
        }
        
        private MenuState _currentState = MenuState.Main;
        
        public bool IsActive { get; set; } = true;
        
        public MainMenu(SpriteFont font, SpriteFont titleFont)
        {
            _font = font;
            _titleFont = titleFont;
            InitializeMenuItems();
        }
        
        private void InitializeMenuItems()
        {
            _menuItems = new List<MenuItem>();
            UpdateMenuItems();
        }
        
        private void UpdateMenuItems()
        {
            _menuItems.Clear();
            
            switch (_currentState)
            {
                case MenuState.Main:
                    _menuItems.Add(new MenuItem("Start Game", () => IsActive = false));
                    _menuItems.Add(new MenuItem("Options", () => _currentState = MenuState.Options));
                    _menuItems.Add(new MenuItem("Credits", () => _currentState = MenuState.Credits));
                    _menuItems.Add(new MenuItem("Exit", () => System.Environment.Exit(0)));
                    break;
                    
                case MenuState.Options:
                    _menuItems.Add(new MenuItem("Back", () => _currentState = MenuState.Main));
                    break;
                    
                case MenuState.Credits:
                    _menuItems.Add(new MenuItem("Back", () => _currentState = MenuState.Main));
                    break;
            }
            
            _selectedIndex = 0;
        }
        
        public void Update(GameTime gameTime)
        {
            _previousKeyboardState = _currentKeyboardState;
            _currentKeyboardState = Keyboard.GetState();
            
            _selectionTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            // Handle menu navigation
            if (_selectionTimer >= SELECTION_DELAY)
            {
                if (_currentKeyboardState.IsKeyDown(Keys.Up) && _previousKeyboardState.IsKeyUp(Keys.Up))
                {
                    _selectedIndex = (_selectedIndex - 1 + _menuItems.Count) % _menuItems.Count;
                    _selectionTimer = 0f;
                }
                else if (_currentKeyboardState.IsKeyDown(Keys.Down) && _previousKeyboardState.IsKeyUp(Keys.Down))
                {
                    _selectedIndex = (_selectedIndex + 1) % _menuItems.Count;
                    _selectionTimer = 0f;
                }
                else if (_currentKeyboardState.IsKeyDown(Keys.Enter) && _previousKeyboardState.IsKeyUp(Keys.Enter))
                {
                    _menuItems[_selectedIndex].Action?.Invoke();
                    UpdateMenuItems();
                    _selectionTimer = 0f;
                }
                else if (_currentKeyboardState.IsKeyDown(Keys.Escape) && _previousKeyboardState.IsKeyUp(Keys.Escape))
                {
                    if (_currentState != MenuState.Main)
                    {
                        _currentState = MenuState.Main;
                        UpdateMenuItems();
                    }
                    _selectionTimer = 0f;
                }
            }
        }
        
        public void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            // Draw background
            graphicsDevice.Clear(Color.Black);
            
            // Draw title
            string title = "BULLET HELL GAME";
            Vector2 titleSize = _titleFont.MeasureString(title);
            Vector2 titlePosition = new Vector2(
                (graphicsDevice.Viewport.Width - titleSize.X) / 2,
                100
            );
            spriteBatch.DrawString(_titleFont, title, titlePosition, Color.Yellow);
            
            // Draw menu items
            float startY = 250;
            float itemSpacing = 60;
            
            for (int i = 0; i < _menuItems.Count; i++)
            {
                Color itemColor = (i == _selectedIndex) ? Color.White : Color.Gray;
                string itemText = _menuItems[i].Text;
                
                // Add selection indicator
                if (i == _selectedIndex)
                {
                    itemText = "> " + itemText + " <";
                }
                
                Vector2 itemSize = _font.MeasureString(itemText);
                Vector2 itemPosition = new Vector2(
                    (graphicsDevice.Viewport.Width - itemSize.X) / 2,
                    startY + (i * itemSpacing)
                );
                
                spriteBatch.DrawString(_font, itemText, itemPosition, itemColor);
            }
            
            // Draw additional content based on current state
            switch (_currentState)
            {
                case MenuState.Options:
                    DrawOptions(spriteBatch, graphicsDevice);
                    break;
                case MenuState.Credits:
                    DrawCredits(spriteBatch, graphicsDevice);
                    break;
            }
            
            // Draw instructions
            string instructions = "Use Arrow Keys to navigate, Enter to select, Escape to go back";
            Vector2 instructionsSize = _font.MeasureString(instructions);
            Vector2 instructionsPosition = new Vector2(
                (graphicsDevice.Viewport.Width - instructionsSize.X) / 2,
                graphicsDevice.Viewport.Height - 50
            );
            spriteBatch.DrawString(_font, instructions, instructionsPosition, Color.DarkGray);
        }
        
        private void DrawOptions(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            string optionsTitle = "OPTIONS";
            Vector2 optionsTitleSize = _font.MeasureString(optionsTitle);
            Vector2 optionsTitlePosition = new Vector2(
                (graphicsDevice.Viewport.Width - optionsTitleSize.X) / 2,
                180
            );
            spriteBatch.DrawString(_font, optionsTitle, optionsTitlePosition, Color.Cyan);
            
            // Add options content here
            string optionText = "Options menu coming soon...";
            Vector2 optionSize = _font.MeasureString(optionText);
            Vector2 optionPosition = new Vector2(
                (graphicsDevice.Viewport.Width - optionSize.X) / 2,
                350
            );
            spriteBatch.DrawString(_font, optionText, optionPosition, Color.White);
        }
        
        private void DrawCredits(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            string creditsTitle = "CREDITS";
            Vector2 creditsTitleSize = _font.MeasureString(creditsTitle);
            Vector2 creditsTitlePosition = new Vector2(
                (graphicsDevice.Viewport.Width - creditsTitleSize.X) / 2,
                180
            );
            spriteBatch.DrawString(_font, creditsTitle, creditsTitlePosition, Color.Cyan);
            
            // Add credits content
            string[] credits = {
                "Game Developer: Your Name",
                "Framework: MonoGame",
                "Language: C#",
                "Special Thanks: MonoGame Community"
            };
            
            float startY = 250;
            float spacing = 40;
            
            for (int i = 0; i < credits.Length; i++)
            {
                Vector2 creditSize = _font.MeasureString(credits[i]);
                Vector2 creditPosition = new Vector2(
                    (graphicsDevice.Viewport.Width - creditSize.X) / 2,
                    startY + (i * spacing)
                );
                spriteBatch.DrawString(_font, credits[i], creditPosition, Color.White);
            }
        }
    }
    
    public class MenuItem
    {
        public string Text { get; set; }
        public System.Action Action { get; set; }
        
        public MenuItem(string text, System.Action action)
        {
            Text = text;
            Action = action;
        }
    }
} 