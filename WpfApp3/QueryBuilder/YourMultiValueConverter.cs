using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaruaConvert.QueryBuilder
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Windows.Data;
    using static Microsoft.WindowsAPICodePack.Shell.PropertySystem.SystemProperties.System;

    public class YourMultiValueConverter : IMultiValueConverter
    {
        private bool _isBitrateChecked;
        public bool IsBitrateChecked
        {
            get => _isBitrateChecked;
            set
            {
                if (_isBitrateChecked != value)
                {
                    _isBitrateChecked = value;
                    OnPropertyChanged(nameof(IsBitrateChecked));
                }
            }

        }

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

        private string _bitRateQuery = string.Empty;
        public string BitRateQuery
        {
            get => _bitRateQuery;
            set
            {
                if (_bitRateQuery != value)
                {

                    _bitRateQuery = value;
                    OnPropertyChanged(nameof(BitRateQuery));


                }
            }
        }


        private string _videoCodecs = string.Empty;
        public string VideoCodecs
        {
            get => _videoCodecs;
            set
            {
                if (_videoCodecs != value)
                {

                    _videoCodecs = value;
                    OnPropertyChanged(nameof(VideoCodecs));


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



        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            string bitrate = _isBitrateChecked ? $"-b:v {values[0]}k " : string.Empty;
            string codec = _isVideoCodec ? $"-codec:v {values[1]}" : string.Empty;
            return $"{bitrate} {codec}".Trim();
        }


        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("ConvertBack is not supported.");
        }
    }
}

