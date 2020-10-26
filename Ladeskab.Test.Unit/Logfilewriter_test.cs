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
        private ILogfileWriter _uut;

        [SetUp]
        public void Setup()
        {
            _uut = Substitute.For<ILogfileWriter>();
        }


        [TestCase(1)]
        public void LoggingWhen_DoorUnlocked(int x)
        {

        }

    }
}