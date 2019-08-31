using Microsoft.Xna.Framework;

namespace Baller.Library.Simulation
{
    public class FieldRegions
    {
        public static Rectangle Keeper;
        public static Rectangle Attack;
        public static Rectangle MidAttack;
        public static Rectangle MidField;

        public static Rectangle KickMidfield = new Rectangle(124, 751, 856, 592);
        public static Rectangle KickClearShot = new Rectangle(209,351,655,186);
        public static Rectangle KickAttack = new Rectangle(136,472,828,477);

        public static Rectangle LeftBar { get; set; }
        public static Rectangle RightBar { get; set; }
        public static Rectangle SmallAreaBounds { get; set; }
        public static Rectangle GoalShadowBounds { get; set; }

    }
}
