using Csharp2_VoiceAssistant.ConcreteCommands;

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
            List<string> instructions = LoadAllInstructionsFromCsv();

            // Start listening for commands after detecting the keyword
            int commandIndex = await _speechRecognitionService.MatchCommandAsync(instructions);

            if (commandIndex != -1)
            {
                string matchedCommand = instructions[commandIndex];
                System.Diagnostics.Debug.WriteLine($"Command matched: {matchedCommand}");

                // Load the commands associated with the matched keyword
                var invoker = new CommandService();
                var commandList = LoadCommandsFromCsv(matchedCommand);

                foreach (var command in commandList)
                {
                    invoker.SetCommand(command);
                }

                // Execute the loaded commands
                invoker.ExecuteCommands();
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("No command matched.");
            }

            Listen(); // Restart listening
        }
    }

    public List<string> LoadAllInstructionsFromCsv()
    {
        List<string> instructions = new List<string>();
        var projectRoot = GetProjectRootDirectory();
        var filePath = Path.Combine(projectRoot, "commands.csv");

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"The command.csv at {filePath} was not found.");
        }
        var lines = File.ReadAllLines(filePath);

        foreach (var line in lines)
        {
            var parts = line.Split(',');
            string stringToMatch = parts[0].Trim();
            if (!instructions.Contains(stringToMatch))
            {
                instructions.Add(stringToMatch);
            }
        }
        return instructions;
    }

    public List<ICommand> LoadCommandsFromCsv(string phrase)
    {
        List<ICommand> commands = new List<ICommand>();

        // Code below shared(or very similar to) other lines in class and in speechrecognitionservice.
        // should be implemented in seperate filesystem class
        var projectRoot = GetProjectRootDirectory();
        var filePath = Path.Combine(projectRoot, "commands.csv");

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"The command.csv at {filePath} was not found.");
        }

        var lines = File.ReadAllLines(filePath);
        
        // Reads all the lines in a csv file, followed by seperating each line on the commas to get a clear distinction between
        // phrase, command, and parameter for each command.
        foreach (var line in lines)
        {
            var parts = line.Split(',');
            if (parts[0].Equals(phrase, StringComparison.OrdinalIgnoreCase))
            {
                var commandType = parts[1];
                var parameter = parts[2];
                if (Enum.TryParse(commandType, true, out AvailableCommands command))
                {
                    switch (command)
                    {
                        case AvailableCommands.VolumeCommand:
                            commands.Add(new VolumeCommand(int.Parse(parameter)));
                            break;
                        case AvailableCommands.OpenAppCommand:
                            commands.Add(new OpenAppCommand(parameter));
                            break;
                        case AvailableCommands.ScreenBrightnessCommand:
                            commands.Add(new ScreenBrightnessCommand(int.Parse(parameter)));
                            break;
                    }
                }
            }
        }
        return commands;
    }

    // Shared with SpeechRecognitionService, should be made to seperate filesystem class
    public static string GetProjectRootDirectory()
    {
        DirectoryInfo? currentDirectory = new(AppContext.BaseDirectory);

        while (currentDirectory != null && !currentDirectory.GetFiles("*.csproj").Any())
        {
            currentDirectory = currentDirectory.Parent;
        }

        if (currentDirectory == null)
        {
            throw new DirectoryNotFoundException("Project root directory not found.");
        }

        return currentDirectory.FullName;
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
