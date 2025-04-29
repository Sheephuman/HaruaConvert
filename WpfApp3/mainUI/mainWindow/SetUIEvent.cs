using HaruaConvert.HaruaInterFace;
using HaruaConvert.HaruaServise;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using static HaruaConvert.Parameter.ParamField;

namespace HaruaConvert.mainUI.mainWindow
{
    public class SetUIEvent
    {
        string[] fileNames { get; set; }
        bool isSelectorBox { get; set; }
        private MainWindow _main;

        public SetUIEvent(MainWindow mainWindow)
        {
            _main = mainWindow;


            //get Interface instance
            _main.mainTabEvents = new IMainTabEvents[]
             {

                new Directory_ClickProcedure(_main.paramField ,_main)
             };
            //var dclicks = new Directory_ClickProcedure(this);


        }




        // 初回実行を判定するための静的フラグ
        private static bool _isLoadedHandled;

        public void SetupEventHandlers()
        {
            if (!_isLoadedHandled)
            {
                // MainWindowのLoadedイベントハンドラを設定
                _main.Loaded += MainWindow_Loaded;
                _isLoadedHandled = true; // フラグを設定して再登録を防ぐ
            }
            // ファイルドロップイベントハンドラの設定
            _main.AllowDrop = true;
            _main.InputSelector.FilePathBox.PreviewDragOver += InputSelector_PreviewDragOver;

            Application.Current.Exit += _main.OnApplicationExit;

            //_main.OutputSelector.FilePathBox.PreviewDragOver += OutSelector_PreviewDragOver;
            _main.DragOver += MainWindow_DragOver;
            _main.Directory_DropButon.Drop += MainWindow_FileDrop;

            _main.InputSelector.FilePathBox.Drop += selector_FileDrop;
            // その他のUI操作に関わるイベントハンドラを設定
            // _main.btnSaveSettings.Click += BtnSaveSettings_Click;
            //   _main.btnLoadSettings.Click += BtnLoadSettings_Click;
        }

        private void selector_FileDrop(object sender, DragEventArgs e)
        {

            GetDropFile(e);

            var ui = new SetUIEvent(_main);
            string withoutEx = Path.GetFileNameWithoutExtension(_main.FileList[0]);
            string dropfileDirectry = Path.GetDirectoryName(_main.FileList[0]);
            _main.InputSelector.FilePathBox.Text = _main.FileList[0];

            _main.OutputSelector.FilePathBox.Text =
                ui.OutputFileRename(dropfileDirectry, withoutEx, _main.harua_View.MainParams[0].endString);


            _main.paramField.check_output = _main.OutputSelector.FilePathBox.Text;
            isSelectorBox = true;
        }

        void GetDropFile(DragEventArgs e)
        {
            fileNames = (string[])e.Data.GetData(DataFormats.FileDrop);


            _main.FileList.Clear();
            foreach (var name in fileNames)
            {
                _main.FileList.Add(name);
            }

        }



        public void MainWindow_FileDrop(object sender, DragEventArgs e)
        {


            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                _main.SorceFileDataBox.Document.Blocks.Clear();

                GetDropFile(e);





                _main.paramField.setFile = _main.FileList[0];
                _main.filePathOutput = _main.paramField.setFile;

                _main.Drop_Label.Content = "Convert";

                _main.SorceFileDataBox.Document.Blocks.Clear();


                _main.FileNameLabel.Text = string.Empty;


                var ClickedControl = sender as TextBox;
                if (_main.InputSelector.FilePathBox == ClickedControl)
                    _main.InputSelector.FilePathBox.Text = _main.paramField.setFile;

                _main.FileNameLabel.Text = _main.paramField.setFile;
                _main.harua_View.SourcePathText = _main.paramField.setFile;

                _main.ClearSourceFileData();

                IMediaInfoManager media = new MediaInfoService(_main);

                var proc = new Directory_ClickProcedure(_main.paramField, _main);
                var analysis = proc.CallFfprobe(_main.paramField.setFile);
                media.DisplayMediaInfo(analysis);

                if (!isSelectorBox)
                {


                    isSelectorBox = true;
                }
                isSelectorBox = false;
            }
        }



        private void InputSelector_PreviewDragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Copy; // マウスカーソルをコピーにする。
            e.Handled = e.Data.GetDataPresent(DataFormats.FileDrop);
            // ドラッグされてきたものがFileDrop形式の場合だけ、このイベントを処理済みにする。
        }


        public void RegisterUIDropEvent()
        {
            if (MainWindow.firstSet)
            {
                Button dropbutton = (Button)_main.Drop_Label.Template.FindName(ButtonNameField.Convert_DropButton, _main.Drop_Label);
                if (dropbutton != null)
                {
                    dropbutton.Click += mainUIButtons_ClickHandle;
                    dropbutton.Drop += MainWindow_FileDrop;
                }


                _main.Directory_DropButon.Click += mainUIButtons_ClickHandle;
            }
        }

        private void MainWindow_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.All;

            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
            e.Handled = true;
        }


        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // MainWindowがロードされたときの初期化処理


            if (MainWindow.firstSet)
            {
                //get Button name from label template
                RegisterUIDropEvent();
            }
        }


        public void mainUIButtons_ClickHandle(object sender, RoutedEventArgs e)
        {

            //   _main.FileNameLabel.Text = string.Empty;
            var ClickedControl = sender as Button;

            if (ClickedControl.Name == _main.Directory_DropButon.Name)
                _main.FileNameLabel.Text = _main.paramField.setFile;




            if (ClickedControl.Name == _main.InputSelector.FilePathBox.Name)
                _main.InputSelector.FilePathBox.Text = _main.paramField.setFile;



            if (ClickedControl.Name == _main.OutputSelector.Name)
                _main.OutputSelector.FilePathBox.Text = _main.paramField.setFile;


            if (ClickedControl.Name == _main.InputSelector.Name + ButtonNameField._openButton)
                _main.InputSelector.FilePathBox.Text = _main.paramField.setFile;


            foreach (var button in _main.mainTabEvents)
            {

                if (((Button)sender).Name == ButtonNameField.Directory_DropButon)
                {
                    button.Directory_DropButon_Click(sender, e);


                    return;
                }

                else if (((Button)sender).Name == ButtonNameField.Convert_DropButton)
                {
                    button.Convert_DropButton_Click(sender, e);


                    return;
                }


            }

        }



        public void InvisibleText_KeyDown(object sender, KeyEventArgs e)
        {
            var selecter = sender as ParamSelector;


            if (selecter == null)
            {
                return;
            }


            if (e.Key == Key.Escape)
            {
                foreach (ParamSelector sp in _main.selectorList)
                {
                    if (!string.IsNullOrEmpty(selecter.invisibleText.Text) && selecter.invisibleText == sp.invisibleText)
                    {

                        selecter.invisibleText.Visibility = Visibility.Hidden;
                        selecter.ParamLabel.Visibility = Visibility.Visible;
                        return;
                    }
                    else if (selecter.invisibleText.Text == "\r\n")
                    {

                        selecter.invisibleText.Text = selecter.ParamLabel.Text;


                        sender = null;
                        return;
                    }
                }

            }




            ///http://www.madeinclinic.jp/c/20180421/
            else if (e.KeyboardDevice.Modifiers == ModifierKeys.Shift && e.Key == Key.Enter)
            {
                _main.paramField.isParam_Edited = true;



                foreach (ParamSelector sp in _main.selectorList)
                {
                    //ImeEnabled = InputMethod.GetIsInputMethodEnabled(sp.invisibleText);

                    // if (ImeEnabled)
                    if (string.IsNullOrEmpty(selecter.invisibleText.Text))
                    {
                        return;
                    }




                    //何故かNUll文字ではなく改行コードが必ず入っているため　原因不明
                    if (selecter.invisibleText.Text != Environment.NewLine)
                    {
                        selecter.invisibleText.Visibility = Visibility.Hidden;

                        //     selecter.ParamLabel.Text = selecter.invisibleText.Text.Replace("\r\n", "", StringComparison.Ordinal);


                        selecter.ParamLabel.Visibility = Visibility.Visible;

                        selecter.ParamLabel.Text = selecter.invisibleText.Text;
                        //isLabelEited = false;


                    }

                    else
                    {
                        MessageBox.Show("名前を設定してください");
                        selecter.invisibleText.Text = selecter.ParamLabel.Text;
                        selecter.ParamLabel.Visibility = Visibility.Hidden;
                        selecter.invisibleText.Visibility = Visibility.Visible;

                        _main.Dispatcher.BeginInvoke((Action)delegate
                        {
                            Keyboard.Focus(sp.invisibleText);
                        }, DispatcherPriority.Render);


                        return;
                    }

                }
            }
        }


        public string OutputFileRename(string opFileName, string outputFileName_withoutEx, string endString)
        {

            //_main.OutputSelector.FilePathBox.Text = Path.GetDirectoryName(_main.ofc.opFileName) + "\\" + _main.paramField.outputFileName_withoutEx
            //    + _main.harua_View.MainParams[0].endString
            //    + ".mp4";


            string result = opFileName + "\\" + outputFileName_withoutEx
                  + endString
                  + ".mp4";
            return result;
        }

    }
}