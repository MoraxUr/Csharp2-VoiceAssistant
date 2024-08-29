using System.Speech.Synthesis;

namespace Csharp2_VoiceAssistant
{
#if WINDOWS
    public static class TTS
    {
        private static SpeechSynthesizer speechSynthesizer = new SpeechSynthesizer();

        public static void speak(string text)
        {
            if (AppSettings.IsSpeakerEnabled)
            {
                speechSynthesizer.Speak(text);
            }
        }
    }
#endif
}
