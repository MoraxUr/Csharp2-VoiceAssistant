using NAudio.Wave;
using Vosk;

namespace Csharp2_VoiceAssistant
{
    public class SpeechRecognitionService
    {
        private readonly VoskRecognizer _recognizer;

        public static string GetProjectRootDirectory()
        {
            var currentDirectory = new DirectoryInfo(AppContext.BaseDirectory);

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
        public static string GetModelPath()
        {
            var projectRoot = GetProjectRootDirectory();
            var modelPath = Path.Combine(projectRoot, "Models", "vosk-model-small-en-us-0.15");
            //var modelPath = Path.Combine(projectRoot, "Models", "vosk-model-nl-spraakherkenning-0.6");

            if (!Directory.Exists(modelPath))
            {
                throw new DirectoryNotFoundException($"The model directory at {modelPath} was not found.");
            }

            return modelPath;
        }

        public SpeechRecognitionService()
        {
            // Load Vosk model
            var modelPath = GetModelPath();

            var model = new Model(modelPath);

            // Initialize recognizer
            _recognizer = new VoskRecognizer(model, 16000.0f);
        }

        public void StartListening()
        {
            using var waveIn = new WaveInEvent
            {
                WaveFormat = new WaveFormat(16000, 1)
            };

            waveIn.DataAvailable += OnDataAvailable;
            waveIn.StartRecording();

            System.Diagnostics.Debug.WriteLine("Listening... Press Ctrl+C to stop.");
            while (true) { } // Keep the app running for demonstration purposes
        }

        private void OnDataAvailable(object sender, WaveInEventArgs e)
        {
            if (_recognizer.AcceptWaveform(e.Buffer, e.BytesRecorded))
            {
                var result = _recognizer.Result();
                System.Diagnostics.Debug.WriteLine($"Recognized: {result}");

                if (result.Contains("laptop", StringComparison.OrdinalIgnoreCase))
                {
                    System.Diagnostics.Debug.WriteLine("success");
                }
            }
            else
            {
                var partialResult = _recognizer.PartialResult();
                System.Diagnostics.Debug.WriteLine($"Partial: {partialResult}");
            }
        }
    }
}
