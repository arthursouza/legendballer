using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using BrazucaLibrary.Characters;
using BrazucaLibrary.Leagues;
using BrazucaLibrary.Util;

namespace BrazucaLibrary
{
    public class GameSave
    {
        public Player Player;
        public int CurrentRound;
        public League League;
        public TimeSpan PlayedTime;

        private static string currentSaveFile = "save.sg";

        private static string[] previousVersions = new[]
        {
            "old.sg"
        };

        public void Save()
        {
            Serializer<GameSave>.SerializeObject(this, currentSaveFile);
        }
        public static GameSave Load()
        {
            var path = currentSaveFile;
            return Serializer<GameSave>.DeserializeObject(path);
        }

        public static bool Exists()
        {
            foreach (var save in previousVersions)
            {
                if (File.Exists(save))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool CurrentExists()
        {
            return File.Exists(currentSaveFile);
        }
    }
}
