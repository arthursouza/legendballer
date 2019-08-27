using System;
using Baller.Library.Input;
using Baller.Library.Simulation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Baller.Library.Scenes
{
    public class PlayerPossessionScene : Scene
    {
        Texture2D seta;

        private bool ballClicked;

        private Ball ball
        { get { return Game.IngameBall; } }

        public PlayerPossessionScene(BallerGame game)
            : base(game)
        {
            this.Game = game;
            seta = Graphics.KickArrow;
        }

        public override void InputDown(Vector2 pos)
        {
            if (ballClicked)
            {
                Vector2 diff = pos - Game.IngameBall.Position;
                Game.KickDirection = diff * -1;                
                Game.KickPower = Math.Min(diff.Length(), Game.Player.Stats.Power);
                Game.KickPower = Math.Max(Game.KickPower, 1);
            }
            else
            {
                ballClicked = Game.IngameBall.CollisionBounds.Contains((int)pos.X, (int)pos.Y);
            }
        }

        public override void MainInput(Vector2 pos)
        {
            if (ballClicked)
            {
                Game.LastBallPosition = Game.IngameBall.Position;
                Game.CurrentState = State.IngameKickZoom;
                ballClicked = false;
            }
        
            base.MainInput(pos);
        }

        public override void Draw(SpriteBatch batch)
        {
            Game.DrawField();
            
            if (ballClicked)
            {
                float size = 1 * (Game.KickPower / Game.Player.Stats.Power);
                float rotation = (float)Math.Atan2(Game.KickDirection.Y, Game.KickDirection.X);
                Vector2 origin = new Vector2(0, seta.Height / 2);
                Rectangle arrow = new Rectangle((int)Game.IngameBall.Position.X, (int)Game.IngameBall.Position.Y, (int)(seta.Width * size), seta.Height);
                batch.Draw(seta, arrow, new Rectangle(0, 0, seta.Width, seta.Height), Color.White, rotation, origin, SpriteEffects.None, 0);
            }
        }

        public override void Update(GameTime gameTime)
        {
            Game.Players.ForEach(p => p.Update(gameTime));
        }

        public Rectangle LeftBar { get; set; }

        public Rectangle RightBar { get; set; }
    }
}
