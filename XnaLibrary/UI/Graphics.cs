using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace LegendBaller.Library.UI
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
            GameJoltLogin = content.Load<Texture2D>("MainMenu/LoginGameJolt");
            GameJoltLoginHover = content.Load<Texture2D>("MainMenu/LoginGameJolt_Hover");

            Black = content.Load<Texture2D>("Black");
            FlatFlag = content.Load<Texture2D>("Flags/Flag Flat");
            FlagStripe = content.Load<Texture2D>("Flags/Flag Stripe");
            FlatUniform = content.Load<Texture2D>("Player/Flat");
            StripesUniform = content.Load<Texture2D>("Player/Stripes");
            Circle = content.Load<Texture2D>("Circle");
            RedSquare = content.Load<Texture2D>("RedSquare");

            NewspaperBackground = content.Load<Texture2D>("Backgrounds/Jornal");

            NewspaperSignContract = content.Load<Texture2D>("Backgrounds/NewsPaperContract");

            NewspaperChampion = content.Load<Texture2D>("Backgrounds/Jornal Championship");
            BlankBackground = content.Load<Texture2D>("Backgrounds/Light Blue Bg");

            FieldBounds = content.Load<Texture2D>("Backgrounds/FieldBounds");
            FieldBackground = content.Load<Texture2D>("Backgrounds/Field");
            Goal = content.Load<Texture2D>("Backgrounds/Goal");
            GoalTopNet = content.Load<Texture2D>("Backgrounds/GoalTopNet");
            GoalShadow = content.Load<Texture2D>("Backgrounds/GoalShadow");

            SimulationBG = content.Load<Texture2D>("Backgrounds/Simulation BG");
            LobbyBackground = content.Load<Texture2D>("Backgrounds/Game Lobby");
            MainMenu = content.Load<Texture2D>("Backgrounds/Main Menu");
            PlayerMarker = content.Load<Texture2D>("Player/PlayerMarker");
            Selected = content.Load<Texture2D>("UI/Selected");
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