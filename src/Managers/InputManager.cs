using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace BulletHellGame.Managers
{
    public class InputManager
    {
        private static InputManager _instance;
        public static InputManager Instance
        {
            get
            {
                _instance ??= new InputManager();
                return _instance;
            }
        }

        private KeyboardState _currentKeyboardState;
        private KeyboardState _previousKeyboardState;
        private MouseState _currentMouseState;
        private MouseState _previousMouseState;

        public void Update()
        {
            _previousKeyboardState = _currentKeyboardState;
            _currentKeyboardState = Keyboard.GetState();
            
            _previousMouseState = _currentMouseState;
            _currentMouseState = Mouse.GetState();
        }

        public bool IsKeyPressed(Keys key)
        {
            return _currentKeyboardState.IsKeyDown(key) && _previousKeyboardState.IsKeyUp(key);
        }

        public bool IsKeyHeld(Keys key)
        {
            return _currentKeyboardState.IsKeyDown(key);
        }

        public bool IsKeyReleased(Keys key)
        {
            return _currentKeyboardState.IsKeyUp(key) && _previousKeyboardState.IsKeyDown(key);
        }

        public bool IsLeftMousePressed()
        {
            return _currentMouseState.LeftButton == ButtonState.Pressed && 
                   _previousMouseState.LeftButton == ButtonState.Released;
        }

        public bool IsLeftMouseHeld()
        {
            return _currentMouseState.LeftButton == ButtonState.Pressed;
        }

        public Vector2 GetMousePosition()
        {
            return _currentMouseState.Position.ToVector2();
        }

        public Vector2 GetMouseDelta()
        {
            return (_currentMouseState.Position - _previousMouseState.Position).ToVector2();
        }
    }
}
