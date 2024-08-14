using System.Diagnostics;

namespace Csharp2_VoiceAssistant;

public partial class MainPage : ContentPage
{
    private readonly SpeechRecognitionService _speechRecognitionService;
    public MainPage()
    {
        InitializeComponent();
        _speechRecognitionService = new SpeechRecognitionService();
        Listen();
    }

    private async void Listen()
    {
        bool recognizedKeyword = await _speechRecognitionService.StartListening();
        if (recognizedKeyword)
        {
            // Define the list of commands to match (should be taken from csv or something)
            List<string> commands = new List<string>
            {
                "volume 100",
                "volume up",
                "volume down",
                "mute",
                // Add more commands as needed
            };

            // Start listening for commands after detecting the keyword
            int commandIndex = await _speechRecognitionService.MatchCommandAsync(commands);

            if (commandIndex != -1)
            {
                System.Diagnostics.Debug.WriteLine($"Command matched: {commands[commandIndex]}");
                // Handle the matched command
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("No command matched.");
            }

            Listen(); // Restart listening
        }
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
