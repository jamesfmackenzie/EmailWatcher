using System;

namespace EmailWatcher.Internal.Loggers
{
    class Logger
    {
        private Logger()
        {
            
        }

        public static void LogInfo(string message)
        {
            Console.WriteLine(message);
        }

        public static void LogError(string message)
        {
            Console.WriteLine(message);
        }
    }
}
