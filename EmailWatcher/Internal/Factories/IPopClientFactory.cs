using EmailWatcher.Internal.Adapters;

namespace EmailWatcher.Internal.Factories
{
    internal interface IPopClientFactory
    {
        IPopClientAdapter CreatePopClientAdapter();
    }
}