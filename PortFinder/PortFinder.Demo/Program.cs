using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortFinder.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            var finder = new PortFinderManager("192.168.2.1", 7000,7050);
            finder.PortSearched += Console.WriteLine;
            finder.PortDone += Console.WriteLine;
            finder.Run();
        }
    }
}
