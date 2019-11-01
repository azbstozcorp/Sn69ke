using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Input;

namespace TextSnake {
    class Program {
        static int Width => Console.WindowWidth;
        static int Height => Console.WindowHeight - 1;

        static int headIndex, motionX = 1, motionY;

        [STAThread]
        static void Main(string[] args) {
            Console.WriteLine("Enter to start game.");
            Console.ReadKey();

            var RNG = new Random(DateTime.Now.Millisecond);
            var screen = new StringBuilder(new string(' ', Width * Height));
            var snake = new LinkedList<int>();
            snake.AddFirst(10);
            snake.AddFirst(11);
            snake.AddFirst(12);

            screen[RNG.Next(Width * Height)] = 'A';
            bool breakAll = false;

            Thread t = new Thread(() => {
                while (true) {
                    headIndex += (motionY * Width) + motionX;
                    snake.AddFirst(headIndex);

                    if (snake.First() >= Width * Height || snake.First() < 0) { breakAll = true; break; }
                    if (screen[snake.First()] == '#') { breakAll = true; break; }

                    if (screen[snake.First()] != 'A') { snake.RemoveLast(); }
                    else {
                        screen[RNG.Next(Width * Height)] = 'A';
                    }

                    screen[snake.First()] = '#';
                    screen[snake.Last()] = ' ';

                    Console.Clear();
                    Console.CursorVisible = false;
                    Console.SetCursorPosition(0, 0);
                    Console.Write(screen.ToString());

                    Thread.Sleep(1000 / 20);
                }
            });
            t.Start();

            while (true) {
                if ((Keyboard.GetKeyStates(Key.Up) & KeyStates.Down) > 0 && motionY == 0) { motionY = -1; motionX = 0; }
                if ((Keyboard.GetKeyStates(Key.Down) & KeyStates.Down) > 0 && motionY == 0) { motionY = 1; motionX = 0; }
                if ((Keyboard.GetKeyStates(Key.Left) & KeyStates.Down) > 0 && motionX == 0) { motionX = -1; motionY = 0; }
                if ((Keyboard.GetKeyStates(Key.Right) & KeyStates.Down) > 0 && motionX == 0) { motionX = 1; motionY = 0; }
                if (breakAll) break;
            }

            Console.Clear();
            Console.WriteLine("You lose!");
            Console.ReadLine();
        }
    }
}