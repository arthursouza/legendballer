using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Baller.Library.Input
{
    public class InputInfo
    {
        public static MouseState MouseState;
        public static MouseState LastMouseState;
        public static KeyboardState KeyboardState;
        public static KeyboardState LastKeyboardState;

        public static Vector2 MousePosition
        {
            get { return new Vector2(MouseState.X, MouseState.Y); }
        }

        public static Position MousePositionPoint
        {
            get { return new Position(MouseState.X, MouseState.Y); }
        }

        public static bool KeyPress(Keys keys)
        {
            return (LastKeyboardState.IsKeyUp(keys) && KeyboardState.IsKeyDown(keys));
        }
        
        public static bool Clicked()
        {
            #if WINDOWS

            return InputInfo.MouseState.LeftButton == ButtonState.Released && InputInfo.LastMouseState.LeftButton == ButtonState.Pressed;

            #endif

            return false;
        }
    }
}
