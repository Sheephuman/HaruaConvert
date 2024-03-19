using HaruaConvert.Command;
using HaruaConvert.HaruaInterFace;
using HaruaConvert.Parameter;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using Windows.System.UserProfile;
using static HaruaConvert.Parameter.ParamField;

namespace HaruaConvert.HaruaServise
{
    public class UIManager
    {

        private MainWindow _mainWindow;
        private static bool FirstSet;

        public UIManager(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
           

            //get Interface instance
            _mainWindow.mainTabEvents = new IMainTabEvents[]
             {

                new Directory_ClickProcedure(_mainWindow)
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
                _mainWindow.Loaded += MainWindow_Loaded;
                _isLoadedHandled = true; // フラグを設定して再登録を防ぐ
            }
            // ファイルドロップイベントハンドラの設定
            _mainWindow.AllowDrop = true;
            _mainWindow.DragOver += MainWindow_DragOver;
            _mainWindow.Drop += MainWindow_FileDrop;

            // その他のUI操作に関わるイベントハンドラを設定
           // _mainWindow.btnSaveSettings.Click += BtnSaveSettings_Click;
         //   _mainWindow.btnLoadSettings.Click += BtnLoadSettings_Click;
        }

        public void MainWindow_FileDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
               _mainWindow.FileList.Clear();
                var fileNames = (string[])e.Data.GetData(DataFormats.FileDrop);
                foreach (var name in fileNames)
                {
                    _mainWindow.FileList.Add(name);
                }

                _mainWindow.paramField.setFile = _mainWindow.FileList[0];
                _mainWindow.filePathOutput = _mainWindow.paramField.setFile;

                _mainWindow.Drop_Label.Content = "Convert";

                var ClickedControl = sender as Control;

                if (ClickedControl.Name == _mainWindow.Directory_DropButon.Name)
                    _mainWindow.SourcePathLabel.Text = _mainWindow.paramField.setFile;




                if (ClickedControl.Name == _mainWindow.InputSelector.FilePathBox.Name)
                    _mainWindow.InputSelector.FilePathBox.Text = _mainWindow.paramField.setFile;



                if (ClickedControl.Name == _mainWindow.OutputSelector.Name)
                    _mainWindow.InputSelector.FilePathBox.Text = _mainWindow.paramField.setFile;


                if (ClickedControl.Name == _mainWindow.InputSelector.Name + ParamField.ButtonNameField._openButton)
                    _mainWindow.InputSelector.FilePathBox.Text = _mainWindow.paramField.setFile;



                _mainWindow.harua_View.SourcePathText = _mainWindow.paramField.setFile;

                // MediaInfoServiceのインスタンスを作成
                IMediaInfoManager mediaInfoDisplay = _mainWindow; // MainWindowがIMediaInfoDisplayを実装していると仮定
                MediaInfoService mediaInfoService = new MediaInfoService(mediaInfoDisplay);

                mediaInfoService.displayMediaInfo(_mainWindow.paramField.setFile);

            }
        }

        public void RegisterUIDropEvent()
        {
            if (MainWindow.firstSet )
            {
                Button dropbutton = (Button)_mainWindow.Drop_Label.Template.FindName(ButtonNameField.Convert_DropButton, _mainWindow.Drop_Label);
                if(dropbutton !=null)
                    dropbutton.Click += DropButton_ClickHandle;



                _mainWindow.Directory_DropButon.Click += DropButton_ClickHandle;

                _mainWindow.AtacchStringsList.Items.Add("[]");
                _mainWindow.AtacchStringsList.Items.Add("{}");
                _mainWindow.AtacchStringsList.Items.Add("<>");

                FirstSet = true;
                return;
            
            }
            
        }

        private void MainWindow_DragOver(object sender, DragEventArgs e)
        {
            throw new NotImplementedException();
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


        public void DropButton_ClickHandle(object sender, RoutedEventArgs e)
        {

            foreach(var button in _mainWindow.mainTabEvents)
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


#pragma warning disable CA1707 // 識別子はアンダースコアを含むことはできません
        public void InvisibleText_KeyDown(object sender, KeyEventArgs e)
#pragma warning restore CA1707 // 識別子はアンダースコアを含むことはできません
        {
            var selecter = sender as ParamSelector;


            if (selecter == null)
            {
                return;
            }


            if (e.Key == Key.Escape)
            {
                foreach (ParamSelector sp in _mainWindow.selectorList)
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
                _mainWindow.paramField.isParam_Edited = true;



                foreach (ParamSelector sp in _mainWindow.selectorList)
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

                        _mainWindow.Dispatcher.BeginInvoke((Action)delegate
                        {
                            Keyboard.Focus(sp.invisibleText);
                        }, DispatcherPriority.Render);


                        return;
                    }

                }
            }
        }

    }
}