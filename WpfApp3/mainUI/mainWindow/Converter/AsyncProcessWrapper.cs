using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace HaruaConvert.mainUI.ConvertProcess
{
    public sealed class AsyncProcessWrapper : IAsyncDisposable, IDisposable
    {
        private readonly Process _process;
        private bool _disposed;

        public AsyncProcessWrapper(ProcessStartInfo startInfo)
        {
            _process = new Process { StartInfo = startInfo };
        }

        public Process Process => _process;



        public void Start() => _process.Start();

        public void Dispose()
        {
            if (_disposed)
                return;

            try
            {
                if (!_process.HasExited)
                    _process.Kill(); // 同期的に終了
                _process.Dispose();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in Dispose: {ex.Message}");
            }
            finally
            {
                _disposed = true;
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (_disposed)
                return;

            try
            {
                if (!_process.HasExited)
                {
                    _process.Kill(); // プロセスを終了
                    await Task.Run(() => _process.WaitForExit(1000)); // 非同期で終了待機
                }

                // 標準入出力ストリームを非同期的にクローズ
                if (_process.StartInfo.RedirectStandardError)
                {
                    await Task.Run(() => _process.StandardError?.Close());
                }
                if (_process.StartInfo.RedirectStandardInput)
                {
                    await Task.Run(() => _process.StandardInput?.Close());
                }
                if (_process.StartInfo.RedirectStandardOutput)
                {
                    await Task.Run(() => _process.StandardOutput?.Close());
                }

                // ProcessのDisposeを非同期コンテキストで実行
                await Task.Run(() => _process.Dispose());
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in DisposeAsync: {ex.Message}");
            }
            finally
            {
                _disposed = true;
            }
        }

        public void BeginErrorReadLine()
        {
            if (_process.StartInfo.RedirectStandardError)
            {
                _process.BeginErrorReadLine();
            }
        }
    }
}
