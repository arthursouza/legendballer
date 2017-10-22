using System;
using LegendBaller.Library.Characters;
using LegendBaller.Library.UI;
using LegendBaller.Library.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LegendBaller.Library.Simulation
{
    public class Ball : GameObject
    {
        public delegate void BallStopHandler();
        public BallStopHandler Stop;

        private float frameDelay = 100f;
        private float frameTimer;
        private int frames = 6;
        private int currentFrame;
        private float gravity;
        private float speedDecay;
        private float shadowSize;
        private Vector2 ballEffect;

        public Texture2D ShadowTexture;
        public bool Animated;
        public Texture2D Texture;
        public float Height;
        public float MaxHeight;
        public float DrawingHeight
        {
            get { return Height * 10; }
        }
        
        public float CollisionRadius;
        public Rectangle CollisionBounds
        {
            get
            {
                return new Rectangle(
                    (int)(Position.X - CollisionRadius),
                    (int)(Position.Y - CollisionRadius),
                    (int)(CollisionRadius * 2),
                    (int)(CollisionRadius * 2));
            }
        }
        public bool Kicked;
        public float BallRadius;
        public Vector2 Direction;
        public float Speed;
        public float VerticalForce;
        public bool PlayerKick;

        public Ball()
        {
            speedDecay = .05f;
            gravity = .03f;
            Kicked = false;
        }

        public override void Draw(SpriteBatch batch)
        {
            Rectangle shadow = new Rectangle(
                (int)(Position.X - shadowSize),
                (int)(Position.Y - shadowSize),
                (int)(shadowSize * 2),
                (int)(shadowSize * 2));

            Rectangle dest = new Rectangle(
                    (int)(Position.X - BallRadius),
                    (int)(Position.Y - BallRadius - DrawingHeight),
                    (int)(BallRadius * 2),
                    (int)(BallRadius * 2));

            if (ShadowTexture != null)
                batch.Draw(ShadowTexture, shadow, Color.Gray);

            if (BrazucaGame.DebugMode)
            {
                batch.DrawString(Fonts.Arial12, "h: "+Height.ToString("0,0"), new Vector2(Position.X + CollisionRadius + 5, Position.Y), Color.White);
            }

            if (Animated)
            {
                Rectangle sourceRect = new Rectangle(50 * currentFrame, 0, 50, 50);
                batch.Draw(Texture, dest, sourceRect, Color.White);
            }
            else
            {
                batch.Draw(Texture, dest, Color.White);
            }
        }

        public void Update(GameTime gameTime)
        {
            if (Speed != 0 || Height != 0)
            {
                frameTimer += gameTime.ElapsedGameTime.Milliseconds;

                if (frameTimer > frameDelay)
                {
                    frameTimer = 0f;
                    currentFrame++;
                    if (currentFrame == frames)
                        currentFrame = 0;
                }
            }

            float ballDirection = (float)Math.Atan2(Direction.Y, Direction.X);

            ballDirection = MathHelper.ToDegrees(ballDirection);
            //ballDirection = ballDirection < 0 ? ballDirection * -1 : ballDirection;
            
            if(ballEffect.X < 0)
                ballEffect.X += 0.01f;
            else if(ballEffect.X > 0)
                ballEffect.X -= 0.01f;
            
            ballDirection -= ballEffect.X;
            ballDirection = ballDirection % 360;

            Direction = new Vector2(
                (float)Math.Cos(MathHelper.ToRadians(ballDirection)),
                (float)Math.Sin(MathHelper.ToRadians(ballDirection)));

            Vector2 dir = Direction;
            if (dir != Vector2.Zero)
                dir.Normalize();

            if (Speed > 0)
                Speed -= speedDecay;

            VerticalForce -= gravity;

            Height += VerticalForce;

            Height = Height > MaxHeight ? MaxHeight : Height;

            if (Height < 0)
            {
                // bola bateu no chão, se a upforce negativa for mto alta, ela vai kickar
                Height = 0;
                VerticalForce *= -0.7f;
            }
            if (Kicked && (Speed <= 0 || dir.Y >= 0))
            {
                if (Stop != null) Stop.Invoke();
            }

            float shadowHeight = Math.Min(5, (int)Height);
            shadowSize = BallRadius * (1 - (shadowHeight / 10));

            Position = Position + dir * Speed;
        }

        public void Kick(float strength, Vector2 direction, Vector2 kickVector, bool playerKick = true)
        {
            Direction = direction;
            Kicked = true;

            ballEffect = new Vector2(kickVector.X, 0);

            // perda de força por bater no canto da bola
            float edge = kickVector.X < 0 ? kickVector.X * -1 : kickVector.X;
            edge = 1 - edge;
            strength = strength * Math.Max(.8f, edge);

            // a força pra cima existe independente de onde você chuta
            // a força pra cima é somada a uma fraçao da força do chute
            if (VerticalForce < 0.2)
                VerticalForce += Math.Max(.5f, kickVector.Y);

            float maxSpeed = 10;
            float minSpeed = 6;

            Speed = (maxSpeed - minSpeed) * (strength / Player.MaxPower);

            Speed += minSpeed;
            this.PlayerKick = playerKick;
        }

        public bool Intersects(Rectangle rect)
        {
            var circleDistanceX = Math.Abs(Position.X - rect.X);
            var circleDistanceY = Math.Abs(Position.Y - rect.Y);

            if (circleDistanceX > (rect.Width / 2 + CollisionRadius)) { return false; }
            if (circleDistanceY > (rect.Height / 2 + CollisionRadius)) { return false; }

            if (circleDistanceX <= (rect.Width / 2)) { return true; }
            if (circleDistanceY <= (rect.Height / 2)) { return true; }

            var cornerDistance_sq = (int)(circleDistanceX - rect.Width / 2) ^ 2 + (int)(circleDistanceY - rect.Height / 2) ^ 2;

            return (cornerDistance_sq <= ((int)CollisionRadius ^ 2));
        }
    }
}
