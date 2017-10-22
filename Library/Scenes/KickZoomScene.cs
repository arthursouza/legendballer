using LegendBaller.Library.Input;
using LegendBaller.Library.Simulation;
using LegendBaller.Library.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LegendBaller.Library.Scenes
{
    public class KickZoomScene : Scene
    {
        private Vector2 kickVector;
        private Texture2D kickBackground;
        private Texture2D kickAim;

        public KickZoomScene(BrazucaGame game)
            : base(game)
        {
            this.Game = game;

            kickBackground = Game.Content.Load<Texture2D>("Backgrounds/KickBackground");
            kickAim = Game.Content.Load<Texture2D>("Kick Aim");
            
            Game.KickBall = new Ball();
            Game.KickBall.Position = new Vector2(BrazucaGame.WindowSize.X / 2, BrazucaGame.WindowSize.Y * .75f);
            Game.KickBall.CollisionRadius = 100;
            Game.KickBall.BallRadius = 100;
            Game.KickBall.Texture = Game.Content.Load<Texture2D>("Ball/Ball");
        }
        
        public override void MouseClick(MouseButton button)
        {
            Vector2 efeito = InputInfo.MousePosition - Game.KickBall.Position;
            if (efeito.Length() <= Game.KickBall.CollisionRadius)
            {
                kickVector = efeito / 100;
                // IMPORTANTE - O CHUTE TEM QUE SER NA BOLA INGAME, A OUTRA É SÓ PRA DAR DETALHE DO CHUTE CACETE
                Game.IngameBall.Kick(Game.KickPower, Game.KickDirection, kickVector);
                Game.CurrentState = State.IngameKickResult;
            }
            //else
            // errou o chute
        }

        public override void Draw(SpriteBatch batch)
        {
            Vector2 aimSize = new Vector2(40, 40);

            batch.Draw(kickBackground, new Rectangle(0, 0, kickBackground.Width, kickBackground.Height), Color.White);

            Game.KickBall.Draw(batch);
            batch.Draw(kickAim, new Rectangle((int)(InputInfo.MousePosition.X - aimSize.X / 2), (int)(InputInfo.MousePosition.Y - aimSize.Y / 2), (int)aimSize.X, (int)aimSize.Y), Color.White);

            Vector2 efeito = (InputInfo.MousePosition - Game.KickBall.Position) / 100;

            if (BrazucaGame.DebugMode)
            {
                //batch.DrawString(Fonts.Arial26, string.Format("({0})", efeito.ToString()), InputInfo.MousePosition, Color.White);
            }

            string text = Game.Simulation.CurrentTime.ToString("00");
            batch.DrawString(Fonts.Arial54, text, new Vector2(20, 0), Color.White);
        }
    }
}
