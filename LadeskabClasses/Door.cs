﻿using System;
using LadeskabClasses.Interfaces;

namespace LadeskabClasses
{
    public class Door : IDoor
    {
        public event EventHandler<DoorEventArgs> DoorEvent;

        private DoorEventArgs State = new DoorEventArgs
        {
            DoorState = false
        };

        // Door state. Open=True and Closed=False
        public bool DoorState { get; private set; }
        // Door Locked. Yes=True and No=False
        public bool DoorLocked { get; private set; }

        public Door()
        {

            DoorState = false;
            DoorLocked = false;
        }

        // Activates when the User closes the door
        public void DoorClosed()
        {
            // If the door is already closed return
            if (!DoorState || DoorLocked) return;
            // Sets state
            State.DoorState = false;
            DoorState = false;
            // Invokes event
            DoorEvent?.Invoke(this, State);
        }

        // Activates when the User opens the door
        public void DoorOpened()
        {
            //If the Door is already open or locked return
            if (DoorState || DoorLocked) return;
            // Sets state
            State.DoorState = true;
            DoorState = true;
            // Invoke Event
            DoorEvent?.Invoke(this, State);
        }

        // Locks the door
        public void LockDoor()
        {
            // Checks if door is already locked
            if (DoorState || DoorLocked) return;
            // Changes state
            DoorLocked = true;
        }

        // Unlocks the door
        public void UnlockDoor()
        {
            // Checks if the door is already unlocked
            if (!DoorLocked) return;

            // Change state
            DoorLocked = false;
        }
    }
}