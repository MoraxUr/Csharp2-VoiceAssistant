using Microsoft.Maui.Storage;

namespace Csharp2_VoiceAssistant
{
    public static class AppSettings
    {
        private const string LanguageKey = "Language";
        private const string CommandRecognitionDurationKey = "CommandRecognitionDuration";
        private const string HistoryRetentionDaysKey = "HistoryRetentionDays";
        private const string IsSpeakerEnabledKey = "IsSpeakerEnabled";

        // Default values
        public static string Language { get; set; } = "en";
        public static int CommandRecognitionDurationInSeconds { get; set; } = 8;
        public static int HistoryRetentionDays { get; set; } = 5;
        public static bool IsSpeakerEnabled { get; set; } = true;

        // Load settings from preferences
        public static void LoadSettings()
        {
            Language = Preferences.Get(LanguageKey, "en");
            CommandRecognitionDurationInSeconds = Preferences.Get(CommandRecognitionDurationKey, 8);
            HistoryRetentionDays = Preferences.Get(HistoryRetentionDaysKey, 5);
            IsSpeakerEnabled = Preferences.Get(IsSpeakerEnabledKey, true);
        }

        // Save settings to preferences
        public static void SaveSettings()
        {
            Preferences.Set(LanguageKey, Language);
            Preferences.Set(CommandRecognitionDurationKey, CommandRecognitionDurationInSeconds);
            Preferences.Set(HistoryRetentionDaysKey, HistoryRetentionDays);
            Preferences.Set(IsSpeakerEnabledKey, IsSpeakerEnabled);
        }
    }
}
