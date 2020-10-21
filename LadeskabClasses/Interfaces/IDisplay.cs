using System;

namespace LadeskabClasses.Interfaces
{
    public interface IDisplay
    {
        void NotCharging();
        void FullyCharged();
        void IsCharging();
        void OverCurrentFail();
        void StateChangedToLocked();
        void ErrorInPhoneConnection();
        void StateChangedToUnlocked();
        void RfidNoMatch();
        void ConnectPhone();
        void ReadRfid();
    }
}

