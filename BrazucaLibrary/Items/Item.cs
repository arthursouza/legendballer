using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrazucaLibrary.Characters;

namespace BrazucaLibrary.Items
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
