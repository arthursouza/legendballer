using System;
using System.Collections.Generic;
using Baller.Library.Characters;
using Baller.Library.Input;
using Baller.Library.Simulation;
using Baller.Library.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Baller.Library.Scenes
{
    public class PlayerPossessionScene : Scene
    {
        Texture2D seta;
        private bool ballClicked;
        private float kickPowerPerc;

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
            ballClicked = Game.IngameBall.TouchBounds.Contains((int)pos.X, (int)pos.Y);
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
        
        public override void InputMoved(Vector2 pos)
        {
            if (ballClicked)
            {
                var maxDiff = 300;
                Vector2 diff = pos - Game.IngameBall.Position;
                var diffLength = Math.Min(diff.Length(), maxDiff);
                kickPowerPerc = diffLength / maxDiff;
                Game.KickPower = kickPowerPerc * Game.Player.Stats.Power;
                Game.KickDirection = diff * -1;
            }
        }
        
        public override void Draw(SpriteBatch batch)
        {
            batch.Draw(Graphics.NewField, BallerGame.WindowBounds, Color.White);
            batch.Draw(Graphics.GoalShadow, FieldRegions.GoalShadowBounds, Color.White);
            batch.Draw(Graphics.Goal, FieldRegions.GoalBounds, Color.White);
            
            Game.Players.ForEach(p => batch.Draw(Graphics.PlayerMarker,
                        new Rectangle(
                            (int)(p.Position.X - Graphics.PlayerMarker.Width / 2f),
                            (int)(p.Position.Y - Graphics.PlayerMarker.Height / 2f),
                            Graphics.PlayerMarker.Width,
                            Graphics.PlayerMarker.Height),
                            (p.Type == PlayerType.Friend ? Color.Blue : Color.Red) * .4f));

            if (!Game.BallInsideGoal())
            {
                batch.Draw(Graphics.GoalTopNet, FieldRegions.GoalBounds, Color.White);
            }

            batch.Draw(Graphics.BallHelp, new Rectangle((int)Game.IngameBall.Position.X - Graphics.BallHelp.Width/2, (int)Game.IngameBall.Position.Y - Graphics.BallHelp.Height/2, 
                Graphics.BallHelp.Width, Graphics.BallHelp.Height), Color.White);

            List<GameObject> renderList = new List<GameObject>();
            renderList.Add(Game.IngameBall);
            renderList.AddRange(Game.Players);
            renderList.Sort();
            renderList.ForEach(o => o.Draw(batch));
            
            if (ballClicked)
            {
                float size = 1 * (Game.KickPower / Game.Player.Stats.Power);
                float rotation = (float)Math.Atan2(Game.KickDirection.Y, Game.KickDirection.X);
                Vector2 origin = new Vector2(0, seta.Height / 2);
                Rectangle arrow = new Rectangle((int)Game.IngameBall.Position.X, (int)Game.IngameBall.Position.Y, (int)(seta.Width * kickPowerPerc), seta.Height);
                batch.Draw(seta, arrow, new Rectangle(0, 0, seta.Width, seta.Height), Color.White, rotation, origin, SpriteEffects.None, 0);
            }

            if (Game.BallInsideGoal())
            {
                batch.Draw(Graphics.GoalTopNet, FieldRegions.GoalBounds, Color.White);
            }
            
            batch.DrawString(Fonts.Arial54, Game.Simulation.CurrentTime.ToString("00"), new Vector2(20, 0), Color.White);
        }

        public override void Update(GameTime gameTime)
        {
            Game.Players.ForEach(p => p.Update(gameTime));
        }

        public Rectangle LeftBar { get; set; }

        public Rectangle RightBar { get; set; }
    }
}
