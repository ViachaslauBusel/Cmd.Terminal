namespace Cmd.Terminal
{
    public class Command : BaseCommand
    {

        private Action m_action;

        public Command(string command, string description, Action action)
        {
            Command = command;
            Description = description;
            m_action = action;
        }

        protected override void AfterUsingFlags(int usedFlagCount)
        {
            m_action?.Invoke();
        }
    }
}