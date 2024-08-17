using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Permissions;

namespace HaruaConvert.QueryBuilder
{
    public class QueryField : INotifyPropertyChanged
    {

        private string _bitRateInput = string.Empty;
        public string BitRateInput
        {
            get => _bitRateInput;
            set
            {
                if (_bitRateInput != value)
                {

                    _bitRateInput = value;
                    OnPropertyChanged(nameof(BitRateInput));

                    
                }
            }
        }

        private string _videoCodecInput = string.Empty;
        public string VideoCodecInput
        {
            get => _videoCodecInput;
            set
            {
                if (_videoCodecInput != value)
                {

                    _videoCodecInput = value;
                    OnPropertyChanged(nameof(VideoCodecInput));
                   
                }
            }
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
        string _videoCodecStrings = string.Empty;

        //Codec Dictionary
        public string VideoCodecStrings
        { get => _videoCodecStrings;

            set
            {

                if (_videoCodecStrings != value)
                    if (_isVideoCodec)
                    {
                        string videoIndex = string.Empty;
                        if (!string.IsNullOrEmpty(value))
                            videoIndex = value.Substring(0, _videoCodecInput.IndexOf(":", StringComparison.CurrentCultureIgnoreCase));
                        _videoCodecStrings = $"-codec:v {videoIndex}";

                    }
                    else if (!_isVideoCodec)

                        _videoCodecStrings = string.Empty;
              
                OnPropertyChanged(nameof(VideoCodecStrings));
     
            }


        }
        private string _allInput = string.Empty;
        public string AllInput
        {
            get => _allInput;
            private set
            {
                if (_allInput != value)
                {
                    _allInput = value;
                    OnPropertyChanged(nameof(AllInput));
                }
            }
        }

     





        private string _ffmpegAudioCodecDic;
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

            protected virtual void OnPropertyChanged(string propertyName)
            {
                // プロパティ名が空またはnullの場合は例外を投げる。
                if (string.IsNullOrEmpty(propertyName)) throw new ArgumentNullException(nameof(propertyName));

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));


            }

     
        }
    }
