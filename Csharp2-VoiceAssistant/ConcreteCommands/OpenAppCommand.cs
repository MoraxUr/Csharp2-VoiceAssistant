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
            Debug.WriteLine($"OPEN APP: {_appName}");
#if WINDOWS
            Process.Start(new ProcessStartInfo( _appName + ":" ) { UseShellExecute = true });
#endif
        }
    }
}
