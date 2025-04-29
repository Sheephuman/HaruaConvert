
using HaruaConvert.mainUI.ConvertProcess;
using HaruaConvert.mainUI.mainWindow;
using HaruaConvert.Methods;
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

        /// <summary>
        /// 共有箇所：LogWindowのConvertStopButton
        /// </summary>


        public EscapePath escapes { get; set; }



        public static Process ffmpegProcess { get; set; } = null!;
#pragma warning disable CA1051 // 参照可能なインスタンス フィールドを宣言しません
        public ParamCreateClasss param;
#pragma warning restore CA1051 // 参照可能なインスタンス フィールドを宣言しません


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
                //先頭パラメータを付ける




                var con = new ConvertFileNameClass();

                //保存先パスの有無判定
                if (!paramField.isOutputButtonChecked)
                    paramField.check_output = Path.GetDirectoryName(_fullPath) + "\\" + con.ConvertFileName(Path.GetFileName(_fullPath), harua_View);
                else
                    paramField.check_output = main.harua_View.OutputPath + "\\" + con.ConvertFileName(Path.GetFileName(main.paramField.setFile), harua_View);


                param = new ParamCreateClasss(_fullPath, paramField.check_output);


                //string pattern = @"\{FileName\}\.(\w+)";
                var target = main.harua_View.MainParams[0].StartQuery;
                //System.Text.RegularExpressions.Match match = Regex.Match(target, pattern);

                string extention = param.GetExtentionFileNamepattern(target);



                escapes = param.AddParamEscape(escapes, extention);
                if (!string.IsNullOrEmpty(extention))
                    paramField.check_output = escapes.NonEscape_outputPath;

                //_fullPath, harua_View
                //StartQueryを追加する
                _arguments = Ffmpc.AddsetQuery(escapes.inputPath, harua_View);

                _arguments = _arguments.Replace("{FileName}" + extention, "");


                //オプションと出力先ファイル文字列の追加
                _arguments = AddOptionClass.AddOption(_arguments) + " " + $"{escapes.outputPath}";
                Debug.WriteLine(_arguments);
            }


            else if (isUserParameter.IsChecked.Value) //used Original paramerter
            {
                var isOrigenelParam = new isUserOriginalParameter(this);
                bool isExecuteProcessed = isOrigenelParam.isUserOriginalParameter_Method(sender);

                if (!paramField.isSuccessdbuildQuery)
                    return false;
                //fetch flag State
            }

            #region ファイル存在判定

            //IDisposable alterr = new IDisposableBase();
            //alterr.Dispose();
            bool checker = false;

            var ifNoFiles = new IfNoFileExsistsClass(this);

            try
            {

                checker = FileExsosts_and_NoDialogCheck(paramField.check_output, NoDialogCheck.IsChecked.Value) ? DialogMethod() : ifNoFiles.IfNoFileExsists(Lw);

                paramField.isExecuteProcessed = checker;

                if (!paramField.isExecuteProcessed)
                    return false;
                //   if(!checker) //pushed No
                //    return false;

                //th1.Start();

                LogWindowShow();


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

            MessageBoxResult msbr = MessageBox.Show("ファイルが存在しますわ。上書きしますか？",
                "メッセージボックス", MessageBoxButton.YesNo,
                MessageBoxImage.Asterisk);


            if (msbr == MessageBoxResult.Yes)
            {


                // Lw = new LogWindow(paramField);

                // ParamField.isExitProcessed = false;

                th1 = new Thread(async () => await FfmpegProcessingAsnc());
                th1.Start();

                return true;

            }
            else
                return false;


        }

        bool FileExsosts_and_NoDialogCheck(string check_output, bool _DialogChecked)        //public async Task<string> CollectStandardOutput()
        {
            var sw = new Stopwatch();

            //-----------------
            // 計測開始
            sw.Start();

            //  var alterExsists = new Alternate_FileExsists();

            //bool exsisted = alterExsists.FileExsists(check_output);
            bool exsisted = File.Exists(check_output);

            bool DialogChecked = _DialogChecked;
            bool satisfied = false;

            if (exsisted && !DialogChecked)
                satisfied = true;

            // 計測停止
            sw.Stop();

            Debug.WriteLine("■処理Aにかかった時間");
            TimeSpan ts = sw.Elapsed;
            Debug.WriteLine($"　{ts}");
            Debug.WriteLine($"　{ts.Hours}時間 {ts.Minutes}分 {ts.Seconds}秒 {ts.Milliseconds}ミリ秒");
            Debug.WriteLine($"　{sw.ElapsedMilliseconds}ミリ秒");
            return satisfied;
        }

        long memorySize { get; set; }

        public static LogWindow Lw { get; set; }
        public bool IsDefaultQuerySet { get; internal set; }


        //TaskCompletionSource<bool> tcs;
        public async Task FfmpegProcessingAsnc(CancellationToken cancellationToken = default)
        {
            try
            {
                using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                await using var asyncProcess = new AsyncProcessWrapper(new ProcessStartInfo
                {

                    FileName = Path.Combine("dll", "ffmpeg.exe"),
                    Arguments = $"{_arguments}",
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardError = true,
                    RedirectStandardInput = true
                });

                ffmpegProcess = asyncProcess.Process;
                ffmpegProcess.EnableRaisingEvents = true;

                MainWindow.ffmpegProcess = ffmpegProcess;
                //nullかどうか判定用

                var tcs = new TaskCompletionSource<bool>();
                ffmpegProcess.ErrorDataReceived += (sender, e) =>
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

                ffmpegProcess.Exited += (sender, e) => tcs.TrySetResult(true);

                ffmpegProcess.Exited += new EventHandler(ffmpeg_Exited);

                asyncProcess.Start();
                paramField.ffmpeg_pid = ffmpegProcess.Id;
                ffmpegProcess.BeginErrorReadLine();

                await Task.WhenAny(
                    Task.Delay(Timeout.Infinite, cts.Token),
                    tcs.Task
                );

                if (cts.Token.IsCancellationRequested)
                {
                    cts.Token.ThrowIfCancellationRequested(); // キャンセル例外をスロー
                }
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
