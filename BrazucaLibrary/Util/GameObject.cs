using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BrazucaLibrary.Util
{
    public class GameObject : IComparable
    {
        public Vector2 Position;
        public virtual void Draw(SpriteBatch batch)
        {
        }

        public int CompareTo(object obj)
        {
            int result = Position.Y.CompareTo((obj as GameObject).Position.Y);
            return result == 0 ? Position.X.CompareTo((obj as GameObject).Position.X) : result;
        }
    }
}
