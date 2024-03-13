using HaruaConvert.Methods;
using HaruaConvert.Parameter;
using System;
using System.IO;
using System.Threading;
using System.Windows;

namespace HaruaConvert
{
    public partial class MainWindow : Window
    {
       public void InitializeViewModels()
        {
            try
            {

                //  Set Default Parameter on FfmpegQueryClass
                harua_View = new Harua_ViewModel(this);

                ///MainWindowのパラメータボックスで読み込むquery
                DataContext = harua_View._Main_Param;
            }

            catch (Exception ex)
            {
                MessageBox.Show($"ViewModelの初期化中にエラーが発生しました: {ex.Message}", "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
            
            

            }

        }


        private void InitializeParameters()
        {
            paramField = new ParamField
            {
                isParam_Edited = false,
                isExitProcessed = true,
                isAutoScroll = true,
                iniPath = Path.Combine(Environment.CurrentDirectory, "Settings.ini")
            };
            Ffmpc = new FfmpegQueryClass(this);
            firstSet = true;
            _arguments = string.Empty;
            th1 = new Thread(() => { });
            escapes = new EscapePath();
        }
        private void LoadSettings()
        {
            isUseOriginalCheckProc(isUserParameter.IsChecked.Value);

            var setiniReader = new IniSettings_IOClass();
            setiniReader.IniSettingReader(paramField, this);

         

        }

        private void SetupUIEvents()
        {
            NumericUpDown1.NUDButtonUP.Click += NUDUP_Button_Click;
            NumericUpDown1.NUDButtonDown.Click += NUD_DownButton_Click;

            InputSelector.AllowDrop = true;
            InputSelector.FilePathBox.AllowDrop = true;
            InputSelector.FilePathBox.Drop += FileDrop;
            InputSelector.openDialogButton.Drop += FileDrop;

            MouseLeftButtonDown += (sender, e) => { DragMove(); };
            InputSelector.openDialogButton.PreviewMouseDown += FileSelector_MouseDown;
            OutputSelector.openDialogButton.PreviewMouseDown += FileSelector_MouseDown;
        }



    }
}
