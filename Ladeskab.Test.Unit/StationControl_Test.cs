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

        [SetUp]
        public void Setup()
        {
            // Common Arrange:
            _fakeDoor = Substitute.For<IDoor>();
            _fakeRfidReader = Substitute.For<IRfidReader>();
            _fakeChargeControl = Substitute.For<IChargeControl>();
            _fakeDisplay = Substitute.For<IDisplay>();
            _uut = new StationControl(_fakeChargeControl, _fakeDoor, _fakeRfidReader, _fakeDisplay);
        }

        #region DoorEventHandler()
        // Tests for when the event is that the door is opened
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
            _fakeDisplay.Received(0).ConnectPhone();
            _fakeDisplay.Received(0).ReadRfid();
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
            _fakeDisplay.Received(0).ConnectPhone();
            _fakeDisplay.Received(0).ReadRfid();
            Assert.That(_uut._state, Is.EqualTo(StationControl.LadeskabState.Locked));
        }


        // Tests for when the event is that the door is closed
        [Test]
        public void DoorEventHandler_DoorClosedStateAvailable_NothingHappens()
        {
            // Arrange
            _uut._state = StationControl.LadeskabState.Available;

            // Act - Raise event in fake
            _fakeDoor.DoorEvent +=
            Raise.EventWith(new DoorEventArgs() { DoorState = false });

            // Assert
            _fakeDisplay.Received(0).ConnectPhone();
            _fakeDisplay.Received(0).ReadRfid();
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
            _fakeDisplay.Received(0).ConnectPhone();
            _fakeDisplay.Received(0).ReadRfid();
            Assert.That(_uut._state, Is.EqualTo(StationControl.LadeskabState.Locked));
        }

        #endregion

    }
}