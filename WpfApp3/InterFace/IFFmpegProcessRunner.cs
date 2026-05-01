using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace HaruaConvert.HaruaInterFace
{
    public interface IFFmpegProcessRunner
    {
        Task<Process> StartAndWaitAsync(
            string arguments,
            DataReceivedEventHandler errorHandler,
            EventHandler exitedHandler,
            CancellationToken cancellationToken = default);
    }
}
