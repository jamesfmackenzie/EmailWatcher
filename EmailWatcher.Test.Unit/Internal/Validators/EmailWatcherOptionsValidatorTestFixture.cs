using EmailWatcher.Internal.Validators;
using EmailWatcher.Public;
using NUnit.Framework;

namespace EmailWatcher.Test.Unit.Internal.Validators
{
    [TestFixture]
    public class EmailWatcherOptionsValidatorTestFixture
    {
        private const string StubHost = "smtp.google.com";
        private const string StubUsername = "jimmy";
        private const string StubPassword = "password";
        private const int StubTimeBetweenRefreshes = 60;

        private readonly EmailWatcherOptionsValidator _emailWatcherOptionsValidator = new EmailWatcherOptionsValidator();

        [Test]
        public void Validate_NullParam_ReturnsFalse()
        {
            // arrange   

            // act
            bool result = _emailWatcherOptionsValidator.Validate(null);

            // assert
            Assert.AreEqual(false, result);
        }

        [Test]
        public void Validate_HostIsNull_ReturnsFalse()
        {
            // arrange   
            EmailWatcherOptions options = new EmailWatcherOptions { Username = StubUsername, Password = StubPassword, TimeBetweenRefreshes = StubTimeBetweenRefreshes };

            // act
            bool result = _emailWatcherOptionsValidator.Validate(options);

            // assert
            Assert.AreEqual(false, result);
        }

        [Test]
        public void Validate_UsernameIsNull_ReturnsFalse()
        {
            // arrange   
            EmailWatcherOptions options = new EmailWatcherOptions { Host = StubHost, Password = StubPassword, TimeBetweenRefreshes = StubTimeBetweenRefreshes };

            // act
            bool result = _emailWatcherOptionsValidator.Validate(options);

            // assert
            Assert.AreEqual(false, result);
        }

        [Test]
        public void Validate_PasswordIsNull_ReturnsFalse()
        {
            // arrange   
            EmailWatcherOptions options = new EmailWatcherOptions { Host = StubHost, Username = StubUsername, TimeBetweenRefreshes = StubTimeBetweenRefreshes };

            // act
            bool result = _emailWatcherOptionsValidator.Validate(options);

            // assert
            Assert.AreEqual(false, result);
        }

        [Test]
        public void Validate_TimeBetweenRefreshesIsNull_ReturnsTrue()
        {
            // arrange   
            EmailWatcherOptions options = new EmailWatcherOptions { Host = StubHost, Username = StubUsername, Password = StubPassword };

            // act
            bool result = _emailWatcherOptionsValidator.Validate(options);

            // assert
            Assert.AreEqual(true, result);
        }

        [Test]
        public void Validate_ValidParams_ReturnsTrue()
        {
            // arrange   
            EmailWatcherOptions options = new EmailWatcherOptions { Host = StubHost, Username = StubUsername, Password = StubPassword, TimeBetweenRefreshes = StubTimeBetweenRefreshes };

            // act
            bool result = _emailWatcherOptionsValidator.Validate(options);

            // assert
            Assert.AreEqual(true, result);
        }
    }
}
