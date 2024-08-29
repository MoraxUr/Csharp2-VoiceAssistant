namespace Csharp2_VoiceAssistant;

public partial class Settings : ContentPage
{
    public Settings()
	{
		InitializeComponent();
		LoadSettings();
	}

    private void LoadSettings()
    {
        // Load current settings into controls
        LanguagePicker.SelectedItem = AppSettings.Language;
        DurationEntry.Text = AppSettings.CommandRecognitionDurationInSeconds.ToString();
        RetentionEntry.Text = AppSettings.HistoryRetentionDays.ToString();
        if (AppSettings.IsSpeakerEnabled)
        {
            YesRadioButton.IsChecked = true;
        }
        else
        {
            NoRadioButton.IsChecked = true;
        }
    }

    private void OnSaveButtonClicked(object sender, EventArgs e)
    {
        // Validate and save settings
        if (string.IsNullOrWhiteSpace(DurationEntry.Text) || !int.TryParse(DurationEntry.Text, out int duration))
        {
            DisplayAlert("Error", "Please enter a valid duration in seconds.", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(RetentionEntry.Text) || !int.TryParse(RetentionEntry.Text, out int retentionDays))
        {
            DisplayAlert("Error", "Please enter a valid number of days for history retention.", "OK");
            return;
        }

        AppSettings.Language = LanguagePicker.SelectedItem.ToString();
        AppSettings.CommandRecognitionDurationInSeconds = duration;
        AppSettings.HistoryRetentionDays = retentionDays;
        if (YesRadioButton.IsChecked)
        {
            AppSettings.IsSpeakerEnabled = true;
        }
        else
        {
            AppSettings.IsSpeakerEnabled = false;
        }

        AppSettings.SaveSettings();
        DisplayAlert("Success", "Settings have been updated.", "OK");
    }
}
