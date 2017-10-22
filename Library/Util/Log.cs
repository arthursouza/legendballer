using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LegendBaller.Library.Util
{
    public static class Log
    {
        public static List<string> ScreenLog
        {
            get;
            set;
        }

        public static void Erro(Exception ex)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"{DateTime.Now.ToString("HH:mm:ss dd/MM/yyyy")} # Erro:");
            sb.AppendLine(ex.ToString());
            sb.AppendLine("###################");
            StreamWriter sw;
            sw = File.AppendText("errorlog.log");
            sw.Write(sb.ToString());
            sw.Close();            
        }
    }
}
