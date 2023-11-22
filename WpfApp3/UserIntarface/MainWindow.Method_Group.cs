using HaruaConvert.Methods;
using HaruaConvert.Parameter;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using WpfApp3.Parameter;

using static HaruaConvert.Parameter.ParamField;

namespace HaruaConvert
{
    //　MainWindowに属するファイル(UI側)

    public partial class MainWindow : Window
    {

     

        string _arguments;

        /// <summary>
        /// 共有箇所：LogWindowのConvertStopButton
        /// </summary>
        public Process _FfmpProcess { get; set; } = null!;



#pragma warning disable CA1051 // 参照可能なインスタンス フィールドを宣言しません
        public ParamCreateClasss param;
#pragma warning restore CA1051 // 参照可能なインスタンス フィールドを宣言しません


        /// <summary>
        /// 共有箇所：LogWindowのClose時
        /// </summary>
        private Thread th1 = null!;

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


            param = new ParamCreateClasss(_fullPath);

            baseArguments = "";

            if (_arguments == null)
                _arguments = "";







            //Whether to use Original Paramerter Query
            var chButton = VisualTreeHelperWrapperHelpers.FindDescendant<Button>(Drop_Label);




            if (ClassShearingMenbers.ButtonName == chButton.Name)
            {
                //先頭パラメータを付ける
                _arguments = param.AddParamEscape(_fullPath);
                

                _arguments = Ffmpc.AddsetQuery(_arguments, harua_View._Main_Param[0].StartQuery);
                _arguments = AddOptionClass.AddOption(_arguments) + " " + $"{OutputPath}";

            }


            else if (isUserParameter.IsChecked.Value) //used Original paramerter
            {
                isUserOriginalParameter_Method(sender);
            }

            #region ファイル存在判定

            //IDisposable alterr = new IDisposableBase();
            //alterr.Dispose();


            using (var Alternate_FileExsists = new Alternate_FileExsists())
            {
                return FileExsosts_and_NoDialogCheck(param.check_output, NoDialogCheck.IsChecked.Value) ? DialogMethod() : IfNoFileExsists();

            }

            #endregion


        }


        bool isUserOriginalParameter_Method(object sender)
        {
            //"FileDropButton2"
            if ((ButtonNameField._ExecButton == ((Button)sender).Name))
            {
                               

                #region foreach Scopes
                foreach (ParamSelector sp in selectorList)
                {             
                    if (sp.SlectorRadio.IsChecked.Value)
                    {
                        baseArguments = sp.ArgumentEditor.Text;
                    }
                }

                foreach (var sp in selectorList)
                {
                    if (sp.SlectorRadio.IsChecked.Value && !string.IsNullOrEmpty(sp.ArgumentEditor.Text))
                    {

                        var inputMatches = new Regex("\\{" + "input" + "\\}");
                        baseArguments = inputMatches.Replace(baseArguments, @"""" + InputSelector.FilePathBox.Text + @"""");


                        var OutputMatches = new Regex("\\{" + "output" + "\\}");

                        baseArguments = OutputMatches.Replace(baseArguments, @"""" + OutputSelector.FilePathBox.Text);
                        //Attach Output Path as Converted FileName



                        baseArguments = baseArguments.Replace("{{" + "input" + "}}", @"""" + InputSelector.FilePathBox.Text + @"""");
                        //"\"{{{input}}}}\""



                        param.check_output = OutputSelector.FilePathBox.Text;

                        th1.DisableComObjectEagerCleanup();

                        if (baseArguments.Contains("%03d", StringComparison.Ordinal))
                        { baseArguments += @""""; }

                        else if (baseArguments.Contains("%04d", StringComparison.Ordinal))
                        {
                            baseArguments += @"""";
                        }

                        _arguments = baseArguments;
                    }
                }
                #endregion
            }
            return true;
        }


        bool IfNoFileExsists()
        {
            //ffmpegが終了している状態のとき
            if (paramField.isExitProcessed)
            {

                th1.Start();

                Lw.Show();
                Lw.Activate();
            }
            else
            {
                MessageBox.Show("ffmpeg.exeが実行中なのです");
            }
            return false;

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

            var alterExsists = new Alternate_FileExsists();

            bool exsisted = alterExsists.FileExsists(check_output);

        
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


                Process[] plist = Process.GetProcessesByName("ffmpeg");

                var ProcessIdList = new List<int>();
                ProcessIdList.Capacity = plist.Length;

                foreach (Process cop in plist)
                {
                    ProcessIdList.Add(cop.Id);
                }

                if (ProcessIdList.Count != 0)
                    paramField.ffmpeg_pid = ProcessIdList[0];


                paramField.ctoken.Token.WaitHandle.WaitOne();

                _FfmpProcess.WaitForExit(0);


            }

        }


        private void ffmpeg_Exited(object sender, EventArgs e)
        {
            //wave出力の初期化
            //Usingステートメントを入れるとならなくなる　Why？
            WaveOutEvent outputDevice = new WaveOutEvent();
            var current = Directory.GetCurrentDirectory();

            AudioFileReader afr = new AudioFileReader(current + @"\\dll\\しょどーる参上.wav");
            outputDevice.Init(afr);
            outputDevice.Play();


            paramField.isExitProcessed = true;
        }















    

        

        private void process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            Debug.WriteLine(e);
        }

    }
}
