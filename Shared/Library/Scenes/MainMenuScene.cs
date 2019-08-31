using Baller.Library.Input;
using Baller.Library.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Baller.Library.Scenes
{
    public class MainMenuScene : Scene
    {
        private TextureButton btnNewGame;
        private TextureButton btnLoadGame;
        //private TextureButton btnGameJoltLogin;

        MessageBox overrideSave;

        bool alerted = false;
        int selectedIndex = -1;
        Rectangle studioLogo;
        Tooltip studioToolTip;
        float tooltipTimer;

        public MainMenuScene(BallerGame game)
            : base(game)
        {
            studioLogo = new Rectangle(14, 613, 91, 91);
            studioToolTip = new Tooltip("Navigate to facebook.com/stuffgamestudio", Position.Zero);
            
            var newGame = new Vector2(BallerGame.WindowBounds.Width/2, 420);
            var loadGame = new Vector2(BallerGame.WindowBounds.Width/2, 510);
            //var gameJoltLogin = new Vector2(20, BrazucaGame.WindowBounds.Height - (Graphics.GameJoltLogin.Height + 20));

            btnNewGame = new TextureButton(UserInterface.ButtonBlue, UserInterface.MainNewCareer, newGame, true);
            btnLoadGame = new TextureButton(UserInterface.ButtonBlue, UserInterface.MainLoadCareer, loadGame, true);
            
            //btnGameJoltLogin = new TextureButton(UserInterface.ButtonBlue, Graphics.GameJoltLogin, gameJoltLogin, false);

            overrideSave = new MessageBox("There is already a save file, starting a new career will erase it.", Color.Black, Fonts.Arial26, MessageBox.MessageBoxButtonType.Ok);

            overrideSave.Close += newCareer_Close;
            overrideSave.Visible = false;
        }
        

        public override void Draw(SpriteBatch batch)
        {
            batch.Draw(Graphics.MainMenu, BallerGame.WindowBounds, Color.White);
            if (tooltipTimer > studioToolTip.Delay)
            {
                //lotuzTooltip.Draw(batch);
            }

            btnLoadGame.Draw(batch);
            btnNewGame.Draw(batch);
            //btnGameJoltLogin.Draw(batch);
            
            if (overrideSave.Visible)
                overrideSave.Draw(batch);

            base.Draw(batch);
        }

        public override void Update(GameTime gameTime)
        {
            if (btnNewGame.Bounds.Contains(InputInfo.MousePositionPoint.X, InputInfo.MousePositionPoint.Y))
            {
                selectedIndex = 0;
            }
            else if (btnLoadGame.Bounds.Contains(InputInfo.MousePositionPoint.X, InputInfo.MousePositionPoint.Y))
            {
                selectedIndex = 1;
            }
            else
                selectedIndex = -1;

            base.Update(gameTime);
        }

        public override void MainInput(Vector2 position)
        {
            if (btnNewGame.Pressed())
            {
                if (GameSave.CurrentExists() && !alerted)
                {
                    // Show override save
                    overrideSave.Visible = true;
                    alerted = true;
                }
                else
                {
                    Game.Transition(State.NewCareer);
                }
            }
            else if (btnLoadGame.Pressed())
            {
                if (GameSave.CurrentExists())
                {
                    LoadGame();
                    Game.Transition(State.Lobby);
                }
            }
            // If override save is visible
            else if (overrideSave.Visible)
            {
                overrideSave.MainInput();
            }
        }

        void newCareer_Close()
        {
            overrideSave.Visible = false;
        }

        private void LoadGame()
        {
            GameSave save = GameSave.Load();
            Game.Player = save.Player;
            Game.League = save.League;
            Game.CurrentLeagueRound = save.CurrentRound;
        }
    }
}
