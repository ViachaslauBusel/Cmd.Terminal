using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cmd.Terminal
{
    internal class FlagToken
    {
        private IFlag m_flag;
        private string m_argument;

        public FlagToken(IFlag flag)
        {
            m_flag = flag;
        }

        public bool IsTakesArgument => m_flag.IsTakesArgument;

        public int Priority => m_flag.Priority;

        internal void AddArgument(string argument)
        {
            m_argument = argument;
        }

        internal void Apply()
        {
            m_flag.Apply(m_argument);
        }
    }
}
