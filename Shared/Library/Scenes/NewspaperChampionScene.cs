using Baller.Library.Input;
using Baller.Library.UI;
using Microsoft.Xna.Framework;

namespace Baller.Library.Scenes
{
    public class NewspaperChampionScene : Scene
    {
        Button continueButton;

        public NewspaperChampionScene(BallerGame game)
            : base(game)
        {
            var marginBottom = 80;
            var marginRight = 180;

            continueButton = new Button(
                "Continue", 
                new Vector2(BallerGame.WindowBounds.Width - marginRight, BallerGame.WindowBounds.Height - marginBottom), 
                Color.White,
                UserInterface.ButtonGreen);

            Controls.Add(continueButton);
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch batch)
        {
            Game.LatestNews = Game.PlayerClub.Name + ", CHAMPIONS!";
            batch.Draw(Graphics.NewspaperChampion, BallerGame.WindowBounds, Color.White);
            batch.DrawString(Fonts.TimesNewRoman26, Game.LatestNews, 
                new Vector2(
                    BallerGame.WindowBounds.Width / 2 - Fonts.TimesNewRoman26.MeasureString(Game.LatestNews).X / 2, 200), 
                Color.Black);
            continueButton.Draw(batch);
            base.Draw(batch);
        }

        public override void MainInput(Vector2 pos)
        {
            if (continueButton.Pressed())
            {
                Game.Transition(State.SeasonResults);
                return;
            }

            base.MainInput(pos);
        }
    }
}
