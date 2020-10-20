using System;

namespace LadeskabClasses.Interfaces
{
    public class DoorEventArgs : EventArgs
    {
        // Door state. Open=True and Closed=False
        public bool DoorState { set; get; }
    }

    public interface IDoor
    {
        // Event triggered on new current value
        event EventHandler<DoorEventArgs> DoorEvent;

        // Lock the door
        void LockDoor();
        // Unlock the door
        void UnlockDoor();

        // Function to indicate User opened the door
        void DoorOpened();

        // Function to indicate User closed the door
        void DoorClosed();
    }
}