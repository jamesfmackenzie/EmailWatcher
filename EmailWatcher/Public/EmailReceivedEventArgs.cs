using System;

namespace EmailWatcher.Public
{
    public class EmailReceivedEventArgs : EventArgs
    {
        private readonly EmailWatcherMessage _message;

        public EmailReceivedEventArgs(EmailWatcherMessage message)
        {
            _message = message;
        }

        public EmailWatcherMessage Message { get { return _message; } }
    }
}
