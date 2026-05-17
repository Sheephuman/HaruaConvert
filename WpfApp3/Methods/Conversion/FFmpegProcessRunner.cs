using HaruaConvert.HaruaInterFace;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace HaruaConvert.Methods.Conversion
{
    public sealed class FFmpegProcessRunner : IFFmpegProcessRunner
    {
        public async Task<Process> StartAndWaitAsync(
            string arguments,
            DataReceivedEventHandler errorHandler,
            EventHandler exitedHandler,            
            Action<Process> onProcessStarted = null,
            CancellationToken cancellationToken = default)
        {
            using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            var tcs = new TaskCompletionSource<bool>();

            var ffProcessInfo = new ProcessStartInfo
            {
                FileName = Path.Combine("dll", "ffmpeg.exe"),
                Arguments = arguments,
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardInput = true
            };

            var process = new Process
            {
                StartInfo = ffProcessInfo,
                EnableRaisingEvents = true
            };

            process.ErrorDataReceived += errorHandler;
            process.Exited += (sender, e) => tcs.TrySetResult(true);
            process.Exited += exitedHandler;

            process.Start();
            process.BeginErrorReadLine();

            onProcessStarted?.Invoke(process);

            await Task.WhenAny(
                Task.Delay(Timeout.Infinite, linkedCts.Token),
                tcs.Task);

            if (linkedCts.Token.IsCancellationRequested)
            {
                linkedCts.Token.ThrowIfCancellationRequested();
            }

            return process;
        }
    }
}
