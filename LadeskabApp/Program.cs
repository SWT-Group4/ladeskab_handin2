using System;

namespace Ladeskab
{

    class Program
    {
        static void Main(string[] args)
        {
            // Assemble your system here from all the classes
            Door door;
            door = new Door();
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
                    door.DoorOpened();
                    break;

                case 'C':
                    door.DoorClosed();
                    break;

                case 'R':
                    System.Console.WriteLine("Indtast RFID id: ");
                    string idString = System.Console.ReadLine();

                    int id = Convert.ToInt32(idString);
                    // Kristoffer implementerer RF ID Reader:
                    // rfidReader.OnRfidRead(id);
                    break;

                default:
                    break;
            }

        } while (!finish);
        }
    }
}