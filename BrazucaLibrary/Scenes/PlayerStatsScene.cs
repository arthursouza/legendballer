using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrazucaLibrary.Input;
using BrazucaLibrary.Leagues;
using BrazucaLibrary.UI;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace BrazucaLibrary.Scenes
{
    public class PlayerStatsScene : Scene
    {
        private SpriteFont font;
        Button btnLobby;

        public PlayerStatsScene(BrazucaGame game)
            : base(game)
        {
            font = Fonts.Arial20;
            btnLobby = new Button(
                "Lobby", 
                new Vector2(
                    windowPadding, 
                    BrazucaGame.WindowBounds.Height - windowPadding), 
                Color.White, 
                UserInterface.ButtonGreen);

            Controls.Add(btnLobby);
        }

        public override void Draw(SpriteBatch batch)
        {
            batch.Draw(Graphics.BlankBackground, BrazucaGame.WindowBounds, Color.White);
            int posY = 50;
            int line = 40;

            batch.DrawString(font, Game.Player.Name, new Vector2(30, posY), Color.White); posY += line;
            batch.DrawString(font, "Age " + Game.Player.Stats.Age, new Vector2(30, posY), Color.White); posY += line;
            batch.DrawString(font, "Club " + Game.Player.Contract.Club.Name, new Vector2(30, posY), Color.White); posY += line;
            batch.DrawString(font, "Fame " + Game.Player.Stats.Fame + " (" + Game.Player.FameDescription() + ")", new Vector2(30, posY), Color.White); posY += line;

            batch.DrawString(font, "Power " + Game.Player.Stats.Power, new Vector2(30, posY), Color.White); posY += line;
            batch.DrawString(font, "Technique " + Game.Player.Stats.Technique, new Vector2(30, posY), Color.White); posY += line;
            posY += line;
            batch.DrawString(font, "Career games " + Game.Player.GamesPlayed, new Vector2(30, posY), Color.White); posY += line;
            batch.DrawString(font, "Career goals " + Game.Player.Goals, new Vector2(30, posY), Color.White); posY += line;
            batch.DrawString(font, "Career assists " + Game.Player.Assists, new Vector2(30, posY), Color.White); posY += line;
            
            batch.DrawString(font, "Games (year) " + Game.Player.GamesPlayedYear, new Vector2(30, posY), Color.White); posY += line;
            batch.DrawString(font, "Goals (year) " + Game.Player.GoalsYear, new Vector2(30, posY), Color.White); posY += line;
            batch.DrawString(font, "Assists (year) " + Game.Player.AssistsYear, new Vector2(30, posY), Color.White); posY += line;

            if (Game.Player.Titles.FindAll(x => x.League == LeagueType.BrazLeague).Count > 0)
                batch.DrawString(font, Game.Player.Titles.FindAll(x => x.League == LeagueType.BrazLeague).Count + "x Braz League Champion", new Vector2(30, posY), Color.White); posY += line;

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
