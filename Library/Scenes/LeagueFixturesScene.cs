using System.Collections.Generic;
using LegendBaller.Library.Input;
using LegendBaller.Library.Leagues;
using LegendBaller.Library.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LegendBaller.Library.Scenes
{
    public class LeagueFixturesScene : Scene
    {
        List<Match> matches;
        Button btnFixtureStandings;
        Button btnBack;
        int round;
        bool standings = true;
        SpriteFont font;

        Vector2 teamName;
        Vector2 points;
        Vector2 wins;
        Vector2 draws;
        Vector2 losses;

        Vector2 proGoals;
        Vector2 conGoals;
        Vector2 goalRes;

        public LeagueFixturesScene(BrazucaGame game)
            : base(game)
        {
            teamName = new Vector2(30, windowPadding);
            points = new Vector2(270, windowPadding);
            wins = new Vector2(340, windowPadding);
            draws = new Vector2(410, windowPadding);
            losses = new Vector2(480, windowPadding);

            proGoals = new Vector2(550, windowPadding);
            conGoals = new Vector2(620, windowPadding);
            goalRes = new Vector2(690, windowPadding);

            btnFixtureStandings = new Button("Fixtures", 
                new Vector2(
                    BrazucaGame.WindowBounds.Width - (windowPadding + UserInterface.ButtonGreen.Width), 
                    BrazucaGame.WindowBounds.Height - (windowPadding + UserInterface.ButtonGreen.Height/2)), 
                Color.White, 
                UserInterface.ButtonGreen);
            
            btnBack = new Button("Lobby", 
                new Vector2(
                    windowPadding,
                    BrazucaGame.WindowBounds.Height - (windowPadding + UserInterface.ButtonGreen.Height/2)),
                Color.White, 
                UserInterface.ButtonGreen);
            
            font = Fonts.Arial26;
            
            Controls.Add(btnBack);
            Controls.Add(btnFixtureStandings);
        }

        public override void Draw(SpriteBatch batch)
        {
            batch.Draw(Graphics.BlankBackground, BrazucaGame.WindowBounds, Color.White);
            Vector2 pos = new Vector2(20, 20);
            if (!standings)
            {
                if (matches != null)
                {
                    batch.DrawString(font, string.Format("Round {0} (Current {1})", round + 1, Game.CurrentLeagueRound + 1), pos, Color.White);
                    for (int i = 0; i < matches.Count; i++)
                    {
                        batch.DrawString(font, string.Format("{0} {1} x {2} {3}", matches[i].Home.Name, matches[i].ResultHome, matches[i].ResultAway, matches[i].Away.Name),
                            pos + new Vector2(0, 40 + font.LineSpacing * i), Color.White);
                    }
                }
            }
            else
            {
                batch.DrawString(font, "Club", teamName, Color.Yellow);
                batch.DrawString(font, "Pt", points, Color.Yellow);
                batch.DrawString(font, "W", wins, Color.Yellow);
                batch.DrawString(font, "D", draws, Color.Yellow);
                batch.DrawString(font, "L", losses, Color.Yellow);
                batch.DrawString(font, "PG", proGoals, Color.Yellow);
                batch.DrawString(font, "CG", conGoals, Color.Yellow);
                batch.DrawString(font, "SG", goalRes, Color.Yellow);

                for (int i = 0; i < Game.League.Standings.Count; i++)
                {
                    LeagueStanding s = Game.League.Standings[i];

                    var color = s.Club.Name == Game.PlayerClub.Name ? Color.OrangeRed : Color.White;

                    batch.DrawString(font, s.Club.Name, teamName + new Vector2(0, font.LineSpacing * (i + 1) + 15), color);
                    batch.DrawString(font, s.Points.ToString(), points + new Vector2(0, font.LineSpacing * (i + 1) + 15), color);
                    batch.DrawString(font, s.Wins.ToString(), wins + new Vector2(0, font.LineSpacing * (i + 1) + 15), color);
                    batch.DrawString(font, s.Draws.ToString(), draws + new Vector2(0, font.LineSpacing * (i + 1) + 15), color);
                    batch.DrawString(font, s.Losses.ToString(), losses + new Vector2(0, font.LineSpacing * (i + 1) + 15), color);
                    batch.DrawString(font, s.ProGoals.ToString(), proGoals + new Vector2(0, font.LineSpacing * (i + 1) + 15), color);
                    batch.DrawString(font, s.ConGoals.ToString(), conGoals + new Vector2(0, font.LineSpacing * (i + 1) + 15), color);
                    batch.DrawString(font, s.GoalScore.ToString(), goalRes + new Vector2(0, font.LineSpacing * (i + 1) + 15), color);
                }
            }

            btnFixtureStandings.Draw(batch);
            btnBack.Draw(batch);
        }

        public override void Update(GameTime gameTime)
        {
            Game.League.SortStandings();
            matches = Game.League.Matches.FindAll(x => x.Round == round);

            if (InputInfo.KeyPress(Keys.Left))
            {
                if (round > 0)
                    round--;
            }
            else if (InputInfo.KeyPress(Keys.Right))
            {
                if (round < 17)
                    round++;
            }
            base.Update(gameTime);
        }

        public override void MouseClick(MouseButton button)
        {
            if (btnBack.MouseOver())
            {
                Game.Transition(State.Lobby);
            }
            else if (btnFixtureStandings.MouseOver())
            {
                standings = !standings;

                if (standings)
                    btnFixtureStandings.Label = "Fixtures";
                else
                    btnFixtureStandings.Label = "Standings";
            }
            base.MouseClick(button);
        }
    }
}
