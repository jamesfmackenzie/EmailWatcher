using EmailWatcher.Public;

namespace EmailWatcher.Internal.Validators
{
    interface IEmailWatcherOptionsValidator
    {
        bool Validate(EmailWatcherOptions options);
    }
}
