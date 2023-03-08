using HaruaConvert.Parameter;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace HaruaConvert
{
    /// <summary>
    /// RogWindow.xaml の相互作用ロジック
    /// </summary>
    /// 





    public partial class LogWindow : Window
    {

        Color PreTextColor;

        Brush TextColor;
        // Pragraph要素のインスタンスを作成します。

        MainWindow main;
        // FlowDocument要素のインスタンスを作成します。

        public LogWindow(MainWindow _main)
        {
            InitializeComponent();
            var textRange = RichTextRogs.Selection;
            // 文字色
            textRange.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(Colors.Blue));

            main = _main;


            this.MouseLeftButtonDown += (sender, e) => { this.DragMove(); };



            PreTextColor = new Color();
            PreTextColor = Color.FromArgb(0, 43, 201, 47);

            TextColor = new SolidColorBrush(PreTextColor);

            ParamInterfase.isPaused= false;

        }



        delegate void KillProcess_deligate(Process target);

        private void ConvertStop_Click(object sender, RoutedEventArgs e)
        {
            //var TC = new Terminate_ProcessClass();

            //TC.mainTerminate_deligateExec(ParamInterfase.ffmpeg_pid);


            StreamWriter inputWriter = main._process.StandardInput;
            inputWriter.WriteLine("q");


            main.Focus();
            ParamInterfase.isExitProcessed = true;

            //IntPtr hwnd = Process.GetProcessById(main.pid).Handle;

            //SendMessage(hwnd, WM_SETTEXT, IntPtr.Zero, "q");

            //Environment.Exit(ProcessIdList[0]);

        }





        private void window_Closed(object sender, EventArgs e)
        {
        }

        private void window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Visibility = Visibility.Collapsed;
        }


        private void ResumeButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.window.WindowState = WindowState.Minimized;
        }

        public event PropertyChangedEventHandler RogWindow_PropertyChanged;

        private void RaisePropertyChanged([CallerMemberName] string propertyName = null)
          => RogWindow_PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));


        //private void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        //    => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //string _filePathOutput;




        private void RichTextRogs_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {


        }


        public Brush NormalText
        {
            get => TextColor;

            set
            {
                TextColor = value;
                RaisePropertyChanged();
            }

        }
        private void RichTextRogs_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {


        }

        private void AutoScroll_Checker_Checked(object sender, RoutedEventArgs e)
        {
            if(!ParamInterfase.isAutoScroll)
                   ParamInterfase.isAutoScroll = true;
            else 
                ParamInterfase.isAutoScroll = false;
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void PauseButton_Checked(object sender, RoutedEventArgs e)
        {
            ParamInterfase.isPaused = !ParamInterfase.isPaused ? true :false ;

            
        }
    }


}