namespace Csharp2_VoiceAssistant;

public partial class MainPage : ContentPage
{
    private readonly SpeechRecognitionService _speechRecognitionService;
    public MainPage()
    {
        InitializeComponent();
        _speechRecognitionService = new SpeechRecognitionService();
        Task.Run(() => _speechRecognitionService.StartListening());
    }

    private async void Instructions_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Instructions());
    }

    private async void History_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new History());
    }

    private async void Settings_Clicked(object sender, EventArgs e)
    {
        Console.WriteLine("SettingsPressed");
        RecognitionTextLabel.Text = "SettingsPressed";
        await Navigation.PushAsync(new Settings());
    }
}
