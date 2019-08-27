using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Baller.Library
{
    public class Transition
    {
        public delegate void TransitionFinishHandler();
        public event TransitionFinishHandler Finish;

        public int Timer;
        public int Interval;
        public float Frames;
        public float CurrentFrame;
        public bool FadeIn;
        private BallerGame game;
        public bool Animating;

        public void Start()
        {
            CurrentFrame = 0;
            FadeIn = false;
            Animating = true;
        }

        public Transition(BallerGame game)
        {
            this.game = game;
            Frames = 20;
            CurrentFrame = 0;
            Interval = 100;
            FadeIn = false;
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            if (Animating)
            {
                float alpha = CurrentFrame / Frames;

                if (FadeIn)
                    alpha = 1 - alpha;

                spriteBatch.Draw(Graphics.Black, BallerGame.WindowBounds, Color.White * alpha);
            }
        }

        public void Update(GameTime gameTime)
        {
            if (Animating)
            {
                Timer += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (Timer > Interval)
                {
                    CurrentFrame++;

                    if (CurrentFrame >= Frames)
                    {
                        Finish.Invoke();
                    }
                }
            }
        }

        public void Rollback()
        {
            FadeIn = true;
            CurrentFrame = 0;
        }

    }
}
