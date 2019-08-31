using Baller.Library.Characters;
using Baller.Library.Input;
using Baller.Library.UI;
using Microsoft.Xna.Framework;

namespace Baller.Library.Scenes
{
    public class SignContractScene : Scene
    {
        TextureButton signButton;
        TextureButton rejectButton;
        MessageBox firstContractMessage;
        MessageBox contractOfferedMessage;
        Contract proposition;

        public SignContractScene(BallerGame game)
            : base(game)
        {
            var buttonSize = new Vector2(UserInterface.ButtonGreen.Width, UserInterface.ButtonGreen.Height/2);
            var buttonScreenPadding = 15;

            signButton = new TextureButton(UserInterface.ButtonGreen, "Sign",
                new Vector2(BallerGame.WindowBounds.Width/2 - (buttonSize.X + buttonScreenPadding), BallerGame.WindowBounds.Height - (buttonSize.Y + buttonScreenPadding)));
            
            rejectButton = new TextureButton(UserInterface.ButtonRed,"Reject",
                new Vector2(BallerGame.WindowBounds.Width / 2 + buttonScreenPadding, BallerGame.WindowBounds.Height - (buttonSize.Y + buttonScreenPadding)));

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
            batch.Draw(Graphics.BlankBackground, BallerGame.WindowBounds, Color.White);
            batch.DrawString(Fonts.Arial26, "Sign Contract", new Vector2(40, 40), Color.White);

            if (proposition != null)
            {
                string teamName = Game.ContractProposition.Club.Name;
                batch.DrawString(Fonts.Arial54, teamName, new Vector2(BallerGame.WindowBounds.Width / 2 - Fonts.Arial54.MeasureString(teamName).X / 2, 90), Color.White);
                batch.DrawString(Fonts.Arial26, string.Format("Club Rating {0} Popularity {1}", Game.ContractProposition.Club.Rating, Game.ContractProposition.Club.Popularity), new Vector2(40, 180), Color.White);
                batch.DrawString(Fonts.Arial26, "Salary $" + proposition.Value, new Vector2(40, 230), Color.White);
                batch.DrawString(Fonts.Arial26, "Victory Bonus $" + proposition.VictoryBonus, new Vector2(40, 280), Color.White);
                batch.DrawString(Fonts.Arial26, "Goal Bonus $" + proposition.GoalBonus, new Vector2(40, 330), Color.White);
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

        public override void MainInput(Vector2 pos)
        {
            if (firstContractMessage.Visible)
            {
                firstContractMessage.MainInput();
                return;
            }
            else if (contractOfferedMessage.Visible)
            {
                contractOfferedMessage.MainInput();
                return;
            }
            else
            {
                if (rejectButton.Pressed())
                {
                    firstContractMessage.Visible = true;
                    return;
                }
                else if (signButton.Pressed())
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
