using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace HaruaConvert.HaruaInterFace
{
    public interface IFFmpegProcessRunner
    {
        /// <summary>
        /// /メソッド 'HaruaConvert.HaruaInterFace.IFFmpegProcessRunner.StartAndWaitAsync(string, System.Diagnostics.DataReceivedEventHandler, System.EventHandler, System.Threading.CancellationToken, System.Action<System.Diagnostics.Process>?)' は、CancellationToken を最後のパラメーターとして受け取る必要があります
        /// </summary>
        /// <param name="arguments"></param>
        /// <param name="errorHandler"></param>
        /// <param name="exitedHandler"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="onProcessStarted"></param>
        /// <returns></returns>
        Task<Process> StartAndWaitAsync(
            string arguments,
            DataReceivedEventHandler errorHandler,
            EventHandler exitedHandler,
            CancellationToken cancellationToken = default,
            Action<Process>? onProcessStarted = null);
    }
}
