using System.Collections.Generic;
using BrazucaLibrary.Leagues;

namespace BrazucaLibrary.Characters
{
    public class Player
    {
        public Contract Contract;
        public string Name;

        public int Experience;
        public int Level;

        public Stats Stats;
        
        public int GamesPlayed;
        public int Goals;
        public int Assists;
        
        public int GamesPlayedYear;
        public int GoalsYear;
        public int AssistsYear;

        public int Money;

        public List<Title> Titles;

        public static int MaxPower = 100;
        public static int MaxTechnique = 100;
        public static int MaxFame = 100;

        public Player()
        {
            Stats = new Stats(16, 0, 30, 30);
            Contract = new Contract();
            Titles = new List<Title>();
            Level = 1;
        }

        public string FameDescription()
        {
            if (Stats.Fame < 10)
                return "Beginner";
            if (Stats.Fame < 20)
                return "Hard Worker";
            else if (Stats.Fame < 40)
                return "Promissing";
            else if (Stats.Fame < 60)
                return "Local Star";
            else if (Stats.Fame < 90)
                return "National Idol";
            else
                return "Worldwide Star";
        }

        public int GetExpToNextLevel()
        {
            return 0;
        }

        public int GetLevelExp(int level)
        {
            return 0;
        }
    }
}
