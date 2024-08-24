namespace Csharp2_VoiceAssistant;

public partial class History : ContentPage
{
	public History()
	{
		InitializeComponent();
        List<CsvItem> history = GetCSV();
        CsvCollectionView.ItemsSource = history;
	}

    public static void AddToHistory(string column1, string column2, string column3)
    {
        var projectRoot = GetProjectRootDirectory();
        var filePath = Path.Combine(projectRoot, "history.csv");

        using (var writer = new StreamWriter(filePath, append: true))
        {
            writer.WriteLine($"{column1},{column2},{column3}");
        }
    }

    public List<CsvItem> GetCSV()
    {
        List<CsvItem> history = new List<CsvItem>();
        var projectRoot = GetProjectRootDirectory();
        var filePath = Path.Combine(projectRoot, "history.csv");

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"The command.csv at {filePath} was not found.");
        }

        foreach (var item in File.ReadLines(filePath))
        {
            var values = item.Split(',');

            history.Add(new CsvItem
            {
                Column1 = values[0],
                Column2 = values[1],
                Column3 = values[2]
            });
        }

        return history;
    }

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
}