using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrazucaLibrary.Input;
using BrazucaLibrary.UI;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using BrazucaLibrary.Util;
using Microsoft.Xna.Framework.GamerServices;

namespace BrazucaLibrary.Scenes
{
    public class NewCareerScene : Scene
    {
        Button acceptButton;
        Textbox txtName;
        MessageBox alertEmptyName;

        Vector2 nameLabelPos = new Vector2(40, 310);
        Vector2 nationalityLabelPos = new Vector2(40, 315);

        public NewCareerScene(BrazucaGame game)
            : base(game)
        {
            var buttonSize = new Vector2(UserInterface.ButtonGreen.Width, UserInterface.ButtonGreen.Height);

            acceptButton = new Button("Go Ahead", 
                new Vector2(
                    BrazucaGame.WindowSize.X - (buttonSize.X + 15),
                    BrazucaGame.WindowSize.Y - (buttonSize.Y/2 + 15)), 
                Color.White, 
                UserInterface.ButtonGreen);

            txtName = new Textbox(nameLabelPos + new Vector2(Fonts.Arial36.MeasureString("Name").X + 10, 0));
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
            batch.Draw(Graphics.BlankBackground, BrazucaGame.WindowBounds, Color.White);
            txtName.Draw(batch);
            batch.DrawString(Fonts.Arial54, "A new career", new Vector2(40, 80), Color.White);
            batch.DrawString(Fonts.Arial26, "Create a new character.", new Vector2(40, 180), Color.White);
            batch.DrawString(Fonts.Arial26, "Start at the age of 16 on a random club.", new Vector2(40, 230), Color.White);
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
        public override void MouseClick(MouseButton button)
        {
            if (button == MouseButton.Left)
            {
                if (alertEmptyName.Visible)
                {
                    alertEmptyName.MouseClick(button);
                    return;
                }

                if (acceptButton.MouseOver())
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
                else if (txtName.MouseOver)
                {
                    txtName.HasFocus = true;
                    return;
                }
                txtName.HasFocus = false;
            }
        }
    }
}
