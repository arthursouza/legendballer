using Baller.Library.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

namespace Baller.Library
{
    public class Button : IControl, IButton
    {
        public Texture2D Texture { get; set; }
        
        public bool CentralizeOrigin { get; set; }

        public Rectangle Bounds
        {
            get { return new Rectangle(CentralizeOrigin ? (int)Position.X - Texture.Width / 2 : (int)Position.X, (int)Position.Y, Texture.Width, Texture.Height / 2); }
        }

        public Vector2 Position;
        public string Label;
        public Color TextColor;

        public Button(string label, Vector2 position, Color textColor, Texture2D texture, bool centralizeOrigin = false)
        {
            Label = label;
            Position = position;
            TextColor = textColor;
            Texture = texture;
            CentralizeOrigin = centralizeOrigin;
        }

        public void Draw(SpriteBatch batch)
        {
            var font = Fonts.Arial26;

            Vector2 textPosition = new Vector2(
                Bounds.X + Bounds.Width/2 - font.MeasureString(Label).X/2,
                Bounds.Y + Bounds.Height / 2 - font.MeasureString(Label).Y / 2);
            
            var sourceRect = new Rectangle(0, MouseOver() ? Texture.Height / 2 : 0, Texture.Width, Texture.Height / 2);
            
            batch.Draw(Texture, Bounds, sourceRect, Color.White);
            batch.DrawString(font, Label, textPosition, TextColor);
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
