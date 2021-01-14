using System;
using Baller.Library;

namespace Game.Windows
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new BallerGame())
                game.Run();
        }
    }
}
