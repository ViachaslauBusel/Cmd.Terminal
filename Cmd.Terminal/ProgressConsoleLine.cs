namespace Cmd.Terminal
{
    public class ProgressConsoleLine : DynamicConsoleLine
    {
        private string _msg;
        private float _progress;
        private long _startTime;

        public ProgressConsoleLine(string msg, float progress, ConsoleColor color) : base(msg, color)
        {
            _msg = msg;
            _progress = Math.Clamp(progress, 0f, 1f);
            _startTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            Print(msg, color);
        }

        public override void Print(string msg, ConsoleColor? color = null)
        {
            long elapsedTime = DateTimeOffset.Now.ToUnixTimeMilliseconds() - _startTime;
            string bar = new string('#', (int)(_progress * 20)).PadRight(20);

            if (_progress >= 1f)
            {
                base.Print($"{_msg} [{bar}] 100.00% (Done in {elapsedTime / 1000.0:0.00}s)", color);
                return;
            }

            base.Print($"{_msg} [{bar}] {_progress * 100:0.00}%", color);
        }

        public void UpdateProgress(float progress)
        {
            _progress = Math.Clamp(progress, 0f, 1f);
            Print(_msg);
        }
    }
}
