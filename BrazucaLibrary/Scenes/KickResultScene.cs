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
using BrazucaLibrary.Characters;

namespace BrazucaLibrary.Scenes
{
    public class KickResultScene : Scene
    {
        private bool animateResult;
        private float kickResultEndDelay = 1000;
        private float kickResultDelayTimer = 0f;
        
        private State nextState;
        Ball Ball
        {
            get { return Game.IngameBall; }
        }

        KickResult result;

        public KickResultScene(BrazucaGame game)
            : base(game)
        {
            this.Game = game;
        }

        public override void MouseDown(MouseButton button)
        {
        }

        public override void MouseClick(MouseButton button)
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Graphics.FieldBackground, Game.GameField, Color.White);
            List<GameObject> renderList = new List<GameObject>();
            renderList.AddRange(Game.Players);
            renderList.Add(Game.IngameBall);
            renderList.Sort();
            renderList.ForEach(o => o.Draw(spriteBatch));

            AnimateResult(spriteBatch);

            //spriteBatch.Draw(Graphics.Black, Game.GoalInnerBounds, Color.White);

            string text = Game.Simulation.CurrentTime.ToString("00");
            spriteBatch.DrawString(Fonts.Pixelade90, text, new Vector2(20, 0), Color.White);
        }

        private void AnimateResult(SpriteBatch batch)
        {
            if (animateResult)
            {
                string text = string.Empty;

                switch (result)
                {
                    case KickResult.None:
                        break;
                    case KickResult.LostBall:
                        text = "Ball lost";
                        break;
                    case KickResult.Pass:
                        text = "Pass";
                        break;
                    case KickResult.Goal:
                        text = "GOAL!";
                        break;
                    case KickResult.Assist:
                        text = "Assist!";
                        break;
                    case KickResult.KeeperCaught:
                        text = "Held by keeper.";
                        break;
                }

                Vector2 textSize = Fonts.BigFont.MeasureString(text);

                batch.DrawString(Fonts.BigFont, text,
                    new Vector2(BrazucaGame.WindowSize.X / 2 - textSize.X / 2, BrazucaGame.WindowSize.Y / 2 - textSize.Y / 2), Color.White);
            }
        }
        public override void Update(GameTime gameTime)
        {
            Game.IngameBall.Update(gameTime);
            bool anythingHappened = false;

            if (!animateResult)
            {
                foreach (Character p in Game.Players)
                {
                    p.Update(gameTime);

                    if ((p.Position - Game.IngameBall.Position).Length() <= 130)
                        UpdateBehavior(p);

                    if (p.Bounds.Contains((int)Game.IngameBall.Position.X, (int)Game.IngameBall.Position.Y) &&
                        Game.IngameBall.PhysicalHeight <= p.Size.Y)
                    {
                        if (p.Type == PlayerType.Keeper)
                        {
                            result = KickResult.KeeperCaught;
                            Game.IngameBall.Speed = 0f;
                            PlayResult(result);
                            anythingHappened = true;
                        }
                        else if (p.Type == PlayerType.Friend)
                        {
                            anythingHappened = true;
                            ProcessPlayerPass(p);
                        }
                        else if (p.Type == PlayerType.Adversary)
                        {
                            result = KickResult.LostBall;
                            Game.IngameBall.Speed = 0f;
                            PlayResult(result);
                            anythingHappened = true;
                        }

                        break;
                    }
                }

                if (!anythingHappened)
                {
                    if (Game.IngameBall.Speed <= 0.1f)
                    {
                        result = KickResult.LostBall;
                        PlayResult(result);
                    }
                }

                // chute pra fora
                if (Game.IngameBall.Position.Y < 79)
                {
                    // passou da linha de fundo. mas entrou no gol?
                    if (Ball.PhysicalHeight > Game.GoalBounds.Height ||
                       (Ball.Position.X - Ball.CollisionRadius < Game.GoalBounds.X ||
                        Ball.Position.X + Ball.CollisionRadius > Game.GoalBounds.X + Game.GoalBounds.Width))
                    {
                        result = KickResult.LostBall;
                        PlayResult(result);
                    }
                    else
                    {
                        //Game.Simulation.FriendlyGoal();

                        if (Ball.PlayerKick)
                        {
                            result = KickResult.Goal;
                            Game.Simulation.GameStatistics.PlayerGoals++;
                        }
                        else
                        {
                            result = KickResult.Assist;
                            Game.Simulation.GameStatistics.PlayerAssists++;    
                        }

                        PlayResult(result);
                    }
                }
            }
            else
            {
                // Enquanto animo resultado, se for um gol, deixo a bola ser animada até bater no fundo da rede
                if (result == KickResult.Goal || result == KickResult.Assist)
                {
                    if (Game.IngameBall.Position.Y + Game.IngameBall.CollisionRadius < Game.GoalInnerBounds.Y)
                    {
                        Game.IngameBall.Position.Y = Game.GoalInnerBounds.Y + Game.IngameBall.CollisionRadius;
                        Game.IngameBall.Speed = 0f;
                    }
                    else if (Game.IngameBall.Position.X< Game.GoalInnerBounds.X)
                    {
                        Game.IngameBall.Position.X = Game.GoalInnerBounds.X + Game.IngameBall.CollisionRadius;
                    }
                    else if (Game.IngameBall.Position.X> Game.GoalInnerBounds.X + Game.GoalInnerBounds.Width)
                    {
                        Game.IngameBall.Position.X = Game.GoalInnerBounds.X + Game.GoalInnerBounds.Width - Game.IngameBall.CollisionRadius;
                    }
                }

                kickResultDelayTimer += gameTime.ElapsedGameTime.Milliseconds;
                if (kickResultDelayTimer > kickResultEndDelay)
                {
                    kickResultDelayTimer = 0f;
                    if (BrazucaGame.HARDCORE_INGAME_TESTING)
                    {
                        Game.CurrentState = State.IngamePlayerPossession;
                        Game.PreparePlayers(SimulationStep.ShotAttempt);
                        Game.ResetBall(BallPositionType.Kick);
                    }
                    else
                        Game.CurrentState = State.SimulationRoling;

                    animateResult = false;
                }
            }
        }

        private void ProcessPlayerPass(Character p)
        {
            Game.Simulation.GameStatistics.PlayerEasyPasses++;

            result = KickResult.Pass;
            Game.IngameBall.Speed = 0f;

            Vector2 goal = new Vector2(Game.GoalBounds.X + Game.GoalBounds.Width/2, Game.GoalBounds.Y);
            if ((p.Position - goal).Length() < 250)
            {
                if (p.Position.Y < Game.IngameBall.Position.Y + Game.IngameBall.CollisionRadius)
                {
                    Game.IngameBall.Position.Y = p.Position.Y - Game.IngameBall.CollisionRadius - p.CollisionRadius;
                }
                Random r = new Random(DateTime.Now.Millisecond);

                Vector2 kickTarget = new Vector2(r.Next(Game.GoalBounds.X, Game.GoalBounds.X + Game.GoalBounds.Width), Game.GoalBounds.Y);
                Vector2 kickDirection = p.Position - kickTarget;
                kickDirection *= -1;
                if (kickDirection != Vector2.Zero)
                    kickDirection.Normalize();

                Ball.Kick(40, kickDirection, new Vector2(0, -1), false);
            }
            else
                PlayResult(result);
        }

        private void PlayResult(KickResult result)
        {
            this.result = result;
            Game.PlayerEventEnded(result);
            animateResult = true;
        }

        private void UpdateBehavior(Character p)
        {
            switch (p.Type)
            {
                case PlayerType.Keeper:
                    if (Game.IngameBall.Position.X - p.Position.X > 4)
                        p.Position.X += p.Speed;
                    if (Game.IngameBall.Position.X - p.Position.X < -4)
                        p.Position.X -= p.Speed;
                    break;
                case PlayerType.Friend:
                    if ((p.Position - Game.IngameBall.Position).Length() <= p.VisionRange)
                    {
                        p.Chase(Game.IngameBall.Position, Game.Players);
                    }
                    break;
                case PlayerType.Adversary:
                    if ((p.Position - Game.IngameBall.Position).Length() <= 25)
                    {
                        p.Chase(Game.IngameBall.Position, Game.Players);
                    }
                    break;
            }
        }
    }
}
