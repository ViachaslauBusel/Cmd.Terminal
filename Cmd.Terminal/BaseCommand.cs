using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cmd.Terminal.Flags;

namespace Cmd.Terminal
{
    public abstract class BaseCommand
    {
        public string Description { get; protected set; }
        public string Command { get; protected set; }

        protected List<IFlag> Flags { get; } = new List<IFlag>();


        internal void Process(Queue<string> commands)
        {
            List<FlagToken> totalFlags = new List<FlagToken>();
            List<FlagToken> flags = new List<FlagToken>();

            int flagIndex = -1;

            while (commands.TryDequeue(out string token))
            {
                if(!AppyToken(totalFlags, flags, ref flagIndex, token))
                {
                    Terminal.PrintLine($"Command not recognized:{token}. Please use 'h' flag", ConsoleColor.Red);//Command not recognized. Please use 'debug help' token
                    return;
                }
            }
            totalFlags.AddRange(flags);

            BeforeUsingFlags();
            foreach(var f in totalFlags.OrderByDescending(f => f.Priority))
            {
                f.Apply();
            }
            AfterUsingFlags(totalFlags.Count);
        }
        private bool AppyToken(List<FlagToken> totalFlags, List<FlagToken> flags, ref int flagIndex, string token)
        {
            if (string.IsNullOrWhiteSpace(token)) return true;
            //token with new flags
            if (token.StartsWith("-"))
            {
                totalFlags.AddRange(flags);
                flagIndex = -1;
                flags.Clear();

                for (int i = 1; i < token.Length; i++)
                {
                    IFlag flag = Flags.FirstOrDefault(f => f.Name == token[i]);
                    if (flag != null)
                    { flags.Add(new FlagToken(flag)); }
                    else return false;
                }
                return true;
            }

            //Argument to apply to previous flags
            while (++flagIndex < flags.Count)
            {
                if (flags[flagIndex].IsTakesArgument)
                {
                    flags[flagIndex].AddArgument(token);
                    return true;
                }
            }
            return false;
        }

        protected virtual void BeforeUsingFlags() { }
        protected virtual void AfterUsingFlags(int usedFlagCount) { }
    }
}
