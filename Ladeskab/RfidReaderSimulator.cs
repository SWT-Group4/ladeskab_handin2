﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Ladeskab
{
    class RfidReaderSimulator : IRfidReader
    {
        public event EventHandler<RfidEventArgs> RfidEvent;

        public void ScanRfidTag(int ID)
        {
            RfidEvent?.Invoke(this, new RfidEventArgs {Id = ID});
        }
    }
}