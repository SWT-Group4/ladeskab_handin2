using LadeskabClasses;
using System;
using UsbSimulator;

namespace Ladeskab.Application
{

    class Program
    {
        static void Main(string[] args)
        {
            // Assemble your system here from all the classes
            Door _door = new Door();
            RfidReaderSimulator _rfid = new RfidReaderSimulator();
            UsbChargerSimulator _usbCharger = new UsbChargerSimulator();
            DisplayOutput _displayOutput = new DisplayOutput();
            DisplaySimulator _display = new DisplaySimulator(_displayOutput);
            ChargeControl _chargeControl = new ChargeControl(_display, _usbCharger);
            LogOutput _logOutput = new LogOutput();
            LogfileWriter _logfile = new LogfileWriter(_logOutput);
            StationControl _stationControl = new StationControl(_chargeControl, _door, _rfid, _display, _logfile);
            bool finish = false;

            System.Console.WriteLine("Indtast:\n" +
                                     "(E) Exit\n" +
                                     "(O) Open\n" +
                                     "(C) Close\n" +
                                     "(R) Read RF-ID");

            do
            {
                string input = null;

                // Gate the blocking ReadLine function
                if (Console.KeyAvailable)
                {
                    input = Console.ReadLine();
                }
                if (string.IsNullOrEmpty(input)) continue;
                
                input = input.ToUpper();

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

                System.Console.WriteLine("Indtast:\n" +
                                         "(E) Exit\n" +
                                         "(O) Open\n" +
                                         "(C) Close\n" +
                                         "(R) Read RF-ID");

            } while (!finish);



        }
        }

}