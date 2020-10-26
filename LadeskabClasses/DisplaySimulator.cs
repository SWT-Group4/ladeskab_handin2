using System;
using System.Collections.Generic;
using System.Text;
using LadeskabClasses.Interfaces;

namespace LadeskabClasses
{
    public class DisplaySimulator : IDisplay
    {

        private IDisplayOutput myOutput;

        public DisplaySimulator(IDisplayOutput output)
        {
            myOutput = output;
        }

        public void NotCharging()
        {
            myOutput.PrintToDisplay("Device is Not Charging");// Write better message
        }

        public void FullyCharged()
        {
            myOutput.PrintToDisplay("Device is Fully Charged"); // Write better message
        }

        public void IsCharging()
        {
            myOutput.PrintToDisplay("Device Is now Charging"); // Write better message
        }

        public void OverCurrentFail()
        {
            myOutput.PrintToDisplay("FAILURE: Over Current"); // Write better message
        }

        public void StateChangedToLocked()
        {
            myOutput.PrintToDisplay("Skabet er låst og din telefon lades. Brug dit RFID tag til at låse op.");
        }

        public void ErrorInPhoneConnection()
        {
            Console.WriteLine("");
            myOutput.PrintToDisplay("Din telefon er ikke ordentlig tilsluttet. Prøv igen.");
        }

        public void StateChangedToUnlocked()
        {
            myOutput.PrintToDisplay("Tag din telefon ud af skabet og luk døren");
        }

        public void RfidNoMatch()
        {
            myOutput.PrintToDisplay("Forkert RFID tag");
        }

        public void ConnectPhone()
        {
            myOutput.PrintToDisplay("Tilslut telefon");
        }

        public void ReadRfid()
        {
            myOutput.PrintToDisplay("Indlæs RFID");
        }
    }
}
