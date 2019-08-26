using LegendBaller.Library.Input;
using LegendBaller.Library.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LegendBaller.Library.Menu
{
    public enum LobbyOptionFunction
    {
        NextGame,
        Training,
        Shop,
        MyStats,
        RecentNews,
        League,
        Business
    }

    public class LobbyOption : IControl, IButton
    {
        public LobbyOption(string text, Vector2 position, Vector2 buttonSize, LobbyOptionFunction option)
        {
            Position = position;
            Text = text;
            Option = option;

            Bounds = new Rectangle((int) Position.X,  (int) Position.Y,  (int) buttonSize.X, (int) buttonSize.Y);
        }

        public LobbyOptionFunction Option { get; set; }
        public Rectangle Bounds { get; set; }

        public void UpdatePosition(Vector2 position)
        {
            Position = position;

            Bounds = new Rectangle((int)Position.X, (int)Position.Y, Bounds.Width, Bounds.Height);
        }

        public string Text
        {
            get; set; 
        }
        public Vector2 Position { get; set; }
        public bool MouseOver()
        {
            return Bounds.Contains((int)InputInfo.MousePosition.X, (int)InputInfo.MousePosition.Y);
        }

        internal void Draw(SpriteBatch batch, bool selected = false)
        {
            //var color = selected ? Color.LightBlue : Color.White;

            var sourceRect = new Rectangle(0, selected ? UserInterface.LobbyButton.Height / 2 : 0, UserInterface.LobbyButton.Width, UserInterface.LobbyButton.Height / 2);

            batch.Draw(UserInterface.LobbyButton, Bounds, sourceRect, Color.White);

            var font = Fonts.Arial26;
            var textSize = font.MeasureString(Text);

            batch.DrawString(
                font, 
                Text,
                Position + new Vector2(Bounds.Width / 2 - textSize.X / 2, Bounds.Height / 2 - textSize.Y / 2), 
                Color.White);
        }
    }
}
