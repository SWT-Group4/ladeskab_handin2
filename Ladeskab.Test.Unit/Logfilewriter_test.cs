using System.Runtime.InteropServices.ComTypes;
using LadeskabClasses;
using LadeskabClasses.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace Ladeskab.Test.Unit
{
    [TestFixture]
    public class Logfilewriter_test
    {
        private LogfileWriter uut;
        private ILogOutput output;

            [SetUp]
        public void Setup()
        {
            output = Substitute.For<ILogOutput>();
            uut = new LogfileWriter(output);

        }


        [TestCase(1)]
        public void LoggingWhen_DoorUnlocked(int x)
        {
            uut.LogDoorUnlocked(x);
            output.Received().LoggingToFile(Arg.Is<string>(str => str.Contains("Skab låst op med RFID: " + x)));
        }

        [TestCase(1)]
        public void LoggingWhen_DoorLocked(int x)
        {
            uut.LogDoorLocked(x);
            output.Received().LoggingToFile(Arg.Is<string>(str => str.Contains("Skab låst med RFID: " + x)));
        }

    }
}