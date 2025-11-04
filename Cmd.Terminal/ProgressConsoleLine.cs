namespace Cmd.Terminal
{
    public class ProgressConsoleLine : DynamicConsoleLine
    {
        private string _msg;
        private float _progress;

        public ProgressConsoleLine(string msg, float progress, ConsoleColor color) : base(msg, color)
        {
            _msg = msg;
            _progress = progress;
        }

        public override void Print(string msg, ConsoleColor? color = null)
        {
            base.Print($"{_msg} [{new string('#', (int)(_progress * 20)).PadRight(20)}] {_progress * 100:0.00}%", color);
        }

        public void UpdateProgress(float progress)
        {
            _progress = progress;
            Print(_msg);
        }
    }
}
