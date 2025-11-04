using System.Collections.Concurrent;

namespace Cmd.Terminal.Debugger.Logger
{
    public class TerminalLogger

    {
        private object m_locker = new object();
        private int m_maxLogs;

        private LinkedList<Note> m_messages { get; } = new LinkedList<Note>();
        public string Format { get; set; } = "[%date][%thread] - %message";
        internal event Action<Note> print;

        internal int MessagesCount => m_messages.Count;
        internal IEnumerable<Note> GetMessagesEnumerator() => m_messages;

        public TerminalLogger(int maxLogs = -1)
        {
            Terminal.AddCommand(new TerminalLoggerCommand(this));
            m_maxLogs = maxLogs;
        }

        internal void Lock()
        {
            Monitor.Enter(m_locker);
        }
        internal void Unlock()
        {
            Monitor.Exit(m_locker);
        }

        /// <summary>
        /// Removes the specified number of messages starting from the first element
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        internal int RemoveFirstMessages(int count, string filter, StringComparison stringComparison)
        {
            int deleted = 0;
            var currentNote = m_messages.First;
            while (currentNote != null && count > 0)
            {
                bool isNeedRemove = string.IsNullOrEmpty(filter) || currentNote.Value.Text.Contains(filter, stringComparison);
                var nextNode = currentNote.Next;
                if (isNeedRemove)
                {
                    m_messages.Remove(currentNote);
                    deleted++;
                    count--;
                }
                currentNote = nextNode;
            }
            return deleted;
        }

        public void Debug(string message)
        {
            Note _m = new Note(message, Environment.StackTrace, Thread.CurrentThread.Name, MsgType.Debug);
            lock(m_locker)
            {
                AddMsg(_m);
                print?.Invoke(_m);
            }
           
        }

        public void Info(string message)
        {
            Note _m = new Note(message, Environment.StackTrace, Thread.CurrentThread.Name, MsgType.INFO);
            lock (m_locker)
            {
                AddMsg(_m);
                print?.Invoke(_m);
            }
        }
        public void Warn(string message)
        {
            Note _m = new Note(message, Environment.StackTrace, Thread.CurrentThread.Name, MsgType.WARNING);
            lock (m_locker)
            {
                AddMsg(_m);
                print?.Invoke(_m);
            }
        }
        public void Error(string message)
        {
            Note _m = new Note(message, Environment.StackTrace, Thread.CurrentThread.Name, MsgType.ERROR);
            lock (m_locker)
            {
                AddMsg(_m);
                print?.Invoke(_m);
            }
        }
        public void Fatal(string message)
        {
            Note _m = new Note(message, Environment.StackTrace, Thread.CurrentThread.Name, MsgType.FATAL);
            lock (m_locker)
            {
                AddMsg(_m);
                print?.Invoke(_m);
            }
        }

        private void AddMsg(Note msg)
        {
            m_messages.AddLast(msg);

            if(m_maxLogs > 0 && m_messages.Count > m_maxLogs)
            {
                int needRemove = m_messages.Count - m_maxLogs;

                RemoveFirstMessages(needRemove, "", StringComparison.OrdinalIgnoreCase);
            }
        }
    }
}
