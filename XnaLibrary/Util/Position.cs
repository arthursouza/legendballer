using Microsoft.Xna.Framework;

namespace LegendBaller.Library.Util
{
    public class Position
    {
        public static Position Zero
        {
            get { return new Position(0, 0); }
        }
        public int X;
        public int Y;

        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }

        public void FromVector(Vector2 vector)
        {
            X = (int)vector.X;
            Y = (int)vector.Y;
        }
        public Vector2 ToVector()
        {
            return new Vector2(X, Y);
        }
    }
}
