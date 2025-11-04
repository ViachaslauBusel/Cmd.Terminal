namespace Cmd.Terminal
{
    public static class Terminal
    {

        private static Dictionary<string, BaseCommand> m_commands = new Dictionary<string, BaseCommand>();
        static Terminal()
        {
            m_commands.Add("help", new Command("help", "Display information about builtin commands", () =>
            {
                foreach (KeyValuePair<string, BaseCommand> c in m_commands)
                { PrintHelp(c.Value.Command, c.Value.Description); }
            }));

          //  m_commands.Add("online", new Command("online", "Number of connected clients", (q) => System.Console.WriteLine($"online: {ClientList.online()}")));
          //  m_commands.Add("keygen", new Command("keygen", "Generate a key to establish a secure connection", (q) => ContainerRSAKey.GenerateKey()));
            m_commands.Add("exit", new Command("exit", "Finish working with the terminal", () => throw new TerminalException()));
        }
        public static void Listen(string terminalName = "Server")
        {
            while (true)
            {
                try
                {
                    Print(terminalName+": ", ConsoleColor.Green);

                    RunCommand(Console.ReadLine());
                }
                catch (TerminalException) { return; }
                catch (Exception e) {  }

            }
        }

        public static void AddCommand(BaseCommand command)
        {
            if (m_commands.ContainsKey(command.Command))
            { m_commands[command.Command] = command; }
            else
            { m_commands.Add(command.Command, command); }
        }
        public static void RunCommand(string cmd)
        {
            Queue<string> commandArray = new Queue<string>(cmd.Split(" ").Select(t => t.Trim()));

            if (commandArray.TryDequeue(out string command))
            {
                command = command.ToLower();

                if (m_commands.ContainsKey(command))
                { m_commands[command].Process(commandArray); }
                else System.Console.WriteLine("Command not found. Please use 'help' command");
            }
        }

        internal static void PrintHelp(string name, string description)
        {
            Print(name, ConsoleColor.Blue);
            PrintLine($"  {description}");
        }

        public static void PrintLine(string msg, ConsoleColor color = ConsoleColor.White)
        {
            ConsoleColor defaultColor = System.Console.ForegroundColor;
            System.Console.ForegroundColor = color;
            System.Console.WriteLine(msg);
            System.Console.ForegroundColor = defaultColor;
        }

        public static void Print(string msg, ConsoleColor color)
        {
            ConsoleColor defaultColor = System.Console.ForegroundColor;
            System.Console.ForegroundColor = color;
            System.Console.Write(msg);
            System.Console.ForegroundColor = defaultColor;
        }

        public static void UpdatePrint(string msg, ConsoleColor color)
        {
            int currentLine = System.Console.CursorTop;
            System.Console.SetCursorPosition(0, currentLine);

            // Clear the line and reset cursor
            System.Console.Write(new string(' ', System.Console.BufferWidth));
            System.Console.SetCursorPosition(0, currentLine);

            Print(msg, color);
        }

        public static DynamicConsoleLine CreateDynamicLine(string msg, ConsoleColor color)
        {
            return new DynamicConsoleLine(msg, color);
        }

        public static ProgressConsoleLine CreateProgressLine(string msg, float progress, ConsoleColor color)
        {
            return new ProgressConsoleLine(msg, progress, color);
        }
    }
}
