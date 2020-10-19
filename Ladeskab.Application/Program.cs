using System;
using UsbSimulator;

namespace Ladeskab
{

    class Program
    {
        static void Main(string[] args)
        {
            // Assemble your system here from all the classes
            Door _door = new Door();
            RfidReaderSimulator _rfid = new RfidReaderSimulator();
            //UsbChargerSimulator _usbCharger = new UsbChargerSimulator();
            //ChargeControl _chargeControl = new ChargeControl(_usbCharger);
            //StationControl _stationControl = new StationControl(_chargeControl, _door, _rfid)
        bool finish = false;
        do
        {
            string input;
            System.Console.WriteLine("Indtast E, O, C, R: ");
            input = Console.ReadLine();
            if (string.IsNullOrEmpty(input)) continue;

            switch (input[0])
            {
                case 'E':
                    finish = true;
                    break;

                case 'O':
                    _door.DoorOpened();
                    break;

                case 'C':
                    _door.DoorClosed();
                    break;

                case 'R':
                    System.Console.WriteLine("Indtast RFID id: ");
                    string idString = System.Console.ReadLine();

                    int id = Convert.ToInt32(idString);
                    _rfid.ScanRfidTag(id);
                    break;

                default:
                    break;
            }

        } while (!finish);
        }
    }
}