using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cmd.Terminal.Flags
{
    public class Flag : IFlag
    {
        private char m_flagName;
        private Action m_action;

        public char Name => m_flagName;
        public bool IsTakesArgument => false;
        public int Priority { get; set; } = 0;

        public Flag(char flagName, Action value)
        {
            m_flagName = flagName;
            m_action = value;
        }

        public void Apply(string argument)
        {
            m_action.Invoke();
        }
    }
   
}
