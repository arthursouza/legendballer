using System.Collections.Generic;
using Game.Windows.Resources;

namespace Baller.Library.Characters
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
                return GameResource.Beginner;
            if (Stats.Fame < 20)
                return GameResource.HardWorker;
            else if (Stats.Fame < 40)
                return GameResource.Promissing;
            else if (Stats.Fame < 60)
                return GameResource.LocalStar;
            else if (Stats.Fame < 90)
                return GameResource.NationalIdol;
            else
                return GameResource.WorldwideStar;
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
