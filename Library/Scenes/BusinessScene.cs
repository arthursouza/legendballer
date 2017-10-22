using System.Globalization;
using LegendBaller.Library.Input;
using LegendBaller.Library.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LegendBaller.Library.Scenes
{
    public class BusinessScene : Scene
    {
        Button btnLobby;

        public SpriteFont textFont { get; set; }
        public SpriteFont teamNameFont { get; set; }

        public BusinessScene(BrazucaGame game)
            : base(game)
        {
            textFont = Fonts.Arial20;
            teamNameFont = Fonts.Arial26;

            btnLobby = new Button(
                "Lobby",
                new Vector2(
                    windowPadding, 
                    BrazucaGame.WindowBounds.Height - (windowPadding + UserInterface.ButtonGreen.Height/2)), 
                Color.White, 
                UserInterface.ButtonGreen);
            
            Controls.Add(btnLobby);
        }

        public override void Draw(SpriteBatch batch)
        {
            batch.Draw(Graphics.BlankBackground, BrazucaGame.WindowBounds, Color.White);

            var textPosition = new Vector2(windowPadding, windowPadding);

            var labelColor = Color.White;
            var valuePadding = 400;
            var valueColor = Color.Yellow;

            batch.DrawString(teamNameFont, Game.PlayerClub.Name, textPosition, Color.White);
            textPosition.Y += teamNameFont.LineSpacing + windowPadding;

            //{0} Popularity {1}" Game.Player.Contract.Club.Rating, Game.Player.Contract.Club.Popularity)

            batch.DrawString(textFont, "Club Rating", textPosition, labelColor);
            batch.DrawString(textFont, Game.PlayerClub.Rating.ToString(), new Vector2(textPosition.X + valuePadding - textFont.MeasureString(Game.PlayerClub.Rating.ToString()).X, textPosition.Y), valueColor);
            textPosition.Y += textFont.LineSpacing;

            batch.DrawString(textFont, "Club Popularity", textPosition, labelColor);
            batch.DrawString(textFont, Game.PlayerClub.Popularity.ToString(), new Vector2(textPosition.X + valuePadding - textFont.MeasureString(Game.PlayerClub.Popularity.ToString()).X, textPosition.Y), valueColor);
            textPosition.Y += textFont.LineSpacing;
            
            batch.DrawString(textFont, "Salary", textPosition, labelColor);
            batch.DrawString(textFont, Game.Player.Contract.Value.ToString("C", new CultureInfo("en-us")), new Vector2(textPosition.X + valuePadding - textFont.MeasureString(Game.Player.Contract.Value.ToString("C", new CultureInfo("en-us"))).X, textPosition.Y), valueColor);
            textPosition.Y += textFont.LineSpacing;

            batch.DrawString(textFont, "Victory Bonus", textPosition, labelColor);
            batch.DrawString(textFont, Game.Player.Contract.VictoryBonus.ToString("C", new CultureInfo("en-us")), new Vector2(textPosition.X + valuePadding - textFont.MeasureString(Game.Player.Contract.VictoryBonus.ToString("C", new CultureInfo("en-us"))).X, textPosition.Y), valueColor);
            textPosition.Y += textFont.LineSpacing;

            batch.DrawString(textFont, "Goal Bonus", textPosition, labelColor);
            batch.DrawString(textFont, Game.Player.Contract.GoalBonus.ToString("C", new CultureInfo("en-us")), new Vector2(textPosition.X + valuePadding - textFont.MeasureString(Game.Player.Contract.GoalBonus.ToString("C", new CultureInfo("en-us"))).X, textPosition.Y), valueColor);
            textPosition.Y += textFont.LineSpacing;

            batch.DrawString(textFont, "Current money", textPosition, labelColor);
            batch.DrawString(textFont, Game.Player.Money.ToString("C", new CultureInfo("en-us")), new Vector2(textPosition.X + valuePadding - textFont.MeasureString(Game.Player.Money.ToString("C", new CultureInfo("en-us"))).X, textPosition.Y), valueColor);

            btnLobby.Draw(batch);
            base.Draw(batch);
        }

        public override void MouseClick(MouseButton button)
        {
            if (btnLobby.MouseOver())
            {
                Game.Transition(State.Lobby);
            }
            base.MouseClick(button);
        }
    }
}
