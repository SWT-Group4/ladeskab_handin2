using System;

namespace Ladeskab
{
    public class Door : IDoor
    {
        public event EventHandler<DoorEventArgs> DoorEvent;
        DoorEventArgs state = new DoorEventArgs();

        public void DoorClosed()
        {
            state.DoorState = false;
            DoorEvent?.Invoke(this, state);
        }

        public void DoorOpened()
        {
            if (state.DoorState)
            {
                state.DoorState = true;
                DoorEvent?.Invoke(this, state);
            }
        }

        public void LockDoor()
        {
            if (state.DoorState)
                return;
            state.DoorState = false;
        }

        public void UnlockDoor()
        {
            if (state.DoorState)
                return;
            state.DoorState = true;
        }
    }
}