using System;
using System.Collections.Generic;
using LegendBaller.Library.Leagues;
using Microsoft.Xna.Framework;

namespace LegendBaller.Library.Simulation
{
    public class MatchSimulation
    {
        public delegate void GameEventHandler(SimulationStep gameEvent, object data);
        public event GameEventHandler GameEvent;
        public Match Match;
        public int EnemyTeamScoringChance
        { get; set; }
        public int FriendlyTeamScoringChance
        { get; set; }
        public Club EnemyTeam
        {
            get
            {
                if (Match.Home.Name == game.PlayerClub.Name)
                    return Match.Away;
                else
                    return Match.Home;
            }
        }
        public GameStatistics GameStatistics
        { get; set; }

        /// <summary>
        /// Match clock 0-90
        /// </summary>
        public int CurrentTime;

        /// <summary>
        /// The timer for ingame minutes passing
        /// </summary>
        private float timer;
        /// <summary>
        /// How long it takes for 1 ingame minute
        /// </summary>
        private float matchTimeStepDelay;

        float decisionTimer;
        float simulationDecisionDelay;

        private Team possession = Team.Home;

        public List<string> GameEvents;

        public float SimulationSpeed = 1f;

        public int HomeScore;
        public int AwayScore;

        public MatchPeriod Period;
        public SimulationStep CurrentSimulationStep;
        public SimulationStep? NextStep;

        Random random;
        BrazucaGame game;

        public MatchSimulation(BrazucaGame game)
        {
            random = new Random(DateTime.Now.Millisecond);
            GameEvents = new List<string>();
            this.game = game;
            timer = 0f;
            decisionTimer = 0f;

            matchTimeStepDelay = 666;
            simulationDecisionDelay = 500;

            SimulationSpeed = 1;
            
            var variation = 1.5f;
            matchTimeStepDelay = matchTimeStepDelay / (SimulationSpeed * variation);
            simulationDecisionDelay = simulationDecisionDelay / (SimulationSpeed * variation);
            
            GameStatistics = new GameStatistics();
        }

        public void Start()
        {
            Period = MatchPeriod.BeforeGame;
            CurrentSimulationStep = SimulationStep.Midfield;

            GameEvents = new List<string>();
            CurrentTime = 0;
            HomeScore = 0;
            AwayScore = 0;
            GameStatistics.PlayerGoals = 0;
            GameStatistics.PlayerAssists = 0;
            FriendlyTeamScoringChance = 25;
            EnemyTeamScoringChance = 25;

            LogSimulation("######################################################");
            LogSimulation($"SIM START {Match.Home.Name} {Match.Home.Rating} x {Match.Away.Name} ({Match.Away.Rating}) hh:mm:ss");
        }

        public void Update(GameTime gameTime)
        {
            timer += gameTime.ElapsedGameTime.Milliseconds;
            decisionTimer += gameTime.ElapsedGameTime.Milliseconds;

            #region During Game
            if (Period == MatchPeriod.FirstTime || Period == MatchPeriod.SecondTime)
            {
                // Roll timer for game time
                if (timer >= matchTimeStepDelay)
                {
                    timer = 0f;
                    CurrentTime++;
                    if (CurrentTime == 45)
                    {
                        HalfTime();
                    }
                    else if (CurrentTime == 90)
                    {
                        MatchEnded();

                        timer = 0f;
                        CurrentTime = 0;
                    }
                }

                // Roll timer for game events
                if (decisionTimer > simulationDecisionDelay)
                {
                    decisionTimer = 0f;
                    NextEvent();
                }
            }
            #endregion
        }

        private void NextEvent()
        {
            if (NextStep.HasValue)
            {
                CurrentSimulationStep = NextStep.Value;
            }

            float homeT = ((float)Match.Home.Rating / 100) * 50 + 50;
            float awayT = ((float)Match.Away.Rating / 100) * 50 + 50;
            
            #region Random Event
            if (random.Next(0, 101) <= 60)
            {
                CreateRandomEvent();
            }
            #endregion

            #region Adjust Difficulty
            // Adjust roll according to difficulty
            // Increase difficulty by 30% in offensive simulations
            if (CurrentSimulationStep == SimulationStep.ShotAttempt)
            {
                if (possession == Team.Home)
                {
                    awayT = awayT * 1.3f;
                }
                else
                {
                    homeT = homeT * 1.3f;
                }
            }
            #endregion

            float playerChance = 50;
            
            // roll
            float homeRoll = random.Next(0, (int)homeT + 1);
            float awayRoll = random.Next(0, (int)awayT + 1);
            
            //LogSimulation($"SIM ROLL ({Enum.GetName(typeof(SimulationStep), CurrentSimulationStep)}) {homeT} x {awayT} / result: {homeRoll} x {awayRoll} [{(homeRoll >= awayRoll ? "HOME" : "AWAY")}]");

            string possessingTeam = possession == Team.Home ? Match.Home.Name : Match.Away.Name;

            bool playSuccessful = (possession == Team.Home && homeRoll >= awayRoll) || (possession == Team.Away && homeRoll < awayRoll);

            #region Game Statistics

            if (possession == Team.Home)
            {
                GameStatistics.HomeRolls++;

                if (playSuccessful)
                {
                    GameStatistics.HomeRollsWon++;
                }

                switch (CurrentSimulationStep)
                {
                    case SimulationStep.Defensive:
                        GameStatistics.HomeDefWon++;
                        break;
                    case SimulationStep.Midfield:
                        GameStatistics.HomeMidWon++;
                        break;
                    case SimulationStep.Attack:
                        GameStatistics.HomeAtkWon++;
                        break;
                    case SimulationStep.ShotAttempt:
                        GameStatistics.HomeGoalWon++;
                        break;
                }
            }
            else
            {
                GameStatistics.AwayRolls++;

                if (playSuccessful)
                {
                    GameStatistics.AwayRollsWon++;
                }

                switch (CurrentSimulationStep)
                {
                    case SimulationStep.Defensive:
                        GameStatistics.AwayDefWon++;
                        break;
                    case SimulationStep.Midfield:
                        GameStatistics.AwayMidWon++;
                        break;
                    case SimulationStep.Attack:
                        GameStatistics.AwayAtkWon++;
                        break;
                    case SimulationStep.ShotAttempt:
                        GameStatistics.AwayGoalWon++;
                        break;
                }
            }

            #endregion

            switch (CurrentSimulationStep)
            {
                case SimulationStep.Defensive:
                    if (playSuccessful)
                    {
                        CreateEvent(SimulationStep.Midfield, playSuccessful, game.Narration.GoodDefense);
                    }
                    else
                    {
                        CreateEvent(SimulationStep.Attack, playSuccessful, game.Narration.BadDefense);
                    }
                    break;
                case SimulationStep.Midfield:
                    if (playSuccessful)
                    {
                        if (possessingTeam == game.PlayerClub.Name && random.Next(1, 101) >= playerChance)
                        {
                            CreatePassChance();
                        }
                        else
                        {
                            CreateEvent(SimulationStep.Attack, playSuccessful, game.Narration.GoodMid);
                        }
                    }
                    else
                    {
                        CreateEvent(SimulationStep.Defensive, playSuccessful, game.Narration.BadMid);
                    }
                    break;
                case SimulationStep.Attack:
                    if (playSuccessful)
                    {
                        //CreateEvent(SimulationStep.ShotAttempt, playSuccessful, game.Narration.GoodAttack);
                        if (possessingTeam == game.PlayerClub.Name && random.Next(1, 101) >= playerChance)
                        {
                            CreateShootingChance();
                        }
                        else
                        {
                            CreateEvent(SimulationStep.ShotAttempt, playSuccessful, game.Narration.GoodAttack);
                        }
                    }
                    else
                    {
                        CreateEvent(SimulationStep.Midfield, playSuccessful, game.Narration.BadAttack);
                    }
                    break;
                case SimulationStep.ShotAttempt:
                    bool scored = possessingTeam == game.PlayerClub.Name
                        ? CreateFriendlyShotEvent()
                        : CreateEnemyShotEvent();
                    NextStep = scored ? SimulationStep.Midfield : SimulationStep.Defensive;
                    possession = possession == Team.Away ? Team.Home : Team.Away;
                    break;
            }
            
        }

        private void CreateEvent(SimulationStep nextStep, bool playSuccessful, List<string> narrationMessages)
        {
            string possessingTeam = possession == Team.Home ? Match.Home.Name : Match.Away.Name;
            string opposingTeam = possession == Team.Home ? Match.Away.Name : Match.Home.Name;

            var narrationMessage = string.Format(narrationMessages[random.Next(0, narrationMessages.Count)], possessingTeam, opposingTeam);
            
            //LogSimulation(narrationMessage);

            if (!playSuccessful)
            {
                possession = possession == Team.Away ? Team.Home : Team.Away;
            }

            NextStep = nextStep;
            GameEvents.Add(narrationMessage);
        }

        private void LogSimulation(string message)
        {
#if DEBUG
            //File.AppendAllText("simulation-" + DateTime.Now.ToString("dd/MM/yy") + ".sl", message + Environment.NewLine);
#endif
        }

        private void HalfTime()
        {
            LogSimulation($"HALF-TIME {Match.Home.Name} {HomeScore} x {AwayScore} {Match.Away.Name} hh:mm:ss");
            Period = MatchPeriod.HalfTime;
        }

        private void MatchEnded()
        {
            #region Log
            LogSimulation($"MATCH-END {Match.Home.Name} {HomeScore} x {AwayScore} {Match.Away.Name} hh:mm:ss");
            LogSimulation($"######");
            LogSimulation($"AwayRollsWon  {GameStatistics.AwayRollsWon}");
            LogSimulation($"HomeRollsWon  {GameStatistics.HomeRollsWon}");
            LogSimulation($"######");
            LogSimulation($"Shots H {GameStatistics.HomeGoalWon} x {GameStatistics.AwayGoalWon} A");
            LogSimulation($"Atk H {GameStatistics.HomeAtkWon} x {GameStatistics.AwayAtkWon} A");
            LogSimulation($"Mid H {GameStatistics.HomeMidWon} x {GameStatistics.AwayMidWon} A"); 
            LogSimulation($"Def H {GameStatistics.HomeDefWon} x {GameStatistics.AwayDefWon} A"); 
            LogSimulation("######################################################");
            #endregion

            Period = MatchPeriod.EndGame;
        }

        private void CreateRandomEvent()
        {
            //GameEvent.Invoke();
            GameEvents.Add(game.Narration.RandomStuff[random.Next(0, game.Narration.RandomStuff.Count)]);
        }

        private bool CreateFriendlyShotEvent()
        {
            string possessingTeam = possession == Team.Home ? Match.Home.Name : Match.Away.Name;
            string opposingTeam = possession == Team.Home ? Match.Away.Name : Match.Home.Name;

            GameEvents.Add(string.Format(game.Narration.GoalAttempts[random.Next(0, game.Narration.GoalAttempts.Count)], game.PlayerClub.Name));

            bool scored = random.Next(0, 101) < FriendlyTeamScoringChance;

            if (scored)
            {
                FriendlyGoal();
                GameEvents.Add(game.PlayerClub.Name.ToUpper() + " SCORES!");
            }
            else
            {
                GameEvents.Add(string.Format(game.Narration.MissedGoals[random.Next(0, game.Narration.MissedGoals.Count)], possessingTeam, opposingTeam));
            }

            return scored;
        }

        private bool CreateEnemyShotEvent()
        {
            string possessingTeam = possession == Team.Home ? Match.Home.Name : Match.Away.Name;
            string opposingTeam = possession == Team.Home ? Match.Away.Name : Match.Home.Name;

            GameEvents.Add(string.Format(game.Narration.GoalAttempts[random.Next(0, game.Narration.GoalAttempts.Count)], EnemyTeam.Name));
            bool scored = random.Next(0, 101) <= EnemyTeamScoringChance;

            if (scored)
            {
                EnemyGoal();
                GameEvents.Add(EnemyTeam.Name.ToUpper() + " SCORES!");
            }
            else
            {
                GameEvents.Add(string.Format(game.Narration.MissedGoals[random.Next(0, game.Narration.MissedGoals.Count)], possessingTeam, opposingTeam));
            }

            return scored;
        }

        private void CreateShootingChance()
        {
            GameEvents.Add(game.Player.Name + " gets a clear shot.");

            GameEvent?.Invoke(SimulationStep.ShotAttempt, null);
        }

        private void CreatePassChance()
        {
            //GameEvents.Add(game.Player.Name + " gets the ball on the middle.");
            //GameEvent?.Invoke(SimulationStep.Pass);
        }

        public void FriendlyGoal()
        {
            if (game.PlayerClub.Name == Match.Home.Name)
                HomeScore++;
            else
                AwayScore++;
        }

        public void EnemyGoal()
        {
            if (game.PlayerClub.Name == Match.Home.Name)
                AwayScore++;
            else
                HomeScore++;
        }
    }
}