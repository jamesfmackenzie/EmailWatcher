using System;
using OpenPop.Mime;

namespace EmailWatcher.Internal.Adapters
{
    internal interface IPopClientAdapter : IDisposable
    {
        void Connect(string host, int port, bool useSsl);
        void Authenticate(string username, string password);
        Message GetMessage(int messageNumber);
        void DeleteMessage(int messageNumber);
        int GetMessageCount();
    }
}