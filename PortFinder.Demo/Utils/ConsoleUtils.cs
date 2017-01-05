using System;
using System.Collections.Generic;
using System.Drawing;
using PortFinder.Demo.Enums;

namespace PortFinder.Demo.Utils
{
    internal static class ConsoleUtils
    {
        private static readonly Dictionary<ReportType, Color> Style = new Dictionary<ReportType, Color>
        {
            {ReportType.OK, Color.LawnGreen},
            {ReportType.WARN, Color.Gold},
            {ReportType.INFO, Color.DarkOrange},
            {ReportType.ERROR, Color.Red}
        };

        public static void Report(string msg, ReportType rtype = ReportType.OK)
        {
            Console.Write("[ ");
            Colorful.Console.Write(rtype.ToString(), Style[rtype]);
            Console.Write($" ] {msg}\n");
        }

        public static void Report(string msg, List<ReportType> rlist)
        {
            foreach (var rtype in rlist)
            {
                Console.Write("[ ");
                Colorful.Console.Write(rtype.ToString(), Style[rtype]);
                Console.Write(" ]");
            }
            Console.Write($" {msg}\n");
        }

        public static void ClearConsoleLine(int line)
        {
            int currentLineCursor = Console.CursorTop;
            int currentLineCursorx = Console.CursorLeft;
            Console.SetCursorPosition(0, line);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(currentLineCursorx, currentLineCursor);
        }
    }
}