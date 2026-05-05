using HaruaConvert.HaruaInterFace;
using System.Threading;
using HaruaConvert;
using System.Threading.Tasks;
using System.Windows;

namespace HaruaConvert.Methods.Conversion
{
    public sealed class MainWindowConversionLauncher : IConversionUiLauncher
    {
        private readonly MainWindow _main;

        public MainWindowConversionLauncher(MainWindow main)
        {
            _main = main;
        }

        public bool HandleConversionWhenNoOverwritePromptRequired()
        {
            if (!_main.paramField.isExecuteProcessed)
            {
                _main.LogWindowShow();
                _main.paramField.isExecuteProcessed = true;

                _main.th1 = new Thread(async () => await _main.FfmpegProcessingAsnc());
                _main.th1.IsBackground = true;
                _main.th1.Start();
                return true;
            }
            else
            {
                MessageBox.Show("ffmpeg.exeが実行中なのです");
            }

            return false;
        }

        public void BeginConversionAfterOverwriteAccepted()
        {
            _main.LogWindowShow();
            _main.paramField.isExecuteProcessed = true;
            _main.th1 = new Thread(async () => await _main.FfmpegProcessingAsnc());
            _main.th1.Start();
        }
    }
}
