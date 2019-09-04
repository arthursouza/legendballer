using Baller.Library.Input;
using Baller.Library.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Baller.Library.Scenes
{
    public class SeasonResultsScene : Scene
    {
        private SpriteFont font;
        TextureButton btnNextSeason;

        public SeasonResultsScene(BallerGame game)
            : base(game)
        {
            var marginBottom = 80;
            var marginRight = 180;

            font = Fonts.Arial20;
            
            btnNextSeason = new TextureButton(
                UserInterface.ButtonGreen,
                "Next Season", 
                new Vector2(
                    BallerGame.WindowBounds.Width - marginRight, 
                    BallerGame.WindowBounds.Height - marginBottom));
        }

        public override void Draw(SpriteBatch batch)
        {
            batch.Draw(Graphics.BlankBackground, BallerGame.WindowBounds, Color.White);
            int posY = 50;
            int line = 40;

            batch.DrawString(Fonts.Arial26, "Season Results", new Vector2(30, 30), Color.White); posY += line;

            if (Game.CurrentLeagueChampion)
            {
                batch.DrawString(font, "Champion of the " + Game.Year + " Braz League", new Vector2(30, posY), Color.White); posY += line;
            }
            else
            {
                batch.DrawString(font, "Braz League Position: " + Game.LeaguePosition, new Vector2(30, posY), Color.White); posY += line;
            }       

            batch.DrawString(font, "Club " + Game.Player.Contract.Club.Name, new Vector2(30, posY), Color.White); posY += line;
            batch.DrawString(font, "Games (year) " + Game.Player.GamesPlayedYear, new Vector2(30, posY), Color.White); posY += line;
            batch.DrawString(font, "Goals (year) " + Game.Player.GoalsYear, new Vector2(30, posY), Color.White); posY += line;
            batch.DrawString(font, "Assists (year) " + Game.Player.AssistsYear, new Vector2(30, posY), Color.White); posY += line;
                        
            btnNextSeason.Draw(batch);
            base.Draw(batch);
        }

        public override void MainInput(Vector2 pos)
        {
            if (btnNextSeason.Pressed())
            {
                Game.NextSeason();
                Game.Transition(State.Lobby);
                return;
            }
            base.MainInput(pos);
        }

    }
}
