using System.Diagnostics;

namespace Csharp2_VoiceAssistant.ConcreteCommands
{
    public class ScreenBrightnessCommand : ICommand
    {
        public int _brightness;
        public ScreenBrightnessCommand(int brightness)
        {
            _brightness = brightness;
        }

        public void Execute()
        {
            // IMPLEMENTATION
            Debug.WriteLine($"BRIGHTNESS: {_brightness}");
        }
    }
}
