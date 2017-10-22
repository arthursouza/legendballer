using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LegendBaller.Library.Leagues;
using Microsoft.Xna.Framework;

namespace SimulationTester
{
    class Program
    {
        static void Main(string[] args)
        {
            LoadClubs();
            
            var simulations = 100;


        }

        private static void LoadClubs()
        {
            Clubs = new List<Club>();
            Clubs.Add(new Club(1, "Vulture RN", 70, 100));
            Clubs.Add(new Club(2, "SP Football", 75, 70));
            Clubs.Add(new Club(3, "SC Hawks", 80, 95));
            Clubs.Add(new Club(4, "CR Oranges", 70, 40));
            Clubs.Add(new Club(5, "Maltese Cross", 50, 60));
            Clubs.Add(new Club(6, "La Lezione", 70, 60));
            Clubs.Add(new Club(7, "Lonely Star FC", 70, 40));
            Clubs.Add(new Club(8, "Salvatore", 40, 40));
            Clubs.Add(new Club(9, "Victory", 40, 40));
            Clubs.Add(new Club(10, "Montain Club", 3, 3));
        }

        public static List<Club> Clubs { get; set; }
    }
}
