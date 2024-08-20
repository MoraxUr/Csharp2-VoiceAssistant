using System.Diagnostics;

namespace Csharp2_VoiceAssistant.ConcreteCommands
{
    public class VolumeCommand : ICommand
    {
        public int _volumeLevel;
        public VolumeCommand(int volumeLevel)
        {
            _volumeLevel = volumeLevel;
        }

        public void Execute()
        {
            // IMPLEMENTATION
            Debug.WriteLine($"VOLUME: {_volumeLevel}");
        }
    }
}
