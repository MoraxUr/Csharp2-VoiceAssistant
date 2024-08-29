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
        var projectRoot = FileSystemHelper.GetProjectRootDirectory();
        var filePath = Path.Combine(projectRoot, "history.csv");

        using (var writer = new StreamWriter(filePath, append: true))
        {
            writer.WriteLine($"{column1},{column2},{column3}");
        }
    }

    public List<CsvItem> GetCSV()
    {
        List<CsvItem> history = new List<CsvItem>();
        var projectRoot = FileSystemHelper.GetProjectRootDirectory();
        var filePath = Path.Combine(projectRoot, "history.csv");

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"The history.csv at {filePath} was not found.");
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
        List<CsvItem> cleaned = new List<CsvItem>();

        foreach (var item in history)
        {
            if (item.Column1.Equals("timestamp"))
            {
                cleaned.Add(item);
            }
            else
            {
                DateTime itemTime = DateTime.Parse(item.Column1);
                DateTime now = DateTime.Now;
                if ((now - itemTime).Days <= AppSettings.HistoryRetentionDays)
                {
                    cleaned.Add(item);

                }
            }
        }

        overwriteHistory(cleaned);

        return cleaned;
    }

    public void overwriteHistory(List<CsvItem> list)
    {
        var projectRoot = FileSystemHelper.GetProjectRootDirectory();
        var filePath = Path.Combine(projectRoot, "history.csv");

        using (var writer = new StreamWriter(filePath, append: false))
        {
            writer.WriteLine($"{list[0].Column1},{list[0].Column2},{list[0].Column3}");
        }

        foreach (var item in list)
        {
            if(item != list[0])
            {
                using (var writer = new StreamWriter(filePath, append: true))
                {
                    writer.WriteLine($"{item.Column1},{item.Column2},{item.Column3}");
                }
            }
        }
    }
}