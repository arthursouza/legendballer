using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrazucaLibrary.Input;
using BrazucaLibrary.Simulation;
using BrazucaLibrary.UI;
using BrazucaLibrary.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BrazucaLibrary.Scenes
{
    public class PlayerPossessionScene : Scene
    {
        Texture2D seta;        
        private bool ballClicked;

        public PlayerPossessionScene(BrazucaGame game)
            : base(game)
        {
            this.Game = game;
            seta = Game.Content.Load<Texture2D>("Seta");                       
        }

        public override void MouseDown(MouseButton button)
        {
            if (ballClicked)
            {
                Vector2 diff = InputInfo.MousePosition - Game.IngameBall.Position;
                Game.KickDirection = diff * -1;                
                Game.KickPower = Math.Min(diff.Length(), Game.Player.Stats.Power);
                Game.KickPower = Math.Max(Game.KickPower, 1);
            }
            else
            {
                ballClicked = Game.IngameBall.CollisionBounds.Contains((int)InputInfo.MousePosition.X, (int)InputInfo.MousePosition.Y);
            }

            //Game.IngameBall.Position = InputInfo.MousePosition;
        }

        public override void MouseClick(MouseButton button)
        {
            if (button == MouseButton.Left)
            {
                if (ballClicked)
                {
                    Game.CurrentState = State.IngameKickZoom;
                    ballClicked = false;
                }
            }
            else
            {
                if(BrazucaGame.HARDCORE_INGAME_TESTING)
                    Game.IngameBall.Position = InputInfo.MousePosition;
            }

            base.MouseClick(button);
        }

        public override void Draw(SpriteBatch batch)
        {
            batch.Draw(Graphics.FieldBackground, new Rectangle(0, 0, Game.Window.ClientBounds.Width, Game.Window.ClientBounds.Height), Color.White);
            batch.Draw(Graphics.FieldBounds, Game.GameField, Color.White);
            batch.Draw(Graphics.GoalShadow, Game.GoalShadowBounds, Color.White);
            batch.Draw(Graphics.Goal, Game.GoalBounds, Color.White);

            //batch.Draw(Graphics.Selected, FieldRegions.Attack, Color.Red);
            //batch.Draw(Graphics.Selected, FieldRegions.MidAttack, Color.Orange);
            //batch.Draw(Graphics.Selected, FieldRegions.MidField, Color.Yellow);

            if (ballClicked)
            {
                float size = 1 * (Game.KickPower / Game.Player.Stats.Power);
                float rotation = (float)Math.Atan2(Game.KickDirection.Y, Game.KickDirection.X);
                Vector2 origin = new Vector2(0, seta.Height / 2);
                Rectangle arrow = new Rectangle((int)Game.IngameBall.Position.X, (int)Game.IngameBall.Position.Y, (int)(seta.Width * size), seta.Height);
                batch.Draw(seta, arrow, new Rectangle(0, 0, seta.Width, seta.Height), Color.White, rotation, origin, SpriteEffects.None, 0);
            }

            //batch.Draw(Graphics.Selected, Game.GoalInnerBounds, Color.Yellow);

            batch.DrawString(Fonts.Arial12, InputInfo.MousePosition.ToString(), InputInfo.MousePosition, Color.White);

            //// Draw player collision radius
            //foreach (Player item in Game.Players)
            //{
            //    spriteBatch.Draw(Graphics.Circle, new Rectangle((int)(item.Position.X - item.CollisionRadius), (int)(item.Position.Y - item.CollisionRadius), (int)item.CollisionRadius * 2, (int)item.CollisionRadius * 2), Color.Green * .5f);
            //}

            //// Draw player collision radius
            //foreach (Player item in Game.Players)
            //{
            //    spriteBatch.Draw(Graphics.Circle, new Rectangle((int)(item.Position.X - item.VisionRange), (int)(item.Position.Y - item.VisionRange), (int)item.VisionRange * 2, (int)item.VisionRange * 2), Color.White * .5f);
            //}
            
            List<GameObject> renderList = new List<GameObject>();
            renderList.Add(Game.IngameBall);
            renderList.AddRange(Game.Players);
            renderList.Sort();
            renderList.ForEach(o => o.Draw(batch));

            batch.Draw(Graphics.GoalTopNet, Game.GoalBounds, Color.White);

            string text = Game.Simulation.CurrentTime.ToString("00");
            batch.DrawString(Fonts.Pixelade90, text, new Vector2(20, 0), Color.White);
            // << KICKING START
        }

        public override void Update(GameTime gameTime)
        {
            Game.Players.ForEach(p => p.Update(gameTime));
        }
    }
}
