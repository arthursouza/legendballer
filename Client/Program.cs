using System;
using LegendBaller.Library;
using LegendBaller.Library.Util;

namespace LegendBaller.Client
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (BrazucaGame game = new BrazucaGame())
            {
                #if DEBUG
                game.Run();
                return;
                #endif

                try
                {
                    game.Run();
                }
                catch (Exception ex)
                {
                    Log.Erro(ex);
                }
                
            }
        }
    }
}

