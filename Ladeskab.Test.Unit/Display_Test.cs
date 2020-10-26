/*
 *  File: ChargeControl_Test.cs
 *  Authors: I4SWT, Team 2020-4
 *  Created: 25-10-2020
 *
 *  Description: Unit Tests for Display
 */


using LadeskabClasses;
using LadeskabClasses.Interfaces;
using NSubstitute;
using NSubstitute.ReceivedExtensions;
using NUnit.Framework;

namespace Ladeskab.Test.Unit
{
    [TestFixture]
    public class DisplayUnitTest
    {
        // Pre-Setup:
        private DisplaySimulator uut;
        private IDisplayOutput output;

            [SetUp]
        public void Setup()
        {
            // Common Arrange:
            output = Substitute.For<IDisplayOutput>();
            uut = new DisplaySimulator(output);

        }

        [Test]
        public void NotCharging_PrintsMessage()
        {
            // Arrange

            // Act
            uut.NotCharging();
            // Assert
            output.Received().PrintToDisplay(Arg.Is<string>(str => str.Contains("Device is Not Charging")));
        }

        [Test]
        public void FullyCharged_PrintsMessage()
        {
            // Arrange

            // Act
            uut.FullyCharged();
            // Assert
            output.Received().PrintToDisplay(Arg.Is<string>(str => str.Contains("Device is Fully Charged")));
        }

        [Test]
        public void IsCharging_PrintsMessage()
        {
            // Arrange

            // Act
            uut.IsCharging();
            // Assert
            output.Received().PrintToDisplay(Arg.Is<string>(str => str.Contains("Device Is now Charging")));
        }

        [Test]
        public void OverCurrentFail_PrintsMessage()
        {
            // Arrange

            // Act
            uut.OverCurrentFail();
            // Assert
            output.Received().PrintToDisplay(Arg.Is<string>(str => str.Contains("FAILURE: Over Current")));
        }

        [Test]
        public void StateChangedToLocked_PrintsMessage()
        {
            // Arrange

            // Act
            uut.StateChangedToLocked();
            // Assert
            output.Received().PrintToDisplay(Arg.Is<string>(str => str.Contains("Skabet er låst og din telefon lades. Brug dit RFID tag til at låse op.")));
        }

        [Test]
        public void ErrorInPhoneConnection_PrintsMessage()
        {
            // Arrange

            // Act
            uut.ErrorInPhoneConnection();
            // Assert
            output.Received().PrintToDisplay(Arg.Is<string>(str => str.Contains("Din telefon er ikke ordentlig tilsluttet. Prøv igen.")));
        }

        [Test]
        public void StateChangedToUnlocked_PrintsMessage()
        {
            // Arrange

            // Act
            uut.StateChangedToUnlocked();
            // Assert
            output.Received().PrintToDisplay(Arg.Is<string>(str => str.Contains("Tag din telefon ud af skabet og luk døren")));
        }

        [Test]
        public void RfidNoMatch_PrintsMessage()
        {
            // Arrange

            // Act
            uut.RfidNoMatch();
            // Assert
            output.Received().PrintToDisplay(Arg.Is<string>(str => str.Contains("Forkert RFID tag")));
        }

        [Test]
        public void ConnectPhone_PrintsMessage()
        {
            // Arrange

            // Act
            uut.ConnectPhone();
            // Assert
            output.Received().PrintToDisplay(Arg.Is<string>(str => str.Contains("Tilslut telefon")));
        }

        [Test]
        public void ReadRfid_PrintsMessage()
        {
            // Arrange

            // Act
            uut.ReadRfid();
            // Assert
            output.Received().PrintToDisplay(Arg.Is<string>(str => str.Contains("Indlæs RFID")));
        }

    }
}