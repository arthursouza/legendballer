﻿using System.Collections.Generic;
using Baller.Library.Input;
using Baller.Library.Menu;
using Baller.Library.UI;
using Microsoft.Xna.Framework;

namespace Baller.Library.Scenes
{
    public class LobbyScene : Scene
    {
        Tooltip nextGameTooltip;

        List<LobbyOption> options = new List<LobbyOption>();

        int selectedIndex;
        Vector2 lobbyButtonSize = new Vector2(160, 160);
        
        private TextureButton btnNextGame;

        public LobbyScene(BallerGame game)
            : base(game)
        {
            var lobbyOptions = new[]
            {
                new LobbyOption("League", Vector2.Zero, lobbyButtonSize, LobbyOptionFunction.League),
                new LobbyOption("Business", Vector2.Zero, lobbyButtonSize, LobbyOptionFunction.Business),
                new LobbyOption("My Stats", Vector2.Zero, lobbyButtonSize, LobbyOptionFunction.MyStats),
                new LobbyOption("Training", Vector2.Zero, lobbyButtonSize, LobbyOptionFunction.Training),
                new LobbyOption("Shop", Vector2.Zero, lobbyButtonSize, LobbyOptionFunction.Shop),
                new LobbyOption("News", Vector2.Zero, lobbyButtonSize, LobbyOptionFunction.RecentNews)
            };

            var buttonColumns = 3;

            var buttonSpacing = (BallerGame.WindowBounds.Width - (buttonColumns * lobbyButtonSize.X)) / (buttonColumns + 1);

            var buttonsStartPosition = BallerGame.WindowBounds.Height - (windowPadding + UserInterface.ButtonGreen.Height + windowPadding + (lobbyButtonSize.Y * 2));

            for (int i = 0; i < lobbyOptions.Length; i++)
            {
                var line = i / buttonColumns;
                var column = i % buttonColumns;

                var posX = buttonSpacing + (buttonSpacing * column) + (lobbyButtonSize.X * column);
                var posY = ((buttonSpacing + lobbyButtonSize.Y) * line) + buttonsStartPosition;

                lobbyOptions[i].UpdatePosition(new Vector2(posX, posY));
            }

            //for (int i = 0; i < 3; i++)
            //{
            //    lobbyOptions[i].UpdatePosition(new Vector2(
            //        buttonSpacing + (buttonSpacing * i) + (lobbyButtonSize.X * i),
            //        BallerGame.WindowBounds.Height - (windowPadding + UserInterface.ButtonGreen.Height/2 + windowPadding + lobbyButtonSize.Y)
            //        ));
            //}

            //for (int i = 3; i < 6; i++)
            //{
            //    var iAlt = i - 2;
            //    lobbyOptions[i].UpdatePosition(new Vector2(
            //        buttonSpacing + (buttonSpacing * iAlt) + (lobbyButtonSize.X * iAlt),
            //        BallerGame.WindowBounds.Height - ((windowPadding * 3) + (lobbyButtonSize.Y * 2) + UserInterface.ButtonGreen.Height / 2)
            //        ));
            //}

            btnNextGame = new TextureButton(
                UserInterface.ButtonGreen, 
                UserInterface.LabelNextGame, 
                new Vector2(BallerGame.WindowBounds.Width - (windowPadding + UserInterface.ButtonGreen.Width), BallerGame.WindowBounds.Height - (windowPadding + UserInterface.ButtonGreen.Height/2)));

            options.AddRange(lobbyOptions);
            Controls.AddRange(lobbyOptions);
            Controls.Add(btnNextGame);
            nextGameTooltip = new Tooltip(string.Empty, Position.Zero);
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch batch)
        {
            batch.Draw(Graphics.BlankBackground, BallerGame.WindowBounds, Color.White);
            batch.Draw(Graphics.LobbyBackground, BallerGame.WindowBounds, Color.White);

            var playerNamePos = new Vector2(windowPadding, windowPadding);
            var teamNamePos = new Vector2(windowPadding, 94);
            var agePos = new Vector2(windowPadding, 129);

            batch.DrawString(Fonts.Arial36, Game.Player.Name, playerNamePos, Color.White);
            batch.DrawString(Fonts.Arial18, Game.Player.Contract.Club.Name + " - Season "+ Game.Year, teamNamePos, Color.White);
            batch.DrawString(Fonts.Arial18, string.Format("Age {0}", Game.Player.Stats.Age, Game.Year), agePos, Color.White);
            batch.DrawString(Fonts.Arial26, Game.Player.Stats.Fame.ToString(), new Vector2(30, 160), Color.White);
            batch.DrawString(Fonts.Arial26, Game.Player.FameDescription(), new Vector2(30, 200), Color.White);
            
            for (int i = 0; i < options.Count; i++)
            {
                options[i].Draw(batch, i == selectedIndex);
            }

            //if (selectedIndex != -1)
            //{
            //    //batch.Draw(Graphics.Selected, options[selectedIndex], Color.White);
            //}

            btnNextGame.Draw(batch);
            base.Draw(batch);
        }

        public override void Update(GameTime gameTime)
        {
            selectedIndex = -1;
            for (int i = 0; i < options.Count; i++)
            {
                if (options[i].Bounds.Contains((int)InputInfo.MousePosition.X, (int)InputInfo.MousePosition.Y))
                {
                    selectedIndex = i;
                    break;
                }
            }

            base.Update(gameTime);
        }

        public override void MainInput(Vector2 pos)
        {
            if (selectedIndex != -1)
            {
                switch (options[selectedIndex].Option)
                {
                    case LobbyOptionFunction.Business:
                        Game.Transition(State.Business);
                        break;
                    case LobbyOptionFunction.League:
                        Game.Transition(State.LeagueFixtures);
                        break;
                    case LobbyOptionFunction.MyStats:
                        Game.Transition(State.PlayerStats);
                        break;
                    case LobbyOptionFunction.RecentNews:
                        break;
                    case LobbyOptionFunction.Shop:
                        break;
                    case LobbyOptionFunction.Training:
                        break;
                }
            }

            if (btnNextGame.Pressed())
            {
                NextGame();
            }
        }

        private void NextGame()
        {
            List<Match> roundGames = new List<Match>(); 
            roundGames = Game.League.Matches.FindAll(x => x.Round == Game.CurrentLeagueRound);
            Game.Simulation.Match = roundGames.Find(x => x.Home.Name == Game.PlayerClub.Name || x.Away.Name == Game.PlayerClub.Name);
            Game.Simulation.Start();
            Game.Transition(State.SimulationRoling);
            //Game.Simulation.Match = Game.League.Matches.Find(x => x.Round == Game.CurrentLeagueRound && (x.Home == Game.PlayerClub || x.Away == Game.PlayerClub));
            
        }
    }
}
