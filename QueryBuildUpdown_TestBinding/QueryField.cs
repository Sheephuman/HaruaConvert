using System.ComponentModel;

namespace QueryBuildUpdown_TestBinding
{
    public class QueryField : INotifyPropertyChanged
    {
        private string _bitrateQuery = string.Empty;
        public string BitRateQuery
        {
            get => _bitrateQuery;
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                if (_bitrateQuery != value)
                {
                    _bitrateQuery = value;
                    OnPropertyChanged(nameof(BitRateQuery));
                    BittatePB = value; // この行は本当に必要ですか？意図的な動作か確認してください。
                }
            }
        }

        private string _bittatePB = string.Empty;
        public string BittatePB
        {
            get => _bittatePB;
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                if (_bittatePB != value)
                {
                    _bittatePB = value;
                    OnPropertyChanged(nameof(BittatePB));
                }
            }
        }

        private string _buildQueryes = string.Empty;
        public string BuildQueryes
        {
            get => _buildQueryes;
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentNullException("value");
                if (_buildQueryes != value)
                {
                    _buildQueryes = value;
                    OnPropertyChanged(nameof(BuildQueryes));
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName)) throw new ArgumentNullException(nameof(propertyName));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
