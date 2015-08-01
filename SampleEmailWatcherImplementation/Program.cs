using System;
using EmailWatcher.Public;

namespace SampleEmailWatcherImplementation
{
    public class Program
    {
        private const string Pop3Hostname = "<pop server address>";
        private const string Username = "<login>";
        private const string Password = "<password>";

        public static void Main()
        {
            var options = new EmailWatcherOptions { Host = Pop3Hostname, Username = Username, Password = Password, TimeBetweenRefreshes = 30 };
            var watcher = EmailWatcher.Public.EmailWatcher.WithOptions(options);
            watcher.EmailReceivedEvent += (sender, args) => Console.WriteLine("Email Received! Id {0} Subject: {1}, Body: {2}", args.Message.Id, args.Message.Subject, args.Message.Body);
            watcher.StartWatching();
        }
    }
}
