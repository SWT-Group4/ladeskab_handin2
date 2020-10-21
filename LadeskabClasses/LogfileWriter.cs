using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using LadeskabClasses.Interfaces;

namespace LadeskabClasses
{
    public class LogfileWriter : ILogfileWriter
    {
        private string _filename = "logfile.txt";

        public void LogDoorUnlocked(int ID)
        {
            using (var writer = File.AppendText(_filename))
            {
                writer.WriteLine(DateTime.Now + ": Skab låst op med RFID: {0}", ID);
            }
        }

        public void LogDoorLocked(int ID)
        {
            using (var writer = File.AppendText(_filename))
            {
                writer.WriteLine(DateTime.Now + ": Skab låst med RFID: {0}", ID);
            }
        }
    }
}
