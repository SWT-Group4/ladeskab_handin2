/*
 *  File: ChargeControl_Test.cs
 *  Authors: I4SWT, Team 2020-4
 *  Created: 08-10-2020
 *
 *  Description: Unit Tests for Ladeskab.ChargeControl.cs
 */


using System;
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

        #region OnChargeCurrentEvent()
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

        #endregion()()

        #region EvaluateCurrent()

        [TestCase(0.000, 0)]
        [TestCase(0.001, 1)]
        [TestCase(5.000, 1)]
        [TestCase(5.001, 2)]
        [TestCase(500.000, 2)]
        [TestCase(500.001, 3)]
        public void EvaluateCurrent_CurrentChange_ChargerStateIsCorrect(double currentChange, int chargerState)
        {
            // Arrange
            var stubDisplay = Substitute.For<IDisplay>();
            var mockUsbCharger = Substitute.For<IUsbCharger>();
            _uut = new ChargeControl(stubDisplay, mockUsbCharger);

            // Act
            mockUsbCharger.ChargingCurrentEvent +=
                Raise.EventWith(new CurrentEventArgs() { Current = currentChange });

            // Assert
            Assert.That(_uut.ReadChargerState, Is.EqualTo(chargerState));
        }

        [Test]
        public void EvaluateCurrent_OverCurrentFail_StopChargeCalledOnce()
        {
            // Arrange
            var stubDisplay = Substitute.For<IDisplay>();
            var mockUsbCharger = Substitute.For<IUsbCharger>();
            _uut = new ChargeControl(stubDisplay, mockUsbCharger);

            // Act
            mockUsbCharger.ChargingCurrentEvent +=
                Raise.EventWith(new CurrentEventArgs() { Current = 800.000 });

            // Assert
            mockUsbCharger.Received(1).StopCharge();
        }

        [Test]
        public void EvaluateCurrent_OverCurrentFail_PersistDisplay()
        {
            // Arrange
            var stubDisplay = Substitute.For<IDisplay>();
            var mockUsbCharger = Substitute.For<IUsbCharger>();
            _uut = new ChargeControl(stubDisplay, mockUsbCharger);

            // Act
            mockUsbCharger.ChargingCurrentEvent +=
                Raise.EventWith(new CurrentEventArgs() { Current = 800.000 });
            
            mockUsbCharger.ChargingCurrentEvent +=
                Raise.EventWith(new CurrentEventArgs() { Current = 0.000 });

            mockUsbCharger.ChargingCurrentEvent +=
                Raise.EventWith(new CurrentEventArgs() { Current = 0.000 });

            // Assert
            Assert.That(_uut.ReadChargerState, Is.EqualTo((int)3));
        }


        #endregion()()

        #region UpdateDisplay()

        [TestCase(0.0, 1, 0, 0, 0)]
        [TestCase(2.5, 0, 1, 0, 0)]
        [TestCase(25.0, 0, 0, 1, 0)]
        [TestCase(525.0, 0, 0, 0, 1)]
    
        public void UpdateDisplay_FullyCharged_CalledOnce
            (double testCurrent, int a, int b, int c, int d)
        {
            // Arrange
            var stubDisplay = Substitute.For<IDisplay>();
            var mockUsbCharger = Substitute.For<IUsbCharger>();
            _uut = new ChargeControl(stubDisplay, mockUsbCharger);

            // Act
            mockUsbCharger.ChargingCurrentEvent +=
                Raise.EventWith(new CurrentEventArgs() { Current = testCurrent });

            // Assert
            stubDisplay.Received(a).NotCharging();
            stubDisplay.Received(b).FullyCharged();
            stubDisplay.Received(c).IsCharging();
            stubDisplay.Received(d).OverCurrentFail();
        }

        
        [TestCase(5,0.0, 5, 0, 0, 0)]
        [TestCase(5, 2.5, 0, 5, 0, 0)]
        [TestCase(10, 25.0, 0, 0, 10, 0)]
        [TestCase(3, 525.0, 0, 0, 0, 3)]
        public void UpdateDisplay_MultiEvents_CalledCorrectNumberOfTimes
            (int events,  double testCurrent, int a, int b, int c, int d)
        {
            // Arrange
            var stubDisplay = Substitute.For<IDisplay>();
            var mockUsbCharger = Substitute.For<IUsbCharger>();
            _uut = new ChargeControl(stubDisplay, mockUsbCharger);

            // Act
            for (int i = 0; i < events; i++)
            {
                mockUsbCharger.ChargingCurrentEvent +=
                    Raise.EventWith(new CurrentEventArgs() {Current = testCurrent});
            }

            // Assert
            stubDisplay.Received(a).NotCharging();
            stubDisplay.Received(b).FullyCharged();
            stubDisplay.Received(c).IsCharging();
            stubDisplay.Received(d).OverCurrentFail();
        }

        #endregion
    }
}