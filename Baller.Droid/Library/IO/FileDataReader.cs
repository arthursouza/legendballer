using System;
using System.IO;

namespace Baller.Droid.Library.IO
{
    public class FileDataReader : FileHelper
    {
        public override string ReadFile(string name, string location)
        {
            return File.ReadAllText(Environment.CurrentDirectory + "/" + location + "/" + name);
        }

        public override void SaveFile(string name, string location, string data)
        {
            File.WriteAllText(Environment.CurrentDirectory + "/" + location + "/" + name, data);
        }
    }
}
