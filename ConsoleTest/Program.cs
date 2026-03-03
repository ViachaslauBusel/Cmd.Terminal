using Cmd.Terminal;

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

                    var line = Terminal.CreateDynamicLine($"Loading worker {id:00}: preparing resources, validating input, initializing pipeline, waiting external dependencies...", ConsoleColor.Yellow);

                    Thread.Sleep(100 + Random.Shared.Next(2_000)); // имитация работы
                    line.Print($"Worker {id:00} completed successfully: resources released, final checks passed, result persisted, shutdown sequence finished.", ConsoleColor.Green);
                });
            }

            ready.Wait(); // все потоки готовы
            start.Set();  // одновременный запуск
            Task.WaitAll(tasks);
        }
    }
}