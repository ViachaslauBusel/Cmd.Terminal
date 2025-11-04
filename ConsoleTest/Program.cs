using Cmd.Terminal.Debugger.Logger;
using Cmd.Terminal;
using System;

namespace Console.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Terminal.PrintLine("Console Terminal Test", ConsoleColor.Cyan);
            var line =  Terminal.CreateDynamicLine("Hello World!, Long message test !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!", ConsoleColor.Green);
            Terminal.PrintLine("This is a test line no 0", ConsoleColor.Magenta);
            line.Print("Hello World!", ConsoleColor.Yellow);
            Terminal.PrintLine("This is a test line", ConsoleColor.Magenta);
            line.Print("Hello World!", ConsoleColor.Yellow);
            line.Print("");
            line.Print("New Line Test!", ConsoleColor.Cyan);

            var progressLine = Terminal.CreateProgressLine("Progress:", 0.0f, ConsoleColor.Green);

            for (int i = 0; i <= 100; i++)
            {
                System.Threading.Thread.Sleep(50);
                progressLine.UpdateProgress(i / 100.0f);
            }

            List<int> list = new List<int>() { 1, 10, 10, 10, 10, 10, 10, 10, 10, 10 };
            int average = (int)list.Average();
            
            TerminalLogger logger = new TerminalLogger(1);
            logger.Warn("test warn 1");
            logger.Info("test info 2");
            logger.Warn("test Warn 3");
            logger.Warn("test warn 4");
            logger.Info($"average:{average}");

            Terminal.Listen("test");
        }
    }
}