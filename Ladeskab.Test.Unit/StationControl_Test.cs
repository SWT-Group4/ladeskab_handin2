/*
 *  File: StationControl_Test.cs
 *  Authors: I4SWT, Team 2020-4
 *  Created: 08-10-2020
 *
 *  Description: Unit Tests for Ladeskab.StationControl.cs
 */

using LadeskabClasses;
using LadeskabClasses.Interfaces;
using NSubstitute;
using NUnit.Framework;
using UsbSimulator;

namespace Ladeskab.Test.Unit
{
    [TestFixture]
    public class StationControlUnitTest
    {
        // Pre-Setup:
        private StationControl _uut;
        private IDoor _fakeDoor;
        private IRfidReader _fakeRfidReader;
        private IChargeControl _fakeChargeControl;
        private IDisplay _fakeDisplay;
        private ILogfileWriter _fakeLogfile;

        [SetUp]
        public void Setup()
        {
            // Common Arrange:
            _fakeDoor = Substitute.For<IDoor>();
            _fakeRfidReader = Substitute.For<IRfidReader>();
            _fakeChargeControl = Substitute.For<IChargeControl>();
            _fakeDisplay = Substitute.For<IDisplay>();
            _fakeLogfile = Substitute.For<ILogfileWriter>();
            _uut = new StationControl(_fakeChargeControl, _fakeDoor, _fakeRfidReader, _fakeDisplay, _fakeLogfile);
        }

        #region DoorEventHandler()
        // Tests for when the door event is that the door is opened
        [Test]
        public void DoorEventHandler_DoorOpenedStateAvailable_stateChangesToDoorOpened()
        {
            // Arrange
            _uut._state = StationControl.LadeskabState.Available;

            // Act - Raise event in fake
            _fakeDoor.DoorEvent +=
            Raise.EventWith(new DoorEventArgs() { DoorState = true});

            // Assert
            Assert.That(_uut._state, Is.EqualTo(StationControl.LadeskabState.DoorOpen));

        }

        [Test]
        public void DoorEventHandler_DoorOpenedStateAvailable_displayCalledOnce()
        {
            // Arrange
            _uut._state = StationControl.LadeskabState.Available;

            // Act - Raise event in fake
            _fakeDoor.DoorEvent +=
                Raise.EventWith(new DoorEventArgs() { DoorState = true });

            // Assert
            _fakeDisplay.Received(1).ConnectPhone();
            _fakeDisplay.Received(0).ReadRfid();

        }

        [Test]
        public void DoorEventHandler_DoorOpenedStateDoorOpen_NothingHappens()
        {
            // Arrange
            _uut._state = StationControl.LadeskabState.DoorOpen;

            // Act - Raise event in fake
            _fakeDoor.DoorEvent +=
                Raise.EventWith(new DoorEventArgs() { DoorState = true });

            // Assert
            Assert.That(_uut._state, Is.EqualTo(StationControl.LadeskabState.DoorOpen));
        }

        [Test]
        public void DoorEventHandler_DoorOpenedStateLocked_NothingHappens()
        {
            // Arrange
            _uut._state = StationControl.LadeskabState.Locked;

            // Act - Raise event in fake
            _fakeDoor.DoorEvent +=
                Raise.EventWith(new DoorEventArgs() { DoorState = true });

            // Assert
            Assert.That(_uut._state, Is.EqualTo(StationControl.LadeskabState.Locked));
        }


        // Tests for when the door event is that the door is closed
        [Test]
        public void DoorEventHandler_DoorClosedStateAvailable_NothingHappens()
        {
            // Arrange
            _uut._state = StationControl.LadeskabState.Available;

            // Act - Raise event in fake
            _fakeDoor.DoorEvent +=
            Raise.EventWith(new DoorEventArgs() { DoorState = false });

            // Assert
            Assert.That(_uut._state, Is.EqualTo(StationControl.LadeskabState.Available));
        }

        [Test]
        public void DoorEventHandler_DoorClosedStateDoorOpen_stateChangesToAvailable()
        {
            // Arrange
            _uut._state = StationControl.LadeskabState.DoorOpen;

            // Act - Raise event in fake
            _fakeDoor.DoorEvent +=
                Raise.EventWith(new DoorEventArgs() { DoorState = false });

            // Assert
            Assert.That(_uut._state, Is.EqualTo(StationControl.LadeskabState.Available));
        }

        [Test]
        public void DoorEventHandler_DoorClosedStateDoorOpen_DisplayCalledOnce()
        {
            // Arrange
            _uut._state = StationControl.LadeskabState.DoorOpen;

            // Act - Raise event in fake
            _fakeDoor.DoorEvent +=
                Raise.EventWith(new DoorEventArgs() { DoorState = false });

            // Assert
            _fakeDisplay.Received(0).ConnectPhone();
            _fakeDisplay.Received(1).ReadRfid();
        }

        [Test]
        public void DoorEventHandler_DoorClosedStateLocked_NothingHappens()
        {
            // Arrange
            _uut._state = StationControl.LadeskabState.Locked;

            // Act - Raise event in fake
            _fakeDoor.DoorEvent +=
                Raise.EventWith(new DoorEventArgs() { DoorState = false });

            // Assert
            Assert.That(_uut._state, Is.EqualTo(StationControl.LadeskabState.Locked));
        }

        #endregion

        #region RfidDetected()
        // Tests for when rfid event is received and state=Available
        [Test]
        public void RfidDetected_StateAvailableAndChargerConnected_DoorCalledOnce()
        {
            // Arrange
            _uut._state = StationControl.LadeskabState.Available;
            _fakeChargeControl.IsConnected().Returns(true);

            // Act - Raise event in fake
            _fakeRfidReader.RfidEvent +=
                Raise.EventWith(new RfidEventArgs() { Id = 1 });

            // Assert
            _fakeDoor.Received(1).LockDoor();
        }

        [Test]
        public void RfidDetected_StateAvailableAndChargerConnected_ChargerCalledOnce()
        {
            // Arrange
            _uut._state = StationControl.LadeskabState.Available;
            _fakeChargeControl.IsConnected().Returns(true);

            // Act - Raise event in fake
            _fakeRfidReader.RfidEvent +=
                Raise.EventWith(new RfidEventArgs() { Id = 1 });

            // Assert
            _fakeChargeControl.Received(1).StartCharge();
        }

        [Test]
        public void RfidDetected_StateAvailableAndChargerConnected_LogDoorLockedCalledOnce()
        {
            // Arrange
            _uut._state = StationControl.LadeskabState.Available;
            _fakeChargeControl.IsConnected().Returns(true);

            // Act - Raise event in fake
            _fakeRfidReader.RfidEvent +=
                Raise.EventWith(new RfidEventArgs() { Id = 12 });

            // Assert
            _fakeLogfile.Received(1).LogDoorLocked(12);
        }

        [Test]
        public void RfidDetected_StateAvailableAndChargerConnected_DisplayCalledOnce()
        {
            // Arrange
            _uut._state = StationControl.LadeskabState.Available;
            _fakeChargeControl.IsConnected().Returns(true);

            // Act - Raise event in fake
            _fakeRfidReader.RfidEvent +=
                Raise.EventWith(new RfidEventArgs() { Id = 1 });

            // Assert
            _fakeDisplay.Received(1).StateChangedToLocked();
        }

        [Test]
        public void RfidDetected_StateAvailableAndChargerConnected_StateChanges()
        {
            // Arrange
            _uut._state = StationControl.LadeskabState.Available;
            _fakeChargeControl.IsConnected().Returns(true);

            // Act - Raise event in fake
            _fakeRfidReader.RfidEvent +=
                Raise.EventWith(new RfidEventArgs() { Id = 1 });

            // Assert
            Assert.That(_uut._state, Is.EqualTo(StationControl.LadeskabState.Locked));
        }

        [Test]
        public void RfidDetected_StateAvailableAndChargerNotConnected_DisplayCalledOnce()
        {
            // Arrange
            _uut._state = StationControl.LadeskabState.Available;
            _fakeChargeControl.IsConnected().Returns(false);

            // Act - Raise event in fake
            _fakeRfidReader.RfidEvent +=
                Raise.EventWith(new RfidEventArgs() { Id = 1 });

            // Assert
            _fakeDisplay.Received(1).ErrorInPhoneConnection();
        }

        // Tests for when rfid event is received and state=DoorOpen
        [Test]
        public void RfidDetected_StateDoorOpen_NothingHappens()
        {
            // Arrange
            _uut._state = StationControl.LadeskabState.DoorOpen;
            _fakeChargeControl.IsConnected().Returns(false);

            // Act - Raise event in fake
            _fakeRfidReader.RfidEvent +=
                Raise.EventWith(new RfidEventArgs() { Id = 1 });

            // Assert
            // Nothing happens
        }

        //Tests that simulate an entire open/close cycle
        [TestCase(12, 12, 1)]
        [TestCase(12, 13, 0)]
        public void RfidDetected_FullCycleSim_DisplayCalls(int id1, int id2, int res)
        {
            // Arrange
            _uut._state = StationControl.LadeskabState.Available;
            _fakeChargeControl.IsConnected().Returns(true);

            // Act - Raise event in fake
            _fakeRfidReader.RfidEvent +=
                Raise.EventWith(new RfidEventArgs() { Id = id1 });
            _fakeRfidReader.RfidEvent +=
                Raise.EventWith(new RfidEventArgs() { Id = id2 });

            // Assert
            //_fakeDisplay.Received(1).StateChangedToLocked();
            _fakeDisplay.Received(res).StateChangedToUnlocked();
        }

        [TestCase(12, 12, 1)]
        [TestCase(12, 13, 0)]
        public void RfidDetected_FullCycleSim_CallsChargerOnce(int id1, int id2, int res)
        {
            // Arrange
            _uut._state = StationControl.LadeskabState.Available;
            _fakeChargeControl.IsConnected().Returns(true);

            // Act - Raise event in fake
            _fakeRfidReader.RfidEvent +=
                Raise.EventWith(new RfidEventArgs() { Id = id1 });
            _fakeRfidReader.RfidEvent +=
                Raise.EventWith(new RfidEventArgs() { Id = id2 });

            // Assert
            _fakeChargeControl.Received(res).StopCharge();
        }

        [TestCase(12, 12, 1)]
        [TestCase(12, 13, 0)]
        public void RfidDetected_FullCycleSim_CallsDoorOnce(int id1, int id2, int res)
        {
            // Arrange
            _uut._state = StationControl.LadeskabState.Available;
            _fakeChargeControl.IsConnected().Returns(true);

            // Act - Raise event in fake
            _fakeRfidReader.RfidEvent +=
                Raise.EventWith(new RfidEventArgs() { Id = id1 });
            _fakeRfidReader.RfidEvent +=
                Raise.EventWith(new RfidEventArgs() { Id = id2 });

            // Assert
            _fakeDoor.Received(res).UnlockDoor();
        }

        [TestCase(12, 12, 1)]
        [TestCase(12, 13, 0)]
        public void RfidDetected_FullCycleSim_CallsLogfileOnce(int id1, int id2, int res)
        {
            // Arrange
            _uut._state = StationControl.LadeskabState.Available;
            _fakeChargeControl.IsConnected().Returns(true);

            // Act - Raise event in fake
            _fakeRfidReader.RfidEvent +=
                Raise.EventWith(new RfidEventArgs() { Id = id1 });
            _fakeRfidReader.RfidEvent +=
                Raise.EventWith(new RfidEventArgs() { Id = id2 });

            // Assert
            _fakeLogfile.Received(res).LogDoorUnlocked(id2);
        }
        
        [Test]
        public void RfidDetected_FullCycleSimRFIDMatch_StateChangesBack()
        {
            // Arrange
            _uut._state = StationControl.LadeskabState.Available;
            _fakeChargeControl.IsConnected().Returns(true);

            // Act - Raise event in fake
            _fakeRfidReader.RfidEvent +=
                Raise.EventWith(new RfidEventArgs() { Id = 12 });
            _fakeRfidReader.RfidEvent +=
                Raise.EventWith(new RfidEventArgs() { Id = 12 });

            // Assert
            Assert.That(_uut._state, Is.EqualTo(StationControl.LadeskabState.Available));
        }
        [Test]
        public void RfidDetected_FullCycleSimNoRFIDMatch_StateChangesBack()
        {
            // Arrange
            _uut._state = StationControl.LadeskabState.Available;
            _fakeChargeControl.IsConnected().Returns(true);

            // Act - Raise event in fake
            _fakeRfidReader.RfidEvent +=
                Raise.EventWith(new RfidEventArgs() { Id = 12 });
            _fakeRfidReader.RfidEvent +=
                Raise.EventWith(new RfidEventArgs() { Id = 13 });

            // Assert
            Assert.That(_uut._state, Is.EqualTo(StationControl.LadeskabState.Locked));
        }
        [Test]
        public void RfidDetected_FullCycleSimNoRFIDMatch_DisplayCalledOnce()
        {
            // Arrange
            _uut._state = StationControl.LadeskabState.Available;
            _fakeChargeControl.IsConnected().Returns(true);

            // Act - Raise event in fake
            _fakeRfidReader.RfidEvent +=
                Raise.EventWith(new RfidEventArgs() { Id = 12 });
            _fakeRfidReader.RfidEvent +=
                Raise.EventWith(new RfidEventArgs() { Id = 13 });

            // Assert
            _fakeDisplay.Received(1).RfidNoMatch();
        }

        #endregion
    }
}
