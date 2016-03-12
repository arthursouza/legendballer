using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BrazucaLibrary.Util
{
    public static class Log
    {
        public static List<string> ScreenLog
        {
            get;
            set;
        }

        public static void LogErro(Exception ex)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("##");
            sb.AppendLine(DateTime.Now.ToString("HH:mm:ss dd/MM/yyyy") + " # Erro: " + ex.Message);
            sb.AppendLine(ex.StackTrace);
            sb.AppendLine("##");
            StreamWriter sw;
            sw = File.AppendText("errorlog.log");
            sw.Write(sb.ToString());
            sw.Close();            
        }
    }
}
