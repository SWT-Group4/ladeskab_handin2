using System;

namespace Ladeskab
{
    public interface IDisplay
    {
        void NotCharging();
        void FullyCharged();
        void IsCharging();
        void OverCurrentFail();
    }
}

