using Cmd.Terminal.Debugger.Logger;
using Cmd.Terminal;

namespace Test.Cmd
{
    class Program
    {
        static void Main(string[] args)
        {
            TerminalLogger logger = new TerminalLogger();
            logger.Warn("test warn");
            logger.Info("test info");

            Terminal.Listen("test");
        }
    }
}