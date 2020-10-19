using System;

namespace LadeskabClasses.Interfaces
{
    public interface IDisplay
    {
        void NotCharging();
        void FullyCharged();
        void IsCharging();
        void OverCurrentFail();
    }
}

