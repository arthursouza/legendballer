using Baller.Library.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

namespace Baller.Library.UI
{
    public class BaseButton : IControl, IButton
    {
        protected bool centralize;
        public Texture2D BackgroundTexture { get; set; }
        public Rectangle Bounds
        {
            get { return new Rectangle(centralize ? (int)Position.X - BackgroundTexture.Width / 2 : (int)Position.X, (int)Position.Y, BackgroundTexture.Width, BackgroundTexture.Height / 2); }
        }
        public Vector2 Position;
        
        
        protected bool MouseOver()
        {
            return Bounds.Contains((int)InputInfo.MousePosition.X, (int)InputInfo.MousePosition.Y);
        }

        public bool Pressed()
        {

#if ANDROID
            var pos = InputInfo.GetTouchPosition();
            if (!pos.HasValue)
                return false;

            return this.Bounds.Contains(pos.Value);
#endif

#if WINDOWS
            
            return MouseOver() && InputInfo.Clicked();

#endif
        }
    }
}
