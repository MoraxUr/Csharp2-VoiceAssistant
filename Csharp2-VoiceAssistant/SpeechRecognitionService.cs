using NAudio.Wave;
using System.Diagnostics;
using System.Text.Json;
using Vosk;

namespace Csharp2_VoiceAssistant
{
    public class SpeechRecognitionService
    {
        private const float SAMPLE_RATE = 16000.0f;
        private const int SAMPLE_RATE_INT = 16000;
        private const int CHANNELS = 1;
        private const int MILLISECONDS_PER_SECOND = 1000;

        private readonly VoskRecognizer _recognizer;
        private bool _foundLaptop = false;
        private TaskCompletionSource<bool> _taskCompletionSource;

        public SpeechRecognitionService()
        {
            // Load Vosk model
            string modelPath = GetModelPath();
            Model model = new(modelPath);

            // Initialize recognizer
            _recognizer = new VoskRecognizer(model, SAMPLE_RATE);
        }

        // Method to set model (should be expanded to include language paramaters)
        public static string GetModelPath()
        {
            var projectRoot = FileSystemHelper.GetProjectRootDirectory();
            string modelPath;
            if (AppSettings.Language == "NL")
            {
                modelPath = Path.Combine(projectRoot, "Models", "vosk-model-nl-spraakherkenning-0.6");
            }
            else
            {
                modelPath = Path.Combine(projectRoot, "Models", "vosk-model-small-en-us-0.15");
            }
            
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
                WaveFormat = new WaveFormat(SAMPLE_RATE_INT, CHANNELS)
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

        private byte[] AmplifyAudio(byte[] buffer, int bytesRecorded)
        {
            float gain = 1.0f;
            byte[] amplifiedBuffer = new byte[bytesRecorded];

            for (int i = 0; i < bytesRecorded; i += 2)
            {
                short sample = BitConverter.ToInt16(buffer, i);
                int amplifiedSample = (int)(sample * gain);
                if (amplifiedSample > short.MaxValue)
                {
                    amplifiedSample = short.MaxValue;
                }
                else if (amplifiedSample < short.MinValue)
                {
                    amplifiedSample = short.MinValue;
                }

                byte[] amplifiedSampleBytes = BitConverter.GetBytes((short)amplifiedSample);
                amplifiedBuffer[i] = amplifiedSampleBytes[0];
                amplifiedBuffer[i + 1] = amplifiedSampleBytes[1];
            }
            return amplifiedBuffer;
        }

        // Checks every word entered to match with the word laptop
        private void OnDataAvailable(object sender, WaveInEventArgs e)
        {
            byte[] amplifiedBuffer = AmplifyAudio(e.Buffer, e.BytesRecorded);
            if (_recognizer.AcceptWaveform(amplifiedBuffer, e.BytesRecorded))
            {
                var result = _recognizer.Result();
                //System.Diagnostics.Debug.WriteLine($"Recognized: {result}");

                if (result.Contains("laptop", StringComparison.OrdinalIgnoreCase))
                {
                    System.Diagnostics.Debug.WriteLine("success1");
#if WINDOWS
                    TTS.speak("Yes?");
#endif
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
        public async Task<int> MatchCommandAsync(List<string> commands)
        {
            var waveIn = new WaveInEvent
            {
                WaveFormat = new WaveFormat(SAMPLE_RATE_INT, CHANNELS)
            };

            waveIn.StartRecording();

            string recognizedText = string.Empty;
            waveIn.DataAvailable += (sender, e) =>
            {
                byte[] amplifiedBuffer = AmplifyAudio(e.Buffer, e.BytesRecorded);
                if (_recognizer.AcceptWaveform(amplifiedBuffer, e.BytesRecorded))
                {
                    recognizedText += _recognizer.Result();
                }
            };

            await Task.Delay(AppSettings.CommandRecognitionDurationInSeconds * MILLISECONDS_PER_SECOND);
            waveIn.StopRecording();

            for (int i = 0; i < commands.Count; i++)
            {
                if (recognizedText.Contains(commands[i], StringComparison.OrdinalIgnoreCase))
                {
                    History.AddToHistory(DateTime.Now.ToString(), "Recognized command", commands[i]);
                    waveIn.Dispose();
#if WINDOWS
                    Random random = new Random();
                    int randomValue = random.Next(0, 4);
                    switch (randomValue)
                    {
                        case 0:
                            TTS.speak("Of course");
                            break;
                        case 1:
                            TTS.speak("As you wish");
                            break;
                        case 2:
                            TTS.speak("On it");
                            break;
                        case 3:
                            TTS.speak("Right away");
                            break;
                    }
#endif
                    return i; // Return the index of the matched command
                }
            }
            waveIn.Dispose();
            try
            {
                if (string.IsNullOrEmpty(recognizedText))
                {
                    History.AddToHistory(DateTime.Now.ToString(), "An error occured trying to recognize a voice command", "error: No text error");
#if WINDOWS
                    TTS.speak("I'm sorry, Could you speak a little louder");
#endif
                    return -1;
                }
                using (JsonDocument doc = JsonDocument.Parse(recognizedText))
                {
                    JsonElement root = doc.RootElement;
                    if (root.TryGetProperty("text", out JsonElement textElement))
                    {
#if WINDOWS
                        TTS.speak("I'm sorry, I didn't recognise any command");
#endif
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
