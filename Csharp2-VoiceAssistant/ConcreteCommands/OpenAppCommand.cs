using System.Diagnostics;

namespace Csharp2_VoiceAssistant.ConcreteCommands
{
    public class OpenAppCommand : ICommand
    {
        public string _appName;
        public OpenAppCommand(string appName)
        {
            _appName = appName;
        }

        public void Execute()
        {
            // IMPLEMENTATION
            Debug.WriteLine($"OPEN APP: {_appName}");
        }
    }
}
