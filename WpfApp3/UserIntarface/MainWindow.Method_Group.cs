﻿using HaruaConvert.Methods;
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
        public Process _FfmpProcess { get; set; } = null!;

       public EscapePath escapes { get; set; }


#pragma warning disable CA1051 // 参照可能なインスタンス フィールドを宣言しません
        public ParamCreateClasss param;
#pragma warning restore CA1051 // 参照可能なインスタンス フィールドを宣言しません


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
        public bool FileConvertExec(string _fullPath,object sender)
        {
            th1 = new Thread(new ThreadStart(ffmpegProsseing));
            //For Kill ffmpeg Process


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
                if (string.IsNullOrEmpty(_fullPath))
                    paramField.check_output = _fullPath + "\\" + con.ConvertFileName(Path.GetFileName(_fullPath), harua_View);
                else
                    paramField.check_output = Path.GetDirectoryName(_fullPath) + "\\" + con.ConvertFileName(Path.GetFileName(_fullPath), harua_View);

                



                param = new ParamCreateClasss(_fullPath, paramField.check_output);

                escapes = param.AddParamEscape(escapes);

                //_fullPath, harua_View

                _arguments = Ffmpc.AddsetQuery(escapes.inputPath, harua_View);
                _arguments = AddOptionClass.AddOption(_arguments) + " " + $"{ escapes.outputPath}";

            }


            else if (isUserParameter.IsChecked.Value) //used Original paramerter
            {
                var isOrigenelParam = new isUserOriginalParameter(this);
                isOrigenelParam.isUserOriginalParameter_Method(sender);
            }

            #region ファイル存在判定

            //IDisposable alterr = new IDisposableBase();
            //alterr.Dispose();
            bool checker = false;

            var ifNoFiles = new IfNoFileExsistsClass(this);
            
            try
            {
                using (var Alternate_FileExsists = new Alternate_FileExsists())
                {
                    checker = FileExsosts_and_NoDialogCheck(paramField.check_output, NoDialogCheck.IsChecked.Value) ? DialogMethod() : ifNoFiles.IfNoFileExsists();
                    
                    return checker;
                }
            }
            
            #endregion

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }



        bool DialogMethod()
        {

            MessageBoxResult msbr = MessageBox.Show("ファイルが存在しますわ。上書きしますか？",
                "メッセージボックス", MessageBoxButton.YesNo,
                MessageBoxImage.Asterisk);


            if (msbr == MessageBoxResult.Yes)
            {

                th1.Start();
                Lw.Show();
                Lw.Activate();


                // ParamField.isExitProcessed = false;

                return false;

            }
            else
            {
                return true;
            }

        }

        bool FileExsosts_and_NoDialogCheck(string check_output, bool _DialogChecled)        //public async Task<string> CollectStandardOutput()
        {
            var sw = new Stopwatch();

            //-----------------
            // 計測開始
            sw.Start();

            //  var alterExsists = new Alternate_FileExsists();

            //bool exsisted = alterExsists.FileExsists(check_output);
            bool exsisted = File.Exists(check_output);

            bool DialogChecled = _DialogChecled;
            bool satisfied = false;

            if (exsisted && !DialogChecled)
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


        public LogWindow Lw { get; set; }

        void ffmpegProsseing()
        {
            ////////
            /////https://qiita.com/skitoy4321/items/10c47eea93e5c6145d48
            ///////
            ///
            ////
            ////Enable Asnc Task Canceller


            using (paramField.ctoken = new CancellationTokenSource())
            using (_FfmpProcess = new Process())
            {


                _FfmpProcess.StartInfo.CreateNoWindow = true;
                _FfmpProcess.StartInfo.UseShellExecute = false;
                _FfmpProcess.StartInfo.RedirectStandardInput = true;

                _FfmpProcess.StartInfo.RedirectStandardError = true;
                _FfmpProcess.StartInfo.FileName = "cmd.exe";


                    _FfmpProcess.StartInfo.Arguments = $"/c dll\\ffmpeg.exe {_arguments}";



                _FfmpProcess.EnableRaisingEvents = true;


                

                _FfmpProcess.Exited += new EventHandler(ffmpeg_Exited);



                _FfmpProcess.ErrorDataReceived += new DataReceivedEventHandler(delegate (object obj, DataReceivedEventArgs e)
                {
                    Dispatcher.Invoke(() =>
                   {
                       Lw.RichTextRogs.AppendText(e.Data);
                       Lw.RichTextRogs.AppendText(Environment.NewLine);

                       Debug.WriteLine(e.Data);
                       Debug.WriteLine(Environment.NewLine);


                       if (paramField.isAutoScroll)
                           Lw.RichTextRogs.ScrollToEnd();

                       //Another thread accessing
                       //-                    
                       ////https://psycodedeveloper.wordpress.com/2019/07/31/how-to-pause-or-resume-a-process-with-c/



                   });
                });
                _FfmpProcess.Start();


                Thread.Sleep(1000);
                _FfmpProcess.BeginErrorReadLine();



                paramField.ctoken.Token.WaitHandle.WaitOne();

                _FfmpProcess.WaitForExit(0);


            }

        }


        private async void ffmpeg_Exited(object sender, EventArgs e)
        {
            var current = Directory.GetCurrentDirectory();


            //wave出力の初期化
            //Usingステートメントを入れるとならなくなる　Why？
            using (WaveOutEvent outputDevice = new WaveOutEvent())
            using (AudioFileReader afr = new AudioFileReader(current + @"\\dll\\しょどーる参上.wav"))
            {
                outputDevice.Init(afr);
                var playbackCompleted = new TaskCompletionSource<bool>();
                outputDevice.PlaybackStopped += (sender, args) => playbackCompleted.TrySetResult(true);

                outputDevice.Play();


                OpenExplorer();
                // 再生が完了するまで待機
                await playbackCompleted.Task;

            }

            paramField.isExitProcessed = true;

        }




        void OpenExplorer()
        {
            if (paramField.isOpenFolder)
                using (Process explorerProcess = new Process())
                {
                    explorerProcess.StartInfo.FileName = "explorer.exe";


                    // /select オプションを使用して、ファイルを選択して表示
                    explorerProcess.StartInfo.Arguments = $"/select, \"{paramField.check_output}\"";



                    // Explorerプロセスを開始
                    explorerProcess.Start();
                }

        }

        private void process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            Debug.WriteLine(e);
        }


        
    }
}
