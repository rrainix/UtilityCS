using System.Diagnostics;

namespace UtilityCS
{
    public static class CmdHelper
    {
        public static void Shutdown(int afterSeconds = 0)
        {
            ShutdownCommand("/s /t", afterSeconds);
        }
        public static void StopShutdown()
        {
            ShutdownCommand("/a");
        }
        public static void Logout()
        {
            ShutdownCommand("/l", 0);
        }
        public static void SleepMode()
        {
            ShutdownCommand("/h", 0);
        }
        public static void Restart(int afterSeconds = 0)
        {
            ShutdownCommand("/r /t", afterSeconds);
        }

        private static void ShutdownCommand(string args, int afterSeconds = 0)
        {
            Process.Start("shutdown", $"{args} {(afterSeconds == 0 ?  "": afterSeconds.ToString())}");
        }

        public static void Run(string filename)
        {
            Process.Start(filename);
        }
    }
}
