using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrazucaLibrary.Input;
using BrazucaLibrary.UI;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace BrazucaLibrary.Scenes
{
    public class BusinessScene : Scene
    {
        Button btnLobby;

        public BusinessScene(BrazucaGame game)
            : base(game)
        {
            font = Fonts.Arial26;
            
            btnLobby = new Button(
                "Lobby",
                new Vector2(
                    BrazucaGame.WindowBounds.Width - (windowPadding + UserInterface.ButtonGreen.Width), 
                    BrazucaGame.WindowBounds.Height - (windowPadding + UserInterface.ButtonGreen.Height/2)), 
                Color.White, 
                UserInterface.ButtonGreen);
            
            Controls.Add(btnLobby);
        }

        public override void Draw(SpriteBatch batch)
        {
            batch.Draw(Graphics.BlankBackground, BrazucaGame.WindowBounds, Color.White);

            batch.DrawString(font, Game.PlayerClub.Name, new Vector2(BrazucaGame.WindowBounds.Width / 2 - font.MeasureString(Game.PlayerClub.Name).X / 2, 100), Color.White);
            batch.DrawString(font, string.Format("Club Rating {0} Popularity {1}", Game.Player.Contract.Club.Rating, Game.Player.Contract.Club.Popularity), new Vector2(40, 170), Color.White);
            batch.DrawString(font, "Salary $" + Game.Player.Contract.Value, new Vector2(40, 220), Color.White);
            batch.DrawString(font, "Victory Bonus $" + Game.Player.Contract.VictoryBonus, new Vector2(40, 270), Color.White);
            batch.DrawString(font, "Goal Bonus $" + Game.Player.Contract.GoalBonus, new Vector2(40, 320), Color.White);
            batch.DrawString(font, "Current money $" + Game.Player.Money, new Vector2(40, 370), Color.White);

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

        public SpriteFont font { get; set; }
    }
}
