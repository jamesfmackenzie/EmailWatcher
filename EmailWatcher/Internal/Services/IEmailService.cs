using System.Collections.Generic;
using EmailWatcher.Public;

namespace EmailWatcher.Internal.Services
{
    internal interface IEmailService
    {
        List<EmailWatcherMessage> FetchAllMessages();
        void DeleteMessage(string messageId);
    }
}