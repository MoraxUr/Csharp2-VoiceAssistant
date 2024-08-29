namespace Csharp2_VoiceAssistant
{
    public static class AppSettings
    {
        public static string Language { get; set; } = "en";
        public static int CommandRecognitionDurationInSeconds { get; set; } = 8;
        public static int HistoryRetentionDays { get; set; } = 5;

    }
}
