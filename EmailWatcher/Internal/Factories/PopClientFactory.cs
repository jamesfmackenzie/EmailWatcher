using EmailWatcher.Internal.Adapters;

namespace EmailWatcher.Internal.Factories
{
    class PopClientFactory : IPopClientFactory
    {
        public IPopClientAdapter CreatePopClientAdapter()
        {
            return new PopClientAdapter();
        }
    }
}
