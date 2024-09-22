using HaruaConvert.HaruaService;
using HaruaConvert.HaruaServise;
using HaruaConvert.Methods;
using HaruaConvert.Parameter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using Xceed.Wpf.Toolkit;

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
            paramField = new ParamField
            {
                isParam_Edited = false,
                isExecuteProcessed = false,
                isAutoScroll = true,
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
            NumericUpDown1.NUDButtonUP.Click += NUDUP_Button_Click;
            NumericUpDown1.NUDButtonDown.Click += NUD_DownButton_Click;

            InputSelector.AllowDrop = true;
            InputSelector.FilePathBox.AllowDrop = true;
            
//            InputSelector.openDialogButton.Drop += FileDrop;

            MouseLeftButtonDown += (sender, e) => { DragMove(); };
            InputSelector.openDialogButton.PreviewMouseDown += FileSelector_MouseDown;
            OutputSelector.openDialogButton.PreviewMouseDown += FileSelector_MouseDown;
        }

        public void InitializeChildComponents()
        {
            selectorList = new List<ParamSelector>();
            childCheckBoxList = new List<CheckBox>();


            //  childCheckBoxList.Capacity = 5; //現在のCheckBoxの数を指定


            // 子要素を列挙し、適切なリストに追加
            this.WalkInChildren(child =>
            {
                if (child is CheckBox checkBox)
                {
                    childCheckBoxList.Add(checkBox);
                }
                else if (child is ParamSelector paramSelector)
                {
                    selectorList.Add(paramSelector);
                }
            });

        }


       public void LoadCheckBoxStates()
        {
            var iniChecker = new IniCheckerClass.CheckboxGetSetValueClass();
            foreach (var checkBox in childCheckBoxList)
            {
                // CheckBoxの状態をINIファイルから読み込む
                checkBox.IsChecked = iniChecker.CheckBoxiniGetVallue(checkBox, paramField.iniPath);
            }

        }

       public void SelectorEventHandlers()
        {
            var gsp = new GenerateSelectParaClass();
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
