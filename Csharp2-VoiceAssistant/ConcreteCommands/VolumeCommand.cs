using System.Diagnostics;
using NAudio.CoreAudioApi;

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
            Debug.WriteLine($"VOLUME: {_volumeLevel}");
#if WINDOWS
            using (var deviceEnumerator = new MMDeviceEnumerator())
            {
                var defaultDevice = deviceEnumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
                defaultDevice.AudioEndpointVolume.MasterVolumeLevelScalar = (float)_volumeLevel/100.0f;
            }
#endif
        }
    }
}
