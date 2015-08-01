using System.Collections.Generic;
using EmailWatcher.Internal.Services;
using EmailWatcher.Internal.Validators;
using EmailWatcher.Public;
using NUnit.Framework;

namespace EmailWatcher.Test.Unit.Public
{
    [TestFixture]
    class EmailWatcherTestFixture
    {
        private FakeEmailWatcherOptionsValidator _fakeEmailWatcherOptionsValidator;
        private FakeEmailService _fakeEmailService;
        private EmailWatcher.Public.EmailWatcher _emailWatcher;
        private EmailWatcherOptions _options;

        [SetUp]
        public void Setup()
        {
            _fakeEmailWatcherOptionsValidator = new FakeEmailWatcherOptionsValidator();
            _fakeEmailService = new FakeEmailService();

            _options = new EmailWatcherOptions { TimeBetweenRefreshes = 15 };
        }

        [Test]
        [ExpectedException(typeof(System.Exception))]
        public void Ctor_NullEmailWatcherOptionsValidator_ThrowsException()
        {
            // arrange

            // act
            new EmailWatcher.Public.EmailWatcher(_options, null, _fakeEmailService);

            // assert
        }

        [Test]
        [ExpectedException(typeof(System.Exception))]
        public void Ctor_NullEmailWatcherService_ThrowsException()
        {
            // arrange

            // act
            new EmailWatcher.Public.EmailWatcher(_options, _fakeEmailWatcherOptionsValidator, null);

            // assert
        }

        [Test]
        public void Ctor_CallsEmailWatcherOptionsValidator()
        {
            // arrange
            _fakeEmailWatcherOptionsValidator.ValueToReturn = true;

            // act
            new EmailWatcher.Public.EmailWatcher(_options, _fakeEmailWatcherOptionsValidator, _fakeEmailService);

            // assert
            Assert.IsTrue(_fakeEmailWatcherOptionsValidator.HasBeenCalled);
        }

        [Test]
        [ExpectedException(typeof(System.Exception))]
        public void Ctor_EmailWatcherOptionsValidatorReturnsFalse_ThrowsException()
        {
            // arrange
            _fakeEmailWatcherOptionsValidator.ValueToReturn = false;

            // act
            new EmailWatcher.Public.EmailWatcher(_options, _fakeEmailWatcherOptionsValidator, _fakeEmailService);

            // assert
        }

        [Test]
        public void ProcessEmails_FetchesMessages()
        {
            // arrange
            _emailWatcher = new EmailWatcher.Public.EmailWatcher(_options, _fakeEmailWatcherOptionsValidator, _fakeEmailService);

            // act 
            _emailWatcher.ProcessEmails();

            // assert
            Assert.IsTrue(_fakeEmailService.HasFetchedMessages);
        }

        [Test]
        public void ProcessEmails_DeletesMessages()
        {
            // arrange
            _emailWatcher = new EmailWatcher.Public.EmailWatcher(_options, _fakeEmailWatcherOptionsValidator, _fakeEmailService);

            // act 
            _emailWatcher.ProcessEmails();

            // assert
            Assert.IsTrue(_fakeEmailService.HasDeletedMessages);
        }

        [Test]
        public void ProcessEmails_RaisesEmailReceivedEventWithExpectedValues()
        {
            // arrange
            EmailWatcherMessage receivedMessage = null;
            _emailWatcher = new EmailWatcher.Public.EmailWatcher(_options, _fakeEmailWatcherOptionsValidator, _fakeEmailService);
            _emailWatcher.EmailReceivedEvent += (sender, args) =>
            {
                receivedMessage = args.Message;
            };

            // act 
            _emailWatcher.ProcessEmails();

            // assert
            Assert.IsNotNull(receivedMessage);
            Assert.AreEqual(FakeEmailService.StubMessageId, receivedMessage.Id);
            Assert.AreEqual(FakeEmailService.StubMessageBody, receivedMessage.Body);
            Assert.AreEqual(FakeEmailService.StubMessageSubject, receivedMessage.Subject);
        }
    }

    internal class FakeEmailService : IEmailService
    {
        public bool HasFetchedMessages { get; set; }
        public bool HasDeletedMessages { get; set; }
        public static readonly string StubMessageId = "1";
        public static readonly string StubMessageSubject = "Stub Message Subject";
        public static readonly string StubMessageBody = "Stub Message Body";

        public List<EmailWatcherMessage> FetchAllMessages()
        {
            HasFetchedMessages = true;
            return new List<EmailWatcherMessage> { new EmailWatcherMessage { Id = StubMessageId, Body = StubMessageBody, Subject = StubMessageSubject } };
        }

        public void DeleteMessage(string messageId)
        {
            HasDeletedMessages = true;
        }
    }

    internal class FakeEmailWatcherOptionsValidator : IEmailWatcherOptionsValidator
    {

        public bool HasBeenCalled { get; set; }

        private bool _valueToReturn = true;
        public bool ValueToReturn
        {
            get { return _valueToReturn; }
            set { _valueToReturn = value; }
        }

        public bool Validate(EmailWatcherOptions options)
        {
            HasBeenCalled = true;
            return ValueToReturn;
        }
    }
}
