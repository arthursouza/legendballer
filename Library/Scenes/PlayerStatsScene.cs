using LegendBaller.Library.Input;
using LegendBaller.Library.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LegendBaller.Library.Scenes
{
    public class PlayerStatsScene : Scene
    {
        private SpriteFont textFont;
        private SpriteFont titleFont;

        Button btnLobby;
        private Vector2 textPosition;
        private Color labelColor;
        private int valuePadding;
        private Color valueColor;
        private SpriteBatch batch;

        public PlayerStatsScene(BrazucaGame game)
            : base(game)
        {
            textFont = Fonts.Arial20;
            titleFont = Fonts.Arial26;

            btnLobby = new Button(
                "Lobby",
                new Vector2(
                   windowPadding,
                    BrazucaGame.WindowBounds.Height - (windowPadding + UserInterface.ButtonGreen.Height / 2)),
                Color.White,
                UserInterface.ButtonGreen);

            labelColor = Color.White;
            valuePadding = 400;
            valueColor = Color.Yellow;

            Controls.Add(btnLobby);
        }

        public override void Draw(SpriteBatch batch)
        {
            textPosition = new Vector2(windowPadding, windowPadding);
            this.batch = batch;

            batch.Draw(Graphics.BlankBackground, BrazucaGame.WindowBounds, Color.White);

            batch.DrawString(titleFont, Game.Player.Name, textPosition, Color.White);
            textPosition.Y += titleFont.LineSpacing + windowPadding;

            DrawLabel("Club Rating");
            DrawValue(Game.PlayerClub.Rating.ToString());
            textPosition.Y += textFont.LineSpacing;

            DrawLabel("Age");
            DrawValue(Game.Player.Stats.Age.ToString());
            textPosition.Y += titleFont.LineSpacing;
            
            DrawLabel("Club");
            DrawValue(Game.Player.Contract.Club.Name);
            textPosition.Y += titleFont.LineSpacing ;

            DrawLabel("Fame");
            DrawValue(Game.Player.Stats.Fame.ToString() + " ("+Game.Player.FameDescription()+")");
            textPosition.Y += titleFont.LineSpacing ;
            
            DrawLabel("Power");
            DrawValue(Game.Player.Stats.Power.ToString());
            textPosition.Y += titleFont.LineSpacing ;

            DrawLabel("Technique");
            DrawValue(Game.Player.Stats.Technique.ToString());
            textPosition.Y += titleFont.LineSpacing ;

            DrawLabel("Career games");
            DrawValue(Game.Player.GamesPlayed.ToString());
            textPosition.Y += titleFont.LineSpacing ;

            DrawLabel("Career goals");
            DrawValue(Game.Player.Goals.ToString());
            textPosition.Y += titleFont.LineSpacing ;

            DrawLabel("Career assists");
            DrawValue(Game.Player.Assists.ToString());
            textPosition.Y += titleFont.LineSpacing;

            DrawLabel("Games (year)");
            DrawValue(Game.Player.GamesPlayedYear.ToString());
            textPosition.Y += titleFont.LineSpacing;

            DrawLabel("Goals (year)");
            DrawValue(Game.Player.GoalsYear.ToString());
            textPosition.Y += titleFont.LineSpacing;

            DrawLabel("Assists (year)");
            DrawValue(Game.Player.AssistsYear.ToString());
            textPosition.Y += titleFont.LineSpacing;
            
            btnLobby.Draw(batch);

            base.Draw(batch);
        }

        private void DrawValue(string value, SpriteFont font = null)
        {
            batch.DrawString(font ?? textFont, value, new Vector2(textPosition.X + valuePadding - textFont.MeasureString(value).X, textPosition.Y), valueColor);
        }

        private void DrawLabel(string label, SpriteFont font = null)
        {
            batch.DrawString(font ?? textFont, label, textPosition, labelColor);
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
