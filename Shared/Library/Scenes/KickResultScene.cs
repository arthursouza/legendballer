using System;
using Baller.Library.Characters;
using Baller.Library.Input;
using Baller.Library.Simulation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Baller.Library.Scenes
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
        
        private Ball ball
        {
            get
            {
                return Game.IngameBall;
            }
        }

        private State nextState;
        
        KickResult result;

        public KickResultScene(BallerGame game)
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
            Game.DrawField();

            AnimateResult(batch);
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

                Vector2 textSize = Fonts.Arial26.MeasureString(text);

                batch.DrawString(Fonts.Arial26, text,
                    new Vector2(BallerGame.WindowSize.X / 2 - textSize.X / 2, BallerGame.WindowSize.Y / 2 - textSize.Y / 2), Color.White);
            }
        }

        public override void Update(GameTime gameTime)
        {
            Game.IngameBall.Update(gameTime);

            bool anythingHappened = false;


            if (ball.Height <= Game.GoalHeight)
            {
                if (ball.CollisionBounds.Intersects(Game.LeftBar))
                {
                    ball.Direction *= new Vector2(-1, -1);
                    //ball.Position += ball.Direction * 
                }
                else if (ball.CollisionBounds.Intersects(Game.RightBar))
                {
                    ball.Direction *= new Vector2(-1, -1);
                }
            }

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
                    if (p.CollisionBounds.Contains((int)Game.IngameBall.Position.X, (int)Game.IngameBall.Position.Y) && Game.IngameBall.Height <= p.Height)
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
                    if (Game.BallInsideGoal())
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
            }
            else
            {
                // Enquanto animo resultado, se for um gol, deixo a bola ser animada até bater no fundo da rede
                if (result == KickResult.Goal || result == KickResult.Assist)
                {
                    if (Game.IngameBall.Position.Y - Game.IngameBall.CollisionRadius < Game.GoalBounds.Y + Game.GoalBounds.Height - Game.GoalInsideGrassArea)
                    {
                        Game.IngameBall.Position.Y = Game.IngameBall.CollisionRadius + Game.GoalBounds.Y + Game.GoalBounds.Height - Game.GoalInsideGrassArea;
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

                    // Bounce ball off the top of the goal net
                    
                    if (ball.Position.Y - ball.CollisionRadius - ball.DrawingHeight < Game.GoalBounds.Y)
                    {
                        //ball.Position.Y = Game.GoalBounds.Y + ball.CollisionRadius + ball.DrawingHeight;
                        Game.IngameBall.VerticalForce *= -0.7f;
                    }
                }

                kickResultDelayTimer += gameTime.ElapsedGameTime.Milliseconds;
                if (kickResultDelayTimer > kickResultEndDelay)
                {
                    kickResultDelayTimer = 0f;
                    if (BallerGame.DebugMode)
                    {
                        Game.CurrentState = State.IngamePlayerPossession;
                        Game.PreparePlayers(SimulationStep.ShotAttempt);
                        Game.ResetBall(BallPositionType.Kick);
                        Game.IngameBall.Position = Game.LastBallPosition;
                    }
                    else
                        Game.CurrentState = State.SimulationRoling;

                    AnimatingResult = false;
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