namespace Baller.Library.Simulation
{
    public class GameStatistics
    {
        public int PlayerEasyPasses;
        public int PlayerHardPasses;
        public int PlayerLostBalls;
        public int PlayerGoals;
        public int PlayerAssists;

        public GameStatistics()
        {
        }

        public int AwayRollsWon { get; set; }
        public int AwayGoalWon { get; set; }
        public int AwayAtkWon { get; set; }
        public int AwayMidWon { get; set; }
        public int AwayDefWon { get; set; }

        public int HomeRollsWon { get; set; }
        public int HomeGoalWon { get; set; }
        public int HomeAtkWon { get; set; }
        public int HomeMidWon { get; set; }
        public int HomeDefWon { get; set; }
        public int AwayRolls { get; set; }
        public int HomeRolls { get; set; }
    }
}