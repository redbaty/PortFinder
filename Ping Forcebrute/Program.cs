using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Threading;
using PortFinder;

namespace Ping_Forcebrute
{
    class Program
    {
        static void Main()
        {
            var p = new PortFinderManager("192.168.2.1",6000,7100);
            
            int toSearch = p.Limits[1] - p.Limits[0];
            int founds = 0;
            bool isRunning = true;


            p.PortFound += delegate(int found)
            {
                Console.WriteLine("Open port found at {0}:{1}", p.Host, found);
                founds++;
            };
            p.PortDone += delegate(bool success)
            {
                Console.WriteLine("Search done. Elapsed time {0}, Ports found {1}, Ports searched {2}, Successful : {3}", p.ElapsedTime, founds, p.Limits[1] - p.Limits[0], success);
                toSearch = 0;
                isRunning = false;
            };
            p.PortSearched += delegate { toSearch -= 1; };
            p.Run();

            while (isRunning)
            {
                Console.SetCursorPosition(0,0);
                Console.Write("                                                         ");
                Console.SetCursorPosition(0, 0);
                Console.WriteLine("Search started. Ports to search {0}", toSearch);
                Thread.Sleep(10);
            }
        }
    }
}
