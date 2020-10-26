using System.IO;
using LadeskabClasses.Interfaces;

namespace LadeskabClasses
{
    public class LogOutput : ILogOutput
    {

        private string _filename = "logfile.txt";

        public void LoggingToFile(string line)
        {
            using (var writer = File.AppendText(_filename))
            {
                writer.WriteLine(line);
            }
        }
    }
}