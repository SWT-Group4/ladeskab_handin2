using System;
using System.Collections.Generic;
using System.Text;
using Ladeskab;

namespace LadeskabClasses
{
    public class RfidReaderSimulator : IRfidReader
    {
        public event EventHandler<RfidEventArgs> RfidEvent;

        public void ScanRfidTag(int ID)
        {
            RfidEvent?.Invoke(this, new RfidEventArgs {Id = ID});
        }
    }
}