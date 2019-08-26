using LegendBaller.Library.Input;
using LegendBaller.Library.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LegendBaller.Library.UI
{
    public class Tooltip
    {
        public Size Size;
        public Position Position;
        public string Text;
        public float Delay;

        int padding = 5;

        public Tooltip(string text, Position position)
        {
            Text = text;
            Position = position;
            Delay = 600;
        }

        public void Update(GameTime gameTime)
        {
            Position = new Util.Position(InputInfo.MousePositionPoint.X + 20, InputInfo.MousePositionPoint.Y + 20);
        }

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(Graphics.Selected, new Rectangle(Position.X, Position.Y, (int) Fonts.Arial12.MeasureString(Text + padding * 2).X, Fonts.Arial12.LineSpacing + padding), Color.White * .5f);
            batch.DrawString(Fonts.Arial12, Text, new Vector2(Position.X + padding, Position.Y), Color.White);
        }
    }
}
