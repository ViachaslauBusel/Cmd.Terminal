using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cmd.Terminal.Flags
{
    public class FlagInt : IFlag
    {
        private char m_flagName;
        private Action<int> m_action;

        public char Name => m_flagName;
        public bool IsTakesArgument => true;
        public int Priority { get; set; } = 0;

        public FlagInt(char flagName, Action<int> value)
        {
            m_flagName = flagName;
            m_action = value;
        }

        public void Apply(string argument)
        {
            int arg = 0;
            if (int.TryParse(argument, out int n)) { arg = n; }
            m_action.Invoke(arg);
        }
    }
}
