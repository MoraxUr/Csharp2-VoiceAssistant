using System.Globalization;

namespace Csharp2_VoiceAssistant;

public partial class Instructions : ContentPage
{
    private List<CsvItem> instructions;
    List<CsvItem> shownInstructions;

    public Instructions()
	{
		InitializeComponent();
        InitializeCommandPicker();
        instructions = GetCSV();
        shownInstructions = instructions.ToList();
        shownInstructions.Remove(instructions[0]);
        CsvCollectionView.ItemsSource = shownInstructions;
    }

    private void InitializeCommandPicker()
    {
        CommandPicker.ItemsSource = Enum.GetValues(typeof(AvailableCommands)).Cast<AvailableCommands>().ToList();
    }

    private void OnAddInstructionClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(KeywordEntry.Text) || CommandPicker.SelectedItem == null || string.IsNullOrWhiteSpace(Column3Entry.Text))
        {
            DisplayAlert("Error", "Please fill in all fields", "OK");
            return;
        }

        var newInstruction = new CsvItem
        {
            Column1 = KeywordEntry.Text, // Keyword
            Column2 = CommandPicker.SelectedItem.ToString(), // Command
            Column3 = Column3Entry.Text  // Additional data
        };

        instructions.Add(newInstruction);
        CsvCollectionView.ItemsSource = null; // Refresh the CollectionView
        CsvCollectionView.ItemsSource = instructions;

        NewInstruction(newInstruction.Column1, newInstruction.Column2, newInstruction.Column3);

        // Clear inputs
        KeywordEntry.Text = string.Empty;
        CommandPicker.SelectedIndex = -1;
        Column3Entry.Text = string.Empty;
    }

    private void OnDeleteInstructionClicked(object sender, EventArgs e)
    {
        var itemToDelete = (CsvItem)((ImageButton)sender).CommandParameter;
        instructions.Remove(itemToDelete);

        SaveInstructionsToFile(); // Save updated list to file
    }

    private async void OnEditInstructionClicked(object sender, EventArgs e)
    {
        var itemToEdit = (CsvItem)((ImageButton)sender).CommandParameter;

        // Prefill the form with existing data for editing
        KeywordEntry.Text = itemToEdit.Column1; // Prefill keyword
        CommandPicker.SelectedItem = Enum.Parse<AvailableCommands>(itemToEdit.Column2); // Prefill command
        Column3Entry.Text = itemToEdit.Column3; // Prefill additional data

        instructions.Remove(itemToEdit);

        SaveInstructionsToFile();
    }

    private void SaveInstructionsToFile()
    {
        var projectRoot = FileSystemHelper.GetProjectRootDirectory();
        var filePath = Path.Combine(projectRoot, "commands.csv");

        using (var writer = new StreamWriter(filePath, append: false))
        {
            foreach (var instruction in instructions)
            {
                writer.WriteLine($"{instruction.Column1},{instruction.Column2},{instruction.Column3}");
            }
        }

        shownInstructions = instructions.ToList();
        shownInstructions.Remove(shownInstructions[0]);
        CsvCollectionView.ItemsSource = null; // Refresh the CollectionView
        CsvCollectionView.ItemsSource = shownInstructions;
    }

    public void NewInstruction(string column1, string column2, string column3)
    {
        var projectRoot = FileSystemHelper.GetProjectRootDirectory();
        var filePath = Path.Combine(projectRoot, "commands.csv");

        using (var writer = new StreamWriter(filePath, append: true))
        {
            writer.WriteLine($"{column1},{column2},{column3}");
        }
        shownInstructions = instructions.ToList();
        shownInstructions.Remove(shownInstructions[0]);
        CsvCollectionView.ItemsSource = null; // Refresh the CollectionView
        CsvCollectionView.ItemsSource = shownInstructions;
    }


    public List<CsvItem> GetCSV()
    {
        List<CsvItem> instructionList = new List<CsvItem>();
        var projectRoot = FileSystemHelper.GetProjectRootDirectory();
        var filePath = Path.Combine(projectRoot, "commands.csv");

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"The commands.csv at {filePath} was not found.");
        }

        foreach (var item in File.ReadLines(filePath))
        {
            var values = item.Split(',');

            instructionList.Add(new CsvItem
            {
                Column1 = values[0],
                Column2 = values[1],
                Column3 = values[2]
            });
        }

        return instructionList;
    }
}