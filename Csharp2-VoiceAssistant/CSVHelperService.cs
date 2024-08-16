namespace Csharp2_VoiceAssistant
{
    public class CSVHelperService
    {
        public CSVHelperService()
        {
            string csvPath = GetCSVPath();
            
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

        public static string GetCSVPath()
        {
            var projectRoot = GetProjectRootDirectory();
            var csvPath = Path.Combine(projectRoot, "Instructions", "Instructions.csv");

            if (!Directory.Exists(csvPath))
            {
                // Make CSV if not present
            }

            return csvPath;
        }
    }
}
