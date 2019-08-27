using System;
using System.IO;
using Baller.Library.Characters;
using Baller.Library.Util;

namespace Baller.Library
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
