using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrazucaLibrary.Input;
using BrazucaLibrary.UI;
using Microsoft.Xna.Framework;

namespace BrazucaLibrary.Scenes
{
    public class NewspaperChampionScene : Scene
    {
        Button continueButton;

        public NewspaperChampionScene(BrazucaGame game)
            : base(game)
        {
            var marginBottom = 80;
            var marginRight = 180;

            continueButton = new Button(
                "Continue", 
                new Vector2(BrazucaGame.WindowBounds.Width - marginRight, BrazucaGame.WindowBounds.Height - marginBottom), 
                Color.White,
                UserInterface.ButtonGreen);

            Controls.Add(continueButton);
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch batch)
        {
            Game.LatestNews = Game.PlayerClub.Name + ", CHAMPIONS!";
            batch.Draw(Graphics.NewspaperChampion, BrazucaGame.WindowBounds, Color.White);
            batch.DrawString(Fonts.TimesNewRoman26, Game.LatestNews, 
                new Vector2(
                    BrazucaGame.WindowBounds.Width / 2 - Fonts.TimesNewRoman26.MeasureString(Game.LatestNews).X / 2, 200), 
                Color.Black);
            continueButton.Draw(batch);
            base.Draw(batch);
        }

        public override void MouseClick(MouseButton button)
        {
            if (button == MouseButton.Left)
            {
                if (continueButton.MouseOver())
                {
                    Game.Transition(State.SeasonResults);
                    return;
                }
            }
            base.MouseClick(button);
        }
    }
}
