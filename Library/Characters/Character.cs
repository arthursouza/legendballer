using System.Collections.Generic;
using LegendBaller.Library.Leagues;
using LegendBaller.Library.UI;
using LegendBaller.Library.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LegendBaller.Library.Characters
{
    public enum PlayerType
    {
        Keeper,
        Friend,
        Adversary
    }

    public class Character : GameObject
    {
        public Vector2 Origin;
        public Vector2 Size;
        public float CollisionRadius;
        public float VisionRange;
        public Texture2D Texture;
        public PlayerType Type;
        
        /// <summary>
        /// 1 2 or 3
        /// </summary>
        public float Speed;
        public int Height { get; set; }

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
        public Rectangle HeightBounds
        {
            get
            {
                return new Rectangle(
                    (int)(Position.X - CollisionRadius),
                    (int)(Position.Y - CollisionRadius),
                    (int)(CollisionRadius * 2),
                    Height);
            }
        }
        public Color UniformColor;
        public Uniform Uniform;
        
        public Character()
        {
            Speed = 2.1f;
            VisionRange = 70;
            CollisionRadius = 5;
            Height = 2;
        }
        
        public void Update(GameTime gameTime)
        {
            Origin = new Vector2(Texture.Width / 2, Texture.Height);
            Size = new Vector2(Texture.Width, Texture.Height);
        }

        public override void Draw(SpriteBatch batch)
        {
            batch.Draw(Texture, 
                new Rectangle(
                    (int)(Position.X - Size.X/2),
                    (int)(Position.Y - Size.Y),
                    (int)(Size.X), 
                    (int)(Size.Y)),
                    Color.White);

            if (Uniform == Uniform.Flat)
            {
                batch.Draw(Graphics.FlatUniform,
                    new Rectangle(
                        (int)(Position.X - Size.X / 2),
                        (int)(Position.Y - Size.Y),
                        (int)(Size.X),
                        (int)(Size.Y)),
                        UniformColor);
            }
            else
            {
                batch.Draw(Graphics.StripesUniform,
                    new Rectangle(
                        (int)(Position.X - Size.X / 2),
                        (int)(Position.Y - Size.Y),
                        (int)(Size.X),
                        (int)(Size.Y)),
                        UniformColor);
            }
        }

        internal void Chase(Vector2 ball, List<Character> list)
        {
            if (ball.X - Position.X > 4)
                Position.X += Speed;
            if (ball.X - Position.X < -4)
                Position.X -= Speed;
            if (ball.Y - Position.Y > 4)
                Position.Y += Speed;
            if (ball.Y - Position.Y < -4)
                Position.Y -= Speed;

            for (int i = 0; i < list.Count; i++)
            {
                Vector2 d = list[i].Position - Position;
                if (d.Length() < CollisionRadius + list[i].CollisionRadius)
                {
                    if (d != Vector2.Zero)
                        d.Normalize();

                    Position = list[i].Position - (d * (CollisionRadius + list[i].CollisionRadius));
                }
            }
        }
    }
}
