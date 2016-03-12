using System;
using BrazucaLibrary;
using System.IO;

namespace Brazuca_2013
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
       {
            using (BrazucaGame game = new BrazucaGame())
            {
                //try
                //{
                    game.Run();
                //}
                //catch (Exception ex)
                //{
                //    Log.LogErro(ex);
                //}
            }

        }
    }
#endif
}

