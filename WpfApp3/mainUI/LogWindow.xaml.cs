using HaruaConvert.Initilize_Method;
using HaruaConvert.Parameter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Media;
using static HaruaConvert.Methods.IniCheckerClass;


namespace HaruaConvert
{
    /// <summary>
    /// RogWindow.xaml の相互作用ロジック
    /// </summary>
    /// 





    public partial class LogWindow : Window
    {


        List<MenuItem> MenuCheckBoxList { get; set; }

        Brush TextColor;
        // Pragraph要素のインスタンスを作成します。


       public ParamField Lw_paramField { get; set; }
        public LogWindow(ParamField _paramField)
        {
            InitializeComponent();

            Lw_paramField = new ParamField();
            Lw_paramField = _paramField;


            //AutoScroll_Checker.IsChecked = true;

            var textRange = RichTextRogs.Selection;
            // 文字色
            textRange.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(Colors.Blue));

            string test = String.Empty;




            this.MouseLeftButtonDown += (sender, e) => { this.DragMove(); };



            //PreTextColor = new Color();
            //PreTextColor = Color.FromArgb(0, 43, 201, 47);

            //TextColor = new SolidColorBrush(PreTextColor);


            
            Lw_paramField.isPaused = false;
           
        }



        delegate void KillProcess_deligate(Process target);

        private void ConvertStop_Click(object sender, RoutedEventArgs e)
        {
            //var TC = new Terminate_ProcessClass();

            //TC.mainTerminate_deligateExec(ParamField.ffmpeg_pid);


            //if (mainWindow._FfmpProcess.d == true)
            //  return;
            //if (paramField.isExecuteProcessed) //Check Stream is Null 
            //    return;

            try
            {
                if (MainWindow.ffmpegProcess != null)
                {
                    return;
                }

                      StreamWriter inputWriter = MainWindow.ffmpegProcess.StandardInput;


                    inputWriter.WriteLine("q");
                

                Focus();

            }

            catch (System.IO.IOException ex)
            {
                MessageBox.Show(ex.Message + "￥r\n未実行のときにStopButtonが押されました");
            }



        }





        public void window_Closed(object sender, EventArgs e)
        {
            var checkProcess = new CheckboxGetSetValueClass();
            checkProcess.CheckediniSetVallue(AutoScroll_Checker, Lw_paramField.iniPath);
            checkProcess.CheckediniSetVallue(BackImage_Checker, Lw_paramField.iniPath);

            this.Close();
        }

        private void window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Visibility = Visibility.Collapsed;
            var checkProcess = new CheckboxGetSetValueClass();

            checkProcess.CheckediniSetVallue(AutoScroll_Checker, Lw_paramField.iniPath);
            checkProcess.CheckediniSetVallue(BackImage_Checker, Lw_paramField.iniPath);


        }


        private void ResumeButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MinimizedButton_Click(object sender, RoutedEventArgs e)
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

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void PauseButton_Checked(object sender, RoutedEventArgs e)
        {
            Lw_paramField.isPaused = !Lw_paramField.isPaused ? true : false;


        }


        private void AutoScroll_Checker_Checked(object sender, RoutedEventArgs e)
        {
            if(Lw_paramField != null)
            Lw_paramField.isAutoScroll = AutoScroll_Checker.IsChecked ? true : false;
            
        }

        private void BackImage_Checker_Checked(object sender, RoutedEventArgs e)
        {





            Lw_paramField.isBackImage = BackImage_Checker.IsChecked ? true : false ;

            ImageBrush image = new ImageBrush();
            try
            {
                string imagePath = "BackImage\\harua.jpg";
                image.ImageSource = new System.Windows.Media.Imaging.BitmapImage(new Uri(imagePath, UriKind.Relative));


                // まずファイルが存在するかを確認
                if (!File.Exists(imagePath))
                {
                    MessageBox.Show($"BackImegeフォルダにharua.jpgが見つかりません: {imagePath}");
                    return;
                }


                if (Lw_paramField.isBackImage)
                {

                    image.Opacity = 0.4;

                    RichTextRogs.Opacity = 1;
                    RichTextRogs.Background = SystemColors.WindowBrush;
                    RichTextRogs.Foreground = Brushes.Black;
                    // ブラシを背景に設定する
                    RichTextRogs.Background = image;

                }
                else
                {
                    image.Opacity = 0;
                    RichTextRogs.Opacity = 0.6;
                    RichTextRogs.Foreground = Brushes.White;
                    RichTextRogs.Background = Brushes.Black;
                }
            }
            catch (DirectoryNotFoundException ex)
            {
                MessageBox.Show("BackImegeフォルダ内にharua.jpgがありません\r\n" + ex.Message);

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);

            }
        }

        private void window_Loaded(object sender, RoutedEventArgs e)
        {
            var initial = new InitilizeCheckBox(Lw_paramField);
            MenuCheckBoxList = initial.InitializeChildCheckBox(this, MenuCheckBoxList);
            

            var checkedProcess = new CheckboxGetSetValueClass();
            // RichTextBoxのContextMenuを取得
     
                  


                        // メニュー項目を取得
                        foreach (var item in MenuCheckBoxList)
                        {

                            // コピーのMenuItemに対する操作
                            item.IsChecked = checkedProcess.CheckBoxiniGetVallue(item, Lw_paramField.iniPath);
                            if(item.Name == AutoScroll_Checker.Name)
                                Lw_paramField.isAutoScroll = true;
                        }

                    }
                
                //paramField.isBackImage = checkedProcess.CheckBoxiniGetVallue(BackImage_Checker, paramField.iniPath);
                // BackImage_Checker.IsChecked = paramField.isBackImage;

                // paramField.IsAutoScroll = checkedProcess.CheckBoxiniGetVallue(AutoScroll_Checker, paramField.iniPath);
                // AutoScroll_Checker.IsChecked = paramField.IsAutoScroll;


            
        
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {

           this.Close();
        }

        private void window_Loded(object sender, RoutedEventArgs e)
        {

        }
    }


}