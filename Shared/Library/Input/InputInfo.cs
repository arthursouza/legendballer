using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace Baller.Library.Input
{
    public class InputInfo
    {
        public static MouseState MouseState;
        public static MouseState LastMouseState;
        public static KeyboardState KeyboardState;
        public static KeyboardState LastKeyboardState;

        public static Vector2? GetTouchPosition()
        {
            TouchCollection touchCollection = TouchPanel.GetState();

            if (touchCollection.Count > 0)
            {
                //Only Fire Select Once it's been released
                if (touchCollection[0].State == TouchLocationState.Moved ||
                    touchCollection[0].State == TouchLocationState.Pressed)
                {
                    return touchCollection[0].Position / BallerGame.Scale;
                }
            }

            return null;
        }

        public static Vector2 MousePosition
        {
            get { return new Vector2(MouseState.X / BallerGame.Scale, MouseState.Y / BallerGame.Scale); }
        }

        public static Position MousePositionPoint
        {
            get { return new Position((int)(MouseState.X / BallerGame.Scale),(int) (MouseState.Y / BallerGame.Scale)); }
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
