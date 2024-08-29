namespace Csharp2_VoiceAssistant
{
    public class FileSystemHelper
    {
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
}
