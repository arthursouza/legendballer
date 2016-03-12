using System;
using System.Collections.Generic;
using System.Linq;
using BrazucaLibrary.Input;
using BrazucaLibrary.Simulation;
using BrazucaLibrary.UI;
using BrazucaLibrary.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using BrazucaLibrary;
using BrazucaLibrary.Scenes;
using System.IO;
using BrazucaLibrary.Leagues;
using BrazucaLibrary.Characters;

namespace BrazucaLibrary
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class BrazucaGame : Microsoft.Xna.Framework.Game
    {
        public MouseCursor MouseCursor;
        public bool CurrentLeagueChampion = false;
        public Narration Narration;
        public League League;
        public int CurrentLeagueRound;
        public int LeaguePosition;
        public int Year;
        public int WinSequence;
        public TimeSpan PlayedSpan;

        public static bool HARDCORE_INGAME_TESTING = true;

        Random rand = new Random(DateTime.Now.Millisecond);
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public State CurrentState;
        public static Vector2 WindowSize;
        public Player Player;
        public static Rectangle WindowBounds
        {
            get { return new Rectangle(0, 0, (int)WindowSize.X, (int)WindowSize.Y); }
        }

        public Club PlayerClub
        {
            get 
            {
                if (Player.Contract != null)
                    return Player.Contract.Club;
                else
                    return null; 
            }
        }
        
        public List<Club> Clubs;
        public List<string> Country;

        // Match data
        public Rectangle GameField;
        public Rectangle GoalBounds;
        public Rectangle GoalInnerBounds;
        public Ball IngameBall;
        public Ball KickBall;
        public List<Character> Players;

        public Contract CurrentContract;
        public Contract ContractProposition;

        public string LatestNews;

        // Simulation Data
        public MatchSimulation Simulation;

        // Kick Information
        public float KickPower;
        public Vector2 KickDirection;


        private Transition transition;
        Dictionary<State, Scene> Scenes;
        private State nextState;

        public BrazucaGame()
        {
            PlayedSpan = new TimeSpan();
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Window.Title = "Legend Baller";
            Narration = new Narration();
            WindowSize = new Vector2(800, 600);
            graphics.PreferredBackBufferWidth = (int)WindowSize.X;
            graphics.PreferredBackBufferHeight = (int)WindowSize.Y;
            MouseCursor = MouseCursor.Normal;
            IsMouseVisible = true;
            Player = new Player();
            Year = 2014;
        }

        protected override void Initialize()
        {
            Simulation = new MatchSimulation(this);
            Simulation.GameEvent += Simulation_GameEvent;
            CurrentState = State.MainMenu;
            Players = new List<Character>();
            base.Initialize();
        }

        private void LoadClubs()
        {
            Clubs = new List<Club>();
            Clubs.Add(new Club(1, "Vulture RN", 70, 100, Color.Red, Color.Black) { Uniform = Uniform.Stripes });
            Clubs.Add(new Club(2, "SP Football", 75, 70, Color.Red, Color.White));
            Clubs.Add(new Club(3, "SC Hawks", 80, 95, Color.White, Color.Black));
            Clubs.Add(new Club(4, "CR Oranges", 70, 40, Color.Red, Color.Green) { Uniform = Uniform.Flat });
            Clubs.Add(new Club(5, "Maltese Cross", 50, 60, Color.White, Color.Black));
            Clubs.Add(new Club(6, "La Lezione", 70, 60, Color.LightGreen, Color.White));
            Clubs.Add(new Club(7, "Lonely Star FC", 70, 40, Color.Black, Color.White) { Uniform = Uniform.Stripes });
            Clubs.Add(new Club(8, "Salvatore", 40, 40, Color.Blue, Color.White));
            Clubs.Add(new Club(9, "Victory", 40, 40, Color.DarkRed, Color.White));
            Clubs.Add(new Club(10, "Montain Club", 30, 20, Color.LightBlue, Color.Black));
        }

        private void NewGame()
        {
            League = new League();
            League.SetupNewLeague(Clubs);
            CurrentLeagueRound = 0;
        }

        void Simulation_GameEvent(SimulationStep gameEvent, object data)
        {
            PreparePlayers(gameEvent);
            switch (gameEvent)
            {
                case SimulationStep.ShotAttempt:
                    ResetBall(BallPositionType.Kick);
                    break;
                case SimulationStep.Midfield:
                case SimulationStep.Attack:
                case SimulationStep.Defensive:
                    ResetBall(BallPositionType.Pass);
                    break;
            }

            CurrentState = State.IngamePlayerPossession;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            LoadGraphics();

            GameField = new Rectangle(
                (int)((WindowSize.X - Graphics.FieldBackground.Width) / 2),
                (int)((WindowSize.Y - Graphics.FieldBackground.Height) / 2),
                Graphics.FieldBackground.Width,
                Graphics.FieldBackground.Height);

            MessageBox.DefaultWindowPosition = new Vector2(
                WindowSize.X / 2 - MessageBox.DefaultWindowSize.X / 2,
                WindowSize.Y / 2 - MessageBox.DefaultWindowSize.Y / 2);

            GoalBounds = new Rectangle(
                396 + GameField.X,
                28 + GameField.Y,
                161, 58);

            GoalInnerBounds = new Rectangle(
                397 + GameField.X,
                53 + GameField.Y,
                158, 25);

            //traveEsquerda = new Rectangle(GoalBounds.X - 3, GoalBounds.Y + GoalBounds.Height - 70, 5, 70);
            //traveDireita = new Rectangle(GoalBounds.X + GoalBounds.Width - 2, GoalBounds.Y + GoalBounds.Height - 70, 5, 70);

            IngameBall = new Ball();
            IngameBall.Animated = true;
            IngameBall.CollisionRadius = 10;
            IngameBall.BallRadius = 7;
            IngameBall.Texture = Content.Load<Texture2D>("Ball/Ball spritesheet");
            IngameBall.ShadowTexture = Content.Load<Texture2D>("Ball/Ball Shadow");

            InputInfo.LastMouseState = Mouse.GetState();
            InputInfo.LastKeyboardState = Keyboard.GetState();

            transition = new Transition(this);
            transition.Finish += transition_Finish;

            LoadScenes();
            LoadCountries();
            LoadClubs();

            if (HARDCORE_INGAME_TESTING)
            {
                //CurrentState = State.IngamePlayerPossession;
                //PreparePlayers(SimulationStep.ShotAttempt);
                //ResetBall(BallPositionType.Kick);
                Player.Contract.Club = Clubs[0];
                Transition(State.SimulationRoling);
                Simulation.Match = new Match(Clubs[0], Clubs[1]);
                Simulation.Start();
            }
        }

        private void LoadCountries()
        {
            Country = new List<string>();
            try
            {
                Country.AddRange(File.ReadAllText("Content/Contries").Split('\n'));
            }
            catch (Exception ex)
            {

            }
        }

        void transition_Finish()
        {
            MouseCursor = MouseCursor.Normal;

            if (!transition.FadeIn)
            {
                CurrentState = nextState;
                transition.Rollback();
            }
            else
            {
                transition.Animating = false;
            }
        }

        private void LoadScenes()
        {
            Scenes = new Dictionary<State, Scene>();
            Scenes.Add(State.IngameKickResult, new KickResultScene(this));
            Scenes.Add(State.IngameKickZoom, new KickZoomScene(this));
            Scenes.Add(State.SimulationRoling, new SimulationScene(this));
            Scenes.Add(State.IngamePlayerPossession, new PlayerPossessionScene(this));
            Scenes.Add(State.Lobby, new LobbyScene(this));
            Scenes.Add(State.NewCareer, new NewCareerScene(this));
            Scenes.Add(State.SignContract, new SignContractScene(this));
            Scenes.Add(State.Newspaper, new NewspaperScene(this));
            Scenes.Add(State.NewspaperChampion, new NewspaperChampionScene(this));
            Scenes.Add(State.MainMenu, new MainMenuScene(this));
            Scenes.Add(State.LeagueFixtures, new LeagueFixturesScene(this));
            Scenes.Add(State.PlayerStats, new PlayerStatsScene(this));
            Scenes.Add(State.SeasonResults, new SeasonResultsScene(this));
            Scenes.Add(State.Business, new BusinessScene(this));
            Scenes.Add(State.Shop, new ShopScene(this));
        }

        private void LoadGraphics()
        {
            Graphics.LoadGraphics(Content);
            
            UserInterface.MessageBox = Content.Load<Texture2D>("Backgrounds/MessageBox");

            UserInterface.Cursor = Content.Load<Texture2D>("Mouse/Cursor1");
            UserInterface.ClickCursor = Content.Load<Texture2D>("Mouse/Cursor2");

            UserInterface.MainNewCareer = Content.Load<Texture2D>("UI/Button Labels/New career");
            UserInterface.MainLoadCareer = Content.Load<Texture2D>("UI/Button Labels/Load career");
            UserInterface.ButtonBlue = Content.Load<Texture2D>("UI/menu button");
            UserInterface.ButtonGreen = Content.Load<Texture2D>("UI/green button");
            UserInterface.ButtonRed = Content.Load<Texture2D>("UI/red button");
            UserInterface.LobbyButton = Content.Load<Texture2D>("UI/Lobby Button");
            UserInterface.LabelNextGame = Content.Load<Texture2D>("UI/Button Labels/Next Game");

            Fonts.DefaultFont = Content.Load<SpriteFont>("Fonts/SpriteFont1");
            Fonts.BigFont = Content.Load<SpriteFont>("Fonts/BigFont");
            Fonts.MediumFont = Content.Load<SpriteFont>("Fonts/MediumFont");
            Fonts.Pixelade90 = Content.Load<SpriteFont>("Fonts/Pixelade90");
            Fonts.Impact26 = Content.Load<SpriteFont>("Fonts/Impact26");
            Fonts.Arial54 = Content.Load<SpriteFont>("Fonts/Arial54");
            Fonts.Arial26 = Content.Load<SpriteFont>("Fonts/Arial26");
            Fonts.Arial36 = Content.Load<SpriteFont>("Fonts/Arial36");
            Fonts.Arial12 = Content.Load<SpriteFont>("Fonts/Arial12");
            Fonts.Arial18 = Content.Load<SpriteFont>("Fonts/Arial18");
            Fonts.Arial20 = Content.Load<SpriteFont>("Fonts/Arial20");
            Fonts.Arial42 = Content.Load<SpriteFont>("Fonts/Arial42");
            Fonts.MaybeMaybeNot = Content.Load<SpriteFont>("Fonts/MaybeMaybeNot");
            Fonts.TimesNewRoman26 = Content.Load<SpriteFont>("Fonts/TimesNR26");
        }

        public void PreparePlayers(SimulationStep simulationStep)
        {
            FieldRegions.Attack = new Rectangle(281 + GameField.X, 150 + GameField.Y, 395, 90);
            FieldRegions.MidAttack = new Rectangle(277 + GameField.X, 397 + GameField.Y, 395, 90);
            FieldRegions.MidField = new Rectangle(277 + GameField.X, 510 + GameField.Y, 395, 90);

            Texture2D friend = Content.Load<Texture2D>("Player/Player2");
            Texture2D enemy = Content.Load<Texture2D>("Player/Player1");
            Texture2D keeper = Content.Load<Texture2D>("Player/Keeper");

            Players = new List<Character>();

            CreatePlayers(PlayerType.Friend, FieldRegions.Attack, 2, friend);
            CreatePlayers(PlayerType.Friend, FieldRegions.MidAttack, 2, friend);
            CreatePlayers(PlayerType.Friend, FieldRegions.MidField, 2, friend);

            CreatePlayers(PlayerType.Adversary, FieldRegions.Attack, 2, enemy);
            CreatePlayers(PlayerType.Adversary, FieldRegions.MidAttack, 2, enemy);
            CreatePlayers(PlayerType.Adversary, FieldRegions.MidField, 2, enemy);

            Character gk = new Character();
            gk.Texture = keeper;
            gk.Speed = 3f;
            gk.Position = new Vector2(
                GameField.X + 476 + rand.Next(-20, 20),
                GameField.Y + 88);
            gk.Type = PlayerType.Keeper;

            Players.Add(gk);
        }

        private void CreatePlayers(PlayerType type, Rectangle region, int amount, Texture2D texture)
        {
            for (int i = 0; i < amount; i++)
            {
                Character p = new Character();
                p.Texture = texture;
                p.Type = type;
                p.Position = GetRandomPosition(region);

                if (p.Type == PlayerType.Friend)
                {
                    p.Uniform = PlayerClub.Uniform;
                    p.UniformColor = PlayerClub.MainColor;
                }
                else if(p.Type == PlayerType.Adversary)
                {
                    p.Uniform = Simulation.EnemyTeam.Uniform;

                    if(Simulation.EnemyTeam.MainColor == PlayerClub.MainColor)
                        p.UniformColor = Simulation.EnemyTeam.SecondColor;
                    else
                        p.UniformColor = Simulation.EnemyTeam.MainColor;
                }

                bool tooClose = false;

                for (int j = 0; j < Players.Count; j++)
                {
                    do
                    {
                        tooClose = (p.Position - Players[j].Position).Length() < 25;
                        if (tooClose)
                        {
                            p.Position = GetRandomPosition(region);
                            break;
                        }
                    }
                    while (tooClose);
                }

                Players.Add(p);
            }
        }

        private Vector2 GetRandomPosition(Rectangle region)
        {
            return new Vector2(
                    rand.Next(region.X, region.X + region.Width),
                    rand.Next(region.Y, region.Y + region.Height));
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            InputInfo.MouseState = Mouse.GetState();
            InputInfo.KeyboardState = Keyboard.GetState();

            PlayedSpan = PlayedSpan.Add(new TimeSpan(0, 0, 0, 0, (int) gameTime.ElapsedGameTime.TotalMilliseconds));

            if (IsActive)
            {

                if (InputInfo.MouseState.LeftButton == ButtonState.Released && InputInfo.LastMouseState.LeftButton == ButtonState.Pressed)
                {
                    MouseClick(MouseButton.Left);
                }
                else if (InputInfo.MouseState.LeftButton == ButtonState.Pressed)
                {
                    MouseDown(MouseButton.Left);
                }

                if (InputInfo.MouseState.RightButton == ButtonState.Released && InputInfo.LastMouseState.RightButton == ButtonState.Pressed)
                {
                    MouseClick(MouseButton.Right);
                }
                else if (InputInfo.MouseState.RightButton == ButtonState.Pressed)
                {
                    MouseDown(MouseButton.Right);
                }

                Scenes[CurrentState].Update(gameTime);
                
                if (transition != null && transition.Animating)
                {
                    transition.Update(gameTime);
                }

                InputInfo.LastMouseState = InputInfo.MouseState;
                InputInfo.LastKeyboardState = InputInfo.KeyboardState;
                base.Update(gameTime);
            }
        }

        private void MouseDown(MouseButton mouseButton)
        {
            Scenes[CurrentState].MouseDown(mouseButton);
        }

        private void MouseClick(MouseButton mouseButton)
        {
            Scenes[CurrentState].MouseClick(mouseButton);

        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LightGray);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            Scenes[CurrentState].Draw(spriteBatch);
            if (transition != null && transition.Animating)
            {
                transition.Draw(spriteBatch);
            }

            if (MouseCursor == MouseCursor.MouseOver)
            {
                IsMouseVisible = false;
                spriteBatch.Draw(UserInterface.ClickCursor, new Rectangle(InputInfo.MousePositionPoint.X, InputInfo.MousePositionPoint.Y, UserInterface.ClickCursor.Width, UserInterface.ClickCursor.Height), Color.White);
            }
            else
            {
                IsMouseVisible = true;
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }

        public void ResetBall(BallPositionType position)
        {
            Rectangle shotPositions = new Rectangle((int)GameField.X + 308, (int)GameField.Y + 196, 338, 196);
            Rectangle passPositions = new Rectangle((int)GameField.X + 192, (int)GameField.Y + 338, 590, 209);

            Random rand = new Random(DateTime.Now.Millisecond);


            if (position == BallPositionType.Kick)
            {
                IngameBall.Position = new Vector2(
                    rand.Next(shotPositions.X, shotPositions.X + shotPositions.Width),
                    rand.Next(shotPositions.Y, shotPositions.Y + shotPositions.Height));
            }
            else
            {
                IngameBall.Position = new Vector2(
                    rand.Next(passPositions.X, passPositions.X + passPositions.Width),
                    rand.Next(passPositions.Y, passPositions.Y + passPositions.Height));
            }

            for (int i = 0; i < Players.Count; i++)
            {
                if ((Players[i].Position - IngameBall.Position).Length() < 60)
                    ResetBall(position);
            }

            IngameBall.Kicked = false;
            IngameBall.Height = 0;
        }


        internal void PlayerEventEnded(KickResult kickResult)
        {
            switch (kickResult)
            {
                case KickResult.None:
                    break;
                case KickResult.LostBall:
                    Simulation.GameEvents.Add(Simulation.CurrentTime + " But they lose possession...");
                    if (IngameBall.PlayerKick)
                        Simulation.GameStatistics.PlayerLostBalls++;
                    break;
                case KickResult.Pass:
                    Simulation.GameEvents.Add(Simulation.CurrentTime + " "+Player.Name+" passes the ball around.");
                    break;
                case KickResult.Goal:
                    Simulation.GameEvents.Add(Simulation.CurrentTime + " AND " + Player.Name.ToUpper() + " SCORES!");
                    break;
                case KickResult.Assist:
                    Simulation.GameEvents.Add(Simulation.CurrentTime + " A BEAUTIFUL ASSIST! " + PlayerClub.Name.ToUpper() + " SCORES!");
                    break;
                case KickResult.KeeperCaught:
                    if(IngameBall.PlayerKick)
                        Simulation.GameStatistics.PlayerLostBalls++;
                    Simulation.GameEvents.Add(Simulation.CurrentTime + " But the keeper has it.");
                    break;
                default:
                    break;
            }

        }

        public void FirstContract()
        {
            Transition(State.SignContract);
            ContractProposition = new Contract();
            ContractProposition.Club = Clubs[rand.Next(0, Clubs.Count)];
            ContractProposition.Value = rand.Next(100, 200);
            ContractProposition.GoalBonus = 0;
            ContractProposition.VictoryBonus = 0;

            NewGame();
        }

        public void Save()
        {
            GameSave save = new GameSave();

            save.Player = Player;
            save.League = League;
            save.CurrentRound = CurrentLeagueRound;

            save.Save();
        }

        internal void Transition(State state)
        {
            nextState = state;
            transition.Start();
        }
        
        public void RoundResults()
        {
            List<Match> matches = League.Matches.FindAll(x => x.Round == CurrentLeagueRound);

            for (int i = 0; i < matches.Count; i++)
            {
                if (matches[i].Away.Name == PlayerClub.Name || matches[i].Home.Name == PlayerClub.Name)
                {
                    matches[i].ResultAway = Simulation.AwayScore;
                    matches[i].ResultHome = Simulation.HomeScore;
                }
                else
                {
                    // simulate

                    matches[i].ResultHome = rand.Next(0, (int)Math.Ceiling((float)matches[i].Home.Rating / 20) + 1);
                    matches[i].ResultAway = rand.Next(0, (int)Math.Ceiling((float)matches[i].Away.Rating / 20) + 1);
                }

                LeagueStanding sH = League.Standings.Find(x => x.Club.Name == matches[i].Home.Name);
                LeagueStanding sA = League.Standings.Find(x => x.Club.Name == matches[i].Away.Name);

                sH.ProGoals += matches[i].ResultHome;
                sH.ConGoals += matches[i].ResultAway;

                sA.ConGoals += matches[i].ResultHome;
                sA.ProGoals += matches[i].ResultAway;

                if(matches[i].ResultHome >  matches[i].ResultAway)
                {
                    sH.Wins++;
                    sA.Losses++;
                }
                else if(matches[i].ResultHome <  matches[i].ResultAway)
                {
                    sH.Losses++;
                    sA.Wins++;
                }
                else if(matches[i].ResultHome ==  matches[i].ResultAway)
                {
                    sH.Draws++;
                    sA.Draws++;
                }
            }


            CurrentLeagueRound++;

            if (CurrentLeagueRound == League.Rounds)
            {
                // League ending
                EndSeason();
            }
        }

        public void EndSeason()
        {
            League.SortStandings();
            CurrentLeagueChampion = League.Standings[0].Club.Name == PlayerClub.Name;

            if (CurrentLeagueChampion)
            {
                Player.Stats.Fame += 10;
                Transition(State.NewspaperChampion);
            }
            else
                Transition(State.SeasonResults);
        }

        public void NextSeason()
        {
            Year++;
            Transition(State.Lobby);
            Player.AssistsYear = 0;
            Player.GoalsYear = 0;
            Player.GamesPlayedYear = 0;
            Player.Stats.Age++;
            CurrentLeagueRound = 0;
            LeaguePosition = 0;
            League.SetupNewLeague(Clubs);
            Save();
        }

        internal void EndRound()
        {
            RoundResults();
            Player.Money += Player.Contract.Value;
            Player.Money += Player.Contract.GoalBonus * Simulation.GameStatistics.PlayerGoals;

            Player.Goals += Simulation.GameStatistics.PlayerGoals;
            Player.Assists += Simulation.GameStatistics.PlayerAssists;
            Player.GamesPlayed++;

            Player.AssistsYear += Simulation.GameStatistics.PlayerAssists;
            Player.GoalsYear += Simulation.GameStatistics.PlayerGoals;
            Player.GamesPlayedYear++;

            League.SortStandings();
            LeaguePosition = League.Standings.FindIndex(x => x.Club.Name == PlayerClub.Name) + 1;

            if (Simulation.GameStatistics.PlayerGoals > 2)
            {
                Player.Stats.Fame++;
                if (Simulation.GameStatistics.PlayerAssists > 1)
                    Player.Stats.Fame += 2;
            }
            if (Simulation.GameStatistics.PlayerGoals > 5)
            {
                Player.Stats.Fame += 3;
            }


            if ((Simulation.Match.Home.Name == Player.Contract.Club.Name && Simulation.HomeScore > Simulation.AwayScore) ||
                (Simulation.Match.Home.Name != Player.Contract.Club.Name && Simulation.HomeScore < Simulation.AwayScore))
            {
                Player.Money += Player.Contract.VictoryBonus;
                Player.Stats.Fame += 1;

                // Se a sequencia for negativa, é de derrotas, então zero antes de botar a primeira vitória
                if (WinSequence < 0)
                    WinSequence = 0;
                WinSequence++;

                
            }
            else
            {
                // Se não venceu esse jogo, ou reseto a sequencia de vitorias ou aumento sequencia de derrotas
                if(WinSequence <= 0)
                    WinSequence--;
                else
                    WinSequence = -1;

                // mais de 5 jogos sem vencer
                if (WinSequence < -5)
                {
                    Player.Stats.Fame -= 5;
                }
            }
        }
    }
}
