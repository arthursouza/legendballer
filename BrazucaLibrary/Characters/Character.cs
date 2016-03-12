﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrazucaLibrary.Leagues;
using BrazucaLibrary.UI;
using BrazucaLibrary.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BrazucaLibrary.Characters
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
        public Rectangle Bounds
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
        public Color UniformColor;
        public Uniform Uniform;

        /// <summary>
        /// 1 2 or 3
        /// </summary>
        public float Speed;
        
        public Character()
        {
            Speed = 2.1f;
            VisionRange = 45;
            CollisionRadius = 12;
        }
        public void Update(GameTime gameTime)
        {
            Origin = new Vector2(Texture.Width / 2, Texture.Height);
            Size = new Vector2(Texture.Width, Texture.Height);
        }
        public override void Draw(SpriteBatch batch)
        {
            switch (Type)
            {
                case PlayerType.Keeper:
                    batch.Draw(Graphics.PlayerMarker,
                        new Rectangle(
                            (int)(Position.X - Graphics.PlayerMarker.Width / 2),
                            (int)(Position.Y - Graphics.PlayerMarker.Height / 2),
                            Graphics.PlayerMarker.Width,
                            Graphics.PlayerMarker.Height),
                            Color.Red * .5f);
                    break;
                case PlayerType.Friend:
                    batch.Draw(Graphics.PlayerMarker,
                        new Rectangle(
                            (int)(Position.X - Graphics.PlayerMarker.Width / 2),
                            (int)(Position.Y - Graphics.PlayerMarker.Height / 2),
                            Graphics.PlayerMarker.Width,
                            Graphics.PlayerMarker.Height),
                            Color.Blue * .5f);
                    break;
                case PlayerType.Adversary:
                    batch.Draw(Graphics.PlayerMarker,
                        new Rectangle(
                            (int)(Position.X - Graphics.PlayerMarker.Width / 2),
                            (int)(Position.Y - Graphics.PlayerMarker.Height / 2),
                            Graphics.PlayerMarker.Width,
                            Graphics.PlayerMarker.Height),
                            Color.Red * .5f);
                    break;
                default:
                    break;
            }

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
