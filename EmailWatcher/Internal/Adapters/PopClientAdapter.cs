using OpenPop.Mime;
using OpenPop.Pop3;

namespace EmailWatcher.Internal.Adapters
{
    class PopClientAdapter : IPopClientAdapter
    {
        private readonly Pop3Client _pop3Client;

        public PopClientAdapter()
        {
            _pop3Client = new Pop3Client();
        }

        public void Connect(string host, int port, bool useSsl)
        {
            _pop3Client.Connect(host, port, useSsl);
        }

        public void Authenticate(string username, string password)
        {
            _pop3Client.Authenticate(username, password);
        }

        public Message GetMessage(int messageNumber)
        {
            return _pop3Client.GetMessage(messageNumber);
        }

        public void DeleteMessage(int messageNumber)
        {
            _pop3Client.DeleteMessage(messageNumber);
        }

        public int GetMessageCount()
        {
            return _pop3Client.GetMessageCount();
        }

        public void Dispose()
        {
            _pop3Client.Dispose();
        }
    }
}
