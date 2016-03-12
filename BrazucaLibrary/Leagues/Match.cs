using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BrazucaLibrary.Leagues
{
    public enum MatchResult
    {
        Win,
        Loss,
        Draw
    }
    public class Match
    {

        public int Round;
        public Club Home;
        public Club Away;
        public int ResultHome;
        public int ResultAway;

        public Match() { }
        public Match(Club home, Club away)
        {
            Home = home;
            Away = away;
        }
    }
}
