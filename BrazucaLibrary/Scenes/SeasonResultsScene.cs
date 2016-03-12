﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrazucaLibrary.Input;
using BrazucaLibrary.UI;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace BrazucaLibrary.Scenes
{
    public class SeasonResultsScene : Scene
    {
        private SpriteFont font;
        Button btnNextSeason;

        public SeasonResultsScene(BrazucaGame game)
            : base(game)
        {
            var marginBottom = 80;
            var marginRight = 180;

            font = Fonts.Arial20;
            
            btnNextSeason = new Button(
                "Next Season", 
                new Vector2(
                    BrazucaGame.WindowBounds.Width - marginRight, 
                    BrazucaGame.WindowBounds.Height - marginBottom), 
                Color.White, 
                UserInterface.ButtonGreen);
        }

        public override void Draw(SpriteBatch batch)
        {
            batch.Draw(Graphics.BlankBackground, BrazucaGame.WindowBounds, Color.White);
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

        public override void MouseClick(MouseButton button)
        {
            if (btnNextSeason.MouseOver())
            {
                Game.NextSeason();
                Game.Transition(State.Lobby);
                return;
            }
            base.MouseClick(button);
        }

    }
}
