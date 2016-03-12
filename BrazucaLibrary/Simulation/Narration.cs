using System.Collections.Generic;

namespace BrazucaLibrary.Simulation
{
    public class Narration
    {
        public List<string> GoalAttempts;
        public List<string> MissedGoals;
        public List<string> TeamWinning;
        public List<string> TeamLosing;
        public List<string> RandomStuff;

        public List<string> GoodDefense;
        public List<string> BadDefense;

        public List<string> GoodMid;
        public List<string> BadMid;

        public List<string> GoodAttack;
        public List<string> BadAttack;

        public Narration()
        {
            // {0} is always defense {1} is always defense
            GoalAttempts = new List<string>
            {
                "A great attempt by {0}!",
                "{0} have a great chance!",
                "{0} get a clear chance!",
                "{0} SHOOTS!",
                "A clear shot opens for {0}!"
            };

            MissedGoals = new List<string>
            {
                "But they give the ball away.",
                "IT GOES BY SO CLOSE!",
                "Oh, but a terrible shot by {0}",
                "But the {1} keeper holds it."
            };

            RandomStuff = new List<string>
            {
                "We can see some nice football tonight.",
                "They need to work on those passes.",
                "The referee might have some trouble.",
                "Keepers are barely working tonight.",
                "Oh what a tackle!",
                "A real spectacle by the crowd.",
                "What an amazing crowd!",
                "The referee is not making any friends tonight.",
                "What an injury. He might have to put some ice there."
            };

            GoodDefense = new List<string>
            {
                "{0} clears the ball away safely.",
                "They clear the ball away safely.",
                "The defense god rid of the danger.",
                "The defenders do their job nicely.",
                "They keep it away from the defense."
            };

            BadDefense = new List<string>
            {
                "{0} loses the ball on their backyard!",
                "The defense gives the ball away!",
                "They gave it away to the attackers!",
                "The defense fails!"
            };

            GoodAttack = new List<string>
            {
                "The {0} attackers are creating a good chance."
            };

            BadAttack = new List<string>
            {
                "The {0} attackers missed a good oportunity."
            };


            GoodMid = new List<string>
            {
                "{0} midfield passes the ball around."
            };

            BadMid = new List<string>
            {
                "{0} loses the ball in the midfield."
            };
        }
    }
}
