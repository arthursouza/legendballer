using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Baller.Library.Characters;
using Baller.Library.Input;
using Baller.Library.Leagues;
using Baller.Library.Scenes;
using Baller.Library.Simulation;
using Baller.Library.UI;
using Baller.Library.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using MouseCursor = Baller.Library.Input.MouseCursor;

namespace Baller.Library
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class BallerGame : Microsoft.Xna.Framework.Game
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

        public static bool DebugMode = false;

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

        // Field Bounds
        public float GoalInsideGrassArea { get; set; }
        public float GoalHeight { get; set; }
        public Rectangle LeftBar { get; set; }
        public Rectangle RightBar { get; set; }
        public Rectangle SmallAreaBounds { get; set; }
        public Rectangle GoalShadowBounds { get; set; }
        public Vector2 LastBallPosition { get; set; }

        private Transition transition;
        private State nextState;
        Dictionary<State, Scene> Scenes;

        public BallerGame()
        {
            PlayedSpan = new TimeSpan();
            graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
            Window.Title = "Legend Baller";
            Narration = new Narration();
            
            var resolutionDownscale = 0.5f;

            WindowSize = new Vector2(1080, 1920);
            WindowSize = new Vector2(WindowSize.X * resolutionDownscale, WindowSize.Y * resolutionDownscale);
            
            graphics.PreferredBackBufferWidth = (int)WindowSize.X;
            graphics.PreferredBackBufferHeight = (int)WindowSize.Y;
            MouseCursor = MouseCursor.Normal;
            IsMouseVisible = true;
            Player = new Player();
            Year = 2016;
        }

        protected override void Initialize()
        {
            Simulation = new MatchSimulation(this);
            Simulation.GameEvent += Simulation_GameEvent;
            CurrentState = State.MainMenu;
            Players = new List<Character>();
            Graphics.GraphicsDevice = this.GraphicsDevice;
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

            MessageBox.DefaultWindowPosition = new Vector2(
                WindowSize.X / 2 - MessageBox.DefaultWindowSize.X / 2,
                WindowSize.Y / 2 - MessageBox.DefaultWindowSize.Y / 2);
            
            IngameBall = new Ball();
            IngameBall.Animated = true;
            IngameBall.CollisionRadius = 7;
            IngameBall.BallRadius = 7;
            IngameBall.Texture = Graphics.IngameBall;
            IngameBall.ShadowTexture = Graphics.IngameBallShadow;
            
            InputInfo.LastMouseState = Mouse.GetState();
            InputInfo.LastKeyboardState = Keyboard.GetState();

            transition = new Transition(this);
            transition.Finish += transition_Finish;

            SetupFieldSize();

            LoadScenes();
            LoadCountries();
            LoadClubs();

            //if (DebugMode)
            //{
            //    //CurrentState = State.IngamePlayerPossession;
            //    //PreparePlayers(SimulationStep.ShotAttempt);
            //    //ResetBall(BallPositionType.Kick);
            //    Player.Contract.Club = Clubs[0];
            //    Transition(State.SimulationRoling);
            //    Simulation.Match = new Match(Clubs[0], Clubs[1]);
            //    Simulation.Start();
            //}
        }

        private void SetupFieldSize()
        {
            var fieldScale = 800f/1200f;
            var goalBarWidth = 2f;
            var innerGoalHeight = 34f;
            GoalInsideGrassArea = 35f * fieldScale;

            GoalHeight = 5f;
            IngameBall.MaxHeight = 12f;
            
            GameField = new Rectangle((int) ((WindowSize.X - ((float) Graphics.FieldBounds.Width*fieldScale))/2),
                (int) (WindowSize.Y - ((float) Graphics.FieldBounds.Height*fieldScale)),
                (int) (Graphics.FieldBounds.Width*fieldScale),
                (int) (Graphics.FieldBounds.Height*fieldScale));

            GoalBounds = new Rectangle(
                (int) (WindowSize.X/2 - (Graphics.Goal.Width*fieldScale)/2),
                (int) (GameField.Y - (Graphics.Goal.Height*fieldScale)) + 2,
                (int) (Graphics.Goal.Width*fieldScale),
                (int) (Graphics.Goal.Height*fieldScale));

            GoalShadowBounds = new Rectangle(
                (int) (WindowSize.X/2 - (Graphics.Goal.Width*fieldScale)/2),
                (int) (GameField.Y - (Graphics.Goal.Height*fieldScale)),
                (int) (Graphics.GoalShadow.Width*fieldScale),
                (int) (Graphics.GoalShadow.Height*fieldScale));

            GoalInnerBounds = new Rectangle(
                (int) (GoalBounds.X + goalBarWidth),
                GameField.Y - (int) (innerGoalHeight*fieldScale) + 2,
                (int) (GoalBounds.Width - (goalBarWidth*2)),
                (int) (innerGoalHeight*fieldScale));

            SmallAreaBounds = new Rectangle((int) (WindowSize.X/2), (int) GameField.Y, (int) (240*fieldScale),
                (int) (70*fieldScale));

            var playableAreaSize = new Vector2(800, 700) * fieldScale;

            var playableArea = new Rectangle(
                (int) (GameField.Width - playableAreaSize.X)/2 + GameField.X,
                (int) (GameField.Height - playableAreaSize.Y)/2 + GameField.Y,
                (int) playableAreaSize.X,
                (int) playableAreaSize.Y);

            LeftBar = new Rectangle(GoalBounds.X, GameField.Y - 50, 4, 50);
            RightBar = new Rectangle(GoalBounds.X + GoalBounds.Width - 4, GameField.Y - 50, 4, 50);

            FieldRegions.Keeper = new Rectangle(GoalBounds.X + ((GoalBounds.Width / 4) * 2) - GoalBounds.Width / 4, GameField.Y, GoalBounds.Width / 2, 50);
            FieldRegions.Attack = new Rectangle(playableArea.X, playableArea.Y, playableArea.Width, playableArea.Height / 3);
            FieldRegions.MidAttack = new Rectangle(playableArea.X, playableArea.Height / 3 + playableArea.Y, playableArea.Width, playableArea.Height / 3);
            FieldRegions.MidField = new Rectangle(playableArea.X, playableArea.Height / 3 * 2 + playableArea.Y, playableArea.Width, playableArea.Height / 3);
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

            Fonts.Arial12 = Content.Load<SpriteFont>("Fonts/Arial12");
            Fonts.Arial18 = Content.Load<SpriteFont>("Fonts/Arial18");
            Fonts.Arial20 = Content.Load<SpriteFont>("Fonts/Arial20");
            Fonts.Arial26 = Content.Load<SpriteFont>("Fonts/Arial26");
            Fonts.Arial36 = Content.Load<SpriteFont>("Fonts/Arial36");
            Fonts.Arial42 = Content.Load<SpriteFont>("Fonts/Arial42");
            Fonts.Arial54 = Content.Load<SpriteFont>("Fonts/Arial54");
            Fonts.TimesNewRoman26 = Content.Load<SpriteFont>("Fonts/TimesNR26");
        }

        public void PreparePlayers(SimulationStep simulationStep)
        {
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
                rand.Next(FieldRegions.Keeper.X, FieldRegions.Keeper.X + FieldRegions.Keeper.Width),
                rand.Next(FieldRegions.Keeper.Y, FieldRegions.Keeper.Y + FieldRegions.Keeper.Height));

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
            var requiredDistance = 50f;
            var requiredDistanceToBall = 90;

            var position = new Vector2(
                rand.Next(region.X, region.X + region.Width),
                rand.Next(region.Y, region.Y + region.Height));

            if (Players == null || !Players.Any())
            {
                return position;
            }

            var free = false;

            do
            {
                foreach (var p in Players)
                {
                    var distance = (p.Position - position).Length();
                    var distanceToBall = (IngameBall.Position - position).Length();

                    free = distance > requiredDistance && distanceToBall > requiredDistanceToBall;

                    if (free == false)
                    {
                        position = new Vector2(rand.Next(region.X, region.X + region.Width), rand.Next(region.Y, region.Y + region.Height));
                        break;
                    }
                }

            } while (free == false);

            return position;
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
            PlayedSpan = PlayedSpan.Add(new TimeSpan(0, 0, 0, 0, (int) gameTime.ElapsedGameTime.TotalMilliseconds));

            #if ANDROID

            UpdateAndroid(gameTime);

            #endif

            #region Not Android
            #if !ANDROID
            
            InputInfo.MouseState = Mouse.GetState();
            InputInfo.KeyboardState = Keyboard.GetState();

            if (IsActive)
            {
                if (InputInfo.MouseState.LeftButton == ButtonState.Released && InputInfo.LastMouseState.LeftButton == ButtonState.Pressed)
                {
                    MainInput(InputInfo.MousePosition);
                }
                else if (InputInfo.MouseState.LeftButton == ButtonState.Pressed)
                {
                    MainInput(InputInfo.MousePosition);
                }
            
                InputInfo.LastMouseState = InputInfo.MouseState;
                InputInfo.LastKeyboardState = InputInfo.KeyboardState;
            }

            #endif
            #endregion
            
            Scenes[CurrentState].Update(gameTime);
                
            if (transition != null && transition.Animating)
            {
                transition.Update(gameTime);
            }
            
            #region Not Android
            #if !ANDROID
            if (IsActive)
            {
                InputInfo.LastMouseState = InputInfo.MouseState;
                InputInfo.LastKeyboardState = InputInfo.KeyboardState;
            }
            #endif
            #endregion

            base.Update(gameTime);
        }

        private void UpdateAndroid(GameTime gameTime)
        {
            TouchCollection touchCollection = TouchPanel.GetState();

            if (touchCollection.Count > 0)
            {
                //Only Fire Select Once it's been released
                if (touchCollection[0].State == TouchLocationState.Moved ||
                    touchCollection[0].State == TouchLocationState.Pressed)
                {
                    Scenes[CurrentState].MainInput(touchCollection[0].Position);
                }
            }
        }
        
        private void MainInput(Vector2 pos)
        {
            Scenes[CurrentState].MainInput(pos);
        }

        //private void MouseDown(MouseButton mouseButton)
        //{
        //    Scenes[CurrentState].MouseDown(mouseButton);
        //}

        //private void MouseClick(MouseButton mouseButton)
        //{
        //    Scenes[CurrentState].MouseClick(mouseButton);

        //}

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

        public void DrawField()
        {
            spriteBatch.Draw(Graphics.FieldBackground, new Rectangle(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height), Color.White);
            spriteBatch.Draw(Graphics.FieldBounds, GameField, Color.White);
            spriteBatch.Draw(Graphics.GoalShadow, GoalShadowBounds, Color.White);
            spriteBatch.Draw(Graphics.Goal, GoalBounds, Color.White);

            Players.ForEach(p => spriteBatch.Draw(Graphics.PlayerMarker,
                        new Rectangle(
                            (int)(p.Position.X - Graphics.PlayerMarker.Width / 2),
                            (int)(p.Position.Y - Graphics.PlayerMarker.Height / 2),
                            Graphics.PlayerMarker.Width,
                            Graphics.PlayerMarker.Height),
                            (p.Type == PlayerType.Friend ? Color.Blue : Color.Red) * .4f));

            if (!BallInsideGoal())
            { spriteBatch.Draw(Graphics.GoalTopNet, GoalBounds, Color.White); }

            List<GameObject> renderList = new List<GameObject>();
            renderList.Add(IngameBall);
            renderList.AddRange(Players);
            renderList.Sort();
            renderList.ForEach(o => o.Draw(spriteBatch));

            //Players.ForEach(p => spriteBatch.Draw(Graphics.Circle, new Rectangle((int)(p.Position.X - p.VisionRange), (int)(p.Position.Y - p.VisionRange), (int)(p.VisionRange * 2), (int)p.VisionRange * 2), Color.Orange));

            if(BallInsideGoal())
            { spriteBatch.Draw(Graphics.GoalTopNet, GoalBounds, Color.White);}

            //spriteBatch.Draw(Graphics.Selected, GoalBounds, Color.Yellow);
            //spriteBatch.Draw(Graphics.Selected, new Rectangle(GoalBounds.X, GoalBounds.Y + GoalBounds.Height - (int)GoalInsideGrassArea, GoalBounds.Width, (int)GoalInsideGrassArea), Color.Orange);

            if (DebugMode)
            {
                //spriteBatch.Draw(Graphics.Selected, FieldRegions.Keeper, Color.Orange);

                //spriteBatch.Draw(Graphics.Selected, LeftBar, Color.Red);
                //spriteBatch.Draw(Graphics.Selected, RightBar, Color.Red);
                //spriteBatch.Draw(Graphics.Selected, new Rectangle((int)(IngameBall.Position.X - IngameBall.CollisionRadius), (int)(IngameBall.Position.Y - IngameBall.CollisionRadius), (int)IngameBall.CollisionRadius *2, (int)IngameBall.CollisionRadius * 2), Color.Red);
                //spriteBatch.Draw(Graphics.Black, IngameBall.CollisionBounds, Color.Yellow);
                //spriteBatch.Draw(Graphics.Selected, GameField, Color.Yellow);

                //spriteBatch.DrawString(Fonts.Arial12, InputInfo.MousePosition.ToString(), InputInfo.MousePosition, Color.White);
            }

            string text = Simulation.CurrentTime.ToString("00");
            spriteBatch.DrawString(Fonts.Arial54, text, new Vector2(20, 0), Color.White);
        }

        public bool BallInsideGoal()
        {
            var belowTopBar = IngameBall.Height < GoalHeight;
            var pastEndLine = IngameBall.Position.Y + IngameBall.CollisionRadius < GameField.Y;
            //var insideGoalArea = ball.Position.Y - ball.CollisionRadius >= Game.GoalInnerBounds.Y;

            var betweenTwoBars = (IngameBall.Position.X - IngameBall.CollisionRadius > GoalBounds.X && IngameBall.Position.X + IngameBall.CollisionRadius < GoalBounds.X + GoalBounds.Width);

            return belowTopBar && pastEndLine && betweenTwoBars;
        }

        public void SetGameResolution(int width, int height)
        {
            WindowSize = new Vector2(width, height);
            graphics.PreferredBackBufferWidth = (int)WindowSize.X;
            graphics.PreferredBackBufferHeight = (int)WindowSize.Y;
        }
    }
}
