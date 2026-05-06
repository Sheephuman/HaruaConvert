using HaruaConvert.HaruaInterFace;
using HaruaConvert.Parameter;
using NAudio.Wave;
using System.IO;
using System.Threading.Tasks;
using HaruaConvert.mainUI.mainWindow.LogWindow;

namespace HaruaConvert.Methods.Conversion
{
    public sealed class FFmpegPostProcessHandler : IFFmpegPostProcessHandler
    {
        private readonly IOpenExplorer _openExplorer;

        public FFmpegPostProcessHandler(IOpenExplorer openExplorer)
        {
            _openExplorer = openExplorer;
        }

        public async Task HandleAfterProcessExitAsync(ParamField paramField)
        {
            var current = Directory.GetCurrentDirectory();
            paramField.isExecuteProcessed = false;

            using (WaveOutEvent outputDevice = new WaveOutEvent())
            using (AudioFileReader afr = new AudioFileReader(current + @"\\dll\\しょどーる参上.wav"))
            {
                outputDevice.Init(afr);
                var playbackCompleted = new TaskCompletionSource<bool>();
                outputDevice.PlaybackStopped += (sender, args) => playbackCompleted.TrySetResult(true);

                outputDevice.Play();
                _openExplorer.OpenExplorer(paramField);

                // Wait until sound playback completes to preserve current behavior.
                await playbackCompleted.Task;
            }
        }
    }
}
