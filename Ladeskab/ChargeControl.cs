using System;
using UsbSimulator;

namespace Ladeskab
{
    public class ChargeControl : IUsbCharger
    {

        public event EventHandler<CurrentEventArgs> CurrentValueEvent;
        public double CurrentValue { get; }
        public bool Connected { get; }
        public void StartCharge()
        {
            throw new NotImplementedException();
        }

        public void StopCharge()
        {
            throw new NotImplementedException();
        }
    }
}
