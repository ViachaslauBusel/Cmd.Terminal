namespace Cmd.Terminal
{
    public class DynamicConsoleLine
    {
        private int _cursorTop;
        private int _lastMsgLength;
        private ConsoleColor _color;

        public DynamicConsoleLine(string msg, ConsoleColor color)
        {
            Console.WriteLine();
            _cursorTop = Console.CursorTop - 1;
            _color = color;
            Print(msg, color);
        }

        public virtual void Print(string msg, ConsoleColor? color = null)
        {
            color ??= _color;

            // Сохраняем позицию курсора после вывода сообщения
            int cursorLeft = Console.CursorLeft;
            int cursorTop = Console.CursorTop;

            // Очищаем предыдущий вывод
            ClearPrevious();

            _lastMsgLength = msg.Length;

            Console.SetCursorPosition(0, _cursorTop);
            Console.ForegroundColor = color.Value;

            Console.Write(msg);

            Console.ResetColor();
            // Восстанавливаем позицию курсора
            Console.SetCursorPosition(cursorLeft, cursorTop);
        }

        private void ClearPrevious()
        {
            Console.SetCursorPosition(0, _cursorTop);
            Console.Write(new string(' ', _lastMsgLength));
        }
    }
}