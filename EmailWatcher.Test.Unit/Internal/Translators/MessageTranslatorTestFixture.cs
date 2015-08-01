using System;
using EmailWatcher.Internal.Translators;
using EmailWatcher.Public;
using EmailWatcher.Test.Unit.Internal.Utilities;
using NUnit.Framework;
using OpenPop.Mime;

namespace EmailWatcher.Test.Unit.Internal.Translators
{
    [TestFixture]
    class MessageTranslatorTestFixture
    {
        private MessageTranslator _translator;
        private string _stubEmailSubject;
        private string _stubEmailBody;
        private Message _stubEmail;

        [SetUp]
        public void Setup()
        {
            _stubEmailSubject = "Stub Email Subject";
            _stubEmailBody = "Stub Email Body";

            _stubEmail = MessageGenerator.GenerateMessage(_stubEmailSubject, _stubEmailBody);

            _translator = new MessageTranslator();
        }

        [Test]
        [ExpectedException(typeof(Exception))]
        public void Translate_NullMessage_ThrowsException()
        {
            // arrange

            // act
            _translator.Translate(null);

            // assert
        }

        [Test]
        public void Translate_ValidMessage_ReturnsExpectedSubject()
        {
            // arrange

            // act
            EmailWatcherMessage result = _translator.Translate(_stubEmail);

            // assert
            Assert.AreEqual(_stubEmailSubject, result.Subject);
        }

        [Test]
        public void Translate_ValidMessage_ReturnsExpectedBody()
        {
            // arrange

            // act
            EmailWatcherMessage result = _translator.Translate(_stubEmail);

            // assert
            Assert.AreEqual(_stubEmailBody + "\r\n", result.Body);
        }

        //todo: add test for setting id        
    }
}
