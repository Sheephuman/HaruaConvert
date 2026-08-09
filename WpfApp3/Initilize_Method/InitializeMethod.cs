using HaruaConvert.HaruaService;
using HaruaConvert.Json;
using HaruaConvert.Methods;
using HaruaConvert.Parameter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows;
using WpfApp3.Parameter;

namespace HaruaConvert
{
    public partial class MainWindow : Window
    {
        public void InitializeViewModels()
        {
            try
            {
                if (paramField.SettingsStore == null)
                {
                    throw new InvalidOperationException("SettingsStore is not initialized. Call InitializeParameters first.");
                }

                var settingsService = new SettingsService(paramField.SettingsStore);
                harua_View = new Harua_ViewModel(settingsService, this);

                DataContext = harua_View.MainParams;
            }
            

            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"ViewModelの初期化中にエラーが発生しました: {ex.Message}", "エラー", MessageBoxButton.OK, MessageBoxImage.Error);



            }

        }


        private void InitializeParameters()
        {
            var baseDirectory = AppContext.BaseDirectory;
            var settingsJsonPath = Path.Combine(baseDirectory, "Settings.json");
            

            paramField = new ParamField()
            {
                isParamEdited = false,
                isExecuteProcessed = false,
                iniPath = settingsJsonPath,
                SettingsStore = new AppSettingsStore(settingsJsonPath),
                profileQueryIni = Path.Combine(baseDirectory, "settings.json"),
            };

            paramField.SettingsStore.Load();
        }


        public void LoadSettings()
        {
            isUseOriginalCheckProc(isUserParameter.IsChecked.Value);

            var setIniReader = new Settings_IOClass();
            setIniReader.JsonSettingReader(paramField, this);


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

    }
}
