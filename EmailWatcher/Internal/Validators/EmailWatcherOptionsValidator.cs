using EmailWatcher.Internal.Constants;
using EmailWatcher.Internal.Loggers;
using EmailWatcher.Public;

namespace EmailWatcher.Internal.Validators
{
    class EmailWatcherOptionsValidator : IEmailWatcherOptionsValidator
    {
        public bool Validate(EmailWatcherOptions options)
        {
            if (options == null)
            {
                Logger.LogError(ExceptionMessageConstants.EmailOptionsCannotBeNull);
                return false;
            }

            if (options.Host == null)
            {
                Logger.LogError(ExceptionMessageConstants.HostCannotBeNull);
                return false;
            }

            if (options.Username == null)
            {
                Logger.LogError(ExceptionMessageConstants.UsernameCannotBeNull);
                return false;
            }

            if (options.Password == null)
            {
                Logger.LogError(ExceptionMessageConstants.PasswordCannotBeNull);
                return false;
            }

            return true;
        }
    }
}
