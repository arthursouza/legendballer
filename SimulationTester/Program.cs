using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrazucaLibrary.Leagues;
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

        public static List<Club> Clubs { get; set; }
    }
}
