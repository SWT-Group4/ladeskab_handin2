using System.Runtime.InteropServices.ComTypes;
using LadeskabClasses;
using LadeskabClasses.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace Ladeskab.Test.Unit
{
    [TestFixture]
    public class DoorUnitTest
    {
        private Door _uut;

        [SetUp]
        public void Setup()
        {
            _uut = new Door();
        }

        #region ClosedDoor Test
        /*
         * Tests for scenarios when the door is closed
         */

        //Try and close a open door
        [Test]
        public void DoorTest_CloseDoor_DoorClosed()
        {
            //Arrange

            //Act
            _uut.DoorOpened();
            _uut.DoorClosed();

            //Assert
            Assert.IsFalse(_uut.DoorState);
        }

        /*[Test]
        public void DoorTest_CloseDoor_EventThrown()
        {
            //Arrange
            bool notified = true;

            //Act
            _uut.DoorEvent += (sender, args) => notified = false;
            _uut.DoorClosed();

            //Assert
            Assert.That(notified, Is.EqualTo(false));
        }*/



        //Try and close a closed door
        [Test]
        public void DoorTest_CloseDoor_DoorAlreadyClosed()
        {
            //Arrange

            //Act
            _uut.DoorClosed();
            _uut.DoorClosed();

            //Assert
            Assert.That(_uut.DoorState, Is.False);
        }

        // Try and lock a closed door
        [Test]
        public void DoorTest_LockClosedDoor_DoorIsLocked()
        {
            //Arrange

            //Act
            _uut.LockDoor();

            //Assert
            Assert.IsTrue(_uut.DoorLocked);
        }

        // Try and unlock a locked door
        [Test]
        public void DoorTest_UnlockClosedDoor_DoorIsUnlocked()
        {
            //Arrange

            //Act
            _uut.LockDoor();
            _uut.UnlockDoor();

            //Assert
            Assert.That(_uut.DoorLocked, Is.False);
        }

        // Try and unlock a locked door
        [Test]
        public void DoorTest_UnlockUnlockedClosedDoor_DoorIsUnlocked()
        {
            //Arrange

            //Act
            _uut.UnlockDoor();
            _uut.UnlockDoor();

            //Assert
            Assert.That(_uut.DoorLocked, Is.False);
        }

        #endregion

        #region OpenDoor Test
        /*
         * Tests for scenarios when the door is open
         */

        // Open the Door test
        [Test]
        public void DoorTest_OpenDoor_DoorOpen()
        {
            //Arrange

            //Act
            _uut.DoorOpened();

            //Assert
            Assert.IsTrue(_uut.DoorState);
        }

        /*[Test]
        public void DoorTest_OpenDoor_EventThrown()
        {
            //Arrange
            bool notified = false;

            //Act
            _uut.DoorEvent += (sender, args) => notified = true;
            _uut.DoorOpened();

            //Assert
            Assert.That(notified, Is.EqualTo(true));
        }*/

        // Try and open the door when it's already open
        [Test]
        public void DoorTest_OpenDoor_DoorAlreadyOpen()
        {
            //Arrange

            //Act
            _uut.DoorOpened();
            _uut.DoorOpened();

            //Assert
            Assert.IsTrue(_uut.DoorState);
        }

        // Try and lock the door when it's open.
        // With this test it's deemed not necessary to test the unlock function 
        // when opened, if it can't lock.
        [Test]
        public void DoorTest_LockOpenDoor_DoorIsNotLocked()
        {
            //Arrange

            //Act
            _uut.DoorOpened();
            _uut.LockDoor();

            //Assert
            Assert.IsFalse(_uut.DoorLocked);
        }

        //Try and open locked door
        [Test]
        public void DoorTest_OpenLockedDoor_DoorNotOpen()
        {
            //Arrange

            //Act
            _uut.DoorClosed();
            _uut.LockDoor();
            _uut.DoorOpened();

            //Assert
            Assert.IsFalse(_uut.DoorState);
        }


        #endregion


    }
}