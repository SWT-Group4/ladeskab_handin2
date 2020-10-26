/*
 *  File: ChargeControl_Test.cs
 *  Authors: I4SWT, Team 2020-4
 *  Created: 08-10-2020
 *
 *  Description: Unit Tests for Ladeskab.ChargeControl.cs
 */


using LadeskabClasses;
using LadeskabClasses.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace Ladeskab.Test.Unit
{
    [TestFixture]
    public class ChargeControlUnitTest
    {
        // Pre-Setup:
        private ChargeControl _uut;

        private IDisplay _stubDisplay;
        private IUsbCharger _mockUsbCharger;

        [SetUp]
        public void Setup()
        {
            // Common Arrange:
            _stubDisplay = Substitute.For<IDisplay>();
            _mockUsbCharger = Substitute.For<IUsbCharger>();

            _uut = new ChargeControl(_stubDisplay, _mockUsbCharger);
        }

        #region IsConnected()

        [Test]
        public void IsConnected_Connected_IsTrue()
        {
            // Arrange
            
            // Act
            _mockUsbCharger.Connected.Returns(true);

            // Assert
            Assert.IsTrue(_uut.IsConnected());
        }

        [Test]
        public void IsConnected_Disconnected_IsFalse()
        {
            // Arrange

            // Act
            _mockUsbCharger.Connected.Returns(false);

            // Assert
            Assert.IsFalse(_uut.IsConnected());
        }

        #endregion

        #region StartCharge()
        [Test]
        public void StartCharge_UsbChargerStarts_CalledOnce()
        {
            // Arrange
            
            // Act
            _uut.StartCharge();

            // Assert
            _mockUsbCharger.Received(1).StartCharge();
        }

        #endregion

        #region StopCharge()
        [Test]
        public void StopCharge_UsbChargerStops_CalledOnce()
        {
            // Arrange

            // Act
            _uut.StopCharge();

            // Assert
            _mockUsbCharger.Received(1).StopCharge();
        }


        #endregion

        #region OnChargeCurrentEvent()
        [Test]
        public void OnNewCurrent_ListenForCurrent_CurrentIsReceived()
        {
            // Arrange

            // Act
            _mockUsbCharger.ChargingCurrentEvent +=
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

            // Act
            _mockUsbCharger.ChargingCurrentEvent +=
                Raise.EventWith(new CurrentEventArgs() { Current = currentChange });

            // Assert
            Assert.That(_uut.ReadChargerState, Is.EqualTo(chargerState));
        }

        [Test]
        public void EvaluateCurrent_OverCurrentFail_StopChargeCalledOnce()
        {
            // Arrange

            // Act
            _mockUsbCharger.ChargingCurrentEvent +=
                Raise.EventWith(new CurrentEventArgs() { Current = 800.000 });

            // Assert
            _mockUsbCharger.Received(1).StopCharge();
        }

        [Test]
        public void EvaluateCurrent_OverCurrentFail_PersistDisplay()
        {
            // Arrange

            // Act
            _mockUsbCharger.ChargingCurrentEvent +=
                Raise.EventWith(new CurrentEventArgs() { Current = 800.000 });
            
            _mockUsbCharger.ChargingCurrentEvent +=
                Raise.EventWith(new CurrentEventArgs() { Current = 0.000 });

            _mockUsbCharger.ChargingCurrentEvent +=
                Raise.EventWith(new CurrentEventArgs() { Current = 0.000 });

            // Assert
            Assert.That(_uut.ReadChargerState, Is.EqualTo((int)3));
        }


        #endregion()()

        #region UpdateDisplay()

        [TestCase(0.0, 0, 0, 0, 0)]
        [TestCase(2.5, 0, 1, 0, 0)]
        [TestCase(25.0, 0, 0, 1, 0)]
        [TestCase(525.0, 0, 0, 0, 1)]
    
        public void UpdateDisplay_ChangeState_CalledOnceExceptIdle
            (double testCurrent, int a, int b, int c, int d)
        {
            // Arrange

            // Act
            _mockUsbCharger.ChargingCurrentEvent +=
                Raise.EventWith(new CurrentEventArgs() { Current = testCurrent });

            // Assert (NotCharging = not called since state is already idle)
            _stubDisplay.Received(a).NotCharging();
            _stubDisplay.Received(b).FullyCharged();
            _stubDisplay.Received(c).IsCharging();
            _stubDisplay.Received(d).OverCurrentFail();
        }

        
        [TestCase(5,0.0, 5, 0, 0, 0)]
        [TestCase(5, 2.5, 0, 5, 0, 0)]
        [TestCase(10, 25.0, 0, 0, 10, 0)]
        [TestCase(3, 525.0, 0, 0, 0, 3)]
        public void UpdateDisplay_ConsecutiveEvents_CalledOnceExceptIdle
            (int events,  double testCurrent, int a, int b, int c, int d)
        {
            // Arrange

            // Act
            for (int i = 0; i < events; i++)
            {
                _mockUsbCharger.ChargingCurrentEvent +=
                    Raise.EventWith(new CurrentEventArgs() {Current = testCurrent});
            }

            // Assert
            _stubDisplay.Received(0).NotCharging();
            _stubDisplay.Received(b / events).FullyCharged();
            _stubDisplay.Received(c / events).IsCharging();
            _stubDisplay.Received(d / events).OverCurrentFail();
        }

        [Test]
        public void UpdateDisplay_ChargerGoesFromChargingToIdle_ChargerStateIdle()
        {
            // Arrange

            // Act
            _mockUsbCharger.ChargingCurrentEvent +=
                Raise.EventWith(new CurrentEventArgs() { Current = 300.000 });
            
            _mockUsbCharger.ChargingCurrentEvent +=
                Raise.EventWith(new CurrentEventArgs() { Current = 0.000 });


            // Assert
            Assert.That(_uut.ReadChargerState, Is.EqualTo((int)0));
        }


        #endregion
    }
}