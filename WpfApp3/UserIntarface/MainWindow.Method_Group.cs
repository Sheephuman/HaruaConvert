using HaruaConvert.Parameter;
using NAudio.Wave;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using HaruaConvert.InterFace;
using HaruaConvert.Methods;
using System.Collections.Generic;
using System.Windows.Documents;

namespace HaruaConvert
{
    //　MainWindowに属するファイル(UI側)

    public partial class MainWindow : Window, IHaruaInterFace.IMainWindwEvents,
        IHaruaInterFace.IMouseEvents
    {

        //  private string OutputPath { get; set; }

        string _arguments;
        public Process _process;
        //public static int processID;
#pragma warning disable CA1051 // 参照可能なインスタンス フィールドを宣言しません
        public ParamCreateClasss param;
#pragma warning restore CA1051 // 参照可能なインスタンス フィールドを宣言しません



        public Thread th1;

        delegate string addOptionDeligate(string _argument);

        /// <summary>
        　///UI側の実行を請け負うメソッド UIの延長として扱う
        ///　UISide の実装は基本的にこちら。
        /// </summary>
        /// <param name="_fullPath"></param>
        /// <returns></returns>
        bool FileConvertExec(string _fullPath)
        {


            th1 = new Thread(new ThreadStart(ffmpegProsseing));
            //For Kill ffmpeg Process



            param = new ParamCreateClasss(_fullPath);

            //var param = new f(fileName);
            baseArguments = "";

            if (_arguments == null)
                _arguments = "";




            var fileStringChk = new Alternate_FileExsists();


            //Whether to use Original Paramerter Query


            var chButton = VisualTreeHelperWrapperHelpers.FindDescendant<Button>(Drop_Label);


            var Alternate_FileExsists = new Alternate_FileExsists();

            if (ParamInterfase.ButtonName == chButton.Name)
            {
                //先頭パラメータを付ける
                _arguments = param.AddParamEscape(_fullPath);



                //var paramQuery = ffmpc.StartQuery;
                //初期化とパラメータ挿入
                //armake = ffmpc.AddsetQuery;
                // //基本パラメータを追加

                //armake += AddOption;
                //Add OutputPath,then attach arguments

                _arguments = Ffmpc.AddsetQuery(_arguments, DCmenber.StartQuery);
                _arguments = AddOptionClass.AddOption(_arguments) + " " + $"{param.output}";


                //  _arguments = makedParam + " " + $"{ParamCreateClasss<string>.output}";

            }


            else if (isUserOriginalParameter.IsChecked.Value) //used Original paramerter
            {
                //"FileDropButton2"
                if ("ExecButton" == ParamInterfase.ButtonName)
                {
                    if (string.IsNullOrEmpty(InputSelector.FilePathBox.Text))
                    {
                        MessageBox.Show("入力ファイルに基本パラメータ" + @"\r\n" + "が指定されていませんわ");
                        return true;
                    }


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

                            baseArguments = inputMatches.Replace(baseArguments, @"""" + InputSelector.FilePathBox.Text + @""""); ;



                            //MessageBox.Show(baseArguments);
                            var OutputMatches = new Regex("\\{" + "output" + "\\}");

                            baseArguments = OutputMatches.Replace(baseArguments, @"""" + OutputSelector.FilePathBox.Text);
                            //Attach Output Path as Converted FileName
                            //  MessageBox.Show(baseArguments);



                            baseArguments = baseArguments.Replace("{{" + "input" + "}}", @"""" + InputSelector.FilePathBox.Text + @"""", StringComparison.CurrentCulture);
                            //"\"{{{input}}}}\""



                            //      MessageBox.Show(baseArguments);

                            param.check_output = OutputSelector.FilePathBox.Text;

                            th1.DisableComObjectEagerCleanup();

                            if (baseArguments.Contains("%03d", StringComparison.Ordinal))
                            { baseArguments += @""""; }

                            else if (baseArguments.Contains("%04d", StringComparison.Ordinal))
                            {
                                baseArguments += @"""";
                            }



                            _arguments = baseArguments;

                            //ParamInterfase.isExitProcessed = false;
                        }
                    }
                }


            }

            #region ファイル存在判定


          return FileExsosts_and_NoDialogCheck(param.check_output,NoDialogCheck.IsChecked.Value) ?  DialogMethod() : IfNoFileExsists() ;

            
          #endregion


        }

        bool IfNoFileExsists()
        {
            //ffmpegが終了している状態のとき
            if (ParamInterfase.isExitProcessed)
            {
                //   ParamInterfase.isExitProcessed = false;
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


                // ParamInterfase.isExitProcessed = false;

                return false;

            }
            else
            {



                return true;
            }

        }

        static bool FileExsosts_and_NoDialogCheck(string check_output, bool _DialogChecled)        //public async Task<string> CollectStandardOutput()
        {
            var alterExsists = new Alternate_FileExsists();
            
            bool exsisted = alterExsists.FileExsists(check_output);
            bool DialogChecled = _DialogChecled;
            bool satisfied = false;

            if (exsisted && !DialogChecled)
                satisfied = true;

            return satisfied;
        }
        //{
        //    return await _process.StandardOutput.ReadToEndAsync();
        //}

        LogWindow Lw;

        void ffmpegProsseing()
        {
            ////////
            /////https://qiita.com/skitoy4321/items/10c47eea93e5c6145d48
            ///////
            using (ParamInterfase.ctoken = new CancellationTokenSource())
            using (_process = new Process())
            {

                _process.StartInfo.CreateNoWindow = true;
                _process.StartInfo.UseShellExecute = false;
                   _process.StartInfo.RedirectStandardInput= true;

                _process.StartInfo.RedirectStandardError = true;
                _process.StartInfo.FileName = "cmd.exe";


                //var ffmpc = new FfmpegQueryClass();
                _process.StartInfo.Arguments = "/c ffmpeg.exe " + _arguments;

                _process.EnableRaisingEvents = true;



              _process.OutputDataReceived += DataReceivedHandler;

                _process.Exited += new EventHandler(ffmpeg_Exited);


                
                _process.ErrorDataReceived += new DataReceivedEventHandler(delegate (object obj, DataReceivedEventArgs e)
                {
                     Dispatcher.Invoke( () =>
                    {
                        Lw.RichTextRogs.AppendText(e.Data);
                        Lw.RichTextRogs.AppendText(Environment.NewLine);
   
                        Debug.WriteLine(e.Data);
                        Debug.WriteLine(Environment.NewLine);

                        
                        if (ParamInterfase.isAutoScroll)
                            Lw.RichTextRogs.ScrollToEnd();
                       
                        //Another thread accessing
                        //-
                        // while (ParamInterfase.isPaused)
                        //     await Task.Delay(10000) ;
                        ////https://psycodedeveloper.wordpress.com/2019/07/31/how-to-pause-or-resume-a-process-with-c/

                        // e.Data = "frame= 4983 fps=309 q=31.0 size=   30720kB time=00:02:46.61 bitrate=1510.4kbits/s dup=979 drop=0 speed=10.3x    "


                    });
                });
                _process.Start();


                Thread.Sleep(1000);
                _process.BeginErrorReadLine();



                //while((line = _process.StandardError.ReadLine()) != null)
                //{

                //}

                Process[] plist = Process.GetProcessesByName("ffmpeg");

                var ProcessIdList = new List<int>();

                foreach (Process cop in plist)
                {
                    ProcessIdList.Add(cop.Id);
                }

                if (ProcessIdList.Count != 0)
                    ParamInterfase.ffmpeg_pid = ProcessIdList[0];


                ParamInterfase.ctoken.Token.WaitHandle.WaitOne();

                _process.WaitForExit(0);

                //   this.IsEnabled = false;
            }

        }
        
       
        private void ffmpeg_Exited(object sender, EventArgs e)
        {
            //wave出力の初期化
            //Usingステートメントはならなくなる　Why？
            WaveOutEvent outputDevice = new WaveOutEvent();
            

           AudioFileReader afr = new AudioFileReader(@"しょどーる参上.wav");
                outputDevice.Init(afr);
                outputDevice.Play();                
            

            ParamInterfase.isExitProcessed = true;
        }






        //private void ErrorDataReceivedEvent(object sender, DataReceivedEventArgs e)
        //{
        //    Dispatcher.Invoke(() =>
        //            {
        //                rw.RichTextRogs.AppendText(e.Data);
        //                rw.RichTextRogs.AppendText(Environment.NewLine);
        //                rw.RichTextRogs.ScrollToEnd();
        //                Debug.WriteLine(e.Data);
        //                Debug.WriteLine(Environment.NewLine);
        //            });
        //}

        private static StringBuilder writer = null;

        int numOutputLines = 0;


        //public TextRange FindTextInRange(TextRange searchRange, String searchText, Color color)
        //{

        //    int offset = searchRange.Text.IndexOf(searchText, StringComparison.OrdinalIgnoreCase);

        //    if (offset < 0)
        //        return null;


        //    var start = GetTextPositionAtOffset(searchRange.Start, offset);



        //    TextRange result = new TextRange(start, GetTextPositionAtOffset(start, searchText.Length));



        //    return result;
        //}


        //TextPointer GetTextPositionAtOffset(TextPointer position, int characterCount)
        //{
        //    while (position != null)
        //    {
        //        if (position.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.Text)
        //        {
        //            int count = position.GetTextRunLength(LogicalDirection.Forward);
        //            if (characterCount <= count)
        //            {
        //                return position.GetPositionAtOffset(characterCount);
        //            }
        //            characterCount -= count;
        //        }
        //    }

        //    return position;
        //}


        private void DataReceivedHandler(object sender, DataReceivedEventArgs e)
        {


            // Collect the sort command output.
            if (!string.IsNullOrEmpty(e.Data))
            {

                // Add the text to the collected output.
                writer.Append(Environment.NewLine +
                    $"[{numOutputLines++}] - {e.Data}");
            }

            //else
            //{
            //   ParamInterfase.isExitProcessed = true;
            //}
        }

        //private void BeginInvoke(Action<string> action, object[] objects)
        //{
        //    throw new NotImplementedException();
        //}

        private void process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            Debug.WriteLine(e);
        }


        //static async Task RecieveOutputAsync(Process p)
        //{

        //    var result = await p.StandardOutput.ReadToEndAsync();
        //    Debug.WriteLine("Contains: " + result);
        //    MessageBox.Show(result);

        //}

    }
}
