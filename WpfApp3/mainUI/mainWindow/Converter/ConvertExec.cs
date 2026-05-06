using HaruaConvert.HaruaInterFace;
using HaruaConvert.mainUI.QueryCreateWindow.LogWindow;
using HaruaConvert.Methods;
using HaruaConvert.Methods.Conversion;
using HaruaConvert.Parameter;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace HaruaConvert
{
    //　MainWindowに属するファイル(UI側)

    public partial class MainWindow : Window
    {

        public string _arguments { get; set; }
        /// <summary>
        /// 共有箇所：LogWindowのConvertStopButton
        /// </summary>


        public EscapePath escapes { get; set; }

        AddOptionClass _addOption = new();
        private readonly IConversionExecutionPreparer _conversionExecutionPreparer = new ConversionExecutionPreparer();
        private readonly IFFmpegProcessRunner _ffmpegProcessRunner = new FFmpegProcessRunner();
        private readonly IFFmpegPostProcessHandler _ffmpegPostProcessHandler = new FFmpegPostProcessHandler(new OpernExplorerClass());
        private readonly IConversionOutputConflictEvaluator _outputConflictEvaluator = new ConversionOutputConflictEvaluator();
        private readonly IOverwritePrompt _overwritePrompt = new WpfOverwritePrompt();
        private IConversionUiLauncher? _conversionUiLauncher;

        private IConversionUiLauncher ConversionUiLauncher =>
            _conversionUiLauncher ??= new MainWindowConversionLauncher(this);

        private IMainFileConversionOrchestrator? _mainFileConversionOrchestrator;

        private IMainFileConversionOrchestrator MainFileConversionOrchestrator =>
            _mainFileConversionOrchestrator ??= new MainFileConversionOrchestrator(
                this,
                _conversionExecutionPreparer,
                _outputConflictEvaluator,
                () => ConversionUiLauncher);
        public static Process ffmpegProcess { get; set; } = null!;
#pragma warning disable CA1051 // 参照可能なインスタンス フィールドを宣言しません
        public ParamCreateClasss param;
#pragma warning restore CA1051 // 参F照可能なインスタンス フィールドを宣言しません


        /// <summary>
        /// 共有箇所：LogWindowのClose時
        /// </summary>
        public Thread th1 { get; set; } = null!;

       

        //   delegate string addOptionDeligate(string _argument);

        /// <summary>
        　///UI側の実行を請け負うメソッド UIの延長として扱う
        ///　UISide の実装は基本的にこちら。
        /// </summary>
        /// <param name="_fullPath"></param>
        /// <returns></returns>
        public bool mainFileConvertExec(string _fullPath, object sender) =>
            MainFileConversionOrchestrator.Execute(_fullPath, sender);

        public void LogWindowShow()
        {
            if (!firstlogWindow)
            {
                th1 = new Thread(async () => await FfmpegProcessingAsnc());
                Lw = new LogWindow(paramField);
                Lw.Show();
                firstlogWindow = true;

            }
            else
            {
                Lw.WindowState = WindowState.Normal;
                Lw.Topmost = true;
                Lw.Activate();
                Lw.Topmost = false;
            }

        }

        internal bool DialogMethod()
        {
            if (!_overwritePrompt.AskOverwriteExistingFile())
            {
                return false;
            }

            ConversionUiLauncher.BeginConversionAfterOverwriteAccepted();
            return true;
        }

        public static LogWindow Lw { get; set; }


        public DataReceivedEventHandler handler { get; set; }
        //TaskCompletionSource<bool> tcs;
        public async Task FfmpegProcessingAsnc(CancellationToken cancellationToken = default)
        {
            try
            {
                handler = (sender, e) =>
                {
                    if (e.Data != null && Lw != null)
                    {
                        Dispatcher.InvokeAsync(() =>
                        {
                            Lw.AppendLogLine(e.Data);
                        });
                    }
                };

                ffmpegProcess = await _ffmpegProcessRunner.StartAndWaitAsync(
                    _arguments,
                    handler,
                    ffmpeg_Exited,
                    cancellationToken,
                    p =>
                    {
                        MainWindow.ffmpegProcess = p;
                        paramField.ffmpeg_pid = p.Id;
                    });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error: {ex.Message}");
                await Dispatcher.InvokeAsync(() =>
                {
                    Lw?.AppendLogLine($"Error: {ex.Message}");
                });
            }
        }

        public IOpenExplorer _openExplorerTest { get; }

 

        private async void ffmpeg_Exited(object sender, EventArgs e)
        {
            try
            {
                await _ffmpegPostProcessHandler.HandleAfterProcessExitAsync(paramField);
            }
            catch (System.IO.FileNotFoundException ex)
            {
                MessageBox.Show(ex.Message + "\r\n" + "Waveファイルが見つかりません");
            }
        }
    }
}
