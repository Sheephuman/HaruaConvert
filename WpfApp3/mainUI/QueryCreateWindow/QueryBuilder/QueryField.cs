using HaruaConvert.mainUI.QueryCreateWindow.ViewModel;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace HaruaConvert.QueryBuilder
{
    public class QueryField : BindableBase
    {
        public QueryWindowViewModel _queryWindowViewModel { get; set; } 

        public QueryField()
        {
            //    _queryWindowViewModel = new(this);
            _queryWindowViewModel = new(this);

            UserSelectedExtension = String.Empty;
            FfmpegAudioCodecDic = new Dictionary<string, string>();
            FfmpegAudioCodecDic = new Dictionary<string, string>();
            FfmpegVideoCacheDic = new Dictionary<string, string>();
        }

        private string _bitRateInput = string.Empty;
        public string BitRateInput
        {
            get => _bitRateInput;
            set {
                SetProperty(ref _bitRateInput, value, null);
                UpdateAllInput(_bitRateInput);
            }
                    
        }




        private bool _isBitrateChecked;
        public bool IsBitrateChecked
        {
            get => _isBitrateChecked;
            set => SetProperty(ref _isBitrateChecked, value);

        }


        bool _isAudioCodec;
        public bool IsAudioCodec
        {

            get => _isAudioCodec;
            set => SetProperty(ref _isAudioCodec, value);

        }

        public string UserSelectedExtension { get => field;
            set
            {
                SetProperty(ref field, value);
                UpdateAllInput(field);
            }
        
        }


        public string OutputFileSuffix
        {
            get => field;
            set
            {
                SetProperty(ref field, value);             
            }
        }



        public void UpdateAllInput(string fileNameExtention )
        {
            if (_queryWindowViewModel is null)
                return;
  
            var _bitRateQuery = _isBitrateChecked ? $"-b:v {_bitRateInput}k " : string.Empty;
            var _videoCodecesQuery = _isVideoCodec ? $"-codec:v {_videoCodecStrings} " : string.Empty;

            var _audioCodecQuery = _isAudioCodec ? $"-codec:a {AudioCodecStrings} " : string.Empty;



            string EnableTwitterQuery = _isEnableTwitter ? new Func<string>(() =>
            {
                _videoCodecesQuery = string.Empty;
                return "-codec:v libx265 -vf yadif=0:-1:1 ";

            })() : string.Empty;



            OutputFileSuffix = _isOtherFileNameEx ? new Func<string>(() =>
            {
                string hit = FileExtentions.FirstOrDefault(x => x == fileNameExtention);
                return "{FileName}" + hit;
            })() : string.Empty;

            AllInput = _bitRateQuery + EnableTwitterQuery + _videoCodecesQuery + _audioCodecQuery + OutputFileSuffix;
            SetProperty(ref _allInput, AllInput);
        }


        bool _isEnableTwitter;
        public bool IsEnableTwitter
        {

            get => _isEnableTwitter;
            set => SetProperty(ref _isEnableTwitter, value);
        }



        string makeIndexer(string value)
        {
            int index = value.IndexOf(" : ", StringComparison.CurrentCultureIgnoreCase);
            string result = string.Empty;
            if (index != -1)
            {
                result = value.Substring(0, index);
            }

            return result;
        }








        public string BuildQueryes
        {
            get { return field; }
            set
            {

                if (string.IsNullOrEmpty(value))
                    throw new ArgumentNullException(value);
                if (field != value)
                {
                    
                    SetProperty(ref field, value);
                }

            }
        }


        string audioIndex = string.Empty;
        private string _audioCodecStrings = string.Empty;
        public string AudioCodecStrings
        {
            get => _audioCodecStrings;
            set
            {

                audioIndex = makeIndexer(value);
                if (!string.IsNullOrEmpty(videoIndex))
                    _audioCodecStrings = $"{audioIndex}";
                SetProperty(ref _audioCodecStrings, value);

            }
        }

        string _videoCodecStrings = string.Empty;
        string videoIndex = string.Empty;
        //Codec Dictionary
        public string VideoCodecStrings
        {
            get => _videoCodecStrings;

            set
            {
                videoIndex = makeIndexer(value);
                // videoIndex = value;
                if (!string.IsNullOrEmpty(videoIndex))
                    _videoCodecStrings = $"{videoIndex}";

                SetProperty(ref _videoCodecStrings, value);

            }


        }

        /// <summary>
        /// _allInputはメソッドで使用しているので消せない
        /// </summary>
        private string _allInput = string.Empty;

        public ObservableCollection<string> FileExtentions { get; } = new ObservableCollection<string>
        {
           ".mp4",
           ".mkv",
           ".webm",
            ".mov",
           ".avi",
           "ts",
           ".flv",
           ".wmv",
           ".mpeg",
           ".gif",
           ".rmvb",
        };


        public string AllInput
        {
            get => _allInput;
            set => SetProperty(ref _allInput, value);　//メソッドを介して簡略化
        }



        private bool _isOtherFileNameEx; //メソッドで使用
        public bool IsOtherFileNameExtension
        {
            get => _isOtherFileNameEx;
            set => SetProperty(ref _isOtherFileNameEx, value);
        }

       






        //private string _ffmpegAudioCodecDic;
        public Dictionary<string, string> FfmpegAudioCodecDic { get; set; }





        public Dictionary<string, string> FfmpegVideoCodecDic { get; internal set; }


        public Dictionary<string, string> FfmpegVideoCacheDic { get; internal set; }

        private bool _isVideoCodec;
        public bool isVideoCodec
        {
            get => _isVideoCodec;


            set
            {
                if (_isVideoCodec != value)
                {
                    _isVideoCodec = value;
                    SetProperty(ref _isVideoCodec, value, null);
                }
            }
        }





    }
}
