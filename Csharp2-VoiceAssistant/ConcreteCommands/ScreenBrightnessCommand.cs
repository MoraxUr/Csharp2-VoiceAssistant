using System.Diagnostics;
using System.Management;

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
            Debug.WriteLine($"BRIGHTNESS: {_brightness}");

#if WINDOWS
            ManagementScope scope = new ManagementScope("root\\wmi");
            SelectQuery query = new SelectQuery("WmiMonitorBrightnessMethods");

            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query))
            {
                using (var collection = searcher.Get())
                {
                    foreach (ManagementObject item in collection)
                    {
                        item.InvokeMethod("WmiSetBrightness", new object[] { 1, _brightness });
                    }
                }
            }
#endif
        }
    }
}
