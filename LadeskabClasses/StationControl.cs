﻿using System;
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
        public enum LadeskabState
        {
            Available,
            Locked,
            DoorOpen
        };

        public LadeskabState _state { get; set; }
        private IChargeControl _charger;
        private IDoor _door;
        private IRfidReader _rfidReader;
        private IDisplay _display;
        private ILogfileWriter _logfile;
        private int _oldId;

        // Constructor
        public StationControl(IChargeControl Charger, IDoor door, IRfidReader rfidReader, IDisplay display, ILogfileWriter logfile)
        {
            // Constructor injection
            _charger = Charger;
            _door = door;
            _rfidReader = rfidReader;
            _display = display;
            _logfile = logfile;

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
                    if (_charger.IsConnected())
                    {
                        _door.LockDoor();
                        _charger.StartCharge();
                        _oldId = e.Id;
                        _logfile.LogDoorLocked(e.Id);

                        _display.StateChangedToLocked();
                        _state = LadeskabState.Locked;
                    }
                    else
                    {
                        _display.ErrorInPhoneConnection();
                    }

                    break;

                case LadeskabState.DoorOpen:
                    throw new System.Exception("ERROR! RFID cannot be received when state is DoorOpen");

                case LadeskabState.Locked:
                    // Check for correct ID
                    if (e.Id == _oldId)
                    {
                        _charger.StopCharge();
                        _door.UnlockDoor();
                        _logfile.LogDoorUnlocked(e.Id);

                        _display.StateChangedToUnlocked();
                        _state = LadeskabState.Available;
                    }
                    else
                    {
                        _display.RfidNoMatch();
                    }
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
                        _display.ConnectPhone();
                    }
                    else
                    {
                        throw new System.Exception("ERROR! Door cannot close when state is Available");
                    }
                    break;

                case LadeskabState.DoorOpen:
                    // Only if the door is closed
                    if (e.DoorState == false)
                    {
                        _state = LadeskabState.Available;
                        _display.ReadRfid();
                    }
                    else
                    {
                        throw new System.Exception("ERROR! Door cannot open when state is DoorOpen");
                    }
                    break;

                case LadeskabState.Locked:
                    throw new System.Exception("ERROR! Door cannot open when state is Locked!");
            }
        }
    }
}
