/*
 *  File: ChargeControl_Test.cs
 *  Authors: I4SWT, Team 2020-4
 *  Created: 08-10-2020
 *
 *  Description: Unit Tests for Ladeskab.ChargeControl.cs
 */


using System.Runtime.InteropServices.ComTypes;
using NSubstitute;
using NSubstitute.ReceivedExtensions;
using NSubstitute.Routing.Handlers;
using NUnit.Framework;
using UsbSimulator;

namespace Ladeskab.Test.Unit
{
    [TestFixture]
    public class ChargeControlUnitTest
    {
        // Pre-Setup:
        private ChargeControl _uut;

        [SetUp]
        public void Setup()
        {
            // Common Arrange:
            // Using NSubstitute = no common Setup?
        }

        #region IsConnected()

        [Test]
        public void IsConnected_Connected_IsTrue()
        {
            // Arrange
            var stubDisplay = Substitute.For<IDisplay>();
            var mockUsbCharger = Substitute.For<IUsbCharger>();
            _uut = new ChargeControl(stubDisplay, mockUsbCharger);

            // Act
            mockUsbCharger.Connected.Returns(true);

            // Assert
            Assert.IsTrue(_uut.IsConnected());
        }

        [Test]
        public void IsConnected_Disconnected_IsFalse()
        {
            // Arrange
            var stubDisplay = Substitute.For<IDisplay>();
            var mockUsbCharger = Substitute.For<IUsbCharger>();
            _uut = new ChargeControl(stubDisplay, mockUsbCharger);

            // Act
            mockUsbCharger.Connected.Returns(false);

            // Assert
            Assert.IsFalse(_uut.IsConnected());
        }

        #endregion

        #region StartCharge()
        [Test]
        public void StartCharge_UsbChargerStarts_CalledOnce()
        {
            // Arrange
            var stubDisplay = Substitute.For<IDisplay>();
            var mockUsbCharger = Substitute.For<IUsbCharger>();
            _uut = new ChargeControl(stubDisplay, mockUsbCharger);
            
            // Act
            _uut.StartCharge();
            // Assert
            mockUsbCharger.Received(1).StartCharge();
        }

        #endregion

        #region StopCharge()
        [Test]
        public void StopCharge_UsbChargerStops_CalledOnce()
        {
            // Arrange
            var stubDisplay = Substitute.For<IDisplay>();
            var mockUsbCharger = Substitute.For<IUsbCharger>();
            _uut = new ChargeControl(stubDisplay, mockUsbCharger);

            // Act
            _uut.StopCharge();
            // Assert
            mockUsbCharger.Received(1).StopCharge();
        }


        #endregion

        #region OnChargeCurrentEvent
        [Test]
        public void OnNewCurrent_ListenForCurrent_CurrentIsReceived()
        {
            // Arrange
            var stubDisplay = Substitute.For<IDisplay>();
            var mockUsbCharger = Substitute.For<IUsbCharger>();
            _uut = new ChargeControl(stubDisplay, mockUsbCharger);

            // Act
            mockUsbCharger.ChargingCurrentEvent +=
                Raise.EventWith(new CurrentEventArgs() {Current = 100.000});

            // Assert
            Assert.That(_uut.ReadChargingCurrent, Is.EqualTo(100.000));
        }

        #endregion
    }
}