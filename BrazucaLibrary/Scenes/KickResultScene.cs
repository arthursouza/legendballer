﻿using System;
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
        /// <summary>
        /// Playing the onscreen result animation (Lost Ball, Goal, Assistence)
        /// </summary>
        /// <value>
        ///   <c>true</c> if [animating result]; otherwise, <c>false</c>.
        /// </value>
        private bool AnimatingResult { get; set; }
        
        private float kickResultEndDelay = 1000;

        private float kickResultDelayTimer = 0f;
        private Ball ball { get { return Game.IngameBall; } }
        private State nextState;
        
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

        public override void Draw(SpriteBatch batch)
        {
            batch.Draw(Graphics.FieldBackground, new Rectangle(0, 0, Game.Window.ClientBounds.Width, Game.Window.ClientBounds.Height), Color.White);
            batch.Draw(Graphics.FieldBounds, Game.GameField, Color.White);
            batch.Draw(Graphics.GoalShadow, Game.GoalShadowBounds, Color.White);
            batch.Draw(Graphics.Goal, Game.GoalBounds, Color.White);

            //batch.Draw(Graphics.Selected, LeftBar, Color.Yellow);
            //batch.Draw(Graphics.Selected, RightBar, Color.Yellow);


            List<GameObject> renderList = new List<GameObject>();
            renderList.AddRange(Game.Players);
            renderList.Add(Game.IngameBall);
            renderList.Sort();
            renderList.ForEach(o => o.Draw(batch));



            if (BallInsideGoal())
            {
                batch.Draw(Graphics.GoalTopNet, Game.GoalBounds, Color.White);
            }

            AnimateResult(batch);

            //spriteBatch.Draw(Graphics.Black, Game.GoalInnerBounds, Color.White);

            string text = Game.Simulation.CurrentTime.ToString("00");
            batch.DrawString(Fonts.Pixelade90, text, new Vector2(20, 0), Color.White);
        }

        private void AnimateResult(SpriteBatch batch)
        {
            if (AnimatingResult)
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
            LeftBar = new Rectangle(Game.GoalBounds.X, Game.GameField.Y - Game.GoalBounds.Height, 4, Game.GoalBounds.Height);
            RightBar = new Rectangle(Game.GoalBounds.X + Game.GoalBounds.Width - 4, Game.GameField.Y - Game.GoalBounds.Height, 4, Game.GoalBounds.Height);

            bool anythingHappened = false;

            if (!AnimatingResult)
            {
                #region Check Players
                foreach (Character p in Game.Players)
                {
                    p.Update(gameTime);

                    // Update player if he can see the ball
                    if ((p.Position - Game.IngameBall.Position).Length() <= p.VisionRange)
                    {
                        UpdateBehavior(p);
                    }

                    // Check if player can get possession of the ball
                    if (p.Bounds.Contains((int)Game.IngameBall.Position.X, (int)Game.IngameBall.Position.Y) && Game.IngameBall.PhysicalHeight <= p.Size.Y)
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
                #endregion

                if (!anythingHappened)
                {
                    if (Game.IngameBall.Speed <= 0.1f)
                    {
                        result = KickResult.LostBall;
                        PlayResult(result);
                    }
                }

                // passou da linha de fundo
                if (Game.IngameBall.Position.Y + Game.IngameBall.CollisionRadius < Game.GameField.Y)
                {
                    // passou da linha de fundo. mas entrou no gol?
                    if (BallInsideGoal())
                    {
                        Game.Simulation.FriendlyGoal();

                        if (ball.PlayerKick)
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
                    else
                    {
                        result = KickResult.LostBall;
                        PlayResult(result);
                    }
                }
                else
                {
                    if (ball.Position.Y - ball.CollisionRadius < Game.GameField.Y)
                    {
                        // if right part of the ball is hitting the bar
                        if (ball.Position.X + ball.CollisionRadius >= LeftBar.X && ball.Position.X + ball.CollisionRadius <= LeftBar.X + LeftBar.Width)
                        {
                            ball.Direction *= new Vector2(-1, -1);
                        }
                        else if (ball.Position.X - ball.CollisionRadius >= LeftBar.X && ball.Position.X - ball.CollisionRadius <= LeftBar.X + LeftBar.Width)
                        {
                            ball.Direction *= new Vector2(-1, -1);
                        }
                    }
                }
            }
            else
            {
                // Enquanto animo resultado, se for um gol, deixo a bola ser animada até bater no fundo da rede
                if (result == KickResult.Goal || result == KickResult.Assist)
                {
                    if (Game.IngameBall.Position.Y - Game.IngameBall.CollisionRadius <
                        Game.GoalBounds.Y + Game.GoalBounds.Height - Game.GoalInsideGrassArea)
                    {
                        Game.IngameBall.Position.Y = Game.IngameBall.CollisionRadius + Game.GoalBounds.Y +
                                                     Game.GoalBounds.Height - Game.GoalInsideGrassArea;
                        Game.IngameBall.Speed = 0f;
                    }
                    else if (Game.IngameBall.Position.X + Game.IngameBall.CollisionRadius < Game.GoalInnerBounds.X)
                    {
                        Game.IngameBall.Position.X = Game.GoalInnerBounds.X + Game.IngameBall.CollisionRadius;
                    }
                    else if (Game.IngameBall.Position.X + Game.IngameBall.CollisionRadius >
                             Game.GoalInnerBounds.X + Game.GoalInnerBounds.Width)
                    {
                        Game.IngameBall.Position.X = Game.GoalInnerBounds.X + Game.GoalInnerBounds.Width -
                                                     Game.IngameBall.CollisionRadius;
                    }

                    if (Game.IngameBall.PhysicalHeight > Game.GoalHeight)
                    {
                        Game.IngameBall.VerticalForce *= -0.7f;
                    }
                }
                else
                {
                    if (Game.IngameBall.Position.X < Game.GoalBounds.X &&
                        Game.IngameBall.Position.X + Game.IngameBall.CollisionRadius > Game.GoalBounds.X)
                    {
                        Game.IngameBall.Position.X = Game.GoalBounds.X - Game.IngameBall.CollisionRadius;
                        Game.IngameBall.Direction *= new Vector2(-1, 1);
                    }
                    else if (
                        Game.IngameBall.Position.X > Game.GoalBounds.X + Game.GoalBounds.Width &&
                        Game.IngameBall.Position.X - Game.IngameBall.CollisionRadius < Game.GoalBounds.X + Game.GoalBounds.Width)
                    {
                        Game.IngameBall.Position.X = Game.GoalBounds.X + Game.GoalBounds.Width + Game.IngameBall.CollisionRadius;
                        Game.IngameBall.Direction *= new Vector2(-1, 1);
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

                    AnimatingResult = false;
                }
            }
        }

        public Rectangle RightBar { get; set; }

        public Rectangle LeftBar { get; set; }

        private bool BallInsideGoal()
        {
            var belowTopBar = ball.PhysicalHeight < Game.GoalHeight;
            var pastEndLine = ball.Position.Y + ball.CollisionRadius < Game.GameField.Y;
            //var insideGoalArea = ball.Position.Y - ball.CollisionRadius >= Game.GoalInnerBounds.Y;
            var betweenTwoBars = (ball.Position.X - ball.CollisionRadius > Game.GoalBounds.X && ball.Position.X + ball.CollisionRadius < Game.GoalBounds.X + Game.GoalBounds.Width);

            return belowTopBar && pastEndLine && betweenTwoBars;
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

                ball.Kick(40, kickDirection, new Vector2(0, -1), false);
            }
            else
                PlayResult(result);
        }

        private void PlayResult(KickResult result)
        {
            this.result = result;
            Game.PlayerEventEnded(result);
            AnimatingResult = true;
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
                    if ((p.Position - Game.IngameBall.Position).Length() <= p.VisionRange)
                    {
                        p.Chase(Game.IngameBall.Position, Game.Players);
                    }
                    break;
            }
        }
    }
}