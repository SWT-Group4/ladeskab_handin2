/*
 *  File: StationControl_Test.cs
 *  Authors: I4SWT, Team 2020-4
 *  Created: 08-10-2020
 *
 *  Description: Unit Tests for Ladeskab.StationControl.cs
 */

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

        [SetUp]
        public void Setup()
        {
            // Common Arrange:
            _fakeDoor = Substitute.For<IDoor>();
            _fakeRfidReader = Substitute.For<IRfidReader>();
            _fakeChargeControl = Substitute.For<IChargeControl>();
            _uut = new StationControl(_fakeChargeControl, _fakeDoor, _fakeRfidReader);

        }

        /*[Test]
        public void EventHandling_DoorOpenedLadeskabAvailable_stateChanges()
        {
            // Raise event in fake
            _fakeDoor.
        }*/

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