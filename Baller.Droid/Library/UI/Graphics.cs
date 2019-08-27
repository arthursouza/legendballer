using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Baller.Droid.Library.UI
{
    public class Graphics
    {
        public static GraphicsDevice GraphicsDevice;

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
        
        public static Texture2D Load(string content)
        {
            using (var stream = TitleContainer.OpenStream ("Content/" + content + ".png"))
            {
                return Texture2D.FromStream (GraphicsDevice, stream);
            }
        }
        
        public static void LoadGraphics(ContentManager content)
        {
            GameJoltLogin = Load("MainMenu/LoginGameJolt");
            GameJoltLoginHover = Load("MainMenu/LoginGameJolt_Hover");

            Black = Load("Black");
            FlatFlag = Load("Flags/Flag Flat");
            FlagStripe = Load("Flags/Flag Stripe");
            FlatUniform = Load("Player/Flat");
            StripesUniform = Load("Player/Stripes");
            Circle = Load("Circle");
            RedSquare = Load("RedSquare");

            NewspaperBackground = Load("Backgrounds/Jornal");

            NewspaperSignContract = Load("Backgrounds/NewsPaperContract");

            NewspaperChampion = Load("Backgrounds/Jornal Championship");
            BlankBackground = Load("Backgrounds/Light Blue Bg");

            FieldBounds = Load("Backgrounds/FieldBounds");
            FieldBackground = Load("Backgrounds/Field");
            Goal = Load("Backgrounds/Goal");
            GoalTopNet = Load("Backgrounds/GoalTopNet");
            GoalShadow = Load("Backgrounds/GoalShadow");

            SimulationBG = Load("Backgrounds/Simulation BG");
            LobbyBackground = Load("Backgrounds/Game Lobby");
            MainMenu = Load("Backgrounds/Main Menu");
            PlayerMarker = Load("Player/PlayerMarker");
            Selected = Load("UI/Selected");
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