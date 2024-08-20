namespace Csharp2_VoiceAssistant
{
    public class CommandService
    {
        private List<ICommand> _commands = new List<ICommand>();

        public void SetCommand(ICommand command)
        {
            _commands.Add(command);
        }

        public void ExecuteCommands()
        {
            foreach (ICommand command in _commands)
            {
                command.Execute();
            }
        }
    }
}
