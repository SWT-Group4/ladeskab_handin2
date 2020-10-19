using System;

namespace LadeskabClasses.Interfaces
{
    public class RfidEventArgs : EventArgs
        {
            // Rfid ID tag
            public int Id { set; get; }
        }

    public interface IRfidReader
    {
        // Event triggered on new current value
        event EventHandler<RfidEventArgs> RfidEvent;
    }
}