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

        public static void ColorConsoleLine(int line, ConsoleColor color)
        {
            var bColor = Console.BackgroundColor;
            Console.BackgroundColor = color;
            ClearConsoleLine(line);
            Console.BackgroundColor = bColor;
        }

        public static void WriteWholeLineWithBackground(int line, string message, ConsoleColor background,
            ConsoleColor foreground)
        {
            ColorConsoleLine(line, background);
            var cPosy = Console.CursorTop;
            var cPosx = Console.CursorLeft;
            var fColor = Console.ForegroundColor;
            var bColor = Console.BackgroundColor;

            Console.SetCursorPosition(0, line);
            Console.ForegroundColor = foreground;
            Console.BackgroundColor = background;
            Console.Write(message + "\n");

            if (cPosy != line)
                Console.SetCursorPosition(cPosx, cPosy);

            Console.ForegroundColor = fColor;
            Console.BackgroundColor = bColor;
        }

        public static void WriteWithBackground(int line, int row,string message, ConsoleColor background, ConsoleColor foreground)
        {
            var cPosy = Console.CursorTop;
            var cPosx = Console.CursorLeft;
            var fColor = Console.ForegroundColor;
            var bColor = Console.BackgroundColor;

            Console.SetCursorPosition(row, line);
            Console.ForegroundColor = foreground;
            Console.BackgroundColor = background;
            Console.Write(message);

            if (cPosy != line)
                Console.SetCursorPosition(cPosx, cPosy);

            Console.ForegroundColor = fColor;
            Console.BackgroundColor = bColor;
        }

        public static void ColorCurrentConsoleLine(int line, ConsoleColor color, ConsoleColor foreground)
            => ColorConsoleLine(Console.CursorTop, color);
    }
}