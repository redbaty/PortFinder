using System;
using System.Drawing;
using PortFinder.Demo.Enums;
using PortFinder.Demo.Utils;
using Console = Colorful.Console;

namespace PortFinder.Demo
{

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteAscii("Port Finder", Color.CornflowerBlue);

            int min = 0;
            int max = 0;
            string host = "";

            if (args.Length != 3)
            {
                ConsoleUtils.Report("Application was not launched correctly.\nPlease ensure that you have set the arguments properly", ReportType.ERROR);
                Pausetoexit();
            }

            try
            {
                min = Convert.ToInt32(args[1]);
                max = Convert.ToInt32(args[2]);
                host = args[0];
            }
            catch (Exception)
            {
                ConsoleUtils.Report("Arguments are invalid.", ReportType.ERROR);
                Pausetoexit();
            }

            ConsoleUtils.Report($"Scanning {host} from port {min} to {max}.", ReportType.INFO);

            var finder = new PortFinderManager(host, min, max);
            finder.PortSearched += delegate(int index, bool opened)
            {
                ConsoleUtils.Report($"Port {index} is {(opened ? "open" : "closed")}", opened ? ReportType.OK : ReportType.WARN);
            };
            finder.Run();

            Console.ReadKey();
        }

        private static void Pausetoexit(int exCode = 0)
        {
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
            Environment.Exit(exCode);
        }


    }
}