using LadeskabClasses;
using LadeskabClasses.Interfaces;
using NUnit.Framework;

namespace Ladeskab.Test.Unit
{
    [TestFixture]
    public class RfidReaderSimulator_test
    {
        private RfidReaderSimulator _uut;

        [SetUp]
        public void Setup()
        {
            _uut = new RfidReaderSimulator();
        }

        [Test]
        public void ScanRfidTag_NoSubscribers_NoThrow()
        {
            // An exception would fail this test case
            _uut.ScanRfidTag(12);
        }

        [Test]
        public void ScanRfidTag_1Subscriber_IsNotified()
        {
            //Arrange.
            bool notified = false;

            //Act
            _uut.RfidEvent += (sender, args) => notified = true;
            _uut.ScanRfidTag(12);

            //Assert
            Assert.IsTrue(notified);
        }

        [TestCase(12)]
        [TestCase(-1)]
        [TestCase(0)]
        public void ScanRfidTag_IDTransmitted_IDReceived(int Id)
        {
            //Arrange.
            int receivedID = 0;

            //Act
            _uut.RfidEvent += (sender, args) => receivedID = args.Id;
            _uut.ScanRfidTag(Id);

            //Assert
            Assert.That(receivedID, Is.EqualTo(Id));
        }
    }
}