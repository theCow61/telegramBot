using System;

namespace ShittyTea
{
    public class Eef
    {
        public string rootPath { get; set; }
        public string userWhoInvoked { get; set; }
        public void poop ()
        {
            Console.WriteLine($"{userWhoInvoked} {rootPath}");
        }
    }
}