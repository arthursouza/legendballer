using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Baller.Library.UI
{
    public class UserInterface
    {
        public static Texture2D ButtonGreen { get; set; }
        public static Texture2D ButtonBlue { get; set; }
        public static Texture2D ButtonRed { get; set; }

        public static Texture2D MainNewCareer { get; set; }
        public static Texture2D MainLoadCareer { get; set; }

        public static Texture2D MessageBox { get; set; }
        public static Texture2D Cursor { get; set; }
        public static Texture2D ClickCursor { get; set; }
        public static Texture2D LobbyButton { get; set; }

        public static Texture2D LabelNextGame { get; set; }

        public static Rectangle DefaultButtonSize
        {
            get { return UserInterface.ButtonGreen?.Bounds ?? new Rectangle(); }
        }

        public static Vector2 BottomRightPosition
        {
            get => new Vector2(
                BallerGame.NativeResolution.Width - (DefaultButtonSize.Width + 64),
                BallerGame.NativeResolution.Height - (DefaultButtonSize.Height/2 + 64));
        }
        
        public static Vector2 BottomCenterRightPosition
        {
            get =>
                new Vector2(
                    BallerGame.NativeResolution.Width /2 + 32,
                    BallerGame.NativeResolution.Height - (DefaultButtonSize.Height/2 + 64));
        }

        public static Vector2 BottomCenterLeftPosition
        {
            get =>
                new Vector2(
                    BallerGame.NativeResolution.Width /2 - (DefaultButtonSize.Width + 32),
                    BallerGame.NativeResolution.Height - (DefaultButtonSize.Height/2 + 64));
        } 
    }
}
