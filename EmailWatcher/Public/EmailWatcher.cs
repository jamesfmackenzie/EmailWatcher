using System;
using System.Threading;
using EmailWatcher.Internal.Constants;
using EmailWatcher.Internal.Factories;
using EmailWatcher.Internal.Loggers;
using EmailWatcher.Internal.Services;
using EmailWatcher.Internal.Translators;
using EmailWatcher.Internal.Validators;

namespace EmailWatcher.Public
{
    public class EmailWatcher
    {
        private readonly EmailWatcherOptions _options;
        private readonly IEmailService _service;

        public event EventHandler<EmailReceivedEventArgs> EmailReceivedEvent;

        internal EmailWatcher(EmailWatcherOptions options, IEmailWatcherOptionsValidator validator, IEmailService service)
        {
            if (validator == null)
            {
                Logger.LogError(ExceptionMessageConstants.ValidatorCannotBeNull);
                throw new Exception();
            }

            if (service == null)
            {
                Logger.LogError(ExceptionMessageConstants.EmailServiceCannotBeNull);
                throw new Exception();
            }

            if (validator.Validate(options) == false)
            {
                Logger.LogError(ExceptionMessageConstants.InvalidEmailOptions);
                throw new Exception();
            }

            _options = options;
            _service = service;
        }

        public static EmailWatcher WithOptions(EmailWatcherOptions options)
        {
            return new EmailWatcher(options, new EmailWatcherOptionsValidator(), new EmailService(options, new PopClientFactory(), new MessageTranslator()));
        }

        public void StartWatching()
        {
            while (true)
            {
                ProcessEmails();
                Thread.Sleep(_options.TimeBetweenRefreshes.Value * 1000);
            }
        }

        private void OnEmailReceived(EmailReceivedEventArgs e)
        {
            EventHandler<EmailReceivedEventArgs> handler = EmailReceivedEvent;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        internal void ProcessEmails()
        {
            var messages = _service.FetchAllMessages();
            messages.ForEach(message =>
            {
                OnEmailReceived(new EmailReceivedEventArgs(message));
                _service.DeleteMessage(message.Id);
            });
        }
    }
}
