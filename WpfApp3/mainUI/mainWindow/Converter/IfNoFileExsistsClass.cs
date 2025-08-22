using System.Threading;
using System.Windows;

namespace HaruaConvert.Methods
{
    public class IfNoFileExsistsClass
    {
        public IfNoFileExsistsClass(MainWindow _main)
        {
            main = _main;
        }
        MainWindow main;

        public bool IfNoFileExsists(LogWindow Lw)
        {
            //ffmpegが終了している状態のとき
            if (!main.paramField.isExecuteProcessed)
            {
                main.LogWindowShow();
                main.paramField.isExecuteProcessed = true;

                main.th1 = new Thread(async () => await main.FfmpegProcessingAsnc());
                main.th1.IsBackground = true; // メインスレッドが終了すると自動的に終了
                main.th1.Start();

            }

            else
            {

                MessageBox.Show("ffmpeg.exeが実行中なのです");
            }
            return false;

        }


    }
}
