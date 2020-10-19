using LadeskabClasses.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace Ladeskab.Test.Unit
{
    [TestFixture]
    public class Door_test
    {
        [SetUp]
        public void setup()
        {

        }
        [Test]
        public void DoorTest_DoorLocked()
        {
            //Test fejler altid, da _door ikke påvirker _test variabler!
            var _door = Substitute.For<IDoor>();
            var _test = Substitute.For<DoorEventArgs>();
            //_test.DoorState = true;
            _door.UnlockDoor();
            Assert.False(_test.DoorState);

        }
    }
}