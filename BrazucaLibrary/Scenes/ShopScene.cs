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
    public class ShopScene : Scene
    {
        Button btnLobby;

        public ShopScene(BrazucaGame game)
            : base(game)
        {
            var marginLeft = 30;
            var marginBottom = 80;

            btnLobby = new Button(
                "Lobby", 
                new Vector2(
                    marginLeft, 
                    BrazucaGame.WindowBounds.Height - marginBottom), 
                Color.White, 
                UserInterface.ButtonGreen);
        }

        public override void Draw(SpriteBatch batch)
        {
            batch.Draw(Graphics.BlankBackground, BrazucaGame.WindowBounds, Color.White);

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
