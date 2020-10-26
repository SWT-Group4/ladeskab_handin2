using System;
using LadeskabClasses.Interfaces;

namespace LadeskabClasses
{
    public class DisplayOutput : IDisplayOutput
    {
        public void PrintToDisplay(string line)
        {
            Console.WriteLine(line);
        }
    }
}