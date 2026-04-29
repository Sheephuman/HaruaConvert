using HaruaConvert.HaruaService;
using HaruaConvert.ViewModel.ffmpegOptions.CheckBox;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using WpfApp3.Parameter;
using static HaruaConvert.IniCreate;
using static HaruaConvert.Parameter.ParamField;

namespace HaruaConvert.Parameter
{

    /// <summary>
    /// 初期化処理
    /// メインパラメータの内容を各パラメータ用変数 "StartQuery" に渡す
    /// ファイル末尾の文字列を変更した際の反映
    /// 各変数の初期化
    /// mainインスタンスのini設定内容読み込み
    /// </summary>
    /// 
    /// 
    /// 責務の再定義；Harua_ViewModelは各ViewModelの参照のみを持つ
    /// <summary>

    public class Harua_ViewModel : BindableBase

    {
        public Harua_ViewModel(MainWindow main) {        
            _main = main;
        }
        MainWindow _main { get;}

        //ParamField paramField { get; set; }

        private ISettingsService _settingsService;

        public Harua_ViewModel(ISettingsService settingsService, MainWindow main)
        {
            _main =main ?? throw new ArgumentNullException(nameof(main));

            // mainWindow = _main ?? throw new ArgumentNullException(nameof(_main));

            _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));

            LoadInitialData(_settingsService.GetIniPath());
            //    mainWindow = _main;


        }
        public void LoadInitialData(string iniPath)
        {

           // Debug.WriteLine("[LoadInitialData] iniPath=" + iniPath);
          //  Debug.WriteLine("[LoadInitialData] exists=" + File.Exists(iniPath));


            ClassShearingMenbers.ffmpegQuery = IniDefinition.GetValueOrDefault
                                          (iniPath, QueryNames.ffmpegQuery, QueryNames.BaseQuery, ClassShearingMenbers.defaultQuery);
            MainParams = new ObservableCollection<MainBindingParam>
            {
               new MainBindingParam { StartQuery = ClassShearingMenbers.ffmpegQuery,
                OutputPath = MainTab_OutputDirectory,
                 endString = IniDefinition.GetValueOrDefault(iniPath, QueryNames.ffmpegQuery , QueryNames.endStrings, "_Harua"),
                SourcePathText = "フォルダ:" + IniDefinition.GetValueOrDefault
                                       (iniPath, "Directory", IniSettingsConst.ConvertDirectory, "Source File"),
                invisibleText = "",
                placement = string.Empty,
                ffmpegOptionsStateModel = new ffmpegDetailsOptionsStateModel(_main._arguments)
                {
                    IsNoAudio = IniDefinition.GetValueOrDefault(iniPath,ClassShearingMenbers.CheckState,"NoAudio", false),
                }

               }
            };


        }


        private ObservableCollection<MainBindingParam> _mainParam = new ObservableCollection<MainBindingParam>();


        /// <summary>
        /// MainParamsプロパティは、MainBindingParamのコレクションを保持します。
        /// ObservableCollectionを使用することで、コレクションの変更を自動的にUIに通知します。
        /// </summary>
        public ObservableCollection<MainBindingParam> MainParams
        {
            get => _mainParam; // コレクションを取得するためのゲッター
            set
            {
                    SetProperty(ref _mainParam, value);

            }
        }


        //public string invisibleText { get; set; }
        //public string StartQuery { get; set; }
        public string OutputPath { get; set; }
        //public string endString { get; set; }
        //public string placement { get; set; }






        
        //原因の切り分けのために例外を投げさせる実装
        public string SourcePathText
        {
            get { return field; }
            set
            {

                    if (string.IsNullOrEmpty(value)) throw new ArgumentNullException(field, "_sourcePathText is null");

                    
                   SetProperty(ref field,value);
                }
            }

        }




        //protected bool SetProperty<T>(ref T field, T newValue, [CallerMemberName] string propertyName = null)
        //{
        //    if (!Equals(field, newValue))
        //    {
        //        field = newValue;
        //        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //        return true;
        //    }

        //    return false;
        //}




    }
