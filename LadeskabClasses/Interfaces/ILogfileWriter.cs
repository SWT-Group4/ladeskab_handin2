using System;
using System.Collections.Generic;
using System.Text;

namespace LadeskabClasses.Interfaces
{
    public interface ILogfileWriter
    {
        void LogDoorUnlocked(int ID);
        void LogDoorLocked(int ID);
    }
}
