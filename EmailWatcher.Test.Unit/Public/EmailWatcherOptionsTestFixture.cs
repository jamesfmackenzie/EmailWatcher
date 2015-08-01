using EmailWatcher.Public;
using NUnit.Framework;

namespace EmailWatcher.Test.Unit.Public
{
    [TestFixture]
    class EmailWatcherOptionsTestFixture
    {
        [Test]
        public void TimeBetweenRefreshes_TimeBetweenRefreshesIsNotSet_Returns30()
        {
            // arrange
            EmailWatcherOptions options = new EmailWatcherOptions();

            // act
            int? timeBetweenRefreshes = options.TimeBetweenRefreshes;

            // assert
            Assert.AreEqual(30, timeBetweenRefreshes);
        }
    }
}
