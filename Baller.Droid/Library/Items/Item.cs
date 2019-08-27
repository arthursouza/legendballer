using Baller.Droid.Library.Characters;

namespace Baller.Droid.Library.Items
{
    public class Item
    {
        public enum ItemType
        {
            Boots,
            Car,
            House,
            Jewels,
            Consumable
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public int Cost { get; set; }
        public ItemType Type { get; set; }
        public Stats StatsBuff { get; set; }
        public int BuffDurationGames { get; set; }
    }
}
