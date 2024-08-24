using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Permissions;
using static Microsoft.WindowsAPICodePack.Shell.PropertySystem.SystemProperties.System;

namespace HaruaConvert.QueryBuilder
{
    public class QueryField : INotifyPropertyChanged
    {

        private string _bitRateInput = string.Empty;
        public string BitRateInput
        {
            get => _bitRateInput;
            set => SetProperty(ref _bitRateInput, value, UpdateAllInput);
        }



        private bool SetProperty<T>(ref T field, T Value, Action onChanged= null, [CallerMemberName] string propertyName = null)
        {
            ///ジェネリック型Tのフィールドfieldと、新しい値Valueが等しくないかどうかを確認
            if (!EqualityComparer<T>.Default.Equals(field,Value))
            {
                field = Value;

                // onChanged が指定されている場合、これを呼び出します
                // これは値が変更された後に実行する追加のアクションを指定するために使用します
                onChanged?.Invoke();

                OnPropertyChanged(propertyName); //こちらの引数は省略出来ない？
                return true;
            }
            return false;
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

        public void UpdateAllInput()
        {
            var _bitRateQuery = _isBitrateChecked ? $"-b:v {_bitRateInput}k " : string.Empty;
            var _videoCodecesQuery = _isVideoCodec ? $"-codec:v {_videoCodecStrings} " :  string.Empty;

            var _audioCodecQuery = _isAudioCodec ? $"-codec:a {AudioCodecStrings}" : string.Empty;

            var EnableTwitterQuery = string.Empty;

            EnableTwitterQuery = _isEnableTwitter ? new Func<string>(() =>
            {
                _videoCodecesQuery = string.Empty;
                return "-codec:v h264 -vf yadif=0:-1:1 ";

            })() : string.Empty;

            AllInput = _bitRateQuery + EnableTwitterQuery + _videoCodecesQuery + _audioCodecQuery;

            OnPropertyChanged();
        }


        bool _isEnableTwitter;
        public bool IsEnableTwitte
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

      






        private string _buildQueryes;

        public string BuildQueryes
        {
            get { return _buildQueryes; }
            set
            {

                if (string.IsNullOrEmpty(value))
                    throw new ArgumentNullException(value);
                if (_buildQueryes != value)
                {
                    _buildQueryes = value;
                    OnPropertyChanged(nameof(BuildQueryes));
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
                

            }
        }

        string _videoCodecStrings = string.Empty;
        string videoIndex = string.Empty;
        //Codec Dictionary
        public string VideoCodecStrings
        { get => _videoCodecStrings;

            set
            {
                       videoIndex = makeIndexer(value);
                        // videoIndex = value;
                        if (!string.IsNullOrEmpty(videoIndex))
                           _videoCodecStrings = $"{videoIndex}";
                                                     
            }


        }
        private string _allInput = string.Empty;
        public string AllInput
        {
            get => _allInput;
            set => SetProperty(ref _allInput, value);　//メソッドを介して簡略化
        }

        //public string AllInput
        //{
        //    get => _allInput;
        //    private set
        //    {
        //        if (_allInput != value)
        //        {
        //            _allInput = value;
        //            OnPropertyChanged(nameof(AllInput));
        //        }
        //    }
        //}
        
       







        //private string _ffmpegAudioCodecDic;
        public Dictionary<string, string> FfmpegAudioCodecDic { get; set; }

       



        public Dictionary<string, string> FfmpegVideoCodecDic { get; internal set; }

        private bool _isVideoCodec;
        public bool isVideoCodec
        {
            get => _isVideoCodec;


            set
            {
                if (_isVideoCodec != value)
                {
                    _isVideoCodec = value;
                    OnPropertyChanged(nameof(isVideoCodec));
                }
            }
        }   



        public event PropertyChangedEventHandler PropertyChanged;

            protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            {
                // プロパティ名が空またはnullの場合は例外を投げる。
                if (string.IsNullOrEmpty(propertyName)) throw new ArgumentNullException(nameof(propertyName));

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));


            }

     
        }
    }
