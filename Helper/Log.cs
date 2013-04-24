using System.Windows.Media;
using Styx.Common;

namespace IWantMovement.Helper
{
    class Log
    {

        public static void Info(string logText, params object[] args)
        {
            if (logText == null) return;
            Logging.Write(LogLevel.Normal, Colors.LawnGreen, "[IWM]: {0}", string.Format(logText, args));
        }

        public static void Warning(string logText, params object[] args)
        {
            if (logText == null) return;
            Logging.Write(LogLevel.Normal, Colors.Fuchsia, "[IWM]: {0}", string.Format(logText, args));
        }

        public static void Debug(string logText, params object[] args)
        {
            if (logText == null) return;
            Logging.Write(LogLevel.Diagnostic, Colors.DodgerBlue, "[IWM]: {0}", string.Format(logText, args));
        }
    }
}
