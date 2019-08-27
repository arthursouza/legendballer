using System.Collections.Generic;
using Baller.Library.Leagues;

namespace Baller.Library
{
    public enum LeagueType
    {
        BrazLeague
    }
    public class League
    {
        public List<LeagueStanding> Standings;
        public List<Match> Matches;
        public int Rounds;

        public void SetupNewLeague(List<Club> clubs)
        {
            Matches = new List<Match>();
            int teams = clubs.Count;
            Rounds = 18;

            for (int round = 0; round < Rounds; round++)
            {
                for (int match = 0; match < 5; match++)
                {
                    int n = teams - 1;
                    int home = (round + match) % n;
                    int away = (round - match + n) % n;

                    if (match == 0)
                        away = teams - 1;

                    Matches.Add(new Match(clubs[home], clubs[away]) { Round = round });
                }
            }
            Standings = new List<LeagueStanding>();
            for (int i = 0; i < clubs.Count; i++)
            {
                Standings.Add(new LeagueStanding() { Club = clubs[i] });
            }
        }

        public void SortStandings()
        {
            Standings.Sort();
        }

        public Match Find(string club, int round)
        {
            return Matches.Find(x => x.Round == round && (x.Home.Name == club || x.Away.Name == club));
        }
    }
}
