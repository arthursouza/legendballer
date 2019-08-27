using Baller.Library.Input;
using Baller.Library.UI;
using Microsoft.Xna.Framework;

namespace Baller.Library.Scenes
{
    public class NewspaperScene : Scene
    {
        Button continueButton;

        public NewspaperScene(BallerGame game)
            : base(game)
        {
            var marginBottom = 15 + UserInterface.ButtonGreen.Height/2;
            var marginRight = 15 + UserInterface.ButtonGreen.Width;

            continueButton = new Button(
                "Continue", 
                new Vector2(BallerGame.WindowBounds.Width - marginRight, BallerGame.WindowBounds.Height - marginBottom),
                Color.White,
                UserInterface.ButtonGreen);

            Controls.Add(continueButton);
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch batch)
        {
            batch.Draw(Graphics.NewspaperBackground, BallerGame.WindowBounds, Color.White);

            var newspaperPosition = new Vector2(
                (BallerGame.WindowBounds.Width - Graphics.NewspaperSignContract.Width)/2,
                (BallerGame.WindowBounds.Height - Graphics.NewspaperSignContract.Height)/2);

            batch.Draw(
                Graphics.NewspaperSignContract, 
                newspaperPosition, 
                Color.White);

            batch.DrawString(Fonts.TimesNewRoman26, 
                Game.LatestNews, 
                new Vector2(
                    BallerGame.WindowBounds.Width / 2 - Fonts.TimesNewRoman26.MeasureString(Game.LatestNews).X / 2, 
                    newspaperPosition.Y + 105), 
                Color.Black);

            continueButton.Draw(batch);

            base.Draw(batch);
        }

        public override void MainInput(Vector2 pos)
        {
            if (continueButton.Pressed())
            {
                Game.Save();
                Game.Transition(State.Lobby);
                return;
            }

            base.MainInput(pos);
        }
    }
}
