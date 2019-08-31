using Baller.Library.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

namespace Baller.Library.UI
{
    public class TextureButton : BaseButton
    {
        public string Text { get; set; }
        public Texture2D LabelTexture { get; set; }
        
        SpriteFont font = Fonts.Arial26;

        public Rectangle InnerBounds
        {
            get
            {
                return new Rectangle(
                    centralize ? (int)Position.X - LabelTexture.Width / 2 : (int)Position.X + BackgroundTexture.Width / 2 - LabelTexture.Width / 2,
                    (int)Position.Y + BackgroundTexture.Height / 4 - LabelTexture.Height / 2,
                    LabelTexture.Width,
                    LabelTexture.Height);

            }
        }
        
        public TextureButton(Texture2D backgroundTexture, string text, Vector2 position, bool centralize = false)
        {
            this.centralize = centralize;
            this.BackgroundTexture = backgroundTexture;
            this.Text = text;
            Position = position;
        }

        public TextureButton(Texture2D backgroundTexture, Texture2D texture, Vector2 position, bool centralize = false)
        {
            this.centralize = centralize;
            this.BackgroundTexture = backgroundTexture;
            this.LabelTexture = texture;
            Position = position;
        }

        public void Draw(SpriteBatch batch)
        {
            var sourceRect = new Rectangle(0, MouseOver() ? BackgroundTexture.Height / 2 : 0, BackgroundTexture.Width, BackgroundTexture.Height / 2);

            batch.Draw(BackgroundTexture, Bounds, sourceRect, Color.White);
            
            if(LabelTexture != null)
                batch.Draw(LabelTexture, InnerBounds, Color.White);
            else
            {
                var font = Fonts.Arial36;

                var textPosition = new Vector2(
                    Bounds.X + Bounds.Width/2 - font.MeasureString(Text).X/2,
                    Bounds.Y + Bounds.Height / 2 - font.MeasureString(Text).Y / 2);
            
                batch.DrawString(font, Text, textPosition, Color.White);
            }
        }
    }
}
