using Baller.Library.Input;
using Baller.Library.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Baller.Library.Scenes
{
    public class NewCareerScene : Scene
    {
        TextureButton acceptButton;
        Textbox txtName;
        MessageBox alertEmptyName;

        Vector2 nameLabelPos = new Vector2(40, 330);
        Vector2 nationalityLabelPos = new Vector2(40, 315);

        public NewCareerScene(BallerGame game) : base(game)
        {
            acceptButton = new TextureButton(UserInterface.ButtonGreen,
                "Go Ahead",
                new Vector2(
                    BallerGame.NativeResolution.Width - (UserInterface.ButtonGreen.Width + 64),
                    BallerGame.NativeResolution.Height - (UserInterface.ButtonGreen.Height/2 + 64))
                );

            txtName = new Textbox(nameLabelPos + new Vector2(0, Fonts.Arial36.MeasureString("Name").Y + 15));
            txtName.Text = "Brazuca";

            alertEmptyName = new MessageBox();
            alertEmptyName.Text = "You must enter a name.";
            alertEmptyName.Close += new MessageBox.CloseHandler(alertEmptyName_Close);

            Controls.Add(acceptButton);
        }

        void alertEmptyName_Close()
        {
            alertEmptyName.Visible = false;
        }

        public override void Draw(SpriteBatch batch)
        {
            batch.Draw(Graphics.BlankBackground, BallerGame.WindowBounds, Color.White);
            txtName.Draw(batch);
            
            batch.DrawString(Fonts.Arial54, "A new career", new Vector2(40, 80), Color.White);
            batch.DrawString(Fonts.Arial26, "Create a new character.", new Vector2(40, 180), Color.White);
            batch.DrawString(Fonts.Arial26, Fonts.WrapText("Start at the age of 16 on a random club.", 450, Fonts.Arial26), new Vector2(40, 230), Color.White);
            batch.DrawString(Fonts.Arial36, "Name", nameLabelPos, Color.White);
            acceptButton.Draw(batch);
            
            if(alertEmptyName.Visible)
                alertEmptyName.Draw(batch);
        }
        
        public override void Update(GameTime gameTime)
        {
            txtName.Update(gameTime);
            base.Update(gameTime);
        }

        public override void MainInput(Vector2 pos)
        {
            if (alertEmptyName.Visible)
            {
                alertEmptyName.MainInput();
                return;
            }

            if (acceptButton.Pressed())
            {
                if(string.IsNullOrEmpty(txtName.Text))
                {
                    alertEmptyName.Visible = true;
                    return;
                }
                else
                {
                    Game.Player.Name = txtName.Text;
                    // salvar informações do jogador criado e tal
                    Game.FirstContract();
                }
                return;
            }
            else if (txtName.Pressed())
            {
                txtName.HasFocus = true;
                return;
            }
            txtName.HasFocus = false;
        }
    }
}
