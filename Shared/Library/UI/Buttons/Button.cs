using Baller.Library.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Baller.Library
{
    public class Button : BaseButton    {
        public string Label;
        public Color TextColor;

        public Button(string label, Vector2 position, Color textColor, Texture2D texture, bool centralizeOrigin = false)
        {
            Label = label;
            Position = position;
            TextColor = textColor;
            BackgroundTexture = texture;
            base.centralize = centralizeOrigin;
        }

        public void Draw(SpriteBatch batch)
        {
            var sourceRect = new Rectangle(0, MouseOver() ? BackgroundTexture.Height / 2 : 0, BackgroundTexture.Width, BackgroundTexture.Height / 2);
            batch.Draw(BackgroundTexture, Bounds, sourceRect, Color.White);
            
            //var font = Fonts.Arial26;

            //Vector2 textPosition = new Vector2(
            //    Bounds.X + Bounds.Width/2 - font.MeasureString(Label).X/2,
            //    Bounds.Y + Bounds.Height / 2 - font.MeasureString(Label).Y / 2);
            
            //batch.DrawString(font, Label, textPosition, TextColor);
        }
    }
}
