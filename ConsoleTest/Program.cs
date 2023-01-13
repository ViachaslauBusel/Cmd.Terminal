using Cmd.Terminal.Debugger.Logger;
using Cmd.Terminal;

namespace Console.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            TerminalLogger logger = new TerminalLogger();
            logger.Warn("test warn 1");
            logger.Info("test info 2");
            logger.Warn("test Warn 3");
            logger.Warn("test warn 4");

            Terminal.Listen("test");
        }
    }
}