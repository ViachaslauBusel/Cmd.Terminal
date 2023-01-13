using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cmd.Terminal
{
    public interface IFlag
    {
        char Name { get; }
        bool IsTakesArgument { get; }
        int Priority { get; set; }

        void Apply(string argument);
    }
}
