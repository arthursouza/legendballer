using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BrazucaLibrary.Util
{
    public class Size
    {
        public int Height;
        public int Width;

        public Size(int width, int height)
        {
            Height = height;
            Width = width;
        }

        public Size(Vector2 size)
        {
            Width = (int)size.X;
            Height = (int)size.Y;
        }

        public Vector2 ToVector()
        {
            return new Vector2(Width, Height);
        }


    }
}
