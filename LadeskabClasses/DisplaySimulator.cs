using System;
using System.Collections.Generic;
using System.Text;
using LadeskabClasses.Interfaces;

namespace LadeskabClasses
{
    public class DisplaySimulator : IDisplay
    {
        public void NotCharging()
        {
            Console.WriteLine("Device is Not Charging"); // Write better message
        }

        public void FullyCharged()
        {
            Console.WriteLine("Device is Fully Charged"); // Write better message
        }

        public void IsCharging()
        {
            Console.WriteLine("Device Is now Charging"); // Write better message
        }

        public void OverCurrentFail()
        {
            Console.WriteLine("FAILURE: Over Current"); // Write better message
        }

        public void StateChangedToLocked()
        {
            Console.WriteLine("Skabet er låst og din telefon lades. Brug dit RFID tag til at låse op.");
        }

        public void ErrorInPhoneConnection()
        {
            Console.WriteLine("Din telefon er ikke ordentlig tilsluttet. Prøv igen.");
        }

        public void StateChangedToUnlocked()
        {
            Console.WriteLine("Tag din telefon ud af skabet og luk døren");
        }

        public void RfidNoMatch()
        {
            Console.WriteLine("Forkert RFID tag");
        }

        public void ConnectPhone()
        {
            Console.WriteLine("Tilslut telefon");
        }

        public void ReadRfid()
        {
            Console.WriteLine("Indlæs RFID");
        }
    }
}
