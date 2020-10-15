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
    }
}