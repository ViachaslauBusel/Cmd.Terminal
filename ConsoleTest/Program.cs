using Cmd.Terminal.Debugger.Logger;
using Cmd.Terminal;
using System;

namespace Console.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            TestConcurrentDynamicLines_NoExceptions();
        }

        // Concurrent update smoke test: multiple threads update different dynamic lines concurrently.
        private static void TestConcurrentDynamicLines_NoExceptions()
        {
            const int workers = 30;
            const int iterations = 50;

            var colors = new[]
            {
        ConsoleColor.Green,
        ConsoleColor.Yellow,
        ConsoleColor.Cyan,
        ConsoleColor.Magenta,
        ConsoleColor.White
    };

            using var ready = new CountdownEvent(workers);
            using var start = new ManualResetEventSlim(false);

            var tasks = new Task[workers];

            for (int w = 0; w < workers; w++)
            {
                int id = w;
                tasks[w] = Task.Run(() =>
                {
                    ready.Signal(); // поток готов
                    start.Wait();   // ждет общий старт

                    Thread.Sleep(Random.Shared.Next(100)); // имитация разной подготовки

                    var color = colors[id % colors.Length];
                    var line = Terminal.CreateDynamicLine($"Loading {id:00}...", color);

                    Thread.Sleep(100 + Random.Shared.Next(1_000)); // имитация работы
                    line.Print($"Line {id:00} done!", color);
                });
            }

            ready.Wait(); // все потоки готовы
            start.Set();  // одновременный запуск
            Task.WaitAll(tasks);
        }
    }
}