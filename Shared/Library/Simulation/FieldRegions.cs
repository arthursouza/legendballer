using Microsoft.Xna.Framework;

namespace Baller.Library.Simulation
{
    public class FieldRegions
    {
        public static Rectangle Keeper;
        public static Rectangle Attack;
        public static Rectangle MidAttack;
        public static Rectangle MidField;

        public static Rectangle KickMidfield = new Rectangle(210, 751, 650, 592);
        public static Rectangle KickClearShot = new Rectangle(210, 351,650,186);
        public static Rectangle KickAttack = new Rectangle(210, 472,650,477);

        public static Rectangle LeftBar { get; set; }
        public static Rectangle RightBar { get; set; }
        public static Rectangle SmallAreaBounds { get; set; }
        public static Rectangle GoalShadowBounds { get; set; }
        public static Rectangle GoalBounds;
        public static Rectangle GoalInnerBounds;
        
        // Field Bounds
        public static float GoalInsideGrassArea { get; set; }
        public static float GoalHeight { get; set; }
    }
}
