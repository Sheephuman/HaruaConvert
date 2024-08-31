using HaruaConvert.HaruaService;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using WpfApp3.Parameter;
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
#pragma warning disable CA1707 // 識別子はアンダースコアを含むことはできません
    public class Harua_ViewModel : INotifyPropertyChanged
#pragma warning restore CA1707 // 識別子はアンダースコアを含むことはできません
    {
        //ParamField paramField { get; set; }

        private ISettingsService _settingsService;

        public Harua_ViewModel(ISettingsService settingsService) 
        {
            // mainWindow = _main ?? throw new ArgumentNullException(nameof(_main));

            _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));

            

            LoadInitialData(_settingsService.GetIniPath());
            //    mainWindow = _main;


        


        }
        public void LoadInitialData(string iniPath)
        {

                MainParams = new ObservableCollection<MainParam>
            {
               new MainParam { StartQuery = IniDefinition.GetValueOrDefault
                                       (iniPath, QueryNames.ffmpegQuery , QueryNames.BaseQuery, "-b:v 700k -codec:v h264 -vf yadif=0:-1:1 -pix_fmt yuv420p -acodec aac -y -threads 2 "),
                OutputPath = MainTab_OutputDirectory,
                 endString = IniDefinition.GetValueOrDefault(iniPath, QueryNames.ffmpegQuery , QueryNames.endStrings, "_Harua"),
                SourcePathText = "フォルダ:" + IniDefinition.GetValueOrDefault
                                       (iniPath, "Directory", IniSettingsConst.ConvertDirectory, "Source File"),
                invisibleText = ""
               }
            };
        }
        private ObservableCollection<MainParam> _mainParam = new ObservableCollection<MainParam>();


        public ObservableCollection<MainParam> MainParams
        {

            get => _mainParam;
            set
            {
                if (_mainParam != value)
                {
                    _mainParam = value;
                    RaisePropertyChanged(nameof(_mainParam));
                }
            }
        }



        // INotifyPropertyChangedの実装
        public event PropertyChangedEventHandler PropertyChanged;


        public string invisibleText { get; set; }
        public string StartQuery { get; set; }
        public string OutputPath { get; set; }
        public string endString { get; set; }

        //public string SourcePathText = "Source File";
   

        protected void RaisePropertyChanged(string propertyName)
        {
             PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string _sourcePathText;
        //原因の切り分けのために例外を投げさせる実装
        public string SourcePathText {             
            get { return _sourcePathText; } 
            set{

                if (_sourcePathText != value)
                {

                    _sourcePathText = value;
                    RaisePropertyChanged(nameof(SourcePathText));
                }
            }           
            
            }

        int _propValue;
        public int PropValue
        {
            get { return _propValue; }
            set
            {
                if (_propValue != value)
                {
                    _propValue = value;
                    RaisePropertyChanged(nameof(PropValue));
                }

            }
        }

        protected bool SetProperty<T>(ref T field, T newValue, [CallerMemberName] string propertyName = null)
        {
            if (!Equals(field, newValue))
            {
                field = newValue;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                return true;
            }

            return false;
        }

        private string buildParameter11;

        public string buildParameter1 { get => buildParameter11; set => SetProperty(ref buildParameter11, value); }





    }
}
