using Baller.Library.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

namespace Baller.Library.UI
{
    public class TextureButton : IControl, IButton
    {
        private bool centralize;
        public Texture2D BackgroundTexture { get; set; }
        public Texture2D Texture { get; set; }
        public Rectangle Bounds
        {
            get { return new Rectangle(centralize ? (int)Position.X - BackgroundTexture.Width / 2 : (int)Position.X, (int)Position.Y, BackgroundTexture.Width, BackgroundTexture.Height / 2); }
        }
        public Rectangle InnerBounds
        {
            get
            {
                return new Rectangle(
                    centralize ? (int)Position.X - Texture.Width / 2 : (int)Position.X + BackgroundTexture.Width / 2 - Texture.Width / 2,
                    (int)Position.Y + BackgroundTexture.Height / 4 - Texture.Height / 2,
                    Texture.Width,
                    Texture.Height);
            }
        }
        public Vector2 Position;
        public Color SelectedColor;

        public TextureButton(Texture2D backgroundTexture, Texture2D texture, Vector2 position, bool centralize = false)
        {
            this.centralize = centralize;
            this.BackgroundTexture = backgroundTexture;
            this.Texture = texture;
            Position = position;
        }

        public void Draw(SpriteBatch batch)
        {
            var sourceRect = new Rectangle(0, MouseOver() ? BackgroundTexture.Height / 2 : 0, BackgroundTexture.Width, BackgroundTexture.Height / 2);
            batch.Draw(BackgroundTexture, Bounds, sourceRect, Color.White);
            batch.Draw(Texture, InnerBounds, Color.White);
        }

        private bool MouseOver()
        {
            return Bounds.Contains((int)InputInfo.MousePosition.X, (int)InputInfo.MousePosition.Y);
        }

        public bool Pressed()
        {

#if ANDROID
            TouchCollection touchCollection = TouchPanel.GetState();

            if (touchCollection.Count > 0)
            {
                //Only Fire Select Once it's been released
                if (touchCollection[0].State == TouchLocationState.Moved ||
                    touchCollection[0].State == TouchLocationState.Pressed)
                {
                    return this.Bounds.Contains(touchCollection[0].Position);
                }
            }

            return false;
#endif

#if WINDOWS
            
            return MouseOver() && InputInfo.Clicked();

#endif
        }
    }
}
