using Baller.Library.Input;
using Baller.Library.Simulation;
using Baller.Library.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Baller.Library.Scenes
{
    public class SimulationScene : Scene
    {
        Button btnKickOff;

        private int currentEventIndex = -1;
        private float eventDelay = 1000;
        private float eventTimer = 0;

        public SimulationScene(BallerGame game)
            : base(game)
        {
            this.Game = game;

            btnKickOff = new Button(
                "Kick Off", 
                new Vector2(BallerGame.WindowBounds.Width - (UserInterface.ButtonGreen.Width + windowPadding), BallerGame.WindowBounds.Height - (windowPadding + UserInterface.ButtonGreen.Height/2)),
                Color.White, 
                UserInterface.ButtonGreen);
        }

        public override void Draw(SpriteBatch batch)
        {
            batch.Draw(Graphics.SimulationBG, BallerGame.WindowBounds, Color.White);

            if (Game.Simulation.Period == MatchPeriod.FirstTime || Game.Simulation.Period == MatchPeriod.SecondTime)
            {
                DrawGameEvents(batch);
            }
            else if (Game.Simulation.Period == MatchPeriod.HalfTime || Game.Simulation.Period == MatchPeriod.EndGame)
            {
                SpriteFont font = Fonts.Arial20;
                int posY = 120;
                batch.DrawString(font, "Passes made " + Game.Simulation.GameStatistics.PlayerEasyPasses, new Vector2(20, posY), Color.White); posY += font.LineSpacing;
                batch.DrawString(font, "Lost balls " + Game.Simulation.GameStatistics.PlayerLostBalls, new Vector2(20, posY), Color.White); posY += font.LineSpacing;
                batch.DrawString(font, "Goals " + Game.Simulation.GameStatistics.PlayerGoals, new Vector2(20, posY), Color.White); posY += font.LineSpacing;
                batch.DrawString(font, "Assists " + Game.Simulation.GameStatistics.PlayerAssists, new Vector2(20, posY), Color.White); posY += font.LineSpacing;
                btnKickOff.Draw(batch);
            }
            else if (Game.Simulation.Period == MatchPeriod.BeforeGame)
            {
                btnKickOff.Draw(batch);
            }

            string text = Game.Simulation.CurrentTime.ToString("00");

            batch.DrawString(Fonts.Arial54, text, new Vector2(BallerGame.WindowBounds.Width - (windowPadding + Fonts.Arial54.MeasureString(text).X), windowPadding), Color.White);

            var flagSize = new Vector2(64, 32);

            Color awayColor = Game.Simulation.Match.Home.MainColor == Game.Simulation.Match.Away.MainColor ? Game.Simulation.Match.Away.SecondColor : Game.Simulation.Match.Away.MainColor;

            string scoreHome = string.Format("{0} {1}", Game.Simulation.HomeScore, Game.Simulation.Match.Home.Name);
            string scoreAway = string.Format("{0} {1}", Game.Simulation.AwayScore, Game.Simulation.Match.Away.Name);

            Texture2D homeFlag = Game.Simulation.Match.Home.Uniform == Uniform.Flat ? Graphics.FlatFlag : Graphics.FlagStripe;
            Texture2D awayFlag = Game.Simulation.Match.Away.Uniform == Uniform.Flat ? Graphics.FlatFlag : Graphics.FlagStripe;

            Vector2 homePosition = new Vector2(windowPadding, windowPadding);
            Vector2 awayPosition = new Vector2(windowPadding, (int)(windowPadding + flagSize.Y + 10));
            
            batch.DrawString(Fonts.Arial26, scoreHome, new Vector2(homePosition.X + 10 + flagSize.X, homePosition.Y), Color.White);
            batch.DrawString(Fonts.Arial26, scoreAway, new Vector2(awayPosition.X + 10 + flagSize.X, awayPosition.Y), Color.White);

            batch.Draw(homeFlag, new Rectangle((int) homePosition.X, (int)homePosition.Y, (int)flagSize.X, (int)flagSize.Y), Game.Simulation.Match.Home.MainColor);
            batch.Draw(awayFlag, new Rectangle((int)awayPosition.X, (int)awayPosition.Y, (int)flagSize.X, (int)flagSize.Y), awayColor);
        }

        private void DrawGameEvents(SpriteBatch batch)
        {
            float height = 80;
            SpriteFont font = Fonts.Arial20;

            if (Game.Simulation.GameEvents.Count > 0 && currentEventIndex > -1)
            {
                // First index = start at the last item
                var firstIndex = Game.Simulation.GameEvents.Count - 1;//currentEventIndex;
                var gameEvent = Game.Simulation.GameEvents[firstIndex];
                
                batch.DrawString(font, "> " + gameEvent, new Vector2(20, 50 + height), Color.White);
                height += font.LineSpacing;

                // Starts from second item
                for (int i = firstIndex - 1; i >= 0; i--)
                {
                    gameEvent = Game.Simulation.GameEvents[i];
                    batch.DrawString(font, gameEvent, new Vector2(20, 50 + height), Color.White);
                    height += font.LineSpacing;
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            Game.Simulation.Update(gameTime);

            if (Game.Simulation.Period == MatchPeriod.HalfTime || Game.Simulation.Period == MatchPeriod.BeforeGame)
                btnKickOff.Label = "Kick Off";

            if (Game.Simulation.Period == MatchPeriod.EndGame)
                btnKickOff.Label = "Lobby";

            UpdateGameEvents(gameTime);
        }

        private void UpdateGameEvents(GameTime gameTime)
        {
            eventTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (eventTimer > eventDelay)
            {
                currentEventIndex++;
                eventTimer = 0;
            }
        }

        public override void MainInput(Vector2 pos)
        {
            if (btnKickOff.Pressed())
            {
                if (Game.Simulation.Period == MatchPeriod.BeforeGame)
                {
                    Game.Simulation.Period = MatchPeriod.FirstTime;
                }
                if (Game.Simulation.Period == MatchPeriod.HalfTime)
                {
                    Game.Simulation.Period = MatchPeriod.SecondTime;
                }
                if (Game.Simulation.Period == MatchPeriod.EndGame)
                {
                    currentEventIndex = -1;
                    Game.Transition(State.Lobby);
                    Game.EndRound();
                    Game.Save();
                    Game.Simulation.Period = MatchPeriod.BeforeGame;
                }
            }
        }
    }
}