using Microsoft.Xna.Framework;

namespace LegendBaller.Library.Leagues
{
    public enum Uniform
    {
        Flat,
        Stripes
    }
    public class Club
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Rating { get; set; }
        public int Popularity { get; set; }
        public Color MainColor { get; set; }
        public Color SecondColor { get; set; }
        public Uniform Uniform { get; set; }

        public Club() { }

        public Club(int id, string name, int rating, int popularity)
        {
            Id = id;
            Name = name;
            Rating = rating;
            Popularity = popularity;
        }


        public Club(int id, string name, int rating, int popularity, Color main, Color second)
        {
            Id = id;
            Name = name;
            Rating = rating;
            Popularity = popularity;
            MainColor = main;
            SecondColor = second;
        }
    }
}
