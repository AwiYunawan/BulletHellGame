using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using BulletHellGame.Managers;

namespace BulletHellGame.UI
{
    public class Button
    {
        private Rectangle _bounds;
        private string _text;
        private SpriteFont _font;
        private Color _normalColor;
        private Color _hoverColor;
        private Color _pressedColor;
        private Color _currentColor;
        private bool _isHovered;
        private bool _isPressed;
        
        public event System.Action OnClick;
        
        public Button(Rectangle bounds, string text, SpriteFont font)
        {
            _bounds = bounds;
            _text = text;
            _font = font;
            _normalColor = Color.White;
            _hoverColor = Color.Yellow;
            _pressedColor = Color.Gray;
            _currentColor = _normalColor;
        }
        
        public void Update()
        {
            var mousePos = InputManager.Instance.GetMousePosition();
            var wasPressed = InputManager.Instance.IsLeftMousePressed();
            
            _isHovered = _bounds.Contains(mousePos.ToPoint());
            
            if (_isHovered && wasPressed)
            {
                _isPressed = true;
                _currentColor = _pressedColor;
            }
            else if (_isPressed && !wasPressed)
            {
                _isPressed = false;
                if (_isHovered)
                {
                    OnClick?.Invoke();
                    _currentColor = _hoverColor;
                }
                else
                {
                    _currentColor = _normalColor;
                }
            }
            else if (_isHovered && !_isPressed)
            {
                _currentColor = _hoverColor;
            }
            else
            {
                _currentColor = _normalColor;
            }
        }
        
        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw button background (simple rectangle for now)
            // spriteBatch.Draw(_texture, _bounds, _currentColor);
            
            // Draw button text
            var textSize = _font.MeasureString(_text);
            var textPosition = new Vector2(
                _bounds.X + (_bounds.Width - textSize.X) / 2,
                _bounds.Y + (_bounds.Height - textSize.Y) / 2
            );
            
            spriteBatch.DrawString(_font, _text, textPosition, _currentColor);
        }
        
        public void SetColors(Color normal, Color hover, Color pressed)
        {
            _normalColor = normal;
            _hoverColor = hover;
            _pressedColor = pressed;
        }
    }
}
