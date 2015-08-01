using System;
using System.Collections.Generic;
using EmailWatcher.Internal.Adapters;
using EmailWatcher.Internal.Constants;
using EmailWatcher.Internal.Factories;
using EmailWatcher.Internal.Loggers;
using EmailWatcher.Internal.Translators;
using EmailWatcher.Public;

namespace EmailWatcher.Internal.Services
{
    class EmailService : IEmailService
    {
        private const int Port = 110;
        private const bool UseSsl = false;

        private readonly string _host;
        private readonly string _username;
        private readonly string _password;
        private readonly IPopClientFactory _factory;
        private readonly MessageTranslator _translator;

        public EmailService(EmailWatcherOptions options, IPopClientFactory factory, MessageTranslator translator)
        {
            if (options == null)
            {
                Logger.LogError(ExceptionMessageConstants.EmailOptionsCannotBeNull);
                throw new Exception();
            }

            if (factory == null)
            {
                Logger.LogError(ExceptionMessageConstants.PopClientFactoryCannotBeNull);
                throw new Exception();
            }

            if (translator == null)
            {
                Logger.LogError(ExceptionMessageConstants.EmailWatcherMessageTranslatorCannotBeNull);
                throw new Exception();
            }

            _host = options.Host;
            _username = options.Username;
            _password = options.Password;
            _factory = factory;
            _translator = translator;
        }

        public List<EmailWatcherMessage> FetchAllMessages()
        {
            List<EmailWatcherMessage> messages = new List<EmailWatcherMessage>();

            try
            {
                using (IPopClientAdapter client = _factory.CreatePopClientAdapter())
                {
                    client.Connect(_host, Port, UseSsl);
                    client.Authenticate(_username, _password);

                    int messageCount = client.GetMessageCount();
                    for (int i = messageCount; i > 0; i--)
                    {
                        EmailWatcherMessage message = _translator.Translate(client.GetMessage(i));
                        messages.Add(message);
                    }
                }
            }
            catch (Exception e)
            {
                Logger.LogError("Error downloading emails: " + e);
            }

            return messages;
        }

        public void DeleteMessage(string messageId)
        {
            // todo: put in info level logging
            
            if (string.IsNullOrEmpty(messageId))
            {
                Logger.LogError(ExceptionMessageConstants.MessageIdCannotBeNull);
                throw new Exception();
            }

            try
            {
                using (IPopClientAdapter client = _factory.CreatePopClientAdapter())
                {
                    client.Connect(_host, Port, UseSsl);
                    client.Authenticate(_username, _password);

                    int messageCount = client.GetMessageCount();
                    for (int i = messageCount; i > 0; i--)
                    {
                        if (client.GetMessage(i).Headers.MessageId == messageId)
                        {
                            client.DeleteMessage(i);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Logger.LogError("Error deleting emails: " + e);
            }
        }
    }
}