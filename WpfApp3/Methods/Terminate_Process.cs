using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace HaruaConvert
{

    public class Terminate_ProcessClass : IDisposable
    {

        Process killp;
        private bool disposedValue;

        public delegate Task KillProcessDeligate(int Target_id);

        public void mainTerminate_deligateExec(int _target_id)
        {

            KillProcessDeligate kp = Terminate_Process;

            kp(_target_id);

        }

        public Task Terminate_Process(int Target_P)
        {
            try
            {


                int target_id = Target_P;

                killp = Process.GetProcessById(target_id);

                using (killp = new Process())
                {
                    killp.StartInfo.FileName = "cmd.exe";
                    killp.StartInfo.UseShellExecute = false;

                    killp.StartInfo.CreateNoWindow = true;
                    killp.StartInfo.Arguments = "/c Taskkill /F /pid " + target_id;

                    killp.Start();
                    killp.WaitForExit();

                }

                if (killp.ExitCode == 0 || killp.Container != null)
                {
                    Debug.WriteLine($"Process {target_id} killed successfully.");

                }

                return Task.CompletedTask;
            }
            catch (System.InvalidOperationException ex)
            {
                Debug.WriteLine(ex.Message + "Sheep is lady!!");
                return Task.CompletedTask;
            }
            catch (System.Threading.Tasks.TaskCanceledException ex)
            {
                Debug.WriteLine(ex.Message + "Sheep is lady!!");
                return Task.CompletedTask;
            }

            catch (ArgumentException ex)
            {
                Debug.WriteLine(ex.Message + "プロセスIDがありません");
                return Task.CompletedTask;
            }

        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: マネージド状態を破棄します (マネージド オブジェクト)
                }

                // TODO: アンマネージド リソース (アンマネージド オブジェクト) を解放し、ファイナライザーをオーバーライドします
                // TODO: 大きなフィールドを null に設定します
                disposedValue = true;
            }
        }



        public void Dispose()
        {
            // このコードを変更しないでください。クリーンアップ コードを 'Dispose(bool disposing)' メソッドに記述します
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}