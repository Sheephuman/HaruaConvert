    using System;
    using System.ComponentModel;
using System.Diagnostics;

namespace HaruaConvert.QueryBuilder
    {
        public class QueryField: INotifyPropertyChanged
        {

         private string _bitrateQuery;
        public string BitRateQuery
        {  get => _bitrateQuery;
            
            set
            { 
                if (value == null)
                    if(_bitrateQuery != value)
                _bitrateQuery = value;
                    OnPropertyChanged(nameof(BitRateQuery));
            
            }
                }


            private  string _buildQueryes;

            public string BuildQueryes
            {
                get { return _buildQueryes; }
                set { 

                    if (string.IsNullOrEmpty(value))
                        throw new ArgumentNullException(value);
                if (_buildQueryes != value)
                {
                    _buildQueryes = value;
                    OnPropertyChanged(nameof(BuildQueryes));
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
