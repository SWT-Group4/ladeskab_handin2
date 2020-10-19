using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LadeskabClasses.Interfaces;
using UsbSimulator;

namespace LadeskabClasses
{
    public class StationControl
    {
        // Enum med tilstande ("states") svarende til tilstandsdiagrammet for klassen
        private enum LadeskabState
        {
            Available,
            Locked,
            DoorOpen
        };

        // Her mangler flere member variable
        private LadeskabState _state;
        private IChargeControl _charger;
        private IDoor _door;
        private IRfidReader _rfidReader;
        private int _oldId;

        private string logFile = "logfile.txt"; // Navnet på systemets log-fil

        // Constructor
        public StationControl(IChargeControl Charger, IDoor door, IRfidReader rfidReader)
        {
            // Constructor injection
            _charger = Charger;
            _door = door;
            _rfidReader = rfidReader;

            // Assigning subscribers to events
            _rfidReader.RfidEvent += RfidDetected;
            _door.DoorEvent += DoorEventHandler;
        }

        // Eksempel på event handler for eventet "RFID Detected" fra tilstandsdiagrammet for klassen
        private void RfidDetected(object sender, RfidEventArgs e)
        {
            switch (_state)
            {
                case LadeskabState.Available:
                    // Check for ladeforbindelse
                    /*
                    if (_charger.Connected)
                    {
                        _door.LockDoor();
                        _charger.StartCharge();
                        _oldId = e.Id;
                        LogDoorLocked(e.Id);

                        Console.WriteLine("Skabet er låst og din telefon lades. Brug dit RFID tag til at låse op.");
                        _state = LadeskabState.Locked;
                    }
                    else
                    {
                        Console.WriteLine("Din telefon er ikke ordentlig tilsluttet. Prøv igen.");
                    }*/

                    break;

                case LadeskabState.DoorOpen:
                    // Ignore
                    break;

                case LadeskabState.Locked:
                    // Check for correct ID
                    /*
                    if (e.Id == _oldId)
                    {
                        _charger.StopCharge();
                        _door.UnlockDoor();
                        LogDoorUnlocked(e.Id);

                        Console.WriteLine("Tag din telefon ud af skabet og luk døren");
                        _state = LadeskabState.Available;
                    }
                    else
                    {
                        Console.WriteLine("Forkert RFID tag");
                    }
                    */
                    break;
            }
        }

        private void DoorEventHandler(object sender, DoorEventArgs e)
        {
            switch (_state)
            {
                case LadeskabState.Available:
                    // Only if the door opens
                    if (e.DoorState == true)
                    {
                        _state = LadeskabState.DoorOpen;
                        Console.WriteLine("Tilslut telefon");
                    }
                    break;

                case LadeskabState.DoorOpen:
                    // Only if the door is closed
                    if (e.DoorState == false)
                    {
                        _state = LadeskabState.Available;
                        Console.WriteLine("Indlæs RFID");
                    }
                    break;

                case LadeskabState.Locked:
                    // Breakin! Should not be possible
                    break;
            }
        }

        // These functions interface with the real world (log-file, system calls, displays etc), so the most correct
        // thing might be to put them in classes of their own.
        private void LogDoorLocked(int ID)
        {
            using (var writer = File.AppendText(logFile))
            {
                writer.WriteLine(DateTime.Now + ": Skab låst med RFID: {0}", ID);
            }
        }
        

        private void LogDoorUnlocked(int ID)
        {
            using (var writer = File.AppendText(logFile))
            {
                writer.WriteLine(DateTime.Now + ": Skab låst op med RFID: {0}", ID);
            }
        }
    }
}
