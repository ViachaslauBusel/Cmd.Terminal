using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cmd.Terminal.Flags
{
    public class FlagString : IFlag
    {
        private char m_flagName;
        private Action<string> m_action;

        public char Name => m_flagName;
        public bool IsTakesArgument => true;
        public int Priority { get; set; } = 0;

        public FlagString(char flagName, Action<string> value)
        {
            m_flagName = flagName;
            m_action = value;
        }

        public void Apply(string argument)
        {
           m_action.Invoke(argument);
        }
    }
}
