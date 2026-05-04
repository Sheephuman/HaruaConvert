using HaruaConvert.HaruaInterFace;
using HaruaConvert.mainUI.mainWindow;
using HaruaConvert.mainUI.QueryCreateWindow.LogWindow;
using HaruaConvert.Methods;
using HaruaConvert.Methods.Conversion;
using HaruaConvert.Parameter;
using NAudio.Wave;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using WpfApp3.Parameter;

namespace HaruaConvert
{
    //　MainWindowに属するファイル(UI側)

    public partial class MainWindow : Window
    {

        public string _arguments { get; set; }
        public static string staticArguments { get; set; }

        /// <summary>
        /// 共有箇所：LogWindowのConvertStopButton
        /// </summary>


        public EscapePath escapes { get; set; }

        AddOptionClass _addOption = new();
        private readonly IConversionExecutionPreparer _conversionExecutionPreparer = new ConversionExecutionPreparer();
        private readonly IFFmpegProcessRunner _ffmpegProcessRunner = new FFmpegProcessRunner();
        private readonly IConversionOutputConflictEvaluator _outputConflictEvaluator = new ConversionOutputConflictEvaluator();
        private readonly IOverwritePrompt _overwritePrompt = new WpfOverwritePrompt();
        private IConversionUiLauncher? _conversionUiLauncher;

        private IConversionUiLauncher ConversionUiLauncher =>
            _conversionUiLauncher ??= new MainWindowConversionLauncher(this);
        public static Process ffmpegProcess { get; set; } = null!;
#pragma warning disable CA1051 // 参照可能なインスタンス フィールドを宣言しません
        public ParamCreateClasss param;
#pragma warning restore CA1051 // 参F照可能なインスタンス フィールドを宣言しません


        /// <summary>
        /// 共有箇所：LogWindowのClose時
        /// </summary>
        public Thread th1 { get; set; } = null!;

        public void GetLw()
        {
            //if(paramField != null)
            //     Lw = new LogWindow(paramField);
            //     return Lw;
        }

        //   delegate string addOptionDeligate(string _argument);

        /// <summary>
        　///UI側の実行を請け負うメソッド UIの延長として扱う
        ///　UISide の実装は基本的にこちら。
        /// </summary>
        /// <param name="_fullPath"></param>
        /// <returns></returns>
        public bool mainFileConvertExec(string _fullPath, object sender)
        {

            escapes = new EscapePath();



            baseArguments = "";

            if (_arguments == null)
                _arguments = "";



            //Whether to use Original Paramerter Query
            var chButton = VisualTreeHelperWrapperHelpers.FindDescendant<Button>(Drop_Label);



            if (ClassShearingMenbers.ButtonName == chButton.Name)
            {
                var prepared = _conversionExecutionPreparer.PrepareDropConversion(
                    _fullPath,
                    harua_View,
                    paramField,
                    NoAudio.IsChecked == true,
                    main.harua_View.OutputPath,
                    main.paramField.setFile);

                paramField.check_output = prepared.CheckOutputPath;
                _arguments = prepared.Arguments;
                escapes = prepared.Escapes;
                param = prepared.ParameterCreator;
            }


            //IsChecked.HasValue は「値があるかどうか」だけを見る
            //通常の2 - stateチェックボックスでは true / false どちらでも値はあるため、ほぼ常に true

            ///test

            else if (isUserParameter.IsChecked == true) //used Original paramerter
            {
                var isOrigenelParam = new isUserOriginalParameter(this);
                _ = isOrigenelParam.isUserOriginalParameter_Method(sender);

                if (!paramField.isSuccessdbuildQuery)
                    return false;
                //fetch flag State
            }

            #region ファイル存在判定

            //IDisposable alterr = new IDisposableBase();
            //alterr.Dispose();
            bool checker = false;
            bool needsOverwritePrompt = _outputConflictEvaluator.ShouldPromptOverwrite(
                paramField.check_output,
                NoDialogCheck.IsChecked == true);

            try
            {
                checker = needsOverwritePrompt
                    ? DialogMethod()
                    : ConversionUiLauncher.HandleConversionWhenNoOverwritePromptRequired();

                if (needsOverwritePrompt)
                {
                    paramField.isExecuteProcessed = checker;
                }
                else
                {
                    if (!checker)
                    {
                        return false;
                    }
                }

                if (!paramField.isExecuteProcessed)
                    return false;
                //   if(!checker) //pushed No
                //    return false;

                //th1.Start();




                //th1 = new Thread(() => ffmpegProsseing());

                //th1.Start();





                return true;
            }

            #endregion

            catch (Exception ex)
            {
                th1.Join();

                MessageBox.Show(ex.Message);
                return false;
            }

        }

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

        bool DialogMethod()
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
                            Lw.RichTextRogs.AppendText(e.Data + Environment.NewLine);
                            if (Lw.AutoScroll_Checker.IsChecked == true)
                                Lw.RichTextRogs.ScrollToEnd();
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
                    Lw?.RichTextRogs.AppendText($"Error: {ex.Message}{Environment.NewLine}");
                });
            }
        }

        public IOpenExplorer _openExplorerTest { get; }

        public MainWindow(IOpenExplorer openExplorer)
        {
            _openExplorerTest = openExplorer;
        }


        private async void ffmpeg_Exited(object sender, EventArgs e)
        {
            try
            {
                // プロセス終了時の処理
                var tcs = new TaskCompletionSource<bool>();
                tcs.TrySetResult(true); // プロセス終了を通知


                var current = Directory.GetCurrentDirectory();



                paramField.isExecuteProcessed = false;


                //wave出力の初期化



                using (WaveOutEvent outputDevice = new WaveOutEvent())
                using (AudioFileReader afr = new AudioFileReader(current + @"\\dll\\しょどーる参上.wav"))
                {
                    outputDevice.Init(afr);
                    var playbackCompleted = new TaskCompletionSource<bool>();
                    outputDevice.PlaybackStopped += (sender, args) => playbackCompleted.TrySetResult(true);

                    outputDevice.Play();



                    var opex = new OpernExplorerClass();
                    opex.OpenExplorer(paramField);




                    //Usingステートメントを入れると即座に破棄されるため、鳴らなくなる　
                    // 再生が完了するまで待機
                    await playbackCompleted.Task;

                }


            }
            catch (System.IO.FileNotFoundException ex)
            {
                MessageBox.Show(ex.Message + "\r\n" + "Waveファイルが見つかりません");
            }

        }





        private void process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            //Debug.WriteLine(e);
        }



    }


}
