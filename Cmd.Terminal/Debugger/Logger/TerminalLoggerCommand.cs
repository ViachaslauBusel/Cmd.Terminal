using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cmd.Terminal.Flags;

namespace Cmd.Terminal.Debugger.Logger
{
    public class TerminalLoggerCommand : BaseCommand
    {
        private TerminalLogger m_logger;
        private string m_filter = null;
        private StringComparison m_stringComparison = StringComparison.Ordinal;


        public TerminalLoggerCommand(TerminalLogger logger, string commandName = "debug", string description = "Working with the log")
        {
            m_logger = logger;
            Command = commandName;
            Description = description;

            Flags.Add(new Flag('h', Help));
            Flags.Add(new Flag('e', Enter) { Priority = -10 });
            Flags.Add(new FlagInt('s', Show) { Priority = 5 });
            Flags.Add(new FlagInt('r', Remove));
            Flags.Add(new FlagString('f', FilterIgnoreCase) { Priority = 10 });
            Flags.Add(new FlagString('F', Filter) { Priority = 10 });
        }

        protected override void BeforeUsingFlags()
        {
            m_filter = null;
            m_logger.Lock();    
        }

        private void FilterIgnoreCase(string filter)
        {
            m_stringComparison = StringComparison.OrdinalIgnoreCase;
            m_filter = filter.Replace("*", " ");
        }
        private void Filter(string filter)
        {
            m_stringComparison = StringComparison.Ordinal;
            m_filter = filter.Replace("*", " ");
        }
        private void Help()
        {
            Terminal.PrintHelp("debug -s 0", "Outputting logs to the console. 0 - Optional parametr(Count)");
            Terminal.PrintHelp("debug -r 0", "Removing logs from storage. 0 - Optional parametr(Count)");
            Terminal.PrintHelp("debug -e", "Monitor Real-time logs");
            Terminal.PrintHelp("debug -f name", "Apply filter to flags. name - Keyword to be used as a filter. Ignore case");
            Terminal.PrintHelp("debug -F name", "Apply filter to flags. name - Keyword to be used as a filter");
        }

        private void Enter()
        {
            Terminal.PrintLine("You entered log monitoring mode. To exit press q");
            m_logger.print += PrintNote;
            m_logger.Unlock();
            while (true)
            {
                ConsoleKeyInfo _c = Console.ReadKey(true);
                if (_c.KeyChar == 'q' || _c.KeyChar == 'Q')
                {
                    m_logger.print -= PrintNote;
                    System.Console.WriteLine("You exited monitoring mode");
                    break;
                }
            }
            m_logger.Lock();
        }
        private void PrintNote(Note note) => Print(note);
        private void Remove(int count)
        {
            if (count == 0) count = Int32.MaxValue;
            int deleted = m_logger.RemoveFirstMessages(count, m_filter, m_stringComparison);
            Terminal.PrintLine($"deleted {deleted} logs. Total logs {m_logger.MessagesCount}");
        }
        private void Show(int count)
        {
            int messagesCount = m_logger.MessagesCount;
            if (count == 0) count = messagesCount;

            Terminal.PrintLine($"Log output:");
            foreach (Note e in m_logger.GetMessagesEnumerator())
            {
                if (count <= 0) break;
                if (Print(e)) count--;
            }
           
        }
        private bool Print(Note note)
        {
            if (string.IsNullOrEmpty(m_filter) || note.Text.Contains(m_filter, m_stringComparison))
            {
                ConsoleColor color = note.Type switch
                {
                    MsgType.Debug => ConsoleColor.White,
                    MsgType.INFO => ConsoleColor.Blue,
                    MsgType.WARNING => ConsoleColor.Yellow,
                    MsgType.ERROR => ConsoleColor.Red,
                    MsgType.FATAL => ConsoleColor.DarkRed,
                    _ => ConsoleColor.White
                };
                Terminal.PrintLine(note.FormatMessage(m_logger.Format), color);
                return true;
            }
            return false;
        }

        protected override void AfterUsingFlags(int usedFlagCount)
        {
            if (usedFlagCount == 0)
            {
                Help();
            }
            m_logger.Unlock();
        }
    }
}
