using System;

namespace BrazucaLibrary.Leagues
{
    public class LeagueStanding : IComparable
    {
        public Club Club;
        public int Wins;
        public int Draws;
        public int Losses;
        public int ProGoals;
        public int ConGoals;
        public int GoalScore
        {
            get { return ProGoals - ConGoals; }
        }

        public int Points
        {
            get { return (Wins * 3) + (Draws * 1); }
        }

        public int CompareTo(object obj)
        {
            if (obj is LeagueStanding)
            {
                int result = (obj as LeagueStanding).Points.CompareTo(Points);
                if (result == 0)
                {
                    result = (obj as LeagueStanding).Wins.CompareTo(Wins);
                    if (result == 0)
                    {
                        result = (obj as LeagueStanding).GoalScore.CompareTo(GoalScore);
                        if (result == 0)
                        {
                            result = (obj as LeagueStanding).ProGoals.CompareTo(ProGoals);
                            if (result == 0)
                            {
                                result = (obj as LeagueStanding).Club.Name.CompareTo(Club.Name);
                            }
                        }
                    }
                }

                return result;
            }
            return 0;
        }
    }
}
