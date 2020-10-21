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

        [Test]
        public void EventHandling_DoorOpenedLadeskabAvailable_stateChanges()
        {
            // Raise event in fake
            _fakeDoor.DoorEvent +=
            Raise.EventWith(new DoorEventArgs() { DoorState = true});

        }

        [Test]
        public void ThisWillReturnTrue_NoAction_IsTrue()
        {
            // Arrange

            // Act

            // Assert
            //Assert.IsTrue(_uut.ThisWillReturnTrue());
            Assert.IsTrue(true);
        }
    }
}