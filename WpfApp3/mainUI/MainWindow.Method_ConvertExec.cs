using HaruaConvert.Methods;
using HaruaConvert.Parameter;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
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
        public bool FileConvertExec(string _fullPath,object sender)
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


                string pattern = @"\{FileName\}\.(\w+)";
                var target = main.harua_View.MainParams[0].StartQuery;
                Match match = Regex.Match(target, pattern);

                string extention = param.GetExtentionFileNamepattern(target);

                

                escapes = param.AddParamEscape(escapes,extention);
                if (!string.IsNullOrEmpty(extention))
                    paramField.check_output = escapes.NonEscape_outputPath;

                //_fullPath, harua_View
                //StartQueryを追加する
                _arguments = Ffmpc.AddsetQuery(escapes.inputPath, harua_View);

                _arguments = _arguments.Replace("{FileName}" + extention, "");

                
                //オプションと出力先ファイル文字列の追加
                _arguments = AddOptionClass.AddOption(_arguments) + " " + $"{ escapes.outputPath}";

            }


            else if (isUserParameter.IsChecked.Value) //used Original paramerter
            {
                var isOrigenelParam = new isUserOriginalParameter(this);
                isOrigenelParam.isUserOriginalParameter_Method(sender);

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
                
                    checker = FileExsosts_and_NoDialogCheck(paramField.check_output, NoDialogCheck.IsChecked.Value) ? DialogMethod() : ifNoFiles.IfNoFileExsists();

                    paramField.isExecuteProcessed = checker;
                //   if(!checker) //pushed No
                //    return false;

                if (Lw != null)
                {
                    if (!ParamField.firstlogWindow)
                    {
                        Lw.Show();
                        ParamField.firstlogWindow = true;

                    }
                    else
                    {
                        Lw.WindowState = WindowState.Normal;
                        Lw.Topmost = true;
                        Lw.Activate();
                        Lw.Topmost = false;
                    }
                }

             

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



        bool DialogMethod()
        {

            MessageBoxResult msbr = MessageBox.Show("ファイルが存在しますわ。上書きしますか？",
                "メッセージボックス", MessageBoxButton.YesNo,
                MessageBoxImage.Asterisk);


            if (msbr == MessageBoxResult.Yes)
            {


               // Lw = new LogWindow(paramField);

                // ParamField.isExitProcessed = false;

                return true;

            }
            else         
                return false;
            

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

        long memorySize { get; set; }
        List<Process> AllExplorerProcesses { get; set; }
        public LogWindow Lw { get; set; }
        public bool IsDefaultQuerySet { get; internal set; }

       public void ffmpegProsseing()
        {
            ////////
            /////https://qiita.com/skitoy4321/items/10c47eea93e5c6145d48
            ///////
            ///
            ////
            ////Enable Asnc Task Canceller




            using (paramField.ctoken = new CancellationTokenSource())
            using (ffmpegProcess = new Process())
            {

                
                ffmpegProcess.StartInfo.CreateNoWindow = true;
                ffmpegProcess.StartInfo.UseShellExecute = false;
                ///UseShellExecute: プロセスを直接起動し、標準入力、標準出力、標準エラーのリダイレクトが可能になります。
                ///
                ffmpegProcess.StartInfo.RedirectStandardInput = true;

                ffmpegProcess.StartInfo.RedirectStandardError = true;
                ffmpegProcess.StartInfo.FileName = "cmd.exe";


                    ffmpegProcess.StartInfo.Arguments = $"/c dll\\ffmpeg.exe {_arguments}";

                    ffmpegProcess.EnableRaisingEvents = true;


                

                ffmpegProcess.Exited += new EventHandler(ffmpeg_Exited);



                ffmpegProcess.ErrorDataReceived += new DataReceivedEventHandler(delegate (object obj, DataReceivedEventArgs e)
                {
                    if (Lw != null)
                        Dispatcher.Invoke(() =>
                   {
                       

                       Lw.RichTextRogs.AppendText(e.Data);
                       Lw.RichTextRogs.AppendText(Environment.NewLine);

                   //    Debug.WriteLine(e.Data);
                    //  Debug.WriteLine(Environment.NewLine);


                       if (Lw.AutoScroll_Checker.IsChecked)
                           Lw.RichTextRogs.ScrollToEnd();

                       //Another thread accessing
                       //-                    
                       ////https://psycodedeveloper.wordpress.com/2019/07/31/how-to-pause-or-resume-a-process-with-c/



                   });
                });
                ffmpegProcess.Start();

                
                paramField.ffmpeg_pid = ffmpegProcess.Id; ;

                Thread.Sleep(1000);
                ffmpegProcess.BeginErrorReadLine();


                paramField.ctoken.Token.WaitHandle.WaitOne();

                ffmpegProcess.WaitForExit(0);
             
            }

        }


        private async void ffmpeg_Exited(object sender, EventArgs e)
        {
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


               await this.Dispatcher.BeginInvoke((Action)delegate
                {
                    Lw.RichTextRogs.AppendText("ffmpegが終了しました!");
                }, DispatcherPriority.Render);


                

                OpenExplorer();
                //Usingステートメントを入れると即座に破棄されるため、鳴らなくなる　
                // 再生が完了するまで待機
                await playbackCompleted.Task;

            }
            
            

        }




        void OpenExplorer()
        {

            //using (Process explorerProcess = new Process())
            //{
            //    explorerProcess.StartInfo.FileName = "explorer.exe";


            //    // /select オプションを使用して、ファイルを選択して表示
            //    explorerProcess.StartInfo.Arguments = $"/select, \"{paramField.check_output}\"";



            //    // Explorerプロセスを開始
            //    explorerProcess.Start();
            //}


            if(paramField.isOpenFolder)
            using (Process explorerProcess = new Process())
            {
                explorerProcess.StartInfo = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = "explorer", // フルパスで指定せず「explorer」とだけ書く
                    Arguments = $"/select, \"{paramField.check_output}\"", // 引数に「/select,」を付ける
                    UseShellExecute = true
                };

                explorerProcess.Start();
                memorySize = explorerProcess.WorkingSet64;
                
               
            }

            
            

            Debug.WriteLine($"/select, \"{paramField.check_output}\"");
            
        }

        private void process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            //Debug.WriteLine(e);
        }


        
    }
}
