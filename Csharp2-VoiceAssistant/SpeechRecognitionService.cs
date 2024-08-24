using NAudio.Mixer;
using NAudio.Wave;
using System.Diagnostics;
using System.Text.Json;
using Vosk;

namespace Csharp2_VoiceAssistant
{
    public class SpeechRecognitionService
    {
        private readonly VoskRecognizer _recognizer;
        private bool _foundLaptop = false;
        private TaskCompletionSource<bool> _taskCompletionSource;

        public SpeechRecognitionService()
        {
            // Load Vosk model
            string modelPath = GetModelPath();
            Model model = new(modelPath);

            // Initialize recognizer
            _recognizer = new VoskRecognizer(model, 16000.0f);
        }

        // Loop to find project root directory independent of device
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

        // Method to set model (should be expanded to include language paramaters)
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

        // Main Listening thread
        public Task<bool> StartListening()
        {
            System.Diagnostics.Debug.WriteLine("start");
            _foundLaptop = false ;
            _taskCompletionSource = new TaskCompletionSource<bool>();

            var waveIn = new WaveInEvent
            {
                WaveFormat = new WaveFormat(16000, 1)
            };

            waveIn.DataAvailable += OnDataAvailable;
            waveIn.StartRecording();

            Task.Run(() =>
            {
                while (!_foundLaptop) { }
                waveIn.StopRecording();
                waveIn.Dispose();
            });
            return _taskCompletionSource.Task;
        }

        // Checks every word entered to match with the word laptop
        private void OnDataAvailable(object sender, WaveInEventArgs e)
        {
            if (_recognizer.AcceptWaveform(e.Buffer, e.BytesRecorded))
            {
                var result = _recognizer.Result();
                //System.Diagnostics.Debug.WriteLine($"Recognized: {result}");

                if (result.Contains("laptop", StringComparison.OrdinalIgnoreCase))
                {
                    System.Diagnostics.Debug.WriteLine("success1");
                    History.AddToHistory(DateTime.Now.ToString(), "Laptop heard", "Moving to Commands");
                    _foundLaptop = true;
                    _taskCompletionSource.TrySetResult(true);
                }
            }
            else
            {
                var partialResult = _recognizer.PartialResult();
                //System.Diagnostics.Debug.WriteLine($"Partial: {partialResult}");
            }
        }

        // Tries to find matching word after laptop is called
        public async Task<int> MatchCommandAsync(List<string> commands, int durationInSeconds = 8)
        {
            var waveIn = new WaveInEvent
            {
                WaveFormat = new WaveFormat(16000, 1)
            };

            waveIn.StartRecording();

            string recognizedText = string.Empty;
            waveIn.DataAvailable += (sender, e) =>
            {
                if (_recognizer.AcceptWaveform(e.Buffer, e.BytesRecorded))
                {
                    recognizedText += _recognizer.Result();
                }
            };

            await Task.Delay(durationInSeconds * 1000);
            waveIn.StopRecording();

            for (int i = 0; i < commands.Count; i++)
            {
                if (recognizedText.Contains(commands[i], StringComparison.OrdinalIgnoreCase))
                {
                    History.AddToHistory(DateTime.Now.ToString(), "Recognized command", commands[i]);
                    waveIn.Dispose();
                    return i; // Return the index of the matched command
                }
            }
            waveIn.Dispose();
            try
            {
                if (string.IsNullOrEmpty(recognizedText))
                {
                    History.AddToHistory(DateTime.Now.ToString(), "An error occured trying to recognize a voice command", "error: No text error");
                    return -1;
                }
                using (JsonDocument doc = JsonDocument.Parse(recognizedText))
                {
                    JsonElement root = doc.RootElement;
                    if (root.TryGetProperty("text", out JsonElement textElement))
                    {
                        var text = textElement.GetString();
                        History.AddToHistory(DateTime.Now.ToString(), "No command recognized", $"Text: {text}");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Write(ex.Message);
            }
            return -1; // No command matched
        }
    }
}
