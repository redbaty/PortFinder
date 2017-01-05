using System;

namespace PortFinder.Demo.Utils
{
    public class ConsoleSpiner
    {
        int _counter;

        public int PositionX { get; set; }
        public int PositionY { get; set; }

        public ConsoleColor Foreground { get; set; } = ConsoleColor.Gray;
        public ConsoleColor Background { get; set; } = ConsoleColor.Black;

        public ConsoleSpiner()
        {
            _counter = 0;
        }

        public void Turn()
        {
            var posx = Console.CursorLeft;
            var posy = Console.CursorTop;
            var fColor = Console.ForegroundColor;
            var bColor = Console.BackgroundColor;

            Console.SetCursorPosition(PositionX, PositionY);
            Console.ForegroundColor = Foreground;
            Console.BackgroundColor = Background;

            _counter++;
            switch (_counter % 4)
            {
                case 0:
                    Console.Write("[/]");
                    break;
                case 1:
                    Console.Write("[-]");
                    break;
                case 2:
                    Console.Write("[\\]");
                    break;
                case 3:
                    Console.Write("[|]");
                    break;
            }

            if (posy != PositionY)
                Console.SetCursorPosition(posx, posy);

            Console.ForegroundColor = fColor;
            Console.BackgroundColor = bColor;
        }
    }
}