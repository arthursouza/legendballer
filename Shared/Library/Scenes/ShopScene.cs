using Baller.Library.Input;
using Baller.Library.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Baller.Library.Scenes
{
    public class ShopScene : Scene
    {
        TextureButton btnLobby;

        public ShopScene(BallerGame game)
            : base(game)
        {
            var marginLeft = 30;
            var marginBottom = 80;

            btnLobby = new TextureButton(
                UserInterface.ButtonGreen,
                "Lobby", 
                new Vector2(
                    marginLeft, 
                    BallerGame.WindowBounds.Height - marginBottom));
        }

        public override void Draw(SpriteBatch batch)
        {
            batch.Draw(Graphics.BlankBackground, BallerGame.WindowBounds, Color.White);

            btnLobby.Draw(batch);
            base.Draw(batch);
        }
        
        public override void MainInput(Vector2 pos)
        {
            if (btnLobby.Pressed())
            {
                Game.Transition(State.Lobby);
            }

            base.MainInput(pos);
        }
    }
}
