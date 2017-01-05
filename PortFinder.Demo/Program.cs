using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using PortFinder.Demo.Enums;
using PortFinder.Demo.Utils;
using Console = Colorful.Console;

namespace PortFinder.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 3)
            {
                ConsoleUtils.WriteWholeLineWithBackground(0, "[-] Setup\n", ConsoleColor.Gray, ConsoleColor.Black);

                var rangePorts = new Range();

                Console.ForegroundColor = Color.White;

                Console.WriteLine("Host");

                Console.ForegroundColor = Color.Gray;
                var host = Console.ReadLine();

                Console.ForegroundColor = Color.White;
                Console.WriteLine("\nMinimum port value");
                Console.ForegroundColor = Color.Gray;
                rangePorts.Min = Convert.ToInt32(Console.ReadLine());

                Console.ForegroundColor = Color.White;
                Console.WriteLine("\nMaximum port value");
                Console.ForegroundColor = Color.Gray;
                rangePorts.Max = Convert.ToInt32(Console.ReadLine());

                Run(host, rangePorts);
            }
            else
            {
                try
                {
                    Run(args[0], new Range(Convert.ToInt32(args[2]), Convert.ToInt32(args[1])));
                }
                catch
                {
                    ConsoleUtils.Report("Arguments are invalid.", ReportType.ERROR);
                    Pausetoexit();
                }
            }

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }

        private static void Run(string host, Range range)
        {
            Console.Clear();
            Console.ResetColor();

            var spin = new ConsoleSpiner {Background = ConsoleColor.Gray, Foreground = ConsoleColor.Black};
            string msg = $"Searching {host} through ports {range.Min} to {range.Max}";

            #region Sanity Checks

            if (range.Min == 0)
            {
                ConsoleUtils.Report("Minimum port value can't be 0", ReportType.ERROR);
                return;
            }

            if (range.Max > 65534)
            {
                ConsoleUtils.Report("Maximum port value can't be greater than 65534", ReportType.ERROR);
                return;
            }

            if (range.Max < range.Min)
            {
                ConsoleUtils.Report("Maximum port value can't be less than minimum port value", ReportType.ERROR);
                return;
            }

            if (!host.Contains("."))
            {
                ConsoleUtils.Report("Can't determine hostname type", ReportType.ERROR);
                return;
            }

            #endregion

            ConsoleUtils.WriteWholeLineWithBackground(0, "[-] PortFinder\n",
                ConsoleColor.Gray, ConsoleColor.Black);
            ConsoleUtils.WriteWithBackground(0, Console.BufferWidth - msg.Length, msg, ConsoleColor.Gray,
                ConsoleColor.Black);

            var finder = new PortFinderManager(host, range);
            finder.PortSearched += delegate(int index, bool open)
            {
                ConsoleUtils.Report($"Port {index} is {(open ? "open" : "closed")}",
                    open ? ReportType.OK : ReportType.WARN);

                spin.Turn();
            };

            finder.Completed += sucess =>
            {
                var fileName = $"Export-{DateTime.Now:yyyy-MM-dd-HH-mm-ss}.txt";

                if (sucess)
                    ConsoleUtils.Report($"Search completed. Found {finder.OpenPortsDictionary.Count} open ports.", ReportType.INFO);
                else
                    ConsoleUtils.Report("Search could not complete sucessfully.", ReportType.ERROR);

                ConsoleUtils.Report($"Exporting results to {fileName}.", ReportType.INFO);
                Export(fileName, finder);
            };

            finder.FindOpenPorts();
        }

        private static void Export(string filename, PortFinderManager finder)
        {
            var content = $"# Application name : {Assembly.GetExecutingAssembly().GetName().Name}{Environment.NewLine}";
            content +=
                $"# Application version : {Assembly.GetExecutingAssembly().GetName().Version}{Environment.NewLine}{Environment.NewLine}";


            if (!finder.Success)
                content +=
                    $"An exception occurred. Please read the details below. {Environment.NewLine}{finder.Exception}";
            else
            {
                content +=
                    $"The search found {finder.OpenPortsDictionary.Count} open ports, and {finder.ClosedPortsDictionary.Count} closed ones.{Environment.NewLine}";

                var openPortsString = $"{Environment.NewLine}# OPEN #{Environment.NewLine}";
                foreach (var b in finder.OpenPortsDictionary)
                    openPortsString = openPortsString + $"[{b.Key}] is open.{Environment.NewLine}";

                var closedPortsString = $"{Environment.NewLine}# CLOSED #{Environment.NewLine}";
                foreach (var b in finder.ClosedPortsDictionary)
                    closedPortsString = closedPortsString + $"[{b.Key}] is closed.{Environment.NewLine}";

                var allPortsString = $"{Environment.NewLine}# ALL PORTS #{Environment.NewLine}";
                foreach (var b in finder.ResultsDictionary)
                    allPortsString = allPortsString +
                                     $"[{b.Key}] is {(b.Value ? "open" : "closed")}.{Environment.NewLine}";

                content += openPortsString;
                content += closedPortsString;
                content += allPortsString;
            }

            File.WriteAllText(filename, content);
        }

        private static void Pausetoexit(int exCode = 0)
        {
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
            Environment.Exit(exCode);
        }
    }
}