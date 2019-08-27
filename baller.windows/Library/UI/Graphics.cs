using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace baller.windows.Library.UI
{
    public class Graphics
    {
        public static Texture2D FieldBackground;
        public static Texture2D Score { get; set; }
        public static Texture2D ScoreboardTimer { get; set; }
        public static Texture2D Circle { get; set; }
        public static Texture2D SimulationBG { get; set; }
        public static Texture2D RedSquare { get; set; }
        public static Texture2D LobbyBackground{ get; set; }
        public static Texture2D Selected { get; set; }
        public static Texture2D BlankBackground { get; set; }


        public static Texture2D NewspaperBackground { get; set; }
        public static Texture2D Black { get; set; }
        public static Texture2D MainMenu { get; set; }


        public static Texture2D FlatFlag { get; set; }
        public static Texture2D FlagStripe { get; set; }
        public static Texture2D FlatUniform { get; set; }
        public static Texture2D StripesUniform { get; set; }
        public static Texture2D NewspaperChampion { get; set; }
        public static Texture2D PlayerMarker { get; set; }
        public static Texture2D SelectedAlt { get; set; }

        public static void LoadGraphics(ContentManager content)
        {
            GameJoltLogin = Graphics.Load("MainMenu/LoginGameJolt");
            GameJoltLoginHover = Graphics.Load("MainMenu/LoginGameJolt_Hover");

            Black = Graphics.Load("Black");
            FlatFlag = Graphics.Load("Flags/Flag Flat");
            FlagStripe = Graphics.Load("Flags/Flag Stripe");
            FlatUniform = Graphics.Load("Player/Flat");
            StripesUniform = Graphics.Load("Player/Stripes");
            Circle = Graphics.Load("Circle");
            RedSquare = Graphics.Load("RedSquare");

            NewspaperBackground = Graphics.Load("Backgrounds/Jornal");

            NewspaperSignContract = Graphics.Load("Backgrounds/NewsPaperContract");

            NewspaperChampion = Graphics.Load("Backgrounds/Jornal Championship");
            BlankBackground = Graphics.Load("Backgrounds/Light Blue Bg");

            FieldBounds = Graphics.Load("Backgrounds/FieldBounds");
            FieldBackground = Graphics.Load("Backgrounds/Field");
            Goal = Graphics.Load("Backgrounds/Goal");
            GoalTopNet = Graphics.Load("Backgrounds/GoalTopNet");
            GoalShadow = Graphics.Load("Backgrounds/GoalShadow");

            SimulationBG = Graphics.Load("Backgrounds/Simulation BG");
            LobbyBackground = Graphics.Load("Backgrounds/Game Lobby");
            MainMenu = Graphics.Load("Backgrounds/Main Menu");
            PlayerMarker = Graphics.Load("Player/PlayerMarker");
            Selected = Graphics.Load("UI/Selected");
        }

        public static Texture2D Goal { get; set; }

        public static Texture2D GoalShadow { get; set; }

        public static Texture2D FieldBounds { get; set; }

        public static Texture2D NewspaperSignContract { get; set; }

        public static Texture2D GameJoltLogin { get; set; }

        public static Texture2D GameJoltLoginHover { get; set; }
        public static Texture2D GoalTopNet { get; set; }
    }
}