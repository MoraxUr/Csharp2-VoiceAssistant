namespace Csharp2_VoiceAssistant;

public partial class MainPage : ContentPage
{
public MainPage()
    {
        InitializeComponent();
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
        await Navigation.PushAsync(new Settings());
    }
}
