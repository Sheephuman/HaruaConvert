using System;
using System.Diagnostics;
using System.Windows;

namespace HaruaConvert
{

    public class Terminate_ProcessClass:IDisposable
    {

        Process killp;
        private bool disposedValue;

        public delegate void KillProcessDeligate(int Target_id);

        public void mainTerminate_deligateExec(int _target_id)
        {

            KillProcessDeligate kp = Terminate_Process;

            kp(_target_id);
            //SomeDelegate a = new SomeDelegate(A);

        }

        public void Terminate_Process(int Target_P)
        {
            try
            {


                int target_id = Target_P;

                killp = Process.GetProcessById(target_id);

                using (killp = new Process())
                {
                    killp.StartInfo.FileName = "cmd.exe";

                    killp.StartInfo.CreateNoWindow = true;
                    killp.StartInfo.Arguments = "/c Taskkill /F /pid " + target_id;

                    killp.Start();
                    killp.WaitForExit();

                }


            }
            catch (System.Threading.Tasks.TaskCanceledException ex)
            {
                Console.WriteLine(ex.Message + "Sheep is lady!!");
            }

            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message + "プロセスIDがありません");
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

        // // TODO: 'Dispose(bool disposing)' にアンマネージド リソースを解放するコードが含まれる場合にのみ、ファイナライザーをオーバーライドします
        // ~Terminate_ProcessClass()
        // {
        //     // このコードを変更しないでください。クリーンアップ コードを 'Dispose(bool disposing)' メソッドに記述します
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // このコードを変更しないでください。クリーンアップ コードを 'Dispose(bool disposing)' メソッドに記述します
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}