using System;
using System.Collections.Generic;
using System.Text;

namespace Ladeskab
{
    public interface IChargeControl
    {
        void StartCharge();
        void StopCharge();
        bool IsConnected();
        
    }
}
