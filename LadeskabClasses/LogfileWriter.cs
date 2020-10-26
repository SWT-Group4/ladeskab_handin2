using System;
using System.Collections.Generic;
using System.Text;
using LadeskabClasses.Interfaces;

namespace LadeskabClasses
{
    public class LogfileWriter : ILogfileWriter
    {
        

        private ILogOutput myOutput;

        public LogfileWriter(ILogOutput Logger)
        {
            myOutput = Logger;
        }

        public void LogDoorUnlocked(int ID)
        {
            myOutput.LoggingToFile(DateTime.Now + ": Skab låst med RFID: " + ID);
        }

        public void LogDoorLocked(int ID)
        {
            myOutput.LoggingToFile(DateTime.Now + ": Skab låst med RFID: " + ID);
        }
    }
}
