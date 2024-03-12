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

     

        Brush TextColor;
        // Pragraph要素のインスタンスを作成します。

        MainWindow main;
        // FlowDocument要素のインスタンスを作成します。

        public LogWindow(MainWindow _main)
        {
            InitializeComponent();
    
            AutoScroll_Checker.IsChecked = true;
            
            var textRange = RichTextRogs.Selection;
            // 文字色
            textRange.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(Colors.Blue));




            main = _main;


            this.MouseLeftButtonDown += (sender, e) => { this.DragMove(); };



            //PreTextColor = new Color();
            //PreTextColor = Color.FromArgb(0, 43, 201, 47);

            //TextColor = new SolidColorBrush(PreTextColor);

            main.paramField.isPaused= false;

        }



        delegate void KillProcess_deligate(Process target);

        private void ConvertStop_Click(object sender, RoutedEventArgs e)
        {
            //var TC = new Terminate_ProcessClass();

            //TC.mainTerminate_deligateExec(ParamField.ffmpeg_pid);


            //if (main._FfmpProcess.d == true)
            //  return;

            StreamWriter inputWriter = main._FfmpProcess.StandardInput;
             inputWriter.WriteLine("q");


            main.Focus();
            main.paramField.isExitProcessed = true;

            

            
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

        /// <summary>
        /// プロパティの書き方
        /// </summary>
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
            if (main == null)
                return;

            main.paramField.isAutoScroll = main.paramField.isAutoScroll ? false: true;
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void PauseButton_Checked(object sender, RoutedEventArgs e)
        {
            main.paramField.isPaused = !main.paramField.isPaused ? true :false ;

            
        }

        private void BackImage_Checker_Checked(object sender, RoutedEventArgs e)
        {
        
            
            main.paramField.isBackImage = !main.paramField.isBackImage ? true : false; ;

            ImageBrush image = new ImageBrush();
            image.ImageSource = new System.Windows.Media.Imaging.BitmapImage(new Uri("BackImage\\harua.jpg", UriKind.Relative));
            if (main.paramField.isBackImage)
            {

                image.Opacity = 0.6;

                main.Lw.RichTextRogs.Opacity = 1;
                main.Lw.RichTextRogs.Background = SystemColors.WindowBrush;
                main.Lw.RichTextRogs.Foreground = Brushes.Black;
                // ブラシを背景に設定する
                main.Lw.RichTextRogs.Background = image;

            }
            else
            {
                image.Opacity = 0;
                main.Lw.RichTextRogs.Opacity = 0.6;
                main.Lw.RichTextRogs.Foreground = Brushes.White;
                main.Lw.RichTextRogs.Background = Brushes.Black;
            }
        }

       
    }


}