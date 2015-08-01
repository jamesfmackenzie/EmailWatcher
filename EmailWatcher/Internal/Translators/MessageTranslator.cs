using System;
using EmailWatcher.Internal.Constants;
using EmailWatcher.Internal.Loggers;
using EmailWatcher.Public;
using OpenPop.Mime;

namespace EmailWatcher.Internal.Translators
{
    internal class MessageTranslator
    {
        public EmailWatcherMessage Translate(Message message)
        {
            if (message == null)
            {
                Logger.LogError(ExceptionMessageConstants.EmailMessageCannotBeNull);
                throw new Exception();
            }

            EmailWatcherMessage toReturn = new EmailWatcherMessage
            {
                Subject = message.Headers.Subject,
                Body = message.FindFirstPlainTextVersion().GetBodyAsText(),
                Id = message.Headers.MessageId
            };

            return toReturn;
        }
    }
}