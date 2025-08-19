using HaruaConvert.HaruaService;
using HaruaConvert.Methods;
using HaruaConvert.Parameter;
using System;
using System.Collections.Generic;
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
                var settingsService = new SettingsService(paramField.iniPath);
                harua_View = new Harua_ViewModel(settingsService);


                ///MainWindowのパラメータボックスで読み込むquery
                DataContext = harua_View.MainParams;
            }

            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"ViewModelの初期化中にエラーが発生しました: {ex.Message}", "エラー", MessageBoxButton.OK, MessageBoxImage.Error);



            }

        }


        private void InitializeParameters()
        {

            //iniPathにカレントディレクトリを設定
            paramField = new ParamField()
            {
                isParam_Edited = false,
                isExecuteProcessed = false,

                iniPath = Path.Combine(Environment.CurrentDirectory, "Settings.ini"),
                profileQueryIni = Path.Combine(Environment.CurrentDirectory, "QueryProfile.ini")
            };
            Ffmpc = new FfmpegQueryClass(this);
            firstSet = true;
            _arguments = string.Empty;
            th1 = new Thread(() => { });
            escapes = new EscapePath();
        }
        public void LoadSettings()
        {
            isUseOriginalCheckProc(isUserParameter.IsChecked.Value);

            var setiniReader = new IniSettings_IOClass();
            setiniReader.IniSettingReader(paramField, this);


        }

        private void SetupUIEvents()
        {
            // NumericUpDown1.NUDButtonUP.Click += NUDUP_Button_Click;
            //  NumericUpDown1.NUDButtonDown.Click += NUD_DownButton_Click;

            InputSelector.AllowDrop = true;
            InputSelector.FilePathBox.AllowDrop = true;

            //            InputSelector.openDialogButton.Drop += FileDrop;

            MouseLeftButtonDown += (sender, e) => { DragMove(); };
            InputSelector.openDialogButton.PreviewMouseDown += FileSelector_MouseDown;
            OutputSelector.openDialogButton.PreviewMouseDown += FileSelector_MouseDown;
        }

        public void SetSelectorList()
        {
            selectorList = new List<ParamSelector>();
            main.WalkInChildren(child =>
            {
                if (child is ParamSelector paramSelector)
                {
                    selectorList.Add(paramSelector);
                }


            });
        }


        public void SelectorEventHandlers()
        {
            var gsp = new GenerateSelectParaClass();

            selectorList = new List<ParamSelector>();
            foreach (var selector in selectorList)
            {
                // Selectorに各種イベントを登録
                gsp.GenerateParaSelector_setPropaties(selector, this);
            }


        }


        public void SetCommandBindings()
        {


        }
    }
}
