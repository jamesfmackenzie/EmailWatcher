using System;
using System.Collections.Generic;
using System.Linq;
using EmailWatcher.Internal.Adapters;
using EmailWatcher.Internal.Factories;
using EmailWatcher.Internal.Services;
using EmailWatcher.Internal.Translators;
using EmailWatcher.Public;
using EmailWatcher.Test.Unit.Internal.Utilities;
using NUnit.Framework;
using OpenPop.Mime;

namespace EmailWatcher.Test.Unit.Internal.Services
{
    [TestFixture]
    class EmailServiceTestFixture
    {
        private EmailService _emailService;

        private EmailWatcherOptions _stubEmailWatcherOptions;
        private FakePopClientFactory _fakePopClientFactory;
        private MessageTranslator _messageTranslator;


        [SetUp]
        public void Setup()
        {
            _stubEmailWatcherOptions = new EmailWatcherOptions();
            _fakePopClientFactory = new FakePopClientFactory();
            _messageTranslator = new MessageTranslator();
        }

        [TearDown]
        public void Teardown()
        {
            // These tests are sharing global state via FakePopClientAdapter.
            // To prevent interference between tests, the global state needs to be reset in test teardown
            // todo: use a mock object framework like Moq and remove Fake objects
            FakePopClientAdapter.HasBeenInvoked = false;
            FakePopClientAdapter.HasBeenDisposed = false;
        }

        [Test]
        [ExpectedException(typeof(Exception))]
        public void Constructor_NullParams_ThrowsException()
        {
            // arrange

            // act
            new EmailService(null, null, null);

            // assert
        }

        [Test]
        [ExpectedException(typeof(Exception))]
        public void Constructor_NullOptions_ThrowsException()
        {
            // arrange

            // act
            new EmailService(null, _fakePopClientFactory, _messageTranslator);

            // assert
        }

        [Test]
        [ExpectedException(typeof(Exception))]
        public void Constructor_NullTranslator_ThrowsException()
        {
            // arrange

            // act
            new EmailService(_stubEmailWatcherOptions, _fakePopClientFactory, null);

            // assert
        }

        [Test]
        [ExpectedException(typeof(Exception))]
        public void Constructor_NullPopClientFactory_ThrowsException()
        {
            // arrange

            // act
            new EmailService(_stubEmailWatcherOptions, null, _messageTranslator);

            // assert
        }

        [Test]
        public void FetchAllMessages_ValidParams_CallsCreatePopClientAdapter()
        {
            // arrange
            _emailService = new EmailService(_stubEmailWatcherOptions, _fakePopClientFactory, _messageTranslator);

            // act
            _emailService.FetchAllMessages();

            // assert
            Assert.IsTrue(_fakePopClientFactory.HasBeenInvoked);
        }

        [Test]
        public void FetchAllMessages_ValidParams_InvokesPopClientAdapter()
        {
            // arrange
            _emailService = new EmailService(_stubEmailWatcherOptions, _fakePopClientFactory, _messageTranslator);

            // act
            _emailService.FetchAllMessages();

            // assert
            Assert.IsTrue(FakePopClientAdapter.HasBeenInvoked);
        }

        [Test]
        public void FetchAllMessages_ValidParams_DisposesPopClientAdapter()
        {
            // arrange
            _emailService = new EmailService(_stubEmailWatcherOptions, _fakePopClientFactory, _messageTranslator);

            // act
            _emailService.FetchAllMessages();

            // assert
            Assert.IsTrue(FakePopClientAdapter.HasBeenDisposed);
        }

        [Test]
        public void FetchAllMessages_ValidParams_ReturnsExpectedMessages()
        {
            // arrange
            _emailService = new EmailService(_stubEmailWatcherOptions, _fakePopClientFactory, _messageTranslator);

            // act 
            List<EmailWatcherMessage> messages = _emailService.FetchAllMessages();

            // assert
            Assert.IsNotNull(messages);
            Assert.AreEqual(FakePopClientAdapter.StubMessageCount, messages.Count);
            Assert.AreEqual(FakePopClientAdapter.StubSubject, messages.FirstOrDefault().Subject);
            Assert.AreEqual(FakePopClientAdapter.StubBody + "\r\n", messages.FirstOrDefault().Body);
        }

        [Test]
        public void FetchAllMessages_FactoryThrowsException_ExceptionIsSwallowed()
        {
            // arrange
            _emailService = new EmailService(_stubEmailWatcherOptions, new FakePopClientFactoryThatAlwaysThrowsExceptions(), _messageTranslator);

            // act

            // assert
        }

        [Test]
        public void FetchAllMessages_PopClientAdapterThrowsException_ExceptionIsSwallowed()
        {
            // arrange
            _emailService = new EmailService(_stubEmailWatcherOptions, new FakePopClientFactoryThatReturnsAPopClientAdapterThatAlwaysThrowsExceptions(), _messageTranslator);

            // act

            // assert
        }

        [Test]
        [ExpectedException(typeof(Exception))]
        public void DeleteMessage_NullMessageId_ThrowsException()
        {
            // arrange
            _emailService = new EmailService(_stubEmailWatcherOptions, _fakePopClientFactory, _messageTranslator);

            // act
            _emailService.DeleteMessage(null);

            // assert
        }

        [Test]
        [ExpectedException(typeof(Exception))]
        public void DeleteMessage_EmptyMessageId_ThrowsException()
        {
            // arrange
            _emailService = new EmailService(_stubEmailWatcherOptions, _fakePopClientFactory, _messageTranslator);

            // act
            _emailService.DeleteMessage("");

            // assert
        }

        [Test]
        public void DeleteMessage_ValidMessageId_InvokesFakePopClientFactory()
        {
            // arrange
            _emailService = new EmailService(_stubEmailWatcherOptions, _fakePopClientFactory, _messageTranslator);

            // act 
            _emailService.DeleteMessage("1");

            // assert
            Assert.IsTrue(_fakePopClientFactory.HasBeenInvoked);
        }

        [Test]
        public void DeleteMessage_ValidMessageId_InvokesFakePopClientAdapter()
        {
            // arrange
            _emailService = new EmailService(_stubEmailWatcherOptions, _fakePopClientFactory, _messageTranslator);

            // act 
            _emailService.DeleteMessage("1");

            // assert
            Assert.IsTrue(FakePopClientAdapter.HasBeenInvoked);
        }

        [Test]
        public void DeleteMessage_ValidMessageId_DisposesFakePopClientAdapter()
        {
            // arrange
            _emailService = new EmailService(_stubEmailWatcherOptions, _fakePopClientFactory, _messageTranslator);

            // act 
            _emailService.DeleteMessage("1");

            // assert
            Assert.IsTrue(FakePopClientAdapter.HasBeenDisposed);
        }
    }

    internal class FakePopClientFactory : IPopClientFactory
    {
        public bool HasBeenInvoked { get; set; }

        public IPopClientAdapter CreatePopClientAdapter()
        {
            HasBeenInvoked = true;
            return new FakePopClientAdapter();
        }
    }

    internal class FakePopClientFactoryThatAlwaysThrowsExceptions : IPopClientFactory
    {
        public IPopClientAdapter CreatePopClientAdapter()
        {
            throw new Exception();
        }
    }

    internal class FakePopClientFactoryThatReturnsAPopClientAdapterThatAlwaysThrowsExceptions : IPopClientFactory
    {
        public IPopClientAdapter CreatePopClientAdapter()
        {
            return new FakePopClientAdapterThatAlwaysThrowsExceptions();
        }
    }

    internal class FakePopClientAdapter : IPopClientAdapter
    {
        public static bool HasBeenInvoked { get; set; }
        public static bool HasBeenDisposed { get; set; }

        public static string StubSubject = "Stub Subject";
        public static string StubBody = "Stub Body";
        public static int StubMessageCount = 5;

        public void Dispose()
        {
            HasBeenDisposed = true;
        }

        public void Connect(string host, int port, bool useSSL)
        {
            HasBeenInvoked = true;
        }

        public void Authenticate(string username, string password)
        {
            HasBeenInvoked = true;
        }

        public Message GetMessage(int messageNumber)
        {
            HasBeenInvoked = true;
            return MessageGenerator.GenerateMessage(StubSubject, StubBody);
        }

        public void DeleteMessage(int messageNumber)
        {
            HasBeenInvoked = true;
        }

        public int GetMessageCount()
        {
            HasBeenInvoked = true;
            return StubMessageCount;
        }
    }

    internal class FakePopClientAdapterThatAlwaysThrowsExceptions : IPopClientAdapter
    {
        public void Dispose()
        {
            throw new Exception();
        }

        public void Connect(string host, int port, bool useSSL)
        {
            throw new Exception();
        }

        public void Authenticate(string username, string password)
        {
            throw new Exception();
        }

        public Message GetMessage(int messageNumber)
        {
            throw new Exception();
        }

        public void DeleteMessage(int messageNumber)
        {
            throw new Exception();
        }

        public int GetMessageCount()
        {
            throw new Exception();
        }
    }
}
