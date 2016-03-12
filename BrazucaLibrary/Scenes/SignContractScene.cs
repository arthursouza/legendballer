using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrazucaLibrary.Characters;
using BrazucaLibrary.Input;
using BrazucaLibrary.UI;
using Microsoft.Xna.Framework;

namespace BrazucaLibrary.Scenes
{
    public class SignContractScene : Scene
    {
        Button signButton;
        Button rejectButton;
        MessageBox firstContractMessage;
        MessageBox contractOfferedMessage;
        Contract proposition;

        public SignContractScene(BrazucaGame game)
            : base(game)
        {
            var buttonSize = new Vector2(UserInterface.ButtonGreen.Width, UserInterface.ButtonGreen.Height/2);
            var buttonScreenPadding = 15;

            signButton = new Button("Sign", 
                new Vector2(BrazucaGame.WindowBounds.Width/2 - (buttonSize.X + buttonScreenPadding), BrazucaGame.WindowBounds.Height - (buttonSize.Y + buttonScreenPadding)), 
                Color.White, UserInterface.ButtonGreen);
            rejectButton = new Button("Reject",
                new Vector2(BrazucaGame.WindowBounds.Width / 2 + buttonScreenPadding, BrazucaGame.WindowBounds.Height - (buttonSize.Y + buttonScreenPadding)),
                Color.White, UserInterface.ButtonRed);

            firstContractMessage = new MessageBox("You must accept your first contract.", Color.Black, Fonts.Arial26, MessageBox.MessageBoxButtonType.Ok);
            firstContractMessage.Close += new MessageBox.CloseHandler(firstContractMessage_Close);
            firstContractMessage.Visible = false;

            Controls.Add(signButton);
            Controls.Add(rejectButton);

            contractOfferedMessage = new MessageBox("You have been offered a contract. This is your first professional chance. Make good use of it.", Color.Black, Fonts.Arial26, MessageBox.MessageBoxButtonType.Ok);
            contractOfferedMessage.Close += new MessageBox.CloseHandler(contractOfferedMessage_Close);
        }

        void contractOfferedMessage_Close()
        {
            contractOfferedMessage.Visible = false;
        }

        void firstContractMessage_Close()
        {
            firstContractMessage.Visible = false;
        }

        public override void Update(GameTime gameTime)
        {
            proposition = Game.ContractProposition;
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch batch)
        {
            batch.Draw(Graphics.BlankBackground, BrazucaGame.WindowBounds, Color.White);
            batch.DrawString(Fonts.Arial26, "Sign Contract", new Vector2(40, 40), Color.White);

            if (proposition != null)
            {
                string teamName = Game.ContractProposition.Club.Name;
                batch.DrawString(Fonts.Arial54, teamName, new Vector2(BrazucaGame.WindowBounds.Width / 2 - Fonts.Arial54.MeasureString(teamName).X / 2, 100), Color.White);
                batch.DrawString(Fonts.Arial26, string.Format("Club Rating {0} Popularity {1}", Game.ContractProposition.Club.Rating, Game.ContractProposition.Club.Popularity), new Vector2(40, 170), Color.White);
                batch.DrawString(Fonts.Arial26, "Salary $" + proposition.Value, new Vector2(40, 220), Color.White);
                batch.DrawString(Fonts.Arial26, "Victory Bonus $" + proposition.VictoryBonus, new Vector2(40, 270), Color.White);
                batch.DrawString(Fonts.Arial26, "Goal Bonus $" + proposition.GoalBonus, new Vector2(40, 320), Color.White);
            }

            signButton.Draw(batch);
            rejectButton.Draw(batch);

            if (firstContractMessage.Visible)
            {
                firstContractMessage.Draw(batch);
            }
            if (contractOfferedMessage.Visible)
            {
                contractOfferedMessage.Draw(batch);
            }
        }

        public override void MouseClick(MouseButton button)
        {
            if (firstContractMessage.Visible)
            {
                firstContractMessage.MouseClick(button);
                return;
            }
            else if (contractOfferedMessage.Visible)
            {
                contractOfferedMessage.MouseClick(button);
                return;
            }
            else
            {
                if (rejectButton.MouseOver())
                {
                    firstContractMessage.Visible = true;
                    return;
                }
                else if (signButton.MouseOver())
                {
                    Game.LatestNews = Game.ContractProposition.Club.Name + " bets on youth player";
                    Game.CurrentContract = Game.ContractProposition;
                    Game.Player.Contract = Game.CurrentContract;
                    Game.ContractProposition = null;
                    Game.Save();
                    Game.Transition(State.Newspaper);
                }
            }
        }
    }
}
