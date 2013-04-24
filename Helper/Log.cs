using System.Windows.Media;
using Styx.Common;

namespace IWantMovement.Helper
{
    class Log
    {

        public static void Info(string logText, params object[] args)
        {
            Logging.Write(LogLevel.Normal, Colors.LawnGreen, string.Format("[IWM]: {0} {1}", logText), args);
        }

    }
}
