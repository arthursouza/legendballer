using Baller.Library.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

namespace Baller.Library.UI
{
    public class TextureButton : BaseButton
    {
        public Texture2D Texture { get; set; }
        
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
    }
}
