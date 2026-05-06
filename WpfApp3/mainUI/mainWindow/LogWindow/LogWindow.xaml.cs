using HaruaConvert.Initilize_Method;
using HaruaConvert.Methods;
using HaruaConvert.Parameter;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;


namespace HaruaConvert
{
    /// <summary>
    /// RogWindow.xaml の相互作用ロジック
    /// </summary>
    /// 





    public partial class LogWindow : Window, IDisposable
    {
        private const int MaxVisibleLogLines = 10000;
        private readonly ConcurrentQueue<string> _pendingLogs = new();
        private readonly ObservableCollection<LogEntry> _logEntries = new();
        private readonly DispatcherTimer _flushTimer;
        private readonly MainWindow _owner;


        List<MenuItem> MenuCheckBoxList { get; set; }

        Brush TextColor;
        // Pragraph要素のインスタンスを作成します。


        public ParamField Lw_paramField { get; set; }
        public LogWindow(MainWindow owner, ParamField _paramField)
        {
            InitializeComponent();

            _owner = owner;
            Lw_paramField = _paramField;
            this.MouseLeftButtonDown += (sender, e) => { this.DragMove(); };

            Lw_paramField.isPaused = false;
            LogGrid.ItemsSource = _logEntries;
            _flushTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(100)
            };
            _flushTimer.Tick += FlushPendingLogs;
            _flushTimer.Start();

        }



        delegate void KillProcess_deligate(Process target);

        public void AppendLogLine(string line)
        {
            if (string.IsNullOrEmpty(line))
            {
                return;
            }

            _pendingLogs.Enqueue(line);
        }

        private void FlushPendingLogs(object? sender, EventArgs e)
        {
            if (_pendingLogs.IsEmpty)
            {
                return;
            }

            bool hasNewLine = false;
            while (_pendingLogs.TryDequeue(out string? line))
            {
                _logEntries.Add(new LogEntry(line));
                hasNewLine = true;
            }

            while (_logEntries.Count > MaxVisibleLogLines)
            {
                _logEntries.RemoveAt(0);
            }

            if (hasNewLine && AutoScroll_Checker.IsChecked == true && _logEntries.Count > 0)
            {
                LogGrid.ScrollIntoView(_logEntries[_logEntries.Count - 1]);
            }
        }

        private void ConvertStop_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process? proc = _owner.ffmpegProcess;
                if (proc == null)
                {
                    return;
                }

                if (proc.HasExited)
                {
                    _owner.ffmpegProcess = null;
                    return;
                }

                try
                {
                    StreamWriter stdin = proc.StandardInput;
                    if (stdin.BaseStream.CanWrite)
                    {
                        stdin.WriteLine("q");
                        stdin.Flush();
                    }
                }
                catch (IOException ex)
                {
                    MessageBox.Show(
                        ex.Message + "\r\n停止コマンドの送信に失敗しました（プロセスは既に終了している可能性があります）。");
                    if (proc.HasExited)
                    {
                        _owner.ffmpegProcess = null;
                    }
                }

                Focus();
            }
            finally
            {
                Lw_paramField.isExecuteProcessed = false;
            }
        }





        public void window_Closed(object sender, EventArgs e)
        {
            var checkProcess = new IniCheckBoxSetClass();
            checkProcess.CheckediniSetVallue(AutoScroll_Checker, Lw_paramField.iniPath);
            checkProcess.CheckediniSetVallue(BackImage_Checker, Lw_paramField.iniPath);

            this.Close();
        }

        private void window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Visibility = Visibility.Collapsed;
            var checkProcess = new IniCheckBoxSetClass();

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
            if (Lw_paramField != null)
                Lw_paramField.isAutoScroll = AutoScroll_Checker.IsChecked ? true : false;

        }

        private void BackImage_Checker_Checked(object sender, RoutedEventArgs e)
        {





            Lw_paramField.isBackImage = BackImage_Checker.IsChecked ? true : false;

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

                    LogGrid.Opacity = 1;
                    LogGrid.Background = SystemColors.WindowBrush;
                    LogGrid.Foreground = Brushes.Black;
                    LogGrid.RowStyle = (Style)FindResource("LogRowTransparentStyle");
                    LogGrid.CellStyle = (Style)FindResource("LogCellTransparentStyle");
                    // ブラシを背景に設定する
                    LogGrid.Background = image;

                }
                else
                {
                    image.Opacity = 0;
                    LogGrid.Opacity = 0.6;
                    LogGrid.Foreground = Brushes.White;
                    LogGrid.RowStyle = (Style)FindResource("LogRowDarkStyle");
                    LogGrid.CellStyle = (Style)FindResource("LogCellDarkStyle");
                    LogGrid.Background = Brushes.Black;
                }
            }
            catch (DirectoryNotFoundException ex)
            {
                MessageBox.Show("BackImegeフォルダ内にharua.jpgがありません\r\n" + ex.Message);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }

        private void window_Loaded(object sender, RoutedEventArgs e)
        {



            var initial = new InitilizeCheckBox(Lw_paramField);
            MenuCheckBoxList = initial.InitializeChildCheckBox(this, MenuCheckBoxList);


            var checkedProcess = new IniCheckBoxSetClass();
            // RichTextBoxのContextMenuを取得




            // メニュー項目を取得
            foreach (var item in MenuCheckBoxList)
            {

                // コピーのMenuItemに対する操作
                item.IsChecked = checkedProcess.CheckBoxiniGetVallue(item, Lw_paramField.iniPath);
                if (item.Name == AutoScroll_Checker.Name)
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
            _owner.firstlogWindow = false;
            //MainWindow.Lw = null;

        }

        private void window_Loded(object sender, RoutedEventArgs e)
        {


        }
        private bool disposed; // 破棄済みかどうかのフラグ

        void IDisposable.Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this); // ガベージコレクターに対してファイナライザーを呼ばないように指示
        }

        // クリーンアップの実際の処理を行うメソッド
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _flushTimer.Stop();
                    _flushTimer.Tick -= FlushPendingLogs;
                    // マネージドリソースの解放
                    // 例えば、イベントハンドラーの解除や、IDisposableなオブジェクトのDispose呼び出しなど
                }

                // アンマネージドリソースの解放
                // 例えば、ファイルハンドルやデータベース接続などの解放

                disposed = true; // 破棄済みフラグを設定
            }
        }

        public sealed class LogEntry
        {
            public LogEntry(string text)
            {
                Text = text;
            }

            public string Text { get; }
        }
    }


}